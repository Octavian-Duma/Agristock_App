using System.Windows;
using System.Windows.Controls;
using Agri_Stock_Dashboard.Views;

namespace Agri_Stock_Dashboard
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                
                viewHome.Visibility = Visibility.Collapsed;
                viewLocations.Visibility = Visibility.Collapsed;
                viewMachinery.Visibility = Visibility.Collapsed;
                viewStock.Visibility = Visibility.Collapsed;
                viewIssues.Visibility = Visibility.Collapsed;
                viewInventoryAlerts.Visibility = Visibility.Collapsed; // Ascundem și noul view
                viewProducts.Visibility = Visibility.Collapsed;
                viewAi.Visibility = Visibility.Collapsed;

               
                if (tag == "HomeView") viewHome.Visibility = Visibility.Visible;
                else if (tag == "LocationsView") viewLocations.Visibility = Visibility.Visible;
                else if (tag == "MachineryView") viewMachinery.Visibility = Visibility.Visible;
                else if (tag == "StockView") viewStock.Visibility = Visibility.Visible;
                else if (tag == "IssuesView") viewIssues.Visibility = Visibility.Visible;
                else if (tag == "InventoryAlertsView") viewInventoryAlerts.Visibility = Visibility.Visible; // Logica nouă
                else if (tag == "ProductsView") viewProducts.Visibility = Visibility.Visible;
                else if (tag == "AiAssistantView") viewAi.Visibility = Visibility.Visible;
            }
        }

        private void Nav_Refresh_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Date au fost actualizate!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}