namespace Hig.GameEngine
{
    using System;

    public class Position
    {
        public int X;
        public int Y;

        public Position(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public int GetDictance(int x, int y)
        {
            return (int)Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2)); 
        }

        public int GetDictance(Position position)
        {
            return GetDictance(position.X, position.Y);
        }

        public Position ToIsometric()
        {
            return ConvertToIsometric(this);
        }

        public Position ToOrthogonal()
        {
            return ConvertToOrthogonal(this);
        }

        public static Position ConvertToIsometric(int x, int y)
        {
            return new Position(x / 2 - y, x / 4 + y / 2);
        }

        public static Position ConvertToIsometric(Position position)
        {
            return ConvertToIsometric(position.X, position.Y);
        }

        public static Position ConvertToOrthogonal(int x, int y)
        {
            return new Position(y - x / 2, x + 2 * y);
        }

        public static Position ConvertToOrthogonal(Position position)
        {
            return ConvertToOrthogonal(position.X, position.Y);
        }
    }
}
