using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using VoipTranslator.Protocol.Dto;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server.Infrastructure
{
    public class PushSender : IPushSender
    {
        public void SendVoipPush(string pushUrl, string callerNumber, string callerName)
        {
            // Send a push notification to the push channel URI of this app to simulate an incoming call
            try
            {
                // Create an HTTPWebRequest that posts the raw notification to the Microsoft Push Notification Service.
                // HTTP POST is the only method allowed to send the notification.
                var sendNotificationRequest = (HttpWebRequest)WebRequest.Create(pushUrl);
                sendNotificationRequest.Method = "POST";

                // Create the raw message.
                byte[] notificationMessage = null;
                using (var ms = new MemoryStream())
                {
                    var notification = new IncomingCallInfo
                    {
                        Name = callerName,
                        Number = callerNumber
                    };

                    var xs = new XmlSerializer(typeof(IncomingCallInfo));
                    xs.Serialize(ms, notification);

                    notificationMessage = ms.ToArray();
                }

                // Set the required web request headers
                sendNotificationRequest.ContentLength = notificationMessage.Length;
                sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers["X-NotificationClass"] = "4"; // Class 4 indicates an incoming VoIP call

                // Write the request body
                sendNotificationRequest.BeginGetRequestStream(arRequest =>
                {
                    try
                    {
                        using (Stream requestStream = sendNotificationRequest.EndGetRequestStream(arRequest))
                        {
                            requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                        }

                        // Get the response.
                        sendNotificationRequest.BeginGetResponse(arResponse =>
                        {
                            try
                            {
                                var response = (HttpWebResponse)sendNotificationRequest.EndGetResponse(arResponse);
                                string notificationStatus = response.Headers["X-NotificationStatus"];
                                string subscriptionStatus = response.Headers["X-SubscriptionStatus"];
                                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                                // The push notification was sent
                                //this.ShowResult(string.Format("Notification: {0}\r\nSubscription: {1}\r\nDevice: {2}", notificationStatus, subscriptionStatus, deviceConnectionStatus));
                                Console.WriteLine("Send push result: Notification: {0}\r\nSubscription: {1}\r\nDevice: {2}", notificationStatus, subscriptionStatus, deviceConnectionStatus);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Send push error: " + ex.Message);
                                //this.ShowResult(ex);
                            }
                        }, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Send push error: " + ex.Message);
                        //this.ShowResult(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send push error: " + ex.Message);
                //this.ShowResult(ex);
            }
        }
    }
}
