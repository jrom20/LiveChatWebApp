using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Chat: BaseEntity
    {
        public Chat()
        {

        }
        public ICollection<Person> People { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
