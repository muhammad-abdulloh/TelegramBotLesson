using System.Security.Cryptography.X509Certificates;
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
            //// Only process text messages
            //if (message.Text is not { } messageText)
            //    return;



            var chatId = update.Message.Chat.Id;
           // var message = update.Message;

            Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}. UserName =>  {message.Chat.Username}");

            //var fileStream = new FileStream(@"C:\Users\dotnetbillioner\Videos\test.mp4", FileMode.Open);
            Console.WriteLine($"Message Type: {message.Type}  => Type {MessageType.Sticker} ");
            /*
            if (message.Type == MessageType.Photo)
            {

                Console.WriteLine("Nima gaapppppp");

                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: InputFile.FromFileId(message.Photo.Last()!.FileId),
                    cancellationToken: cancellationToken);
            }

            else if (message.Text == "albom")
            {
                await botClient.SendMediaGroupAsync(
                    chatId: chatId,
                    media: new IAlbumInputMedia[]
                    {
                        new InputMediaPhoto(
                            InputFile.FromUri("https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg")),
                        new InputMediaPhoto(
                            InputFile.FromUri("https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg")),
                    },
                    cancellationToken: cancellationToken);
            }
            */ //6198571336
            //if (message.Text == "pool")
            //{
            //    await botClient.SendPollAsync(
            //        chatId: "-1001915953272",
            //        question: "Did you ever hear the tragedy of Darth Plagueis The Wise?",
            //        options: new[]
            //        {
            //            "Yes for the hundredth time!",
            //            "No, who`s that?",
            //            "Nope, who is that?",
            //        },
            //        cancellationToken: cancellationToken);
            //}

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

            


                /**

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
                else if (messageText == "/sdfsdafsadfasd")
                {
                    var fileStream = new FileStream(@"C:\Users\dotnetbillioner\Videos\test.mp4", FileMode.Open);

                    await botClient.SendVideoAsync(
                    chatId: chatId,
                    video: InputFile.FromFileId(message.Video!.FileId),
                    thumbnail: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/2/docs/thumb-clock.jpg"),
                    supportsStreaming: true,
                    cancellationToken: cancellationToken);
                }
                else if (messageText == "/rasm")
                {
                    var fileStream = new FileStream(@"C:\Users\dotnetbillioner\Pictures\dotnet.png", FileMode.Open);

                    await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: InputFile.FromStream(fileStream),
                    cancellationToken: cancellationToken);
                }
                */



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
