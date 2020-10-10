// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.CompilerServices;

//TODO: Move this to a different file!
[assembly: InternalsVisibleTo("SciterCore.UnitTests")]

namespace SciterCore
{
    public struct TextMetrics
    {
        public float MinWidth { get; internal set; }

        public float MaxWidth { get; internal set; }
        
        public float Height { get; internal set; }
        
        public float Ascent { get; internal set; }
        
        public float Descent { get; internal set; }
        
        public int LineCount { get; internal set; }

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