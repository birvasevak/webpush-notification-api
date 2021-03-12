using System.Collections.Generic;
using System.Linq;
using webpush_notifications_api.Models;

namespace webpush_notifications_api.DAL
{
    public class NotificationDAL
    {
        private static Dictionary<string, SubscriptionModel> StaticDic = new Dictionary<string, SubscriptionModel>();

        public static SubscriptionModel GetSubscription(string client)
        {
            SubscriptionModel res;
            StaticDic.TryGetValue(client, out res);
            return res;
        }

        public static void SaveSubscription(string client, SubscriptionModel subscription)
        {
            StaticDic.Add(client, subscription);
        }

        public static void RemoveSubscription(string client)
        {
            StaticDic.Remove(client);
        }


        public static List<string> GetClientNames()
        {
            return StaticDic.Keys.ToList();
        }

    }
}
