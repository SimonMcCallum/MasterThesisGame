using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using FastColoredTextBoxNS;
using Xna = Microsoft.Xna.Framework;
using Hig.Compiler.LexicalAnalysis;

namespace MasterThesisGame
{
    public partial class FormMain : Form
    {
        private const string _path = "Levels";

        private readonly string[] _keywords = { "SELECT", "FROM", "WHERE", "TOP", "AS", "ON", "ORDER", "BY", "JOIN", "INNER", "LEFT", "RIGHT", "CROSS", "DISTINCT", "AND", "OR", "LIKE", "ASC", "DESC" };
        private readonly string[] _tables = { "Entities", "Species", "Classes", "Feeding" };
        private readonly List<string> _columns = new List<string>();

        private SQLGame _game;
        private Lexer _lexer;
        private AutocompleteMenu _popupMenu;
        private int _currentLevel = 0;
        private DateTime _timeStart;

        public bool CanGoToNextLevel
        {
            get
            {
                if (_game != null)
                    return _game.Level.IsFinished;

                return false;
            }
        }

        private readonly List<ILexerRule> _lexerRules = new List<ILexerRule>();

        private readonly static List<Task> _levels = new List<Task>();

        public FormMain()
        {
            InitializeComponent();
            Init();
        }

        private void LinkClick(object sender, HtmlElementEventArgs e)
        {
            string tag = ((HtmlElement)sender).GetAttribute("tag");

            if (!String.IsNullOrEmpty(tag))
            {
                tbcManager.SelectTab(2);
                tbSqlEditor.Text = tag;
            }
        }

        private void Init()
        {

            //string connStringCI = "Data Source=GameDB.sdf";
            //string connStringCS = "Data Source=GameDB.sdf";
            //SqlCeEngine engine = new SqlCeEngine(connStringCI);
            //engine.Upgrade(connStringCS);

            List<AutocompleteItem> items = new List<AutocompleteItem>();
            var table = SelectData("select distinct Column_name from INFORMATION_SCHEMA.Columns order by Column_name");

            foreach (var item in _keywords)
            {
                items.Add(new AutocompleteItem(item));
                _lexerRules.Add(new LexerRule(item));
            }

            foreach (var item in _tables)
            {
                items.Add(new AutocompleteItem(item));
                _lexerRules.Add(new LexerRule(item));
            }

            foreach (DataRow row in table.Rows)
            {
                string item = row["Column_name"].ToString();
                items.Add(new AutocompleteItem(item));
                items.Add(new MethodAutocompleteItem(item));
                _lexerRules.Add(new LexerRule(item));
            }

            _popupMenu = new AutocompleteMenu(tbSqlEditor);
            _popupMenu.Items.SetAutocompleteItems(items);
            _popupMenu.SearchPattern = @"[\w\.:=!<>]";
            _popupMenu.AllowTabKey = true;

            _lexerRules.Add(new LexerRegexRule("string", "^\'.*\'$"));
            _lexerRules.Add(new LexerRegexRule("digit", @"^\d+$|^\d*\.\d+$"));
            _lexerRules.Add(new LexerRule("point", false, "."));
            _lexerRules.Add(new LexerRule("symbol", false, "*", "=", "<>", ">", "<", ",", "(", ")"));
            _lexerRules.Add(new LexerRegexRule("literal", "^\\S+$"));

            _lexer = new Lexer(
                _lexerRules.ToArray(),
                "\r\n",
                new[] { new ShieldSymbols('\'') },
                @" |\t|\+|\-|/|%|\*|\(|\)|<|>|=|\,|\.|\n|\r");
        }

        private static void FindLevels()
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);

                return;
            }

            List<Task> tasks = new List<Task>();

            foreach (var roundPath in Directory.GetFiles(_path, "*.rrl"))
                tasks.Add(Task.Load(roundPath));

            _levels.AddRange(tasks.OrderBy(t => t.Number));
        }

        private DataTable SelectData(string query)
        {
            SqlCeCommand cmd = new SqlCeCommand(query);
            DataTable dt = new DataTable();
            SqlCeConnection con = new SqlCeConnection(Properties.Settings.Default.GameDBConnectionString);
            cmd.Connection = con;
            SqlCeDataAdapter sda = new SqlCeDataAdapter();
            cmd.CommandType = CommandType.Text;

            try
            {
                con.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                dt.Columns.Add("Error");
                dt.Rows.Add(ex.Message);

                return dt;
            }
            finally
            {
                con.Close();
                cmd.Dispose();
                sda.Dispose();
            }

            return dt;
        }

        private void InsertData(string query)
        {
            SqlCeCommand cmd = new SqlCeCommand(query);
            SqlCeConnection con = new SqlCeConnection(Properties.Settings.Default.GameDBConnectionString);
            cmd.Connection = con;
            SqlCeDataAdapter sda = new SqlCeDataAdapter();
            cmd.CommandType = CommandType.Text;

            try
            {
                con.Open();
                sda.InsertCommand = cmd;
                sda.InsertCommand.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
                sda.Dispose();
            }
        }

        private bool CheckMandatoryLexems(Token[] tokens)
        {
            bool[] hasMandatoryLexems = new bool[_game.Level.MandatoryLexems.Count];

            for (int i = 0; i < tokens.Length; i++)
            {
                int count = 0;
                bool has = true;

                for (int j = 0; j < _game.Level.MandatoryLexems.Count; j++)
                {
                    if (tokens[i].Name.ToUpper() == _game.Level.MandatoryLexems[j].Text.ToUpper())
                        count++;

                    if (count == _game.Level.MandatoryLexems[j].Count)
                        hasMandatoryLexems[j] = true;

                    has &= hasMandatoryLexems[j];
                }

                if (has)
                    return true;
            }

            return false;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbSqlEditor.Text))
            {
                bool isCorrect = true;
                string errorMessage = "Invalid instruction";
                Token[] tokens = _lexer.Analyze(tbSqlEditor.Text);

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].Name == "literal" && (i == 0 || tokens[i - 1].Name != "AS") && (i >= tokens.Length - 1 || tokens[i + 1].Name != "point"))
                    {
                        isCorrect = false;
                        errorMessage += ": " + tokens[i].Attribute;
                        break;
                    }
                }

                if (isCorrect && _game.Level.MandatoryLexems.Count != 0 && !CheckMandatoryLexems(tokens))
                {
                    isCorrect = false;
                    errorMessage = "Please read the task carefully.";
                }

                DataTable table = null;

                if (isCorrect)
                {
                    table = SelectData(tbSqlEditor.Text);
                }
                else
                {
                    table = new DataTable();
                    table.Columns.Add("Error");
                    table.Rows.Add(errorMessage);
                }

                dgvResult.DataSource = table;

                foreach (DataGridViewColumn column in dgvResult.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;

                if (isCorrect)
                {
                    _game.SelectAnimals(table);

                    if (!_game.IsFinalAnimActivated && _levels[_currentLevel].Score != 0)
                        _levels[_currentLevel].Score--;
                }
            }
        }

        private void CreateLevels()
        {
            string path = @"Levels\task{0}.rrl";
            string head = "<head><style>.link { cursor:pointer; color:blue; text-decoration:underline; }</style></head>";

            #region " SQL Patter table "

            string patternTable = @"
<br><br><table border='3'><caption>SQL Patterns</caption>
<tr><td>SQL pattern</td><td>Description</td><td>Link</td></tr>
<tr>
    <td>SELECT *<br>FROM table_name</td>
    <td>Select rows from table 'table_name'</td>
    <td><a name='link1' class='link' tag=
'SELECT * 
FROM table_name'
>Try</a></td>
</tr><tr>
    <td>SELECT *<br>FROM table_name<br>ORDER BY column_name</td>
    <td>Select rows from table 'table_name' and sort them by column_name</td>
    <td><a name='link2' class='link' tag=
'SELECT * 
FROM table_name
ORDER BY column_name'
>Try</a></td>
</tr><tr>
    <td>SELECT TOP(N) *<br>FROM table_name</td>
    <td>Select first N rows from table 'table_name'</td>
    <td><a name='link3' class='link' tag=
'SELECT TOP(N) * 
FROM table_name'
>Try</a></td>
</tr><tr>
    <td>SELECT *<br>FROM table_name<br>WHERE condition</td>
    <td>Select rows from table 'table_name' that satisfy the condition</td>
    <td><a name='link4' class='link' tag=
'SELECT * 
FROM table_name
WHERE condition'
>Try</a></td>
</tr><tr>
    <td>SELECT *<br>FROM table_name<br>WHERE condition<br>ORDER BY column_name</td>
    <td>Select rows from table 'table_name' that satisfy the condition and sort them by column_name</td>
    <td><a name='link5' class='link' tag=
'SELECT * 
FROM table_name
WHERE condition
ORDER BY column_name'
>Try</a></td>
</tr><tr>
    <td>SELECT *<br>FROM table_name_1 AS t1<br>INNER JOIN table_name_2 AS t2 ON t1.column_name = t2.column_name</td>
    <td>Combine rows from tables based on a common field between them</td>
    <td><a name='link6' class='link' tag=
'SELECT *
FROM table_name_1 AS t1
INNER JOIN table_name_2 AS t2 ON t1.column_name = t2.column_name'
>Try</a></td>
</tr><tr>
    <td>SELECT *<br>FROM table_name_1 AS t1<br>INNER JOIN table_name_2 AS t2 ON t1.column_name = t2.column_name<br>WHERE condition</td>
    <td>Combine rows from tables based on a common field between them and filter them by condition</td>
    <td><a name='link7' class='link' tag=
'SELECT *
FROM table_name_1 AS t1
INNER JOIN table_name_2 AS t2 ON t1.column_name = t2.column_name
WHERE condition'
>Try</a></td>
</tr></table>";

            #endregion

            Task task;
            int i = 1;

            #region " Tasks "

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 1: Task 1</h2><h4>Select all animals on the map.</h4></body>";
            task.Info = head + @"
<body><h3>Task: Select all animals on the map.</h3>
<h4>Hello my friend. We are inhabitants of X planet. We love your planet and we want to learn more about it. Help us to collect animals for study. Do not worry, all animals will be safely returned.
We will visit your planet each level and you purpose is choosing of some animals that we will need and send their coordinates. You have access to our database and you can use SQL.</h4>
<h3>What is SQL?</h3>
<h4><ul><li>SQL stands for Structured Query Language</li>
<li>SQL lets you access and manipulate databases</li>
<li>SQL is an ANSI (American National Standards Institute) standard</li></ul></h4>
<h3>What can SQL do?</h3>
<h4><ul><li>SQL can execute queries against a database</li>
<li>SQL can retrieve data from a database</li>
<li>SQL can insert records in a database</li>
<li>SQL can update records in a database</li>
<li>SQL can delete records from a database</li>
<li>SQL can create new databases</li>
<li>SQL can create new tables in a database</li>
<li>SQL can create stored procedures in a database</li>
<li>SQL can create views in a database</li>
<li>SQL can set permissions on tables, procedures, and views</li></ul></h4>
<h3>How to play?</h3>
<h4>Above this text you can see 3 tabs: 'Info', 'Tables', 'SQL'.
<ul><li>'Info' shows information about task</li>
<li>'Tables' shows structure of database</li>
<li>In 'SQL' tab you can write SQL query and execute it by the button 'Run'</li></ul>
In the left corner you can see 5 stars. It is your score for the current task. If you make a wrong SQL query then your score decrease. But you will able to finish a task with 0 stars.
<br>A database most often contains one or more tables. Each table is identified by a name (In our database are 'Entities', 'Species', 'Classes' and 'Feeding'). Tables contain records (rows) with data.
<br><table border='3'><caption>Table Entities</caption>
<tr><td>EntityId</td><td>SpeciesId</td><td>X</td><td>Y</td><td>Weight</td><td>Age</td><td>Sex</td></tr>
<tr><td>1</td><td>1</td><td>3</td><td>3</td><td>25</td><td>5</td><td>True</td></tr>
<tr><td>2</td><td>2</td><td>5</td><td>2</td><td>100</td><td>15</td><td>False</td></tr>
<tr><td>3</td><td>2</td><td>3</td><td>6</td><td>6</td><td>3</td><td>False</td></tr>
</table>
<br>
The table above contains 3 records (one for each entity) and 7 columns (EntityId, SpeciesId, X, Y, Weight, Age, and Sex).
<h3>SQL Statements</h3>
<h4>Most of the actions you need to perform on a database are done with SQL statements. The following SQL statement selects all the records in the 'Entities' table:
<br><br><a name='link1' class='link' tag='SELECT * FROM Entities'>SELECT * FROM Entities</a>
<br>(Clik this query and then click the 'Run' button on the 'SQL' tab)</h4></body>";
            task.Points.Add(new Xna.Point(3, 4));
            task.Points.Add(new Xna.Point(0, 2));
            task.Points.Add(new Xna.Point(1, 1));
            task.Points.Add(new Xna.Point(6, 3));
            task.Points.Add(new Xna.Point(4, 6));
            task.Points.Add(new Xna.Point(5, 5));
            task.Points.Add(new Xna.Point(1, 6));
            task.Points.Add(new Xna.Point(6, 1));
            task.Points.Add(new Xna.Point(4, 2));
            //level.MandatoryLexems.Add(new Level.MandatoryLexem("join", 1));
            //level.VisibleInformation.Add("sex");
            //level.VisibleInformation.Add("age");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 1: Task 2</h2><h4>Select all animals on the map.</h4></body>";
            task.Info = head + @"
<body><p><h3>Task: Select all animals on the map.</h3>
<h4>Do you remember the table example from the previous task? Right. It was the 'Entities' table. Each row of that table is one animal on the map. For getting data from that table you should write a SELECT statement.</h4>
<h3>SQL SELECT Syntax</h3>
<h4>
SELECT * FROM <i>table_name</i>;
<br><br>and
<br><br>SELECT <i>column_name</i>, <i>column_name</i>
<br>FROM <i>table_name</i>;
<br><br>Note: '*' in the first query means selects all columns. 
</h4>
</p>
</body>";
            task.Points.Add(new Xna.Point(3, 4));
            task.Points.Add(new Xna.Point(0, 2));
            task.Points.Add(new Xna.Point(1, 1));
            task.Points.Add(new Xna.Point(6, 3));
            task.Points.Add(new Xna.Point(4, 6));
            task.Points.Add(new Xna.Point(5, 5));
            task.Points.Add(new Xna.Point(1, 6));
            task.Points.Add(new Xna.Point(6, 1));
            task.Points.Add(new Xna.Point(4, 2));
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 2: Task 1</h2><h4>Select all male animals on the map.</h4></body>";
            task.Info = head + @"
<body><p><h3>Task: Select all male animals on the map.</h3>
<h4>The 'Entities' table has 'Sex' column: 1 = Male, 0 = Female.</h4>
<h3>SQL WHERE Syntax</h3>
<h4>SELECT <i>column_name</i>, <i>column_name</i>
<br>FROM <i>table_name</i>
<br>WHERE <i>column_name operator value</i>;
</h4>

<h3>WHERE Clause Example</h3>
<h4>The following SQL statement selects all the rows with name 'Monkey', in the 'Species' table:
<br><br>SELECT * FROM Species WHERE Name = 'Monkey'</h4>

<h3>Text Fields vs. Numeric Fields</h3>
<h4>SQL requires single quotes around text values (most database systems will also allow double quotes). However, numeric fields should not be enclosed in quotes.
<br><br>SELECT * FROM Species WHERE SpeciesId = 2</h4>

<h3>Operators in The WHERE Clause</h3>
<h4>The following operators can be used in the WHERE clause:
<table border='3'>
<tr><td>Operator</td><td>Description</td></tr>
<tr><td>=</td><td>Equal</td></tr>
<tr><td><></td><td>Not equal. Note: In some versions of SQL this operator may be written as !=</td></tr>
<tr><td>></td><td>Greater than</td></tr>
<tr><td><</td><td>Less than</td></tr>
<tr><td>>=</td><td>Greater than or equal</td></tr>
<tr><td><=</td><td>Less than or equal</td></tr>
<tr><td>BETWEEN</td><td>Between an inclusive range</td></tr>
<tr><td>LIKE</td><td>Search for a pattern</td></tr>
<tr><td>IN</td><td>To specify multiple possible values for a column</td></tr>
</table>
<br><br>Also some levels have SQL patterns in the bottom of 'Info' tab. It should simplify you task. Just choose pattern that you think can solve the current task and click 'Try' link.
<br>(Note: This table contains just common patterns but not real solutions. Use it as help information)" +
patternTable + "</h4></p></body>";
            
            task.Points.Add(new Xna.Point(0, 2));
            task.Points.Add(new Xna.Point(1, 1));
            task.Points.Add(new Xna.Point(4, 6));
            task.Points.Add(new Xna.Point(4, 2));
            task.VisibleInformation.Add("sex");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 2: Task 2</h2><h4>Select all animals that have weight more than 10 kg and less than 100 kg.</h4></body>";
            task.Info = head + @"
<body><p><h3>Task: Select all animals that have weight more than 10 kg and less than 100 kg.</h3>
<h4>The 'Entities' table has colunm 'Weight'. It contains weights of animals in kg.
<br>The AND & OR operators are used to filter records based on more than one condition.</h4>

<h3>Example</h3>
<h4>SELECT * 
<br>FROM Species 
<br>WHERE Name = 'Monkey' AND IsEndangered = 0

<br><br>You can also combine AND and OR (use parenthesis to form complex expressions)." + patternTable + "</h4></p></body>";
            task.Points.Add(new Xna.Point(3, 4));
            task.Points.Add(new Xna.Point(4, 6));
            task.Points.Add(new Xna.Point(1, 6));
            task.Points.Add(new Xna.Point(4, 2));
            task.VisibleInformation.Add("weight");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 2: Task 3</h2><h4>Select all female animals that are 3 years old or 19 years old.</h4></body>";
            task.Info = head + "<body><p><h3>Task: Select all female animals that are 3 years old or 19 years old.</h3><h4>The 'Entities' table contains 'Sex' and 'Age' columns.</h4></p></body>";
            task.Points.Add(new Xna.Point(6, 3));
            task.Points.Add(new Xna.Point(5, 5));
            task.Points.Add(new Xna.Point(1, 6));
            task.VisibleInformation.Add("sex");
            task.VisibleInformation.Add("age");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 3: Task 1</h2><h4>Select 3 the oldest animals.</h4></body>";
            task.Info = head + @"
<body><p><h3>Task: Select 3 the oldest animals.</h3>
<h4>You can sort animals by age descending and take the first 3.</h4>
<h3>The SQL SELECT TOP Clause</h3>
<h4>The SELECT TOP clause is used to specify the number of records to return. The SELECT TOP clause can be very useful on large tables with thousands of records. Returning a large number of records can impact on performance.

<br><br>SELECT TOP(<i>number</i>) <i>column_name(s)</i>
<br>FROM <i>table_name</i></h4>

<h3>The SQL ORDER BY Keyword</h3>
<h4>The ORDER BY keyword is used to sort the result-set by one or more columns. The ORDER BY keyword sorts the records in ascending order by default. To sort the records in a descending order, you can use the DESC keyword.

<br><br>SELECT <i>column_name</i>, <i>column_name</i>
<br>FROM <i>table_name</i>
<br>ORDER BY <i>column_name</i>, <i>column_name</i> ASC|DESC;" + patternTable + "</h4></p></body>";
            task.Points.Add(new Xna.Point(0, 2));
            task.Points.Add(new Xna.Point(5, 5));
            task.Points.Add(new Xna.Point(4, 2));
            task.VisibleInformation.Add("age");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 3: Task 2</h2><h4>Select 3 the youngest female animals.</h4></body>";
            task.Info = head + "<body><h3>Task: Select 3 the youngest female animals.</h3><h4>Note: ORDER BY statement should be placed after WHERE statement.</h4></body>";
            task.Points.Add(new Xna.Point(3, 4));
            task.Points.Add(new Xna.Point(6, 3));
            task.Points.Add(new Xna.Point(1, 6));
            task.VisibleInformation.Add("sex");
            task.VisibleInformation.Add("age");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 4: Task 1</h2><h4>Select all animals that are endangered species.</h4></body>";
            task.Info = head + @"
<body><p><h3>Task: Select all animals that are endangered species.</h3>
<h4>The 'Species' table contains 'IsEndangered' column. But you should get data from 'Entities' table. How to do it? Right, you should combine two tables into one and filter rows by 'IsEndangered' column. Endangered animals are marked with word 'Yes' on the map.</h4>

<h3>SQL Aliases</h3>
<h4>SQL aliases are used to give a database table, or a column in a table, a temporary name. Basically aliases are created to make column names more readable.</h4>

<h3>SQL Alias Syntax for Columns</h3>
<h4>SELECT <i>column_name</i> AS <i>alias_name</i>
<br>FROM <i>table_name</i></h4>

<h3>SQL Alias Syntax for Tables</h3>
<h4>SELECT <i>column_name(s)</i>
<br>FROM <i>table_name</i> AS <i>alias_name</i></h4>

<h3>SQL JOIN Keyword</h3>
<h4>An SQL JOIN clause is used to combine rows from two or more tables, based on a common field between them.
Let's look at a selection from the 'Entities' table:

<br><table border='3'>
<tr><td>EntityId</td><td>SpeciesId</td><td>X</td><td>Y</td><td>Weight</td><td>Age</td><td>Sex</td></tr>
<tr><td>1</td><td>1</td><td>3</td><td>3</td><td>25</td><td>5</td><td>True</td></tr>
<tr><td>2</td><td>2</td><td>5</td><td>2</td><td>100</td><td>15</td><td>False</td></tr>
<tr><td>3</td><td>2</td><td>3</td><td>6</td><td>6</td><td>3</td><td>False</td></tr>
</table>

<br>Then, have a look at a selection from the 'Species' table:

<br><br><table border='3'>
<tr><td>SpeciesId</td><td>ClassId</td><td>FeedingId</td><td>Name</td><td>IsEndangered</td></tr>
<tr><td>1</td><td>1</td><td>1</td><td>Monkey</td><td>False</td></tr>
<tr><td>2</td><td>1</td><td>2</td><td>Elephant</td><td>False</td></tr>
</table>

<br>Notice that the 'SpeciesId' column in the 'Species' table refers to the entity in the 'Entities' table. The relationship between the two tables above is the 'SpeciesId' column. And if you use INNER JOIN statment it produces result common tables with all columns from both tables.</h4>

<h3>SQL INNER JOIN Syntax</h3>
<h4>SELECT <i>column_name(s)</i>
<br>FROM <i>table1</i>
<br>INNER JOIN <i>table2</i> ON <i>table1.column_name</i> = <i>table2.column_name</i
<br><br>or
<br><br>SELECT <i>column_name(s)</i>
<br>FROM <i>table1</i>
<br>JOIN <i>table2</i> ON <i>table1.column_name</i> = <i>table2.column_name</i>;
</h4>" + patternTable + "</p></body>";
            task.Points.Add(new Xna.Point(4, 2));
            task.VisibleInformation.Add("isendangered");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 4: Task 2</h2><h4>Select all mammalia.</h4></body>";
            task.Info = head + "<body><h3>Task: Select all mammalia.</h3><h4>Mammalia is a class of animals. You can get information about it from 'Classes' table." + patternTable + "</h4></body>";
            task.Points.Add(new Xna.Point(3, 4));
            task.Points.Add(new Xna.Point(0, 2));
            task.Points.Add(new Xna.Point(1, 1));
            task.Points.Add(new Xna.Point(4, 6));
            task.Points.Add(new Xna.Point(5, 5));
            task.Points.Add(new Xna.Point(6, 1));
            task.Points.Add(new Xna.Point(4, 2));
            task.VisibleInformation.Add("classname");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 4: Task 3</h2><h4>Select all omnivore mammalia.</h4></body>";
            task.Info = head + "<body><h3>Task: Select all omnivore mammalia.</h3><h4>Animals are devided into tree types: Herbivore, Carnivore, Omnivore. You can get information about it from 'Feeding' table.</h4></body>";
            task.Points.Add(new Xna.Point(3, 4));
            task.Points.Add(new Xna.Point(1, 1));
            task.Points.Add(new Xna.Point(4, 6));
            task.Points.Add(new Xna.Point(6, 1));
            task.Points.Add(new Xna.Point(4, 2));
            task.VisibleInformation.Add("FeedingName");
            task.VisibleInformation.Add("classname");
            task.Save(String.Format(path, task.Number));

            #endregion

            #region "Task"

            task = new Task();
            task.Number = i++;
            task.Text = "<head/><body><h2>Level 4: Task 4</h2><h4>Select 2 the heaviest omnivore mammalia.</h4></body>";
            task.Info = "<head/><body><h3>Task: Select 2 the heaviest omnivore mammalia.</h3><h4>This is the last task ;)</h4></body>";
            task.Points.Add(new Xna.Point(4, 6));
            task.Points.Add(new Xna.Point(4, 2));
            task.VisibleInformation.Add("Weight");
            task.VisibleInformation.Add("FeedingName");
            task.VisibleInformation.Add("classname");
            task.Save(String.Format(path, task.Number));

            #endregion

            #endregion
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //CreateLevels();

            _game = new SQLGame(pnlCanvas.Handle, pnlCanvas.ClientSize.Width, pnlCanvas.ClientSize.Height);
            _game.StratMainLoop();
            _game.StateUpdated += GameStateUpdated;
            WindowState = FormWindowState.Maximized;

            NewGame();
        }

        private void NewGame()
        {
            _timeStart = DateTime.Now;

            FindLevels();
            UpdateLevelText();
            StartLevel();
        }

        protected override void OnClosed(EventArgs e)
        {
            _game.StopMainLoop();
            _game.Dispose();

            base.OnClosed(e);
        }

        private void NextLevel()
        {
            if (++_currentLevel >= _levels.Count)
                _currentLevel = 0;

            UpdateLevelText();
            StartLevel();
        }

        private void GameStateUpdated(object sender, EventArgs e)
        {
            btnRun.Invoke(new Action(() =>
            {
                btnRun.Enabled = btnNext.Enabled = btnPrev.Enabled = !_game.IsFinalAnimActivated;

                if (btnRun.Enabled)
                {
                    if (_currentLevel == _levels.Count - 1)
                    {
                        int sum = 0;

                        for (int i = 0; i < _levels.Count; i++)
                            sum += _levels[i].Score;

                        string text = String.Format("You have finished the game! Rememver this data:{0}Your total score: {1} star(s).{0}Your time: {2} minute(s).{0}Restart?", Environment.NewLine, (int)Math.Round((double)sum / _levels.Count), Math.Round((DateTime.Now - _timeStart).TotalMinutes));

                        MessageBox.Show(this, text, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        NewGame();
                    }
                    else
                    {
                        MessageBox.Show(this, "You are winner! Your score: " + _game.Level.Score + " star(s). Next Level?", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _game.Level.IsFinished = true;
                        tbcManager.SelectTab(0);
                        NextLevel();
                    }
                }
            }));
        }

        private void StartLevel()
        {
            tbSqlEditor.Clear();
            dgvResult.DataSource = null;
            _game.Level = _levels[_currentLevel];
            _game.Level.Score = 5;

            string columns = "EntityId";
            List<string> visibleInformation = _game.Level.VisibleInformation;

            for (int i = 0; i < visibleInformation.Count; i++)
                columns += ", " + visibleInformation[i];

            DataTable tableVisibleInfo = SelectData(
                "select distinct " + columns +
                @" from (
                select EntityId, 
                'X:' + convert(nvarchar, X) as X, 
                'Y:' + convert(nvarchar, Y) as Y, 
                convert(nvarchar, Weight) + ' kg' as Weight, 
                convert(nvarchar, Age) + ' year(s)' as Age,
                case when Sex = 1 then 'Male' else 'Female' end as Sex, 
                s.Name as SpeciesName, 
                case when IsEndangered = 1 then 'Yes' else 'No' end as IsEndangered, 
                c.Name as ClassName,
                f.Name as FeedingName
                from Entities as e
                inner join Species as s on e.SpeciesId = s.SpeciesId
                inner join Classes as c on s.ClassId = c.ClassId
                inner join Feeding as f on f.FeedingId = s.FeedingId) as t");

            _game.UpdateMap(SelectData("select * from Entities"), tableVisibleInfo);
        }

        private void UpdateLevelText()
        {
            if (_currentLevel >= 0 && _currentLevel < _levels.Count)
            {
                wbTask.DocumentText = _levels[_currentLevel].Text;
                wbInfo.DocumentText = _levels[_currentLevel].Info;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_currentLevel > 0)
                _currentLevel--;

            UpdateLevelText();
            StartLevel();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (CanGoToNextLevel)
                NextLevel();
        }

        private void wbInfo_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            int i = 1;
            bool isNotEnd;

            do
            {
                isNotEnd = false;
                var el = wbInfo.Document.GetElementById("link" + i++);

                if (el != null)
                {
                    el.Click += new HtmlElementEventHandler(LinkClick);
                    isNotEnd = true;
                }

            } while (isNotEnd);
        }
    }
}
