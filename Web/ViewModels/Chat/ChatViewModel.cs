using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels.Chat
{
    public class ChatViewModel
    {
        public int ChatId { get; set; }
        public DateTime StartedDate { get; set; }
        public string ChatName { get; set; }
        public int  PeopleNumber { get; set; }

    }
}
