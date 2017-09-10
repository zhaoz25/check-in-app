using CheckInApp.controller;
using CheckInApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckInApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogChartView : ContentPage
    {

        string username = "";
        List<int> monthList;
        int indexPicker1 = -1;
        int indexPicker2 = -1;

        public LogChartView(string username)
        {
            InitializeComponent();

            this.username = username;
        }

        protected async override void OnAppearing()
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(username) == true)
            {
                var token = Application.Current.Properties[username] as string;
                // lấy tất cả tháng có chấm công hiển thị lên picker
                monthList = new List<int>();
                List<string> pickerSource = new List<string>();
                UserHandler handler = new UserHandler();

                monthList = await handler.GetMonth(token);
                for (int i = 0; i < monthList.Count; i++)
                {
                    pickerSource.Add("Tháng " + monthList[i]);
                }

                picker.ItemsSource = pickerSource;
            }
        }

        private async Task loadingChart()
        {
            // nếu picker 1 và picker đều đã chọn
            if (indexPicker1 >= 0 && indexPicker2 >= 0)
            {
                // lấy token lưu trong bộ nhớ
                if (Application.Current.Properties.ContainsKey(username) == true)
                {
                    var token = Application.Current.Properties[username] as string;

                    var chart = new OxyExData();
                    bool check;
                    // hiển thị loading progress
                    handlerLoading(true);
                    // nếu chọn biểu đồ cột
                    if (indexPicker2 == 0)
                    {
                        check = await chart.CreateBarChart(false, "Biểu đồ chấm công", token, monthList[indexPicker1]);
                    }
                    else
                    {
                        check = await chart.CreatePieChart("Biểu đồ chấm công", token, monthList[indexPicker1]);
                    }
                    handlerLoading(false);

                    if (check == true)
                    {
                        BindingContext = chart;
                    }
                    else
                    {
                        await DisplayAlert("Lỗi hiển thị biểu đồ", "Đã xảy ra lổi. Hãy thử lại sau", "OK");
                    }
                }
            }
        }

        private async void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                indexPicker1 = selectedIndex;
                await loadingChart();
            }
        }

        private void handlerLoading(bool running)
        {
            if(running == true)
            {
                chart.IsEnabled = false;
                loadingProgress.IsEnabled = true;
                loadingProgress.IsRunning = true;
            }
            else
            {
                chart.IsEnabled = true;
                loadingProgress.IsEnabled = false;
                loadingProgress.IsRunning = false;
            }
        }

        private async void picker2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                indexPicker2 = selectedIndex;
                await loadingChart();
            }
        }
    }
}