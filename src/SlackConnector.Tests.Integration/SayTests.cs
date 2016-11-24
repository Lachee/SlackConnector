using System;
using System.Linq;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class SayTests
    {
        [Test]
        public void should_say_something_on_channel()
        {
            // given
            var config = new ConfigReader().GetConfig();
            
            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;
            var message = new BotMessage
            {
                Text = "Test text for SIMPLE test",
                ChatHub = connection.ConnectedChannels().First(x => x.Name.Equals("#general", StringComparison.InvariantCultureIgnoreCase))
            };
            
            // when
            connection.Say(message).Wait();

            // then
        }

        [Test]
        public void should_say_something_complex_on_channel()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;
            var message = new BotMessage
            {
                Text = "Test Text for COMPLEX messages",
                Name = "Jeffery Bot",
                Icon = "http://lorempixel.com/128/128/cats/",
                ChatHub = connection.ConnectedChannels().First(x => x.Name.Equals("#general", StringComparison.InvariantCultureIgnoreCase))
            };

            // when
            connection.Say(message).Wait();

            // then
        }
    }
}