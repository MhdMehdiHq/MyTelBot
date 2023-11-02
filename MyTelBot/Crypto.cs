using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelBot
{
    public class Crypto
    {
        static string Token = "6883562513:AAG2LogXoXWqZ8X-3Vk6YbpU3dXVyNtBml4";
        static HttpClient client = new HttpClient();
        string url = $"https://api.telegram.org/bot{Token}/";

        HttpClient httpClient = new HttpClient();
        string stringAPI = "https://api.wallex.ir/v1/currencies/stats";

        //Telegram_API.FileUploadInfo
        public async Task HttpResponse(string key)
        {
            long offset = 0;
            

            string data1 = await client.GetStringAsync(url + $"getupdates?offset={offset}");
            var dJson = JsonConvert.DeserializeObject<Root>(data1);
            foreach (var item in dJson.result)
            {
                HttpResponseMessage response = await httpClient.GetAsync(stringAPI);
                if (response.IsSuccessStatusCode)
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    ApiResponseWrapper apiWrapper = JsonConvert.DeserializeObject<ApiResponseWrapper>(apiresponse);
                    List<ResultItem> result = apiWrapper.Result;
                    foreach (var itemm in result)
                    {
                        if (result.Exists(X => X.key == key))
                        {
                            ResultItem item2 = result.Find(X => X.key == key);
                            var price = item2.price;
                            await client.GetStringAsync($"{url}sendmessage?chat_id={item.message.chat.id}&text={price}$");
                            Console.WriteLine("sent");
                        }
                        break;
                    }
                }
                offset++;
            }
        }
    }

    public class ApiResponseWrapper
    {
        public List<ResultItem> Result { get; set; }
    }
    public class ResultItem
    {
        public string key { get; set; }
        public string name_en { get; set; }
        public int? rank { get; set; }
        public float price { get; set; }
    }
}
