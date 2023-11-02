using Newtonsoft.Json;

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
        // این بات می تواند قیمت ارز بیت کوین  و دو ارز دیگر را به کاربر نمایش دهد
        // و در نهایت به این بات، هر کلمه یا جمله ای بجز اینها بهش بگیم،دقیقا همان جمله را برای ما برمیگرداند
        // این بات 7 کار انجام میدهد

        static HttpClient httpClient = new HttpClient();
        static string stringAPI = "https://api.wallex.ir/v1/currencies/stats";
        //توکن بات تلگرام
        static string Token = "6883562513:AAG2LogXoXWqZ8X-3Vk6YbpU3dXVyNtBml4";
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Crypto crypto = new Crypto();

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
                    if (msgText == "my photo" || msgText == "My photo")
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
                    else if (msgText == "photo sticker" || msgText == "Photo sticker")
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
                    else if (msgText == "photo document" || msgText == "Photo document")
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
                    else if (msgText == "my contact" || msgText == "My contact")
                    {
                        client?.GetStringAsync($"{url}sendContact?chat_id={msgChatId}&phone_number=1234&first_name=Mehdi");
                    }

                    //ارسال نقشه واقعی..زیر کشور ترکیه
                    else if (msgText == "map" || msgText == "Map")
                    {
                        client?.GetStringAsync($"{url}sendLocation?chat_id={msgChatId}&longitude=32.98787&latitude=34.8766");
                    }

                    //******************************************************************************************************************************************************************
                    //ارسال قیمت اتریوم
                    else if (msgText == "Ethereum price" || msgText == "ethereum price" || msgText.StartsWith("Eth") || msgText.StartsWith("eth") || msgText == "قیمت اتریوم")
                    {
                        //دو حالت برای چاپ مقدار وجود دارد
                        //یک
                        //با استفاده از کلاس
                        //در این روش، در اولین باری که قیمت را میخواهید،باید دوبار نام رمز ارز را صدا بزنید
                        //این کار فقط در مرتبه اول نیاز است و در ادامه با یکبار صدا کردن نام رمزارز،مقدار آن به ما نمایش داده میشود
                        //نکته اینجاست که فرقی نمیکنه کدوم رمزارز رو میخوایم صدا کنیم
                        //هرکدومو که اول صدا میزنیم،باید دوبار صداش کنیم.اما برای دیگر رمزارز ها،دیگر نیاز نیست دوبار صدا کنیم
                        //فقط موقع صدا کردن اولین رمزارز باید دوبار نام رمزارز رو بصورت پیامک به بات فرستاد
                        crypto?.HttpResponse("ETH");

                        //اما روش دوم
                        //با استفاده از کد پایین
                        //فقط بخاطر اینکه تعداد خطوط کم بشه و سرعت برنامه بالا بره،براشون کلاس زدم
                        //اگه از کد پایین بخواید استفاده کنید نیازی نیست که برای بار اول،دوبار نام رمزارز را صدا کنید
                        #region روش دوم
                        //HttpResponseMessage response = await httpClient.GetAsync(stringAPI);
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    string apiresponse = await response.Content.ReadAsStringAsync();
                        //    ApiResponseWrapper apiWrapper = JsonConvert.DeserializeObject<ApiResponseWrapper>(apiresponse);
                        //    List<ResultItem> result = apiWrapper.Result;

                        //    foreach (var itemm in result)
                        //    {
                        //        if (result.Exists(X => X.key == "ETH"))
                        //        {
                        //            ResultItem item2 = result.Find(X => X.key == "ETH");
                        //            var price = item2.price;
                        //            await client.GetStringAsync($"{url}sendmessage?chat_id={msgChatId}&text={price}$");
                        //        }
                        //برک گذاشتم چون درغیر اینصورت همش چاپش میکرد
                        //        break;
                        //    }
                        //}
                        #endregion
                    }
                    //ارسال قیمت بیت کوین
                    else if (msgText == "Bitcoin price" || msgText == "bitcoin price" || msgText.StartsWith("Bit") || msgText.StartsWith("bit") || msgText == "قیمت بیت کوین")
                    {
                        crypto?.HttpResponse("BTC");

                        #region روش دوم 
                        //HttpResponseMessage response = await httpClient.GetAsync(stringAPI);
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    string apiresponse = await response.Content.ReadAsStringAsync();
                        //    ApiResponseWrapper apiWrapper = JsonConvert.DeserializeObject<ApiResponseWrapper>(apiresponse);
                        //    List<ResultItem> result = apiWrapper.Result;

                        //    foreach (var itemm in result)
                        //    {
                        //        if (result.Exists(X => X.key == "BTC"))
                        //        {
                        //            ResultItem item2 = result.Find(X => X.key == "BTC");
                        //            var price = item2.price;
                        //            await client.GetStringAsync($"{url}sendmessage?chat_id={msgChatId}&text={price}$");
                        //        }
                        //        break;
                        //    }
                        //}
                        #endregion
                    }

                    //ارسال قیمت تتر
                    else if (msgText == "tether price" || msgText == "Tether price" || msgText.StartsWith("Tet") || msgText.StartsWith("tet") || msgText == "قیمت تتر")
                    {
                        crypto?.HttpResponse("USDT");

                        #region روش دوم 
                        //HttpResponseMessage response = await httpClient.GetAsync(stringAPI);
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    string apiresponse = await response.Content.ReadAsStringAsync();
                        //    ApiResponseWrapper apiWrapper = JsonConvert.DeserializeObject<ApiResponseWrapper>(apiresponse);
                        //    List<ResultItem> result = apiWrapper.Result;

                        //    foreach (var itemm in result)
                        //    {
                        //        if (result.Exists(X => X.key == "USDT"))
                        //        {
                        //            ResultItem item2 = result.Find(X => X.key == "USDT");
                        //            var price = item2.price;
                        //            await client.GetStringAsync($"{url}sendmessage?chat_id={msgChatId}&text={price}$");
                        //        }
                        //        break;
                        //    }
                        //}
                        #endregion
                    }
                    //بقیه ارز ها هم به همین شکل قابل دسترسی هستند

                    else
                    {
                        //ارسال پیام کاربر به خودش
                        await client.GetStringAsync($"{url}sendmessage?chat_id={msgChatId}&text={msgText}");
                    }
                    //چاپ اطلاعات در کنسول
                    Console.WriteLine(msgChatId + "\n" + item.update_id + "\n-------------------------------------------------------");

                    offset = item.update_id + 1;
                }
            }
        }
    }
}