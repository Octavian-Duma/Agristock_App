using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Agri_Stock_Dashboard.Services;

namespace Agri_Stock_Dashboard.Views
{
    public partial class HomeView : UserControl
    {
        private readonly ApiService _apiService;

        public HomeView()
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
                this.Cursor = Cursors.Wait;

                var taskMachinery = _apiService.GetAllMachinery();  //cereri imediate simultan toate 3 pt ca nu am pus await
                var taskInventory = _apiService.GetAllInventory();
                var taskWarehouses = _apiService.GetAllWarehouses();

                await Task.WhenAll(taskMachinery, taskInventory, taskWarehouses); //asteptam sa le rezolve si le salvam

                var machinery = taskMachinery.Result;
                var inventory = taskInventory.Result;
                var warehouses = taskWarehouses.Result;

                lblTotalMachinery.Text = machinery.Count.ToString();
                lblBrokenMachinery.Text = $"{machinery.Count(m => m.Status == "STRICAT" || m.Status == "IN_REPARATIE")} cu probleme"; //utilaje
                lblTotalStock.Text = $"{inventory.Sum(i => i.CurrentQuantityKg):N0} kg"; //cantitatea totala + afisaj mai frumos
                lblWarehouseCount.Text = $"în {warehouses.Count} depozite";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare date: {ex.Message}");
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}