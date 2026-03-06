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
    public partial class MachineryView : UserControl
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Machinery> _allMachinery = new List<Machinery>();
        private List<Warehouse> _allWarehouses = new List<Warehouse>();
        private Machinery _selectedMachineryToEdit;

        public MachineryView()
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
                var taskMachinery = _apiService.GetAllMachinery();
                var taskWarehouses = _apiService.GetAllWarehouses();

                await Task.WhenAll(taskMachinery, taskWarehouses);

                _allMachinery = taskMachinery.Result;
                _allWarehouses = taskWarehouses.Result;

                var whList = new List<Warehouse> { new Warehouse { Id = -1, Name = "TOATE LOCAȚIILE" } }; //artificial introducem id=-1  un fel de reset pentru a le vedea pe toate
                whList.AddRange(_allWarehouses);

                comboMachineryWarehouse.ItemsSource = whList;
                if (comboMachineryWarehouse.SelectedIndex == -1) comboMachineryWarehouse.SelectedIndex = 0;

                if (comboMachineryStatus.SelectedIndex == -1) comboMachineryStatus.SelectedIndex = 0;

                comboEditMachWarehouse.ItemsSource = _allWarehouses;

                FilterMachinery();
            }
            catch (Exception ex) { MessageBox.Show($"Eroare date: {ex.Message}"); }
        }

        private void Filter(object sender, object e) => FilterMachinery();

        private void btnClearFilters(object sender, RoutedEventArgs e)
        {
            txtMachinerySearch.Text = "";
            comboMachineryWarehouse.SelectedIndex = 0;
            comboMachineryStatus.SelectedIndex = 0;
            comboMachinerySort.SelectedIndex = 0;
            FilterMachinery();
        }

        private void FilterMachinery() //filtrare dupa campuri
        {
            if (gridMachineryReport == null) return;
            var result = _allMachinery.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtMachinerySearch.Text))
                result = result.Where(m => m.Name.ToLower().Contains(txtMachinerySearch.Text.ToLower()) ||
                                          (m.StatusDescription != null && m.StatusDescription.ToLower().Contains(txtMachinerySearch.Text.ToLower())));

            if (comboMachineryWarehouse.SelectedItem is Warehouse wh && wh.Id != -1)
                result = result.Where(m => m.Warehouse != null && m.Warehouse.Id == wh.Id);

            if (comboMachineryStatus.SelectedItem is ComboBoxItem item && item.Content.ToString() != "TOATE")
                result = result.Where(m => m.Status.Equals(item.Content.ToString(), StringComparison.OrdinalIgnoreCase));

            if (comboMachinerySort != null && comboMachinerySort.SelectedItem is ComboBoxItem sortItem)
            {
                string tag = sortItem.Tag?.ToString();
                if (tag == "name_asc") result = result.OrderBy(m => m.Name);
                else if (tag == "loc_asc") result = result.OrderBy(m => m.Warehouse != null ? m.Warehouse.Name : "");
            }

            gridMachineryReport.ItemsSource = result.ToList();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id && MessageBox.Show("Sigur ștergi?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                if (await _apiService.DeleteMachinery(id)) await RefreshAllData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Machinery m)
            {
                _selectedMachineryToEdit = m;
                txtEditName.Text = m.Name;
                txtEditDesc.Text = m.StatusDescription;

                foreach (ComboBoxItem item in comboEditStatus.Items)
                    if (item.Content.ToString() == m.Status) comboEditStatus.SelectedItem = item;

                comboEditMachWarehouse.SelectedItem = _allWarehouses.FirstOrDefault(w => w.Id == m.Warehouse.Id);

                overlayEdit.Visibility = Visibility.Visible;
            }
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e) => overlayEdit.Visibility = Visibility.Collapsed;

        private async void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMachineryToEdit == null) return;

            string newStatus = (comboEditStatus.SelectedItem as ComboBoxItem).Content.ToString();
            string newDesc = txtEditDesc.Text;

            int newWhId = (comboEditMachWarehouse.SelectedItem as Warehouse).Id;

            bool success = await _apiService.UpdateMachinery(_selectedMachineryToEdit.Id, newStatus, newDesc, newWhId);
            if (success)
            {
                overlayEdit.Visibility = Visibility.Collapsed;
                await RefreshAllData();
                MessageBox.Show("Utilaj actualizat!");
            }
            else
            {
                MessageBox.Show("Eroare la actualizare.");
            }
        }
    }
}