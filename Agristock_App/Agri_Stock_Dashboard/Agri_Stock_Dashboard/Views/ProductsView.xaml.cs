using Agri_Stock_Dashboard;
using Agri_Stock_Dashboard.DTOs;
using Agri_Stock_Dashboard.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Agri_Stock_Dashboard.Views
{
    public partial class ProductsView : UserControl
    {
        private readonly ApiService _apiService;
        private List<InventoryItem> _allInventory = new List<InventoryItem>();

        public ProductsView()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _allInventory = await _apiService.GetAllInventory(); 
                //preluam toate produsele ce am stoc existent,le facem distincte,le ordonam alfabetic si le salvam
                var uniqueProducts = _allInventory
                    .Select(i => i.Product)
                    .GroupBy(p => p.Name)
                    .Select(g => g.First())
                    .OrderBy(p => p.Name)
                    .ToList();

                lstProducts.ItemsSource = uniqueProducts;
                lstProducts.DisplayMemberPath = "Name";
            }
            catch
            {
            }
        }

        private void lstProducts_SelectionChanged(object sender, SelectionChangedEventArgs e) // calcul cantitate totala
        {
            if (lstProducts.SelectedItem is Product p)
            {
                var items = _allInventory.Where(i => i.Product != null && i.Product.Name == p.Name).ToList();
                lblTotalProductQty.Text = $"{items.Sum(i => i.CurrentQuantityKg):N0} kg";
                gridProductDistribution.ItemsSource = items;
            }
        }
    }
}