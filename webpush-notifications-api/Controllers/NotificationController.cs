using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using WebPush;
using webpush_notifications_api.DAL;
using webpush_notifications_api.Models;

namespace webpush_notifications_api.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class NotificationController : Controller
    {
        private readonly IConfiguration configuration;

        public NotificationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        [Route("Clients")]
        public List<string> GetAllClients()
        {
            return NotificationDAL.GetClientNames();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("subscribe/{client}")]
        public IActionResult Subscribe(string client, [FromBody] SubscriptionModel subscription)
        {
            if (client == null)
            {
                return BadRequest("No Client Name parsed.");
            }
            if (NotificationDAL.GetClientNames().Contains(client))
            {
                return BadRequest("Client Name already used.");
            }

            NotificationDAL.SaveSubscription(client, subscription);
            return Ok(NotificationDAL.GetClientNames());
        }

        [HttpPost("unsubscribe/{client}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public List<string> Unsubscribe(string client)
        {
            var item = NotificationDAL.GetClientNames();
            if (item != null)
            {
                NotificationDAL.RemoveSubscription(client);
            }
            return NotificationDAL.GetClientNames();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("notify")]
        public IActionResult Notify(string client)
        {
            if (client == null)
            {
                return BadRequest("No Client Name parsed.");
            }
            SubscriptionModel sub = NotificationDAL.GetSubscription(client);
            PushSubscription subscription = new PushSubscription(sub.endpoint, sub.keys.p256dh, sub.keys.auth);
            if (subscription == null)
            {
                return BadRequest("Client was not found");
            }

            var subject = configuration["VAPID:subject"];
            var publicKey = configuration["VAPID:publicKey"];
            var privateKey = configuration["VAPID:privateKey"];

            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);

            NotificationModel message = new NotificationModel()
            {
                Title = "Web Push Notification",
                Message = "This is msg from .net core api",
                Url = "https://blog.angular-university.io/angular-push-notifications/",
            };

            string serializedMessage = JsonConvert.SerializeObject(message);

            var webPushClient = new WebPushClient();
            try
            {
                webPushClient.SendNotification(subscription, serializedMessage, vapidDetails);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return Ok("Notification Sent!");
        }

    }
}
