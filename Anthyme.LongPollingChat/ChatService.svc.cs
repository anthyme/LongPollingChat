using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Anthyme.LongPollingChat.Business;
using Anthyme.LongPollingChat.Models;

namespace Anthyme.LongPollingChat
{
    public class ChatService : IChatService
    {
        private static int _userAutoIncId;
        public CreateSessionResult CreateSession(CreateSessionParameter parameter)
        {
            var userId = Interlocked.Increment(ref _userAutoIncId);
            var key = new SessionKey
            {
                User = new User { Id = userId, Name = parameter.Name },
                Id = Guid.NewGuid(),
            };
            var session = new UserSession(key);

            ChatEngine.Current.Sessions.TryAdd(key.Id, session);

            return new CreateSessionResult {Key = key};
        }

        public GetUpdatesResult GetUpdates(GetUpdatesParameter parameter)
        {
            var userSession = GetSession(parameter.SessionKey);

            var timeout = new TimeSpan(0, 0, 15);

            var wait = new EventWaitHandle(false, EventResetMode.ManualReset);

            EventHandler waiter = (s, e) => wait.Set();
            userSession.HasChanged += waiter;

            if (userSession.HasChanges(parameter.Ticks) == false)
            {
                wait.WaitOne(timeout);
            }

            userSession.HasChanged -= waiter;
            var changes = userSession.GetUpdates(parameter.Ticks);

            return new GetUpdatesResult
                       {
                           Ticks = (changes.Any() ? changes.Max(n => n.Date).ToClientTicks() : 0),
                           Messages = changes.OfType<MessageNotification>().Select(n => n.Message).ToList(),
                       };
        }

        public SendMessageResult SendMessage(SendMessageParameter parameter)
        {
            var userSession = GetSession(parameter.SessionKey);

            ChatEngine.Current.SendMessage(
                new Message { Date = DateTime.Now, Text = parameter.Message, User = userSession.Key.User });

            return new SendMessageResult();
        }

        private UserSession GetSession(SessionKey key)
        {
            UserSession userSession;

            if (ChatEngine.Current.Sessions.TryGetValue(key.Id, out userSession))
                return userSession;

            throw new FaultException("La session utilisateur n'existe pas", new FaultCode("NoSession"));
        }
    }
}
