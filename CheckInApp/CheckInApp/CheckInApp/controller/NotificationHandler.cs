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
    class NotificationHandler
    {
        public const string URL = "http://10.0.3.2:1515/";
        public const string URL_NOTIFICATION = "api/Notification";

        public async Task<ResultAndNotification> GetNotification(string token)
        {
            var resultAndNotification = new ResultAndNotification();
            var client = new System.Net.Http.HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);

                var response = await client.GetAsync(URL_NOTIFICATION + "?token=" + token);

                resultAndNotification = JsonConvert.DeserializeObject<ResultAndNotification>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }

            return resultAndNotification;
        }
    }
}
