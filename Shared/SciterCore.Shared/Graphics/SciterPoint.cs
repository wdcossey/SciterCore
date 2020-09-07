
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace SciterCore
{
    public struct SciterPoint
    {
        public static readonly SciterSize Empty = new SciterSize();

        public SciterPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int X { get; set; }

        public int Y { get; set; }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}