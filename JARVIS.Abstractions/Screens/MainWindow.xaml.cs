using System.Windows;

using Jarvis.ScreenControllers;

namespace Jarvis.Screens
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowScreenController();
            InitializeComponent();
        }
    }
}
