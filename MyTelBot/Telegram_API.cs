using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MyTelBot
{
    internal class Telegram_API
    {
        //این کلاس برای اینه که عکس و استیکر و.. رو به کاربر بدیم
        //قبل از اینکه این کلاس رو تولید کنم، وحشتناک تعداد خطوط برنامم بالا بود 
        //بعد از تولید این کلاس تعداد خطوط برنامه به حداقل ممکن رسید
        public enum TypeMessage : byte
        {
            photo, voice,
            document, sticker, audio
        }

        public class FileUploadInfo
        {
            public string Token { get; set; }
            public string srcFile { get; set; }
            public long Chat_Id { get; set; }
        }
        //با این متد میتوانیم اون مقادیر عکس و غیره رو ارسال کنیم
        public static string UploadFile(FileUploadInfo infoFile, TypeMessage Type)
        {
            string urlpic = $"https://api.telegram.org/bot{infoFile.Token}/send{Type}?chat_id={infoFile.Chat_Id}";


            using (var Client = new HttpClient())
            {
                using (var contect = new MultipartFormDataContent())
                {
                    contect.Add(new StreamContent(new MemoryStream(File.ReadAllBytes(infoFile.srcFile))), Type.ToString(), Path.GetFileName(infoFile.srcFile));
                    using (var message = Client.PostAsync(urlpic, contect).Result)
                    {
                        return message.Content.ReadAsStringAsync().Result;
                    }
                }
            }
        }
    }
}
