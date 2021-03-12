namespace webpush_notifications_api.Models
{
    public class NotificationModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }

    public class SubscriptionModel
    {
        public string endpoint { get; set; }
        public Keys keys { get; set; }
    }

    public class Keys
    {
        public string p256dh { get; set; }
        public string auth { get; set; }
    }
}
