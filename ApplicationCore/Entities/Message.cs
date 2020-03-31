using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Message : BaseEntity
    {
        public Message()
        {

        }

        public Message(int ChatId, DateTime CapturedDate)
        {
            this.ChatId = ChatId;
            this.CapturedDate = CapturedDate;
        }

        public string TextMessage { get; set; }
        public DateTime CapturedDate { get; set; }
        public Chat Chat { get; set; }
        public int ChatId { get; set; }
        public Person Person { get; set; }

    }
}
