using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class ChatWithPeopleSpecification : BaseSpecification<Chat>
    {
        public ChatWithPeopleSpecification()
       : base(b => b.Id > 0)
        {
            AddInclude(b => b.People);
        }
    }
}
