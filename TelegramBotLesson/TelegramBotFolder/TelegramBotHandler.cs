

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotLesson.TelegramBotFolder
{
    public class TelegramBotHandler
    {
        public string Token { get; set; }
        public TelegramBotHandler(string token) 
        {
            this.Token = token;
        }

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
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}. UserName =>  {message.Chat.Username}");


            if (messageText == "dotnet" || messageText == "C#")
            {
                //Echo received message text
                Message sendMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Trying *all the parameters* of `sendMessage` method",
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyToMessageId: update.Message.MessageId,
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithUrl(
                            text: "Dotnet Uzga Booor",
                            url: "https://docs.dot-net.uz/")),
                    cancellationToken: cancellationToken);
            }
            else if (messageText == "salom")
            {
                //Echo received message text
                Message sendMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Salom {message.Chat.Username}",
                    replyToMessageId: update.Message.MessageId,
                    cancellationToken: cancellationToken);
            }
            else if (messageText == "/weather" || messageText == "/obhavo")
            {
                //Echo received message text
                Message sendMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Ob Havo Ma'lumotlarini ko'rish",
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyToMessageId: update.Message.MessageId,
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithUrl(
                            text: "Bugungi Ob havoni ko'rish",
                            url: "https://www.google.com/search?q=ob+havo+toshkent&oq=ob+havo+&gs_lcrp=EgZjaHJvbWUqBwgCEAAYgAQyBggAEEUYOTIPCAEQIxgnGJ0CGIAEGIoFMgcIAhAAGIAEMgcIAxAAGIAEMgcIBBAAGIAEMgcIBRAAGIAEMgcIBhAAGIAEMgcIBxAAGIAEMgcICBAAGIAEMgcICRAAGIAE0gEIMzU2OGowajeoAgCwAgA&sourceid=chrome&ie=UTF-8")
                        ),
                    cancellationToken: cancellationToken);
            }
            else if (messageText == "/start")
            {
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                        {
                            InlineKeyboardButton.WithUrl("Youtube", "https://www.youtube.com/"),
                            InlineKeyboardButton.WithUrl("Google", "https://www.google.com/"),
                            InlineKeyboardButton.WithUrl("Instagram", "https://www.instagram.com/"),
                            InlineKeyboardButton.WithUrl("GitHub", "https://www.github.com/"),
                            InlineKeyboardButton.WithUrl("Linkedin", "https://www.Linkedin.com/"),
                            InlineKeyboardButton.WithUrl("dotnet uz", "https://docs.dot-net.uz/"),
                        });

                    
                //Echo received message text
                Message sendMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Dollar kursini ko'rvolish",
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyToMessageId: update.Message.MessageId,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }



            //Echo received message text
            //Message sentMessage2 = await botClient.SendTextMessageAsync(
            //    chatId: chatId,
            //    text: $"Bio Ketyabdi:\n + {messageText} => {message.Chat.Bio}",
            //    cancellationToken: cancellationToken);
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
