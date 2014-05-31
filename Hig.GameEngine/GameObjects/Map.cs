namespace Hig.GameEngine.GameObjects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    using Hig.GameEngine.Graphics;

    public sealed class Map
    {
        private readonly Cell[][] _cells;
        private readonly Dictionary<IGameObject, Point> _objects = new Dictionary<IGameObject, Point>();

        private readonly ushort _countCellsX;
        private readonly ushort _countCellsY;

        public ushort Width { get; private set; }
        public ushort Height { get; private set; }

        public Animation Animation { get; private set; }

        public Cell this[int x, int y]
        {
            get { return _cells[x][y]; }
            set { _cells[x][y] = value; }
        }

        public Map(ushort width, ushort height, Animation animation, Color color)
        {
            Width = width;
            Height = height;
            Animation = animation;
            _countCellsX = (ushort)(Width / Cell.Width);
            _countCellsY = (ushort)(Height / Cell.Height);

            _cells = new Cell[width][];

            for (int x = 0; x < Width; x++)
            {
                _cells[x] = new Cell[Height];

                for (int y = 0; y < Height; y++)
                    _cells[x][y] = new Cell() { Ground = new Animation(animation.Texture, animation.FrameWidth, animation.FrameHeight, animation.Speed, animation.DrawOffset, animation.Color, animation.IsLoop), Color = color };
            }
        }

        public void ClearColor()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _cells[x][y].Ground.Color = _cells[x][y].Color;
        }

        public void ClearAnimals()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _cells[x][y] = new Cell()
                    {
                        Ground = new Animation(Animation.Texture, Animation.FrameWidth, Animation.FrameHeight, Animation.Speed, Animation.DrawOffset, Animation.Color, Animation.IsLoop),
                        Animal = null
                    };
        }

        public void Clear()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cell cell = _cells[x][y];

                    cell.Ground.Color = cell.Color;
                    cell.Animal = null;
                    cell.Texts.Clear();
                }
            }
        }

        public Point? GetPosition(IGameObject gameObject)
        {
            if (_objects.ContainsKey(gameObject))
                return _objects[gameObject];

            return null;
        }

        public bool RegistrObject(IGameObject gameObject)
        {
            if (gameObject != null)
            {
                int mapX = gameObject.Position.X / Cell.Width;
                int mapY = gameObject.Position.Y / Cell.Height;

                if (mapX >= 0 && mapX < _countCellsX && mapY >= 0 && mapY < _countCellsY)
                {
                    Point mapPos = new Point(mapX, mapY);

                    if (_objects.ContainsKey(gameObject))
                        _objects[gameObject] = mapPos;
                    else
                        _objects.Add(gameObject, mapPos);

                    return true;
                }
            }

            return false;
        }

        public bool UnregistrObject(IGameObject gameObject)
        {
            if (_objects.ContainsKey(gameObject))
            {
                _objects.Remove(gameObject);

                return true;
            }

            return false;
        }
    }
}
