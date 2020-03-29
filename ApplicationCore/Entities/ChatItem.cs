using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class ChatItem: BaseEntity
    {
        public ChatItem()
        {

        }
        public Person Person { get; set; }
    }
}
