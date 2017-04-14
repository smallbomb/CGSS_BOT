
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.random;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DiscordBot
{
    public class MyBot
    {
        private DiscordSocketClient client;

        public MyBot()
        {


        }

        public async Task Start()
        {

            client = new DiscordSocketClient(new DiscordSocketConfig() );

            await client.LoginAsync(TokenType.Bot, "");
            await client.StartAsync();
            /*
            var map = new DependencyMap();
            map.Add(client);

            handler = new CommandHandler();
            await handler.Install(map);
            */



            await Task.Delay(-1);
            
        }

        /*
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            
        }
        */
    }
}