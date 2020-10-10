namespace SciterCore
{
    public static class SciterTextExtensions
    {
        public static SciterValue ToValue(this SciterText sciterText)
        {
            return sciterText?.ToValueInternal();
        }

        public static bool TryToValue(this SciterText sciterText, out SciterValue sciterValue)
        {
            sciterValue = default;
            return sciterText?.TryToValueInternal(sciterValue: out sciterValue) == true;
        }

        public static TextMetrics GetMetrics(this SciterText sciterText)
        {
            return sciterText?.GetMetricsInternal() ?? default;
        }

        public static bool TryGetMetrics(this SciterText sciterText, out TextMetrics textMetrics)
        {
            textMetrics = default;
            return sciterText?.TryGetMetricsInternal(textMetrics: out textMetrics) == true;
        }
		
        public static void SetBox(this SciterText sciterText, float width, float height)
        {
            sciterText?.SetBoxInternal(width: width, height: height);
        }
		
        public static bool TrySetBox(this SciterText sciterText, float width, float height)
        {
            return sciterText?.TrySetBoxInternal(width: width, height: height) == true;
        }
    }
}