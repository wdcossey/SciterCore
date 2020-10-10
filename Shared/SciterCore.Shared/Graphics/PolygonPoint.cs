using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    public struct PolygonPoint
    {
        public float X { get; internal set;}
        
        public float Y { get; internal set;}

        public IEnumerable<float> Value => new[] {X, Y};
        
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