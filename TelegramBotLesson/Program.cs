﻿using System.IO.Compression;
using TelegramBotLesson.TelegramBotFolder;

namespace TelegramBotLesson
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string token = "6827225607:AAHMZ2m1n61NuZq7FBq1dkpklz0jJknsJQk";

            TelegramBotHandler handler = new TelegramBotHandler(token);

            try
            {
                await handler.BotHandle();
            }
            catch (Exception ex)
            {
                throw new Exception("nima gap");
            }

            // string startPath = @"D:\dotnetexternal\flyuchun\test";
            // string zipPath = @"D:\dotnetexternal\flyuchun\result.zip";
            //// string extractPath = @".\extract";

            // ZipFile.CreateFromDirectory(startPath, zipPath);

            //ZipFile.ExtractToDirectory(zipPath, extractPath);

        }
    }
}
