// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.17.1

using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EchoBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EchoBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly IConfiguration _configuration;

        public EchoBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Add Logic To Connect To Rest API
            MessagePrompt message = new MessagePrompt()
            {
                prompt = turnContext.Activity.Text,
                temperature = 1,
                top_p = 0.5f,
                frequency_penalty = 0,
                presence_penalty = 0,
                max_tokens = 500,
            };

            // Get Variables from Configuration File

            // Create Users via JSON File
            var client = new RestClient(_configuration.GetValue<string>("AzureAI:RootUrl"));

            // Create a new request
            var request = new RestRequest(_configuration.GetValue<string>("AzureAI:PathUrl"), Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", _configuration.GetValue<string>("AzureAI:ApiKey"));
            request.AddJsonBody(message);

            // Execute the request and get the response
            var response = client.Execute(request);

            // Deseralize Response
            var responseObject = JsonSerializer.Deserialize<MessageResponse>(response.Content);

            await turnContext.SendActivityAsync(MessageFactory.Text(responseObject.choices[0].text, responseObject.choices[0].text), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Welcome to the Chat GPT Bot!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
