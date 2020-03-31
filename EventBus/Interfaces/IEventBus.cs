using EventBus.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Interfaces
{
    public interface IEventBus
    {
        void Publish(EventReply activity);
        void Subscribe();

    }
}
