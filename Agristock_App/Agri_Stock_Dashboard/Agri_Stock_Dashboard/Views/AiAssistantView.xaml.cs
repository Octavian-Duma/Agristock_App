
using Agri_Stock_Dashboard;
using Agri_Stock_Dashboard.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Agri_Stock_Dashboard.Views
{
    public partial class AiAssistantView : UserControl
    {
        private readonly ApiService _apiService;

        public AiAssistantView()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void btnSendAi_Click(object sender, RoutedEventArgs e) => await HandleAiChat();
        private async void txtChatInput_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) await HandleAiChat(); }

        private async Task HandleAiChat()
        {
            string prompt = txtChatInput.Text;
            if (string.IsNullOrWhiteSpace(prompt)) return;

            txtChatInput.IsEnabled = false;
            btnSendAi.IsEnabled = false;

            txtChatHistory.AppendText($"TU: {prompt}\n");
            txtChatInput.Clear();

            string response = await _apiService.AskAi(prompt);

            txtChatHistory.AppendText($"AI: {response}\n\n");

            txtChatHistory.ScrollToEnd();
            txtChatInput.IsEnabled = true;
            btnSendAi.IsEnabled = true;
            txtChatInput.Focus();
        }
    }
}