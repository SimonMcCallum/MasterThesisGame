namespace MasterThesisGame
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tbcManager = new System.Windows.Forms.TabControl();
            this.tpInfo = new System.Windows.Forms.TabPage();
            this.wbInfo = new System.Windows.Forms.WebBrowser();
            this.tpTables = new System.Windows.Forms.TabPage();
            this.picTablesDiagram = new System.Windows.Forms.PictureBox();
            this.tpSql = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRun = new System.Windows.Forms.Button();
            this.tbSqlEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.wbTask = new System.Windows.Forms.WebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tbcManager.SuspendLayout();
            this.tpInfo.SuspendLayout();
            this.tpTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTablesDiagram)).BeginInit();
            this.tpSql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSqlEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.0838F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.9162F));
            this.tableLayoutPanel1.Controls.Add(this.pnlCanvas, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 494F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(895, 494);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(3, 3);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(487, 488);
            this.pnlCanvas.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tbcManager, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnPrev, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnNext, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.wbTask, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(496, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(396, 488);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // tbcManager
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.tbcManager, 3);
            this.tbcManager.Controls.Add(this.tpInfo);
            this.tbcManager.Controls.Add(this.tpTables);
            this.tbcManager.Controls.Add(this.tpSql);
            this.tbcManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcManager.Location = new System.Drawing.Point(3, 103);
            this.tbcManager.Name = "tbcManager";
            this.tbcManager.SelectedIndex = 0;
            this.tbcManager.Size = new System.Drawing.Size(390, 352);
            this.tbcManager.TabIndex = 1;
            // 
            // tpInfo
            // 
            this.tpInfo.Controls.Add(this.wbInfo);
            this.tpInfo.Location = new System.Drawing.Point(4, 22);
            this.tpInfo.Name = "tpInfo";
            this.tpInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpInfo.Size = new System.Drawing.Size(382, 326);
            this.tpInfo.TabIndex = 0;
            this.tpInfo.Text = "Info";
            this.tpInfo.UseVisualStyleBackColor = true;
            // 
            // wbInfo
            // 
            this.wbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbInfo.Location = new System.Drawing.Point(3, 3);
            this.wbInfo.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbInfo.Name = "wbInfo";
            this.wbInfo.Size = new System.Drawing.Size(376, 320);
            this.wbInfo.TabIndex = 0;
            this.wbInfo.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wbInfo_Navigated);
            // 
            // tpTables
            // 
            this.tpTables.Controls.Add(this.picTablesDiagram);
            this.tpTables.Location = new System.Drawing.Point(4, 22);
            this.tpTables.Name = "tpTables";
            this.tpTables.Size = new System.Drawing.Size(382, 326);
            this.tpTables.TabIndex = 2;
            this.tpTables.Text = "Tables";
            this.tpTables.UseVisualStyleBackColor = true;
            // 
            // picTablesDiagram
            // 
            this.picTablesDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picTablesDiagram.Image = global::MasterThesisGame.Properties.Resources.TableDiagram;
            this.picTablesDiagram.Location = new System.Drawing.Point(0, 0);
            this.picTablesDiagram.Name = "picTablesDiagram";
            this.picTablesDiagram.Size = new System.Drawing.Size(382, 326);
            this.picTablesDiagram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTablesDiagram.TabIndex = 0;
            this.picTablesDiagram.TabStop = false;
            // 
            // tpSql
            // 
            this.tpSql.Controls.Add(this.splitContainer1);
            this.tpSql.Location = new System.Drawing.Point(4, 22);
            this.tpSql.Name = "tpSql";
            this.tpSql.Padding = new System.Windows.Forms.Padding(3);
            this.tpSql.Size = new System.Drawing.Size(382, 326);
            this.tpSql.TabIndex = 1;
            this.tpSql.Text = "SQL";
            this.tpSql.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvResult);
            this.splitContainer1.Size = new System.Drawing.Size(376, 320);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbSqlEditor, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(376, 205);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 158);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(370, 44);
            this.panel1.TabIndex = 1;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(284, 10);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbSqlEditor
            // 
            this.tbSqlEditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.tbSqlEditor.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.tbSqlEditor.BackBrush = null;
            this.tbSqlEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSqlEditor.CharHeight = 14;
            this.tbSqlEditor.CharWidth = 8;
            this.tbSqlEditor.CommentPrefix = "--";
            this.tbSqlEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbSqlEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tbSqlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSqlEditor.IsReplaceMode = false;
            this.tbSqlEditor.Language = FastColoredTextBoxNS.Language.SQL;
            this.tbSqlEditor.LeftBracket = '(';
            this.tbSqlEditor.Location = new System.Drawing.Point(3, 3);
            this.tbSqlEditor.Name = "tbSqlEditor";
            this.tbSqlEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.tbSqlEditor.RightBracket = ')';
            this.tbSqlEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tbSqlEditor.Size = new System.Drawing.Size(370, 149);
            this.tbSqlEditor.TabIndex = 2;
            this.tbSqlEditor.Zoom = 100;
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToOrderColumns = true;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(0, 0);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.Size = new System.Drawing.Size(376, 111);
            this.dgvResult.TabIndex = 0;
            // 
            // btnPrev
            // 
            this.btnPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrev.Location = new System.Drawing.Point(3, 461);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(94, 24);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.Location = new System.Drawing.Point(299, 461);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(94, 24);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // wbTask
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.wbTask, 3);
            this.wbTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbTask.Location = new System.Drawing.Point(3, 3);
            this.wbTask.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbTask.Name = "wbTask";
            this.wbTask.ScrollBarsEnabled = false;
            this.wbTask.Size = new System.Drawing.Size(390, 94);
            this.wbTask.TabIndex = 3;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 494);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormMain";
            this.Text = "SQL Game";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tbcManager.ResumeLayout(false);
            this.tpInfo.ResumeLayout(false);
            this.tpTables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTablesDiagram)).EndInit();
            this.tpSql.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbSqlEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.TabControl tbcManager;
        private System.Windows.Forms.TabPage tpInfo;
        private System.Windows.Forms.TabPage tpSql;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TabPage tpTables;
        private System.Windows.Forms.PictureBox picTablesDiagram;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private FastColoredTextBoxNS.FastColoredTextBox tbSqlEditor;
        private System.Windows.Forms.WebBrowser wbInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.WebBrowser wbTask;
    }
}

