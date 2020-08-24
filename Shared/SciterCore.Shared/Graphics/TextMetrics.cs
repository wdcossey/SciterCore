// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    public readonly struct TextMetrics
    {
        public float MinWidth { get; }

        public float MaxWidth { get; }
        
        public float Height { get; }
        
        public float Ascent { get; }
        
        public float Descent { get; }
        
        public int LineCount { get; }

        internal TextMetrics(float minWidth, float maxWidth, float height, float ascent, float descent, int noLines)
        {
            MinWidth = minWidth;
            MaxWidth = maxWidth;
            Height = height;
            Ascent = ascent;
            Descent = descent;
            LineCount = noLines;
        }

        internal static TextMetrics Create(float minWidth, float maxWidth, float height, float ascent, float descent, int noLines)
        {
            return new TextMetrics(minWidth: minWidth, maxWidth: maxWidth, height: height, ascent: ascent, descent: descent, noLines: noLines);
        }
    }
}