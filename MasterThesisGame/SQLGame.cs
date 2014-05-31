using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hig.GameEngine;
using Hig.GameEngine.GameObjects;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Hig.GameEngine.Graphics;
using Microsoft.Xna.Framework;
using System.Data;
using GDI = System.Drawing;
using System.Drawing.Imaging;

namespace MasterThesisGame
{
    public class SQLGame : Game
    {
        private struct MapCell
        {
            public Cell Cell;
            public Point Point;
        }

        private const float _scale = 1.0f;

        private readonly string[] _texturesNames = { "Parrot", "Monkey", "Elephant", "Shark", "Panda" };
        private readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private readonly ushort _mapWidth = (ushort)(Cell.Width * 2 * _scale);
        private readonly ushort _mapHeight = (ushort)(Cell.Height * _scale);

        private SpriteBatch _spriteBatch;
        private PrimitiveBatch _primitiveBatch;

        private int _scrX = 210;
        private int _scrY = 200;
        private int _currentWp = 0;

        private Map _map;
        private Unit _ufo;
        private Position _ufoStartPos = new Position(-30, -400);
        public Task Level { get; set; }

        private bool _isFinalAnimActivated = false;
        public bool IsFinalAnimActivated
        {
            get { return _isFinalAnimActivated; }

            set
            {
                if (_isFinalAnimActivated != value)
                {
                    _isFinalAnimActivated = value;
                    OnStateUpdated();
                }
            }
        }

        private Waypoint[] _wps = new Waypoint[]{
            new Waypoint() { Position = new Position(-30, -80), Radius = 10 },
            new Waypoint() { Position = new Position(-30, 400), Radius = 10 }
        };

        public event EventHandler StateUpdated;

        public SQLGame(IntPtr canvasHandle, int backBufferWidth, int backBufferHeight)
            : base(canvasHandle, backBufferWidth, backBufferHeight)
        {
            Initialize();
        }

        private void OnStateUpdated()
        {
            if (StateUpdated != null)
                StateUpdated(this, new EventArgs());
        }

        private Texture2D LoadTexture(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
                return Texture2D.FromStream(_graphicsDevice, fs);
        }

        private void Initialize()
        {
            _map = new Map(8, 8, new Animation(_textures["Ground"], 64, 32, 0, Color.White), Color.Green);
            _map[0, 7].Color = _map[0, 6].Color = _map[0, 5].Color = _map[0, 4].Color = _map[1, 7].Color = _map[1, 6].Color = _map[1, 5].Color = _map[2, 7].Color = _map[2, 6].Color = Color.Blue;
        }

        protected override void LoadContent()
        {
            _textures.Clear();
            _textures.Add("Ground", LoadTexture(@"Content\Ground.png"));
            _textures.Add("Monkey", LoadTexture(@"Content\Monkey.png"));
            _textures.Add("Parrot", LoadTexture(@"Content\Parrot.png"));
            _textures.Add("Elephant", LoadTexture(@"Content\Elephant.png"));
            _textures.Add("Shark", LoadTexture(@"Content\Shark.png"));
            _textures.Add("Panda", LoadTexture(@"Content\Panda.png"));
            _textures.Add("UFO", LoadTexture(@"Content\UFO.png"));
            _textures.Add("Star", LoadTexture(@"Content\Star.png"));

            if (_map != null)
                _map.Animation.Texture = _textures["Ground"];

            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _primitiveBatch = new PrimitiveBatch(_graphicsDevice);

            _ufo = new Unit(new Animation(_textures["UFO"], 128, 128, 0, Color.White), new Position(_ufoStartPos.X, _ufoStartPos.Y), 60);
            _ufo.Angle = 90;
        }

        protected override void UnloadContent()
        {
            if (_primitiveBatch != null)
                _primitiveBatch.Dispose();

            if (_spriteBatch != null)
                _spriteBatch.Dispose();

            foreach (var item in _textures)
                item.Value.Dispose();
        }

        protected override void Update(uint milliseconds)
        {
            if (IsFinalAnimActivated)
            {
                if (_wps[_currentWp].Check(_ufo.Position))
                {
                    // UFO is under the map.
                    if (_currentWp == 0)
                    {
                        bool check = true;
                        byte step = 5;

                        foreach (var point in Level.Points)
                        {
                            Cell cell = _map[point.X, point.Y];

                            if (cell.Animal != null)
                            {
                                check = false;

                                if (cell.Animal.Color.A >= step)
                                    cell.Animal.Color.A -= step;
                                else
                                    cell.Animal = null;
                            }
                        }

                        if (check)
                        {
                            _map.ClearColor();
                            _currentWp = 1;
                        }
                    }
                    else
                    {
                        // Finish animation and return the UFO to start point.
                        IsFinalAnimActivated = false;
                        _currentWp = 0;
                        _ufo.Position = new Position(_ufoStartPos.X, _ufoStartPos.Y);
                    }
                }
                else
                {
                    _ufo.Move(milliseconds);
                }
            }
        }

        protected override void Draw(uint milliseconds)
        {
            _graphicsDevice.Clear(Color.SteelBlue);

            _spriteBatch.Begin();

            for (int y = 0; y < _map.Height; y++)
            {
                for (int x = 0; x < _map.Width; x++)
                {
                    Cell cell = _map[x, y];
                    Position pos = Position.ConvertToIsometric(x * _mapWidth, y * _mapHeight);
                    int tx = _scrX + (int)pos.X;
                    int ty = _scrY + (int)pos.Y;

                    _spriteBatch.Draw(
                        cell.Ground,
                        new Rectangle(tx, ty, _mapWidth, _mapHeight),
                        cell.Ground.Frame,
                        cell.Ground.Color);

                    if (cell.Animal != null)
                    {
                        Animation anim = cell.Animal;

                        _spriteBatch.Draw(
                            anim,
                            new Rectangle(tx + anim.DrawOffset.X, ty + anim.DrawOffset.Y, anim.FrameWidth, anim.FrameHeight),
                            anim.Frame,
                            anim.Color);
                    }
                }
            }

            for (int y = 0; y < _map.Height; y++)
            {
                for (int x = 0; x < _map.Width; x++)
                {
                    Cell cell = _map[x, y];

                    if (cell.Animal != null)
                    {
                        Position pos = Position.ConvertToIsometric(x * _mapWidth, y * _mapHeight);
                        int tx = _scrX + (int)pos.X;
                        int ty = _scrY + (int)pos.Y;

                        for (int i = 0; i < cell.Texts.Count; i++)
                        {
                            var tex = cell.Texts[i];

                            _spriteBatch.Draw(tex, new Vector2(tx, ty + 10 + 10 * i), Color.White);
                        }
                    }
                }
            }

            _spriteBatch.Draw(
                    _ufo.Animation.Texture,
                    new Vector2(_scrX + (int)_ufo.Position.X, _scrY + (int)_ufo.Position.Y),
                    _ufo.Animation.Frame,
                    _ufo.Animation.Color);


            if (Level != null)
                for (int i = 0; i < Level.Score; i++)
                    _spriteBatch.Draw(_textures["Star"], new Vector2(10 + 16 * i, 10), Color.White);

            _spriteBatch.End();
        }

        private MapCell? GetCellByRow(DataRow row)
        {
            try
            {
                byte x = Convert.ToByte(row["X"]);
                byte y = Convert.ToByte(row["Y"]);

                return new MapCell() { Cell = _map[x, y], Point = new Point(x, y) };
            }
            catch
            {
                return null;
            }
        }

        public void SelectAnimals(DataTable table)
        {
            _map.ClearColor();

            if (table.Rows.Count > 0 && table.Columns.Contains("X") && table.Columns.Contains("Y"))
            {
                List<Point> points = new List<Point>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    MapCell? mapCell = GetCellByRow(table.Rows[i]);

                    if (mapCell != null)
                    {
                        points.Add(mapCell.Value.Point);
                        mapCell.Value.Cell.Ground.Color = Color.Yellow;
                    }
                }

                bool isWin = true;

                if (Level.Points.Count == points.Count)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (!Level.Points.Contains(points[i]))
                        {
                            isWin = false;
                            break;
                        }
                    }
                }
                else
                {
                    isWin = false;
                }

                if (isWin)
                    IsFinalAnimActivated = true;
            }
        }

        private Texture2D CreateTextTexture(string text, UniversalColor color)
        {
            // Get font
            var font = new GDI.Font("Arial", 9, System.Drawing.FontStyle.Bold);

            // Get text size
            var bitmap = new GDI.Bitmap(1, 1);
            var graphics = GDI.Graphics.FromImage(bitmap);
            var textSize = graphics.MeasureString(text, font);
            bitmap.Dispose();

            // Draw text on bitmap
            using (bitmap = new GDI.Bitmap((int)textSize.Width, (int)textSize.Height))
            {
                using (graphics = GDI.Graphics.FromImage(bitmap))
                {
                    //graphics.Clear(GDI.Color.FromArgb(100, GDI.Color.White));
                    //graphics.FillRectangle(new GDI.SolidBrush(GDI.Color.FromArgb(200, GDI.Color.White)), 0, 0, bitmap.Width, bitmap.Height);
                    graphics.TextRenderingHint = GDI.Text.TextRenderingHint.AntiAliasGridFit;
                    graphics.DrawString(text, font, new System.Drawing.SolidBrush(color), 0f, 0f);

                    using (Stream fs = new MemoryStream())
                    {
                        bitmap.Save(fs, ImageFormat.Png);

                        return Texture2D.FromStream(_graphicsDevice, fs);
                    }
                }
            }
        }

        public void UpdateMap(DataTable table, DataTable tableVisibleInfo)
        {
            if (table.Columns.Contains("SpeciesId") && table.Columns.Contains("X") && table.Columns.Contains("Y"))
            {
                _map.Clear();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    int id = (int)row["SpeciesId"] - 1;
                    int entityId = (int)row["EntityId"];

                    MapCell? mapCell = GetCellByRow(row);

                    if (mapCell != null)
                    {
                        Cell cell = mapCell.Value.Cell;

                        for (int j = 0; j < tableVisibleInfo.Rows.Count; j++)
                        {
                            if ((int)tableVisibleInfo.Rows[j]["EntityId"] == entityId)
                            {
                                object[] columns = tableVisibleInfo.Rows[j].ItemArray;

                                if (columns.Length >= 1)
                                    for (int k = 1; k < columns.Length; k++)
                                        cell.Texts.Add(CreateTextTexture(columns[k].ToString(), Color.Red));

                                break;
                            }
                        }

                        cell.Animal = new Animation(_textures[_texturesNames[id]], 32, 32, 0, new Point(16, -8), Color.White);
                    }
                }
            }
        }
    }
}
