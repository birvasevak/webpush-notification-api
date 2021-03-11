using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPush;

namespace webpush_notifications_api.DAL
{
    public class NotificationDAL
    {
        private static Dictionary<string, PushSubscription> StaticDic = new Dictionary<string, PushSubscription>();

        public static PushSubscription GetSubscription(string client)
        {
            PushSubscription res;
            StaticDic.TryGetValue(client, out res);
            return res;
        }

        public static void SaveSubscription(string client, PushSubscription subscription)
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
