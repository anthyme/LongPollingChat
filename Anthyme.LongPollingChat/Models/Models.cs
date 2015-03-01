using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anthyme.LongPollingChat.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Message
    {
        public User User { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }

    public class SessionKey
    {
        public Guid Id { get; set; }
        public User User { get; set; }
    }
}