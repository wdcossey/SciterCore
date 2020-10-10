
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Diagnostics.CodeAnalysis;

namespace SciterCore
{
    public struct SciterPoint
    {
        public static readonly SciterPoint Empty = new SciterPoint(0, 0);

        public SciterPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int X { get; set; }

        public int Y { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}