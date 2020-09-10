using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    public struct PolylinePoint
    {
        public float X { get; internal set; }
        
        public float Y { get; internal set; }
        
        public IEnumerable<float> Value => new[] {X, Y};
        
        private PolylinePoint(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public static PolylinePoint Create(float x, float y)
        {
            return new PolylinePoint(x: x, y: y);
        }
        
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}