﻿using AI_Customer_Service_Lee_8900.Data;

namespace AI_Customer_Service_Lee_8900.Models
{
    public class ViewModel1
    {
        public int userId { get; set; }
        public string username { get; set; }

        public List<Conversations> chats { get; set; }
    }
}
