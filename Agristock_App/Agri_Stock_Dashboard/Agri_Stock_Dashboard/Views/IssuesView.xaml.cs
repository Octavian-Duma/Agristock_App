using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Agri_Stock_Dashboard.Services;

namespace Agri_Stock_Dashboard.Views
{
    public partial class IssuesView : UserControl
    {
        private readonly ApiService _apiService;

        public IssuesView()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData() //afisam date filtrate
        {
            try
            {
                var allMachinery = await _apiService.GetAllMachinery();  //LINQ (Interogare C# in memorie)/ineficient
                gridIssues.ItemsSource = allMachinery
                    .Where(m => m.Status == "STRICAT" || m.Status == "IN_REPARATIE")
                    .ToList();
            }
            catch
            {
            }
        }
    }
}