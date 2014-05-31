namespace Hig.GameEngine.GameObjects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    using Hig.GameEngine.Graphics;

    public class Cell
    {
        public const ushort Width = 32;
        public const ushort Height = 32;

        public Animation Ground { get; set; }
        public Animation Animal { get; set; }
        public Color Color { get; set; }
        //public List<string> Texts { get; protected set; }
        public List<Texture2D> Texts { get; protected set; }

        public Cell()
        {
            Texts = new List<Texture2D>();
        }
    }
}
