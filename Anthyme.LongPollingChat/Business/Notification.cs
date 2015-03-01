using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anthyme.LongPollingChat.Models;

namespace Anthyme.LongPollingChat.Business
{
    public class Notification
    {
        public DateTime Date { get; set; }
    }

    public class MessageNotification : Notification
    {
        public Message Message { get; set; }
    }
}