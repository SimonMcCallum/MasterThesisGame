namespace Hig.GameEngine.GameObjects
{
    public class Waypoint
    {
        public Position Position { get; set; }
        public ushort Radius { get; set; }

        public virtual bool Check(Position position)
        {
            return Radius >= Position.GetDictance(position);
        }
    }
}
