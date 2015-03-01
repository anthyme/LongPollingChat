using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anthyme.LongPollingChat.Models;

namespace Anthyme.LongPollingChat.Business
{
    public class UserSession
    {
        public UserSession(SessionKey key)
        {
            Key = key;
            Notifications = new BlockingCollection<Notification>();
        }

        public DateTime LastActivityDate { get; set; }
        public SessionKey Key { get; set; }

        protected BlockingCollection<Notification> Notifications { get; set; }

        public event EventHandler HasChanged;

        protected void NotifyChanges()
        {
            EventHandler handler = HasChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private object notifyAddLock = new object();
        public void Notify(Notification notification)
        {
            lock (notifyAddLock)
            {
                notification.Date = DateTime.Now;
                Notifications.Add(notification);
            }

            NotifyChanges();
        }

        public ICollection<Notification> GetUpdates(long ticks)
        {
            return Notifications.Where(n => n.Date.ToClientTicks() > ticks).ToList();
        }

        public bool HasChanges(long ticks)
        {
            return Notifications.Any(n => n.Date.ToClientTicks() > ticks);
        }
    }
}