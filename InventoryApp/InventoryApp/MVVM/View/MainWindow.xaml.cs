using InventoryClient.MVVM.ViewModel; 
using System.Windows;

namespace InventoryApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           
            this.DataContext = new MainViewModel();
        }
    }
}