using Agri_Stock_Dashboard; 
using Agri_Stock_Dashboard.DTOs;
using Agri_Stock_Dashboard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Agri_Stock_Dashboard.Views
{
    public partial class StockView : UserControl
    {
        private readonly ApiService _apiService = new ApiService();
        private List<InventoryItem> _allInventory = new List<InventoryItem>();
        private List<Warehouse> _allWarehouses = new List<Warehouse>();
        private InventoryItem _selectedStockToEdit;

        public StockView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAllData();
        }

        private async Task RefreshAllData()
        {
            try
            {
                var taskInventory = _apiService.GetAllInventory();
                var taskWarehouses = _apiService.GetAllWarehouses();

                await Task.WhenAll(taskInventory, taskWarehouses);

                _allInventory = taskInventory.Result;
                _allWarehouses = taskWarehouses.Result;

                var whList = new List<Warehouse> { new Warehouse { Id = -1, Name = "TOATE LOCAȚIILE" } };//inseram artificial categoria la inceput prin id=-1,pt ca in baza de date nu exista
                whList.AddRange(_allWarehouses);
                comboStockWarehouse.ItemsSource = whList;
                if (comboStockWarehouse.SelectedIndex == -1) comboStockWarehouse.SelectedIndex = 0;

                var catList = new List<string> { "TOATE CATEGORIILE" };
                catList.AddRange(_allInventory.Select(i => i.Product.Category).Distinct().OrderBy(c => c));
                comboStockCategory.ItemsSource = catList;
                if (comboStockCategory.SelectedIndex == -1) comboStockCategory.SelectedIndex = 0;

                if (comboStockSort.SelectedIndex == -1) comboStockSort.SelectedIndex = 0;

                FilterStock();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare date: {ex.Message}");
            }
        }

        private void FilterStock_Changed(object sender, object e) => FilterStock();

        private void btnClearStockFilters_Click(object sender, RoutedEventArgs e)
        {
            txtStockSearch.Text = "";
            comboStockCategory.SelectedIndex = 0;
            comboStockWarehouse.SelectedIndex = 0;
            txtMinQty.Text = "";
            txtMaxQty.Text = "";
            comboStockSort.SelectedIndex = 0;
            FilterStock();
        }

        private void FilterStock()  //filtrare
        {
            if (gridStockReport == null) return;
            var result = _allInventory.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtStockSearch.Text))
                result = result.Where(i => i.Product != null && i.Product.Name.ToLower().Contains(txtStockSearch.Text.ToLower()));

            if (comboStockCategory.SelectedItem is string cat && cat != "TOATE CATEGORIILE")
                result = result.Where(i => i.Product != null && i.Product.Category == cat);

            if (comboStockWarehouse.SelectedItem is Warehouse wh && wh.Id != -1)
                result = result.Where(i => i.Warehouse != null && i.Warehouse.Id == wh.Id);

            if (double.TryParse(txtMinQty.Text, out double min))
                result = result.Where(i => i.CurrentQuantityKg >= min);

            if (double.TryParse(txtMaxQty.Text, out double max))
                result = result.Where(i => i.CurrentQuantityKg <= max);

            if (comboStockSort != null && comboStockSort.SelectedItem is ComboBoxItem sortItem)
            {
                string tag = sortItem.Tag?.ToString();
                if (tag == "name_asc") result = result.OrderBy(i => i.Product != null ? i.Product.Name : "");
                else if (tag == "qty_asc") result = result.OrderBy(i => i.CurrentQuantityKg);
                else if (tag == "qty_desc") result = result.OrderByDescending(i => i.CurrentQuantityKg);
            }

            gridStockReport.ItemsSource = result.ToList();
        }

        private async void btnDeleteInventory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id && MessageBox.Show("Sigur ștergi?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                if (await _apiService.DeleteInventory(id)) await RefreshAllData();
        }

        private void btnEditInventory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is InventoryItem i)
            {
                _selectedStockToEdit = i;
                lblEditStockName.Text = i.Product.Name;
                txtEditStockQty.Text = i.CurrentQuantityKg.ToString();
                overlayEditStock.Visibility = Visibility.Visible;
            }
        }

        private void btnCancelEditStock_Click(object sender, RoutedEventArgs e) => overlayEditStock.Visibility = Visibility.Collapsed;

        private async void btnSaveEditStock_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStockToEdit == null) return;
            if (double.TryParse(txtEditStockQty.Text, out double newQty))
            {
                bool success = await _apiService.UpdateInventory(_selectedStockToEdit.Id, newQty);
                if (success)
                {
                    overlayEditStock.Visibility = Visibility.Collapsed;
                    await RefreshAllData();
                    MessageBox.Show("Stoc actualizat!");
                }
                else
                {
                    MessageBox.Show("Eroare la actualizare.");
                }
            }
            else
            {
                MessageBox.Show("Introdu o cantitate validă.");
            }
        }
    }
}