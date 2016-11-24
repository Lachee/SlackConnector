using System.Collections.Generic;

namespace SlackConnector.Models
{
    public class BotMessage
    {
        public IList<SlackAttachment> Attachments { get; set; }

        /// <summary>
        /// The ChatHub object of the message
        /// </summary>
        public SlackChatHub ChatHub { get; set; }

        /// <summary>
        /// The message contents
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The emoji or URL of the bot's avatar. Example: http://lorempixel.com/128/128/cats/ or :koala:
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The name of the bot
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Should it use the Markdown formatting?
        /// </summary>
        public bool UseMarkdown { get; set; }

        public BotMessage()
        {
            Attachments = new List<SlackAttachment>();
            UseMarkdown = true;
        }

        /// <summary>
        /// Does this message have a emoji icon? If not, it must be using a URL icon.
        /// </summary>
        /// <returns>true if emoji</returns>
        public bool UsingEmoji()
        {
            return !Icon.Contains("http");
        }
    }
}