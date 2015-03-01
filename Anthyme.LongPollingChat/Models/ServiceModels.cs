using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anthyme.LongPollingChat.Models
{
    public class ParameterBase
    {
        public SessionKey SessionKey { get; set; }
    }

    public class GetUpdatesParameter : ParameterBase
    {
        public long Ticks { get; set; }
    }

    public class GetUpdatesResult
    {
        public long Ticks { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

    public class SendMessageParameter : ParameterBase
    {
        public string Message { get; set; }
    }

    public class SendMessageResult { }

    public class CreateSessionParameter
    {
        public string Name { get; set; }
    }

    public class CreateSessionResult
    {
        public SessionKey Key { get; set; }
    }
}