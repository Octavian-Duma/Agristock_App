using Agri_Stock_Dashboard.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Agri_Stock_Dashboard.Views
{
    public partial class InventoryAlertsView : UserControl
    {
        private readonly ApiService _apiService;

        public InventoryAlertsView()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var allInventory = await _apiService.GetAllInventory();

                gridLowStock.ItemsSource = allInventory
                    .Where(i => i.CurrentQuantityKg < 1000)
                    .OrderBy(i => i.CurrentQuantityKg) // cele mai mici primele
                    .ToList();

                gridHighStock.ItemsSource = allInventory
                    .Where(i => i.CurrentQuantityKg > 10000)
                    .OrderByDescending(i => i.CurrentQuantityKg) // cele mai mari primele
                    .ToList();
            }
            catch
            {
                MessageBox.Show("Nu s-au putut încărca datele.");
            }
        }
    }
}