using System;
using System.Collections.Generic;

namespace MohmalAPI.Test
{
    internal class Program
    {
        static void Main()
        {
            MohmalSession session = new MohmalSession();
            string email = session.Create();

            Console.WriteLine($"Current Email: {email}\n");

            while (true)
            {
                Console.Write("Press any key to check for new emails.");

                Console.ReadLine();
                List<MohmalMessage> messages = session.GetMessages();

                if (messages.Count == 0)
                    Console.WriteLine("You don't have any messages.");
                else
                {
                    foreach (MohmalMessage message in messages)
                    {
                        Console.WriteLine($"---- Message ----\n");
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"Sender: {message.Sender}");
                        Console.WriteLine($"Message Content: {message.Content}\n");
                        Console.WriteLine($"---- Message ----");
                    }
                }
            }
        }
    }
}