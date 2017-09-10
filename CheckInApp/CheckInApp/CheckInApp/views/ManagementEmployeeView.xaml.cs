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
    public partial class ManagementEmployeeView : ContentPage
    {
        string username = "";

        public ManagementEmployeeView(string username,string departmentName)
        {
            InitializeComponent();
            this.username = username;

            labelDS.Text = "Danh sách nhân viên " + departmentName;
        }

        protected async override void OnAppearing()
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(username) == true)
            {
                var token = Application.Current.Properties[username] as string;

                UserHandler handler = new UserHandler();
                ResultAndEmployee rs = new ResultAndEmployee();


                rs = await handler.GetEmployeeList(token);

                if (rs.result.error == true)
                {
                    await DisplayAlert("Lỗi đăng nhập", rs.result.errorDetail, "OK");
                    // quay về màn hình trước
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    for(int i = 0; i < rs.employeeInfoList.Count; i++)
                    {
                        rs.employeeInfoList[i].fullName = rs.employeeInfoList[i].getName();
                    }
                    lvEmployee.ItemsSource = rs.employeeInfoList;
                }
            }
            else
            {
                await DisplayAlert("Lỗi đăng nhập", "Hãy đăng nhập lại!", "OK");
                // quay về màn hình trước
                await Navigation.PopToRootAsync();
            }
        }

        private async void lvEmployee_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            var emp = e.SelectedItem as Employee;

            await Navigation.PushAsync(new LogDetailView(this.username, emp));
        }

        private async void btChart_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LogChartView(this.username));
        }
    }
}