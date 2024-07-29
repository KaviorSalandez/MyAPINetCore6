using MyAPINetCore6.Data;
using MyAPINetCore6.Hubs;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace MyAPINetCore6.SubscribeTableDependencies
{
    public class SubcribeNotificationTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Notification> tableDependency;
        NotificationHub _notificationHub;
        public SubcribeNotificationTableDependency(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }
        public void SubscribeTableDependency(string connectString)
        {
            tableDependency = new SqlTableDependency<Notification>(connectString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Notification)} SqlTableDependenecy error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Notification> e)
        {
            if(e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var notification = e.Entity;
                if(notification.NotificationType == 1) //Alll
                {
                    await _notificationHub.SendNotificationToAll(notification.Description);
                }
                else if (notification.NotificationType == 2)
                {
                    await _notificationHub.SendNotificationToClient(notification.Description, notification.UserSendId);
                }
            }
        }
    }
}
