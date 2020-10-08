using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace fwAssistant
{
	public static class Modules
	{
        public static async Task<dynamic> GetOldWeatherAsync()
        {
            string jsonstring = string.Empty;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://weatherbit-v1-mashape.p.rapidapi.com/current?lang=pl&lon=20.689959&lat=49.435910"))
                {
                    request.Headers.TryAddWithoutValidation("x-rapidapi-host", "weatherbit-v1-mashape.p.rapidapi.com");
                    request.Headers.TryAddWithoutValidation("x-rapidapi-key", "712290b574msh6f1dcf74db1ef32p1a7c59jsn5decf224c462");

                    var response = await httpClient.SendAsync(request);
                    jsonstring = response.Content.ReadAsStringAsync().Result;
                }
            }

            dynamic json = JsonConvert.DeserializeObject(jsonstring);
            return json["data"][0];
        }

        public static async Task<dynamic> GetCurrentWeatherAsync()
        {
            string jsonstring = string.Empty;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.openweathermap.org/data/2.5/weather?q=Piwniczna-Zdrój&appid=2c45c81ed4cd8e26776565e0ffc20b76&lang=PL&units=metric"))
                {
                    var response = await httpClient.SendAsync(request);
                    jsonstring = response.Content.ReadAsStringAsync().Result;
                }
            }

            dynamic json = JsonConvert.DeserializeObject(jsonstring);
            return json;
        }

        public static async Task<dynamic> GetDailyWeatherAsync(int day)
        {
            string jsonstring = string.Empty;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.openweathermap.org/data/2.5/onecall?lat=49.43&lon=20.67&exclude=current,minutely,hourly&appid=2c45c81ed4cd8e26776565e0ffc20b76&lang=PL&units=metric"))
                {
                    var response = await httpClient.SendAsync(request);
                    jsonstring = response.Content.ReadAsStringAsync().Result;
                }
            }

            dynamic json = JsonConvert.DeserializeObject(jsonstring);
            return json["daily"][day];
        }
    }
}