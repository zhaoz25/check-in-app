using CheckInApp.controller;
using CheckInApp.messages;
using CheckInApp.models;
using CheckInApp.views;
using SharedCheckInApp;
using SharedCheckInApp.messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace CheckInApp
{
    public partial class MainPage : ContentPage
    {
        private bool isEmailValid = false;
        public static bool check = true;


        public MainPage()
        {
            InitializeComponent();

            
            // nếu có username lưu trong bộ nhớ máy thì in lên label
            if (Application.Current.Properties.ContainsKey("username") == true)
            {
                etEmail.Text = Application.Current.Properties["username"] as string;
                etPass.Text = "";
            }
            else
            {
                etEmail.Text = "";
                etPass.Text = "";
            }
            // tạo sự kiện clicked cho label forget pass
            labelForget.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnLabelClicked()),
            });

        }

        protected async override void OnAppearing()
        {

            if(check == true)
            {
                check = false;
                // nếu có lưu tài khoản trong bộ nhớ thì chuyển sang view info
                if (Application.Current.Properties.ContainsKey("username") == true
                && Application.Current.Properties.ContainsKey("password") == true)
                {
                    var username = Application.Current.Properties["username"] as string;
                    var password = Application.Current.Properties["password"] as string;

                    await Navigation.PushAsync(new EmployeeInfoView(username, password));
                }
            }
        }

        private async void OnLabelClicked()
        {
            await Navigation.PushAsync(new ForgetPassView());
        }

        async void btnLogin_Clicked(object sender, EventArgs e)
        {
            // kiểm tra định dạng email
            if(isEmailValid == false)
            {
                etEmail.Focus();
                return;
            }
            // kiểm tra nhập mật khẩu
            if(etPass.Text.Equals("") == true)
            {
                labelError.Text = "Hãy nhập mật khẩu";
                labelError.TextColor = Color.Red;

                etPass.Focus();
                return;
            }

            UserHandler handler = new UserHandler();
            // ghi username , password vào lớp LoginInfo
            LoginInfo info = new LoginInfo();
            info.username = etEmail.Text;
            info.password = etPass.Text;
            try
            {
                // tạo dialog loading
                loadingProgress.IsRunning = true;
                // ẩn các button
                btnLogin.IsVisible = false;
                labelForget.IsVisible = false;

                // gọi phương thức login sau đó trả kq cho lớp result
                var result = await handler.PostLogin(info);
                // kết thức loading
                loadingProgress.IsRunning = false;
                btnLogin.IsVisible = true;
                labelForget.IsVisible = true;
                // nếu sai mật khẩu
                if (result.error == true)
                {
                    await DisplayAlert("Lỗi đăng nhập", "Tài khoản hoặc mật khẩu không đúng !", "OK");
                    etPass.Text = "";
                }
                else
                {
                    // xóa password trong bộ nhớ
                    if (Application.Current.Properties.ContainsKey("password") == true)
                    {
                        Application.Current.Properties.Remove("password");
                    }
                    //xóa tài khoản và token trong bộ nhớ
                    if (Application.Current.Properties.ContainsKey("username") == true)
                    {
                        Application.Current.Properties.Remove("username");
                        Application.Current.Properties.Remove(info.username);
                    }
                    // lưu username và token vào bộ nhớ
                    Application.Current.Properties["username"] = info.username;
                    Application.Current.Properties["password"] = info.password;
                    Application.Current.Properties[info.username] = result.token;
                    System.Diagnostics.Debug.WriteLine(Application.Current.Properties["password"]);
                    etPass.Text = "";
                    await Navigation.PushAsync(new EmployeeInfoView(info.username,info.password));
                }
                
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        private void etEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            // kiểm tra định dạng email
            if(etEmail.Text.IndexOf("@") == -1 || etEmail.Text.IndexOf(".") == -1)
            {
                labelError.Text = "Địa chỉ email không hợp lệ";
                labelError.TextColor = Color.Red;
                isEmailValid = false;
            }
            else if (etEmail.Text.Length < 6)
            {
                labelError.Text = "Địa chỉ email không hợp lệ";
                labelError.TextColor = Color.Red;
                isEmailValid = false;
            }
            else
            {
                labelError.Text = " ";
                isEmailValid = true;
            }
            
        }
        
    }
}
