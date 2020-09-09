using System.Diagnostics.CodeAnalysis;

namespace SciterCore
{
    public readonly struct PolygonPoint
    {
        public float X { get; }
        
        public float Y { get; }

        public float[] Value => new[] {X, Y};
        
        private PolygonPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static PolygonPoint Create(float x, float y)
        {
            return new PolygonPoint(x: x, y: y);
        }
        
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}