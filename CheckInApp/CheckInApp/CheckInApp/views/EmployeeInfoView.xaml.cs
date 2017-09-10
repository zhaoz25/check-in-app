using CheckInApp.controller;
using CheckInApp.messages;
using CheckInApp.models;
using Java.IO;
using Java.Net;
using SharedCheckInApp.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckInApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeInfoView : ContentPage
    {
        private string username = "";
        private string pass = "";
        private Employee emp;

        public EmployeeInfoView(string username,string pass)
        {
            InitializeComponent();
            // lấy tài khoản người dùng
            this.username = username;
            this.pass = pass;
            // gửi message cho android để bắt đầu chạy service
            callService();

        }

        private async Task loadingData(string username)
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(username) == true)
            {
                var token = Application.Current.Properties[username] as string;

                UserHandler handler = new UserHandler();
                ResultAndEmployee rs = new ResultAndEmployee();

                loadingProgress.IsRunning = true;

                rs = await handler.GetEmployeeInfo(token);
                loadingProgress.IsRunning = false;

                if (rs.result.error == true)
                {
                    await DisplayAlert("Lỗi đăng nhập", rs.result.errorDetail, "OK");
                    // quay về màn hình login
                    await Navigation.PopAsync();
                }
                else
                {
                    emp = new Employee();
                    emp = rs.employee;

                    // lấy tên nv từ firstname và lastname
                    rs.employee.fullName = rs.employee.getName();
                    // rút gọn ngày
                    rs.employee.fromDateDay = rs.employee.getFromDate();
                    rs.employee.toDateDay = rs.employee.getToDate();
                    rs.employee.joinedDateDay = rs.employee.getJoinedDate();

                    BindingContext = rs.employee;
                    // lấy tên nv gán vào label
                    labelWelcome.Text = "Welcome " + rs.employee.lastName;
                }
            }
            else
            {
                await DisplayAlert("Lỗi đăng nhập", "Đăng nhập đã hết hạn, hãy đăng nhập lại", "OK");
                // quay về màn hình login
                await Navigation.PopAsync();
            }
        }

        

        private void callService()
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(this.username) == true)
            {

                var token = Application.Current.Properties[this.username] as string;

                var message = new StartTaskGetNotification(token);
                MessagingCenter.Send(message, "StartTask");
            }
        }
                

        protected async override void OnAppearing()
        {
            
            await loadingData(this.username);
        }

        

        private void btnAdvance_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new ManagementEmployeeView(this.username,this.emp.departmentName));
        }

        private async void checkIn_Clicked(object sender, EventArgs e)
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(this.username) == true)
            {
                var token = Application.Current.Properties[this.username] as string;

                Result result = new Result();
                // gán thông tin cho class checkin
                CheckIn checkIn = new CheckIn();
                checkIn.InOutMode = 1; // 1 là check in
                checkIn.SpecifiedMode = 0; // kiểu mặc định
                checkIn.MachineSerial = Android.OS.Build.Serial;// lấy serial thiết bị
                checkIn.token = token;

                UserHandler handler = new UserHandler();
                loadingProgress.IsRunning = true;
                // xử lý check in
                result = await handler.PostCheckIn(checkIn);
                loadingProgress.IsRunning = false;

                if (result.error == true)
                {
                    if (result.errorType == 1)
                    {
                        await DisplayAlert("Lỗi đăng nhập", "Đăng nhập đã hết hạn, hãy đăng nhập lại", "OK");
                        // quay về màn hình login
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Lỗi hệ thống", result.errorDetail, "OK");
                    }
                }
                else
                {
                    
                    await DisplayAlert("Success", "Check In thành công!\n"+"Thời gian:"+result.time, "OK");
                }
            }
            else
            {
                await DisplayAlert("Lỗi đăng nhập", "Đăng nhập đã hết hạn, hãy đăng nhập lại", "OK");
                // quay về màn hình login
                await Navigation.PopAsync();
            }
        }

        private async void checkOut_Clicked(object sender, EventArgs e)
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(this.username) == true)
            {
                var token = Application.Current.Properties[this.username] as string;

                Result result = new Result();
                // gán thông tin cho class checkin
                CheckIn checkIn = new CheckIn();
                checkIn.InOutMode = 2; // 2 là check out
                checkIn.SpecifiedMode = 0; // kiểu mặc định
                checkIn.MachineSerial = Android.OS.Build.Serial;// lấy serial thiết bị
                checkIn.token = token;

                UserHandler handler = new UserHandler();
                loadingProgress.IsRunning = true;
                // xử lý check in
                result = await handler.PostCheckIn(checkIn);
                loadingProgress.IsRunning = false;

                if (result.error == true)
                {
                    await DisplayAlert("Lỗi hệ thống", result.errorDetail, "OK");
                    // quay về màn hình login
                    await Navigation.PopAsync();
                }
                else
                {

                    await DisplayAlert("Success", "Check Out thành công!\n" + "Thời gian:" + result.time, "OK");
                }
            }
            else
            {
                await DisplayAlert("Lỗi đăng nhập", "Đăng nhập đã hết hạn, hãy đăng nhập lại", "OK");
                // quay về màn hình login
                await Navigation.PopAsync();
            }

        }

        private void toolChangePass_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePasswordView(this.username,this.pass));
        }

        private async void toolLogout_Clicked(object sender, EventArgs e)
        {
            // lấy token lưu trong bộ nhớ
            if (Application.Current.Properties.ContainsKey(this.username) == true)
            {
                var token = Application.Current.Properties[this.username] as string;

                Result result = new Result();
                UserHandler handler = new UserHandler();

                loadingProgress.IsRunning = true;
                result = await handler.GetLogout(token);
                loadingProgress.IsRunning = false;
                // xóa password trong bộ nhớ
                if (Application.Current.Properties.ContainsKey("password") == true)
                {
                    Application.Current.Properties.Remove("password");
                }
            }
            //Tắt service
            var message = new StopTaskGetNotification();
            MessagingCenter.Send(message, "StopTask");
            // quay về màn hình login
            await Navigation.PopAsync();

        }
    }
}