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
	public partial class ForgetPassView : ContentPage
	{
		public ForgetPassView ()
		{
			InitializeComponent ();

            etForgetEmail.Text = "";
            etMail.Text = "";
            etCode.Text = "";
            etNewPass.Text = "";
            etConfirmPass.Text = "";


        }

       

        private async void btnSendMail_Clicked(object sender, EventArgs e)
        {
            if(checkEmail(etForgetEmail.Text) == false)
            {
                await DisplayAlert("Không thể gửi email", "Email không hợp lệ", "OK");
                return;
            }

            UserHandler handler = new UserHandler();
            // tạo màn hình loading
            loadingProgress.IsRunning = true;
            // ẩn các button
            btnSendMail.IsVisible = false;
            btnUpdateForgotPass.IsVisible = false;

            Result result = await handler.GetSendEmail(etForgetEmail.Text);
            // kết thúc loading
            loadingProgress.IsRunning = false;
            btnSendMail.IsVisible = true;
            btnUpdateForgotPass.IsVisible = true;

            if (result.error == true)
            {
                await DisplayAlert("Không thể gửi email", result.errorDetail, "OK");
            }
            else
            {
                await DisplayAlert("Success", "Đã gửi code phục hồi mật khẩu. Lấy code trong email và nhập vào phẩn bên dưới để đổi mật khẩu.", "OK");
                etForgetEmail.Text = "";
            }
        }
        
        private async void btnUpdateForgotPass_Clicked(object sender, EventArgs e)
        {
            // kiểm tra email đúng định dạng ko?
            if(checkEmail(etMail.Text) == false)
            {
                await DisplayAlert("Không thể cập nhật mật khẩu", "Email không hợp lệ", "OK");
                return;
            }
            // kiểm tra các entry có được nhập ko?
            if(checkField(etMail.Text) == false || checkField(etCode.Text) == false 
                || checkField(etNewPass.Text) == false || checkField(etConfirmPass.Text) == false)
            {
                await DisplayAlert("Không thể cập nhật mật khẩu", "Hãy nhập đầy đủ thông tin", "OK");
                return;
            }
            // kiểm tra mật khẩu và nhập lại mật khẩu giống nhau ko?
            if(etNewPass.Text.Equals(etConfirmPass.Text) == false)
            {
                await DisplayAlert("Lỗi nhập dữ liệu", "Mật khẩu không giống nhau", "OK");
                return;
            }

            // tạo màn hình loading
            loadingProgress2.IsRunning = true;
            // ẩn các button
            btnSendMail.IsVisible = false;
            btnUpdateForgotPass.IsVisible = false;

            UserHandler handler = new UserHandler();
            // ghi username , password vào lớp LoginInfo
            LoginInfo info = new LoginInfo();
            info.username = etMail.Text;
            info.resetCode = etCode.Text;
            info.newPassword = etNewPass.Text;
            // xử lý update mật khẩu 
            Result result = await handler.PutUpdatePassword(info);
            // kết thúc loading
            loadingProgress2.IsRunning = false;
            btnSendMail.IsVisible = true;
            btnUpdateForgotPass.IsVisible = true;


            // kiểm tra kết quả
            if (result.error == true)
            {
                await DisplayAlert("Không thể cập nhật mật khẩu", result.errorDetail, "OK");
            }
            else
            {
                await DisplayAlert("Succes", "Đã thay đổi mật khẩu!", "OK");
                await Navigation.PushAsync(new MainPage() { Title = "ĐĂNG NHẬP" });
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
            if(text.Equals("") == true)
            {
                return false;
            }

            return true;
        }
    }
}