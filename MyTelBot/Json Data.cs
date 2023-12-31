﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelBot
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Chat
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }
    }

    public class Entity
    {
        public int offset { get; set; }
        public int length { get; set; }
        public string type { get; set; }
    }

    public class From
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string language_code { get; set; }
    }

    public class Message
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public List<Entity> entities { get; set; }
    }

    public class Result
    {
        public int update_id { get; set; }
        public Message message { get; set; }
    }

    public class Root
    {
        public bool ok { get; set; }
        public List<Result> result { get; set; }
    }
}
