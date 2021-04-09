using System.Windows;

namespace SciterCore.JS.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SciterPlatform.Initialize();
            SciterPlatform.EnableDragAndDrop();
        }
    }
}