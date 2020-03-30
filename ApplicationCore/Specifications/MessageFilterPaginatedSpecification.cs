using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class MessageFilterPaginatedSpecification : BaseSpecification<Message>
    {
        public MessageFilterPaginatedSpecification(int skip, int take, int chatId)
                    : base(i => i.ChatId == chatId) 
        {
            ApplyPaging(skip, take);
            ApplyOrderByDescending(c => c.Id);
        }
    }
}
