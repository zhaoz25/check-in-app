
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckInApp.models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CheckInApp.controller;

namespace CheckInApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordView : ContentPage
    {
        private string username = "";
        private string pass = "";

        public ChangePasswordView(string username,string pass)
        {
            InitializeComponent();

            this.username = username;
            this.pass = pass;

            etMail.Text = username;
            etOldPass.Text = "";
            etNewPass.Text = "";
            etConfirmPass.Text = "";

        }

        private async void btnChangePass_Clicked(object sender, EventArgs e)
        {
            // kiểm tra email đúng định dạng ko?
            if (checkEmail(etMail.Text) == false)
            {
                await DisplayAlert("Lỗi nhập dữ liệu", "Email không hợp lệ", "OK");
                return;
            }
            // kiểm tra các entry có được nhập ko?
            if (checkField(etOldPass.Text) == false || checkField(etNewPass.Text) == false
                || checkField(etConfirmPass.Text) == false)
            {
                await DisplayAlert("Lỗi nhập dữ liệu", "Hãy nhập đầy đủ thông tin", "OK");
                return;
            }
            // kiểm tra mật khẩu và nhập lại mật khẩu giống nhau ko?
            if (etNewPass.Text.Equals(etConfirmPass.Text) == false)
            {
                await DisplayAlert("Lỗi nhập dữ liệu", "Mật khẩu không giống nhau", "OK");
                return;
            }
            // kiểm tra có đúng mật khẩu cũ ko?
            if(etOldPass.Text.Equals(this.pass) == false)
            {
                await DisplayAlert("Lỗi nhập dữ liệu", "Mật khẩu cũ không đúng", "OK");
                return;
            }

            Result result = new Result();
            UserHandler handler = new UserHandler();
            LoginInfo info = new LoginInfo();
            info.username = this.username;
            info.newPassword = etNewPass.Text;

            loadingProgress.IsRunning = true;
            result = await handler.PutUpdatePassword(info);
            loadingProgress.IsRunning = false;

            if(result.error == true)
            {
                await DisplayAlert("Lỗi hệ thống", "Không thể thay đổi mật khẩu", "OK");
            }
            else
            {
                await DisplayAlert("Success", "Đã thay đổi mật khẩu", "OK");
                // quay về màn hình trước
                await Navigation.PopAsync();
            }

        }

        private bool checkEmail(string email)
        {
            // kiểm tra định dạng email
            if (email.IndexOf("@") == -1 || email.IndexOf(".") == -1)
            {
                return false;
            }
            else if (email.Length < 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool checkField(string text)
        {
            if (text.Equals("") == true)
            {
                return false;
            }

            return true;
        }
    }
}