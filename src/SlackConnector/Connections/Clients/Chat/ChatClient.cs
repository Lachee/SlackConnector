using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    internal class ChatClient : IChatClient
    {
        private readonly IRequestExecutor _requestExecutor;
        internal const string SEND_MESSAGE_PATH = "/api/chat.postMessage";
        internal const string DELETE_MESSAGE_PATH = "/api/chat.delete";

        public ChatClient(IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
        }
        
        public async Task PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments)
        {
            var request = new RestRequest(SEND_MESSAGE_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("channel", channel);
            request.AddParameter("text", text);
            request.AddParameter("as_user", "true");

            if (attachments != null && attachments.Any())
            {
                request.AddParameter("attachments", JsonConvert.SerializeObject(attachments));
            }

            await _requestExecutor.Execute<StandardResponse>(request);
        }
        public async Task PostMessage(string slackKey, BotMessage message)
        {
            var request = new RestRequest(SEND_MESSAGE_PATH);
            bool as_user = true;

            //Add the required API parameters
            request.AddParameter("token", slackKey);

            //Slack messages goodness
            request.AddParameter("channel", message.ChatHub.Id);
            request.AddParameter("text", message.Text);

            //MOAR GOODNESS
            //Name of the sender
            if (!string.IsNullOrEmpty(message.Name))
            {
                //We want a custom name. As a result, we are not allowed to have user permissions
                as_user = false;
                request.AddParameter("username", message.Name);
            }

            //The Icon of the sender
            if (!string.IsNullOrEmpty(message.Icon))
            {
                //We want a custom icon. As the same as a name, we are not allowed to have permissions if we get one
                as_user = false;
                request.AddParameter(message.UsingEmoji() ? "icon_emoji" : "icon_url", message.Icon);
            }

            //Use markdown?
            request.AddParameter("parse", message.UseMarkdown ? "full" : "none");

            //Add attachments
            if (message.Attachments != null && message.Attachments.Any())
            {
                request.AddParameter("attachments", JsonConvert.SerializeObject(message.Attachments));
            }

            //Should we be considered a user?
            request.AddParameter("as_user", as_user);

            //Send the request
            await _requestExecutor.Execute<StandardResponse>(request);
        }

        public async Task DeleteMessage(string slackKey, string channel, double timestamp)
        {
            var request = new RestRequest(DELETE_MESSAGE_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("ts", timestamp.ToString());
            request.AddParameter("channel", channel);
            request.AddParameter("as_user", "true");

            await _requestExecutor.Execute<StandardResponse>(request);
        }
        public async Task DeleteMessage(string slackKey, SlackMessage message)
        {
            await DeleteMessage(slackKey, message.ChatHub.Id, message.TimeStamp);
        }
    }
}