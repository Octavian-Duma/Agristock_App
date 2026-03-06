
using Agri_Stock_Dashboard;
using Agri_Stock_Dashboard.DTOs;
using Agri_Stock_Dashboard.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Agri_Stock_Dashboard.Views
{
    public partial class LocationsView : UserControl
    {
        private readonly ApiService _apiService;
        private List<Machinery> _machinery = new List<Machinery>();
        private List<InventoryItem> _inventory = new List<InventoryItem>();

        public LocationsView()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData() //incarcam toata baza de date in memorie
        {
            try
            {
                var taskMachinery = _apiService.GetAllMachinery();
                var taskInventory = _apiService.GetAllInventory();
                var taskWarehouses = _apiService.GetAllWarehouses();

                await Task.WhenAll(taskMachinery, taskInventory, taskWarehouses);

                _machinery = taskMachinery.Result;
                _inventory = taskInventory.Result;
                lstWarehouses.ItemsSource = taskWarehouses.Result;
            }
            catch
            {
            }
        }

        private void lstWarehouses_SelectionChanged(object sender, SelectionChangedEventArgs e)  //filtrari rapide im memorie
        {
            if (lstWarehouses.SelectedItem is Warehouse wh)
            {
                gridLocMachinery.ItemsSource = _machinery.Where(m => m.Warehouse.Id == wh.Id).ToList();
                gridLocInventory.ItemsSource = _inventory.Where(i => i.Warehouse.Id == wh.Id).ToList();
            }
        }
    }
}