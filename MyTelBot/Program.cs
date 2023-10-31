using System.Data;
using Telegram.Bot;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using RestSharp;
using static System.Console;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.WebSockets;
using System.Security;
using System;
using Telegram.Bot.Types;

namespace MyTelBot
{
    internal class Program
    {
        // مهدی حقیقی فشی 
        //دانشجوی ترم سه مهندسی کامپیوتر


        //این بات قادر به این است که عکس خودم بصورت فایل jpg
        //و همچنین ارسال همان عکس بصورت استیکر و داکیومنت را دارد
        //همچنین این بات میتواند یک مخاطب را با شماره فرضی برای ما چاپ کند
        //البته که میتوان با تغییر شماره تلفن در کد مربوط به این فانکشن،مخاطب واقعی را از کاربر دریافت کرد
        //و این بات قادر به ارسال نقشه میباشد که مختصات آن بصورت نقطه جغرافیایی به متد داده شده است و ربات نقشه واقعی آن نقطه را برای ما ارسال میکند
        // این بات می تواند قیمت ارز بیت کوین را با دستور bitcoin price به کاربر نمایش دهد
        // و در نهایت به این بات، هر کلمه یا جمله ای بجز اینها بهش بگیم،دقیقا همان جمله را برای ما برمیگرداند
        // این بات 7 کار انجام میدهد

        //توکن بات تلگرام
        static string Token = "6883562513:AAG2LogXoXWqZ8X-3Vk6YbpU3dXVyNtBml4";
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            long offset = 0;

            string url = $"https://api.telegram.org/bot{Token}/";
            while (true)
            {
                //قدم اول برای اینکه مدیریت کنیم پیامایی که ارسال کردیم، و به پیامایی که فرستاده شده جواب ندیم،میتونیم از پارارمتر آفست استفاده کنیم
                string data1 = await client.GetStringAsync(url + $"getupdates?offset={offset}");
                var dJson = JsonConvert.DeserializeObject<Root>(data1);
                foreach (var item in dJson.result)
                {
                    //برای راحتی کار و کاهش خطوط برنامه،مقدار هامو داخل متغییر ریختم که براحتی بتونم صداشون کنم بجای اینکه همش بخوام این کد طولانی رو بزنم
                    var msgChatId = item.message?.chat?.id;
                    var msgText = item.message?.text;
                    
                    //نمایش تصویر خودم
                    if (msgText == "my photo")
                    {
                        Telegram_API.UploadFile(new Telegram_API.FileUploadInfo()
                        {
                            Chat_Id = (long)msgChatId,
                            srcFile = @"C:\Users\Asus\Desktop\aks.jpg",
                            Token = Token
                        },
                        Telegram_API.TypeMessage.photo);
                    }

                    //نمایش عکس به فرم استیکر
                    else if (msgText == "photo sticker")
                    {
                        Telegram_API.UploadFile(new Telegram_API.FileUploadInfo()
                        {
                            Chat_Id = (long)msgChatId,
                            srcFile = @"C:\Users\Asus\Desktop\aks.jpg",
                            Token = Token
                        },
                        Telegram_API.TypeMessage.sticker);
                    }

                    //ارسال عکس به فرم داکیومنت
                    else if (msgText == "photo document")
                    {
                        Telegram_API.UploadFile(new Telegram_API.FileUploadInfo()
                        {
                            Chat_Id = (long)msgChatId,
                            srcFile = @"C:\Users\Asus\Desktop\aks.jpg",
                            Token = Token
                        },
                        Telegram_API.TypeMessage.document);
                    }

                    //ارسال مخاطب به کاربر 
                    else if (msgText == "my contact")
                    {
                        client?.GetStringAsync($"{url}sendContact?chat_id={msgChatId}&phone_number=1234&first_name=Mehdi");
                    }

                    //ارسال نقشه واقعی..زیر کشور ترکیه
                    else if (msgText == "map")
                    {
                        client?.GetStringAsync($"{url}sendLocation?chat_id={msgChatId}&longitude=32.98787&latitude=34.8766");
                    }
                    
                    //ارسال قیمت بیت کوین
                    else if (msgText == "bitcoin price")
                    {
                        HttpClient httpClient = new HttpClient();
                        string stringAPI = "https://api.wallex.ir/v1/currencies/stats";

                        //وصل کردن ای پی آی با کلاینت
                        HttpResponseMessage response = await httpClient.GetAsync(stringAPI);
                        if (response.IsSuccessStatusCode)
                        {
                            string apiresponse = await response.Content.ReadAsStringAsync();
                            ApiResponseWrapper apiWrapper = JsonConvert.DeserializeObject<ApiResponseWrapper>(apiresponse);
                            List<ResultItem> result = apiWrapper.Result;

                            foreach (var itemm in result)
                            {
                                if (result.Exists(X => X.key == "BTC"))
                                {
                                    ResultItem item2 = result.Find(X => X.key == "BTC");
                                    var price = item2.price;
                                    //Console.WriteLine(item2.price);
                                    await client.GetStringAsync($"{url}sendmessage?chat_id={msgChatId}&text={price}$");
                                }
                                //برک گذاشتم چون درغیر اینصورت همش چاپش میکرد
                                break;
                            }
                        }
                    }
                    else
                    {
                        //ارسال پیام کاربر به خودش
                        await client.GetStringAsync($"{url}sendmessage?chat_id={msgChatId}&text={msgText}");
                    }
                    WriteLine(msgChatId + "\n" + item.update_id + "\n-------------------------------------------------------");

                    offset = item.update_id + 1;
                }
            }
        }
    }
    
    //مدل سازی ای پی آی ارز
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