using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;

namespace TelegramBotLesson.TelegramBotFolder
{
    public class TelegramBotHandler
    {
        public string Token { get; set; }
        public TelegramBotHandler(string token) 
        {
            this.Token = token;
        }

        public readonly string[] sites = { "Google", "Github", "Telegram", "Wikipedia" };
        public readonly string[] siteDescriptions =
                {
                    "Google is a search engine",
                    "Github is a git repository hosting",
                    "Telegram is a messenger",
                    "Wikipedia is an open wiki"
                };

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient($"{this.Token}");

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();


           

            // Send cancellation request to stop bot
            cts.Cancel();

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            //// Only process text messages
            //if (message.Text is not { } messageText)
            //    return;



            var chatId = update.Message.Chat.Id;
           // var message = update.Message;

            Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}. UserName =>  {message.Chat.Username}");

            //var fileStream = new FileStream(@"C:\Users\dotnetbillioner\Videos\test.mp4", FileMode.Open);
            Console.WriteLine($"Message Type: {message.Type}  => ");

            if (update.Message.Text == "go")
            {
                //await (update.Type switch
                //{
                //    UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                //    UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                //});
            }



            /**

             if (message.Text == "albom")
             {
                 ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                     {
                         new KeyboardButton[] { "Help me" },
                         new KeyboardButton[] { "Call me ☎️" },
                     })
                     {
                         ResizeKeyboard = true
                     };

                 Message sentMessage = await botClient.SendTextMessageAsync(
                     chatId: chatId,
                     text: "Choose a response",
                     replyMarkup: replyKeyboardMarkup,
                     cancellationToken: cancellationToken);
             }

             if(message.Text == "Help me")
             {
                 ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                     {
                         new KeyboardButton[] { "Nima gap" },
                         new KeyboardButton[] { "Orqaga" },
                     })
                 {
                     ResizeKeyboard = true
                 };

                 Message sentMessage = await botClient.SendTextMessageAsync(
                     chatId: chatId,
                     text: "Choose a response",
                     replyMarkup: replyKeyboardMarkup,
                     cancellationToken: cancellationToken);
             }

             if(message.Text == "Orqaga")
             {
                 await BacktoStart(botClient, chatId, cancellationToken);
             }
            */

        }
        

        public async Task BotOnInlineQueryReceived(ITelegramBotClient bot, InlineQuery inlineQuery)
        {

        var results = new List<InlineQueryResult>();

            var counter = 0;
            foreach (var site in sites)
            {
                results.Add(new InlineQueryResultArticle(
                    $"{counter}", // we use the counter as an id for inline query results
                    site, // inline query result title
                    new InputTextMessageContent(siteDescriptions[counter])) // content that is submitted when the inline query result title is clicked
                );
                counter++;
            }

            await bot.AnswerInlineQueryAsync(inlineQuery.Id, results); // answer by sending the inline query result list
        }

        public Task BotOnChosenInlineResultReceived(ITelegramBotClient bot, ChosenInlineResult chosenInlineResult)
        {
            if (uint.TryParse(chosenInlineResult.ResultId, out var resultId) // check if a result id is parsable and introduce variable
                && resultId < sites.Length)
            {
                Console.WriteLine($"User {chosenInlineResult.From} has selected site: {sites[resultId]}");
            }

            return Task.CompletedTask;
        }

        public async Task BacktoStart(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken )
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                        new KeyboardButton[] { "Help me" },
                        new KeyboardButton[] { "Call me ☎️" },
                    })
            {
                ResizeKeyboard = true
            };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Choose a response",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }


        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
