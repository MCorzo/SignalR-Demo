using Microsoft.AspNetCore.SignalR;
using SignalRDemoCore.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;

namespace SignalRDemoCore
{
    public class NotificationTicker
    {
        private readonly Timer _timer;
        private volatile bool _updatingNotifications;
        private readonly ConcurrentDictionary<string, string> _notifications = new ConcurrentDictionary<string, string>();
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(5000);
        private readonly Subject<string> _subject = new Subject<string>();
        private readonly SemaphoreSlim _updateNotificationLock = new SemaphoreSlim(1, 1);
        private IHubContext<NotificationHub> _hub { get; set; }

        public NotificationTicker(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
            LoadInitialNotifications();
            _timer = new Timer(UpdateNotifications, null, _updateInterval, _updateInterval);
        }

        public void LoadInitialNotifications()
        {
            for (int i = 0; i < 10; i++)
            {
                var guid = Guid.NewGuid().ToString();
                _notifications.TryAdd(guid, $"un nuevo mensaje con id-- > ${guid}");
            }
        }

        public IEnumerable<string> GetAllNotifications()
        {
            return _notifications.Values;
        }

        private async void UpdateNotifications(object state)
        {
            await _updateNotificationLock.WaitAsync();
            try
            {
                if (!_updatingNotifications)
                {
                    _updatingNotifications = true;

                    var guid = Guid.NewGuid().ToString();
                    _notifications.TryAdd(guid, $"un nuevo mensaje con id-- > ${guid}");

                    _subject.OnNext(guid);
                }
                _updatingNotifications = false;
            }
            finally
            {
                _updateNotificationLock.Release();
            }

            await _hub.Clients.All.SendAsync("ReceiveNotification", _notifications.ToArray());
        }

    }
}
