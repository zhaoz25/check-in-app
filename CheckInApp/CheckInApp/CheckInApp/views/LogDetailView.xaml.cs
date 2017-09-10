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
    public partial class LogDetailView : ContentPage
    {

        string username = "";
        Employee emp;
        List<string> pickerSource;
        List<int> monthList;

        public LogDetailView(string username,Employee emp)
        {
            InitializeComponent();

            this.username = username;
            this.emp = emp;

            labelDS.Text = "Nhân viên " + emp.fullName;
            
        }

        protected async override void OnAppearing()
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(username) == true)
            {
                var token = Application.Current.Properties[username] as string;

                UserHandler handler = new UserHandler();
                ResultAndTimeLogList resultAndTimeLogList = new ResultAndTimeLogList();

                loadingProgress.IsRunning = true;
                loadingProgress.IsVisible = true;
                resultAndTimeLogList = await handler.GetLogDetails(token,this.emp.employeeATID);
                loadingProgress.IsRunning = false;
                loadingProgress.IsVisible = false;

                if (resultAndTimeLogList.result.error == true)
                {
                    await DisplayAlert("Lỗi đăng nhập", "Hãy đăng nhập lại", "OK");
                    // quay về màn hình trước
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    for (int i = 0; i < resultAndTimeLogList.TimeLogList.Count; i++)
                    {
                        resultAndTimeLogList.TimeLogList[i].getInOut();
                        resultAndTimeLogList.TimeLogList[i].getTime();
                    }
                    
                    lvLog.ItemsSource = resultAndTimeLogList.TimeLogList;
                    // tạo source cho picker
                    pickerSource = new List<string>();
                    pickerSource.Add("Tất cả");
                    monthList = resultAndTimeLogList.month;

                    for (int j = 0; j < resultAndTimeLogList.month.Count; j++)
                    {
                        pickerSource.Add("Tháng " + resultAndTimeLogList.month[j]);
                    }
                    picker.ItemsSource = pickerSource;

                }
            }
            else
            {
                await DisplayAlert("Lỗi đăng nhập", "Hãy đăng nhập lại", "OK");
                // quay về màn hình trước
                await Navigation.PopToRootAsync();
            }
        }

        private async void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                // lấy token lưu trong bộ nhớ
                if (Application.Current.Properties.ContainsKey(username) == true)
                {
                    var token = Application.Current.Properties[username] as string;

                    UserHandler handler = new UserHandler();
                    ResultAndTimeLogList resultAndTimeLogList = new ResultAndTimeLogList();

                    loadingProgress.IsRunning = true;
                    loadingProgress.IsVisible = true;
                    // nếu tìm tất cả
                    if(selectedIndex == 0)
                    {
                        resultAndTimeLogList = await handler.GetLogDetails(token, this.emp.employeeATID);
                    }
                    else // tìm theo tháng
                    {
                        resultAndTimeLogList = await handler.GetLogDetails(token, this.emp.employeeATID, monthList[selectedIndex-1]);
                    }
                    loadingProgress.IsRunning = false;
                    loadingProgress.IsVisible = false;

                    if (resultAndTimeLogList.result.error == true)
                    {
                        await DisplayAlert("Lỗi đăng nhập", "Hãy đăng nhập lại", "OK");
                        // quay về màn hình trước
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        for (int i = 0; i < resultAndTimeLogList.TimeLogList.Count; i++)
                        {
                            resultAndTimeLogList.TimeLogList[i].getInOut();
                            resultAndTimeLogList.TimeLogList[i].getTime();
                        }

                        lvLog.ItemsSource = resultAndTimeLogList.TimeLogList;
                    }
                }
                else
                {
                    await DisplayAlert("Lỗi đăng nhập", "Hãy đăng nhập lại", "OK");
                    // quay về màn hình trước
                    await Navigation.PopToRootAsync();
                }
            }
        }
    }
}