﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NSEBot.Dialogs
{
    [Serializable]
    public class AgeDialog : IDialog<int>
    {
        private string name;
        private int attempts = 3;

        public AgeDialog(string name)
        {
            this.name = name;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"{ this.name } is the selected scrip..");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            int age;

            if (Int32.TryParse(message.Text, out age) && (age > 0))
            {
                context.Done(age);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. What is your age (e.g. '42')?");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not a valid age."));
                }
            }
        }
    }
}