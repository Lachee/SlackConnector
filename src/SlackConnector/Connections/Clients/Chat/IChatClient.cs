﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    internal interface IChatClient
    {
        //Task PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments);
        Task PostMessage(string slackKey, BotMessage message);
        Task DeleteMessage(string slackKey, SlackMessage message);
    }
}