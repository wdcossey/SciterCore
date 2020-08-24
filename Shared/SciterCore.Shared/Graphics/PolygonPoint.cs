namespace SciterCore
{
    public readonly struct PolygonPoint
    {
        public float X { get; }
        
        public float Y { get; }

        private PolygonPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static PolygonPoint Create(float x, float y)
        {
            return new PolygonPoint(x: x, y: y);
        }
    }
}