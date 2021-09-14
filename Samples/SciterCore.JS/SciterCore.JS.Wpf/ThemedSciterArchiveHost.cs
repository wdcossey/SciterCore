using Windows.UI.ViewManagement;
using SciterCore.Interop;

namespace SciterCore.JS.Wpf
{
    public class ThemedSciterArchiveHost : SciterArchiveHost
    {
        private readonly UISettings _uiSettings;

        public ThemedSciterArchiveHost()
        {
            _uiSettings = new Windows.UI.ViewManagement.UISettings();

            _uiSettings.ColorValuesChanged += (sender, args) =>
            {
                var frame = this.RootElement.SelectFirst("frame#content");
                this.EvalScript(@"var frame = document.$(""frame""); frame.frame.loadFile(frame.frame.document.url());");
            };
        }

        protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
        {
            //watcher://theme/windows.css
            if (args.Uri.Scheme.Equals("watcher") && args.Uri.Host.Equals("theme") && args.Uri.PathAndQuery.Equals("/windows.css"))
            {
                var cssBytes = GenerateThemeContent(args);
                Sciter.SciterApi.SciterDataReady(Window.Handle, args.Uri.ToString(), cssBytes, (uint) cssBytes.Length);
                return LoadResult.Ok;
            }

            return base.OnLoadData(sender, args);
        }

        private byte[] GenerateThemeContent(LoadDataArgs args)
        {
            var accentColor = _uiSettings.GetColorValue(UIColorType.Accent);
            var backgroundColor = _uiSettings.GetColorValue(UIColorType.Background);
            var foregroundColor = _uiSettings.GetColorValue(UIColorType.Foreground);
            var accentLight2Color = _uiSettings.GetColorValue(UIColorType.AccentLight2);

            SciterColor sciterAccentColor =
                SciterColor.Create(accentColor.R, accentColor.G, accentColor.B, accentColor.A);
            SciterColor sciterBackgroundColor = SciterColor.Create(backgroundColor.R, backgroundColor.G,
                backgroundColor.B, backgroundColor.A);
            SciterColor sciterForeground = SciterColor.Create(foregroundColor.R, foregroundColor.G,
                foregroundColor.B, foregroundColor.A);
            SciterColor sciterAccentLight2 = SciterColor.Create(accentLight2Color.R, accentLight2Color.G,
                accentLight2Color.B, accentLight2Color.A);

            var themeCss =
                @$" html:theme(system) {{
    var(accent-color): {sciterAccentColor.ToShortHtmlColor()};
    var(main-bg-color): {sciterBackgroundColor.ToShortHtmlColor()};
    var(main-color): {sciterForeground.ToShortHtmlColor()};

    var(card-bg-color): morph(color(main-color), opacity: 7.5%);
    var(card-bd-color): morph(color(main-color), opacity: 50%);
    var(card-bd-width): 2dip;

    var(header-bg-color): color(accent-color);

    var(toolbar-bg-color): morph(color(main-color), opacity: 10%);

    var(menu-main-color): #D0D2D6;

    var(menu-color): morph(rgb(255,255,255), opacity: 85%);
    var(menu-bg-color): morph(#262930, darken: 4%);

    var(menu-h-color): morph(color(menu-color), lighten: 10%);
    var(menu-h-bg-color): morph(color(accent-color), opacity: 75%);
}}

card:theme(system) {{
    border: transparent;
    box-shadow: none;
}}

card:hover:theme(system) {{
    box-shadow: none;
}}

cardheader:theme(system) > div:nth-child(2) {{
    color: {sciterAccentLight2.ToShortHtmlColor()};
    opacity: 1;
}}

";
            return System.Text.Encoding.UTF8.GetBytes(themeCss);
        }
    }
}