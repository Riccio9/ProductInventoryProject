using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TestSistemi
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent(); // Carica il XAML
            ViewModel = new MainViewModel(); // Inizializza il ViewModel
            this.DataContext = ViewModel; // Associa il ViewModel al DataContext
        }

        public void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddProduct();
        }

        public void UpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateProduct();
        }

        public void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteProduct();
        }

      
    }

}