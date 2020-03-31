using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Person: BaseEntity
    {
        public Person()
        {

        }
        public string UserName { get; set; }

        public ICollection<Message> Messages { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
