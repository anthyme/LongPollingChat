using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anthyme.LongPollingChat.Models;

namespace Anthyme.LongPollingChat.Business
{
    public class ChatEngine
    {
        private ChatEngine()
        {
            Sessions = new ConcurrentDictionary<Guid, UserSession>();
        }

        private static ChatEngine _current;
        public static ChatEngine Current
        {
            get { return _current ?? (_current = new ChatEngine()); }
        }

        public event EventHandler Changed;

        public void OnChanged(EventArgs e)
        {
            EventHandler handler = Changed;
            if (handler != null) handler(this, e);
        }

        public ConcurrentDictionary<Guid, UserSession> Sessions { get; set; }

        public void SendMessage(Message message)
        {
            var notification = new MessageNotification
            {
                Message = message,
            };

            Sessions.Values.Notify(notification);
        }
    }

    public static class Extensions
    {
        public static void Notify(this IEnumerable<UserSession> sessions, Notification notification)
        {
            foreach (var userSession in sessions.AsParallel())
            {
                userSession.Notify(notification);
            }
        }

        public static long ToClientTicks(this DateTime dateTime)
        {
            return dateTime.Ticks / 100;
        }
    }
}