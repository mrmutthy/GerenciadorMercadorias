using System.Windows;
using GerenciadorMercadorias.UI.ViewModels;

namespace GerenciadorMercadorias.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}