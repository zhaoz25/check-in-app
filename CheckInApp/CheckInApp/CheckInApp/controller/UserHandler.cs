using CheckInApp.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.controller
{
    class UserHandler
    {
        public const string URL = "http://10.0.3.2:1515/";
        public const string URL_USER = "api/User";
        public const string URL_EMPLOYEEINFO = "api/EmployeeInfo";
        public const string URL_EMPLOYEELIST = "api/EmployeeList";
        public const string URL_CHECKINOUT = "api/CheckInOut";
        public const string URL_TIMELOG = "api/TimeLog";
        public const string URL_CHARTlOG = "api/ChartLog";


        public async Task<Result> GetSendEmail(string email)
        {
            // http://localhost:1515/api/EmployeeInfo

            var result = new Result();
            var client = new System.Net.Http.HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);

                var response = await client.GetAsync(URL_USER + "?email=" + email);

                result = JsonConvert.DeserializeObject<Result>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return result;

        }
        // lấy thông tin nhân viên
        public async Task<ResultAndEmployee> GetEmployeeInfo(string token)
        {

            var result = new ResultAndEmployee();
            var client = new System.Net.Http.HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);

                var response = await client.GetAsync(URL_EMPLOYEEINFO + "?token=" + token);

                result = JsonConvert.DeserializeObject<ResultAndEmployee>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return result;

        }
        // lấy thông tin tất cả nhân viên phòng ban
        public async Task<ResultAndEmployee> GetEmployeeList(string token)
        {

            var result = new ResultAndEmployee();
            var client = new System.Net.Http.HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);

                var response = await client.GetAsync(URL_EMPLOYEELIST + "?token=" + token);

                result = JsonConvert.DeserializeObject<ResultAndEmployee>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return result;

        }

        // đăng xuất
        public async Task<Result> GetLogout(string token)
        {
            var result = new Result();
            var client = new System.Net.Http.HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);

                var response = await client.GetAsync(URL_USER + "?token=" + token);

                result = JsonConvert.DeserializeObject<Result>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return result;
        }

        public async Task<ResultAndTimeLogList> GetLogDetails(string token, string employeeId,int month = 0)
        {
            var resultAndTimeLogList = new ResultAndTimeLogList();
            var client = new System.Net.Http.HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);
                // tạo đường dẫn
                string url = URL_TIMELOG;
                // tìm tất cả
                if(month == 0)
                {
                    url += "?token=" + token + "&employeeID=" + employeeId;
                }
                else
                {
                    url += "?token=" + token + "&employeeID=" + employeeId + "&month=" + month;
                }

                var response = await client.GetAsync(url);
                
                resultAndTimeLogList = JsonConvert.DeserializeObject<ResultAndTimeLogList>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return resultAndTimeLogList;
        }

        public async Task<ResultAndChartData> GetLogForChart(string token, int month)
        {
            var resultAndChartData = new ResultAndChartData();
            var client = new System.Net.Http.HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);
                // tạo đường dẫn
                string url = URL_CHARTlOG+"?token="+token+"&month="+month;
                
                var response = await client.GetAsync(url);

                resultAndChartData = JsonConvert.DeserializeObject<ResultAndChartData>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return resultAndChartData;
        }

        public async Task<List<int>> GetMonth(string token)
        {
            List<int> monthList = new List<int>();
            var client = new System.Net.Http.HttpClient();

            try
            {
                client.BaseAddress = new Uri(URL);
                // tạo đường dẫn
                string url = URL_CHARTlOG + "?token=" + token;

                var response = await client.GetAsync(url);

                monthList = JsonConvert.DeserializeObject<List<int>>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return monthList;
        }

        public async Task<Result> PostLogin(LoginInfo info)
        {
            var client = new System.Net.Http.HttpClient();
            var result = new Result();
            try
            {
                var data = JsonConvert.SerializeObject(info);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(URL + URL_USER, content);

                result = JsonConvert.DeserializeObject<Result>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return result;

        }

        public async Task<Result> PutUpdatePassword(LoginInfo info)
        {
            var client = new System.Net.Http.HttpClient();
            var result = new Result();
            try
            {
                var data = JsonConvert.SerializeObject(info);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(URL + URL_USER, content);

                result = JsonConvert.DeserializeObject<Result>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return result;

        }

        public async Task<Result> PostCheckIn(CheckIn checkIn)
        {
            var client = new System.Net.Http.HttpClient();
            var result = new Result();
            try
            {
                var data = JsonConvert.SerializeObject(checkIn);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(URL + URL_CHECKINOUT, content);

                result = JsonConvert.DeserializeObject<Result>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return result;
        }

        
    }
}
