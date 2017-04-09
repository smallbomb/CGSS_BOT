using Discord;
using Discord.Commands;

using DiscordBot.random;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiscordBot
{
    public class MyBot
    {
        public static DiscordClient client { get; private set; }
        public static CommandService commands { get; private set; }
        string discordbotToken = "";


        public MyBot()
        {
            /* init */
            client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;

            });
           
            client.UsingCommands(input =>
            {
                input.PrefixChar = '!';
                input.AllowMentionPrefix = true;
            });

            commands = client.GetService<CommandService>();
            /* init */

            /*
             *  Default info 
             */
            commands.CreateCommand("hello").Do(async (e) =>
                {
                    await e.Channel.SendMessage("Hi! " + e.User.NicknameMention );
                });

            commands.CreateCommand("help").Do(async (e) =>
                {
                    string help_infomation = "現在指令有\n";
                    help_infomation += "!draw       : 10連抽卡\n";


                    help_infomation += "\n\n而每個指令如\n!draw -help\n則有該指令的詳細說明\n"; // 暫定需要做到的功能
                    await e.Channel.SendMessage(help_infomation);
                });
            commands.CreateCommand("千川ちひろ").Do(async (e) =>
            {
                await e.Channel.SendMessage("My boss is rockon590");
            });

            /*
             *  End Default info
             */


            /*
             * Add feature 
             */
            UserInfo();
            Draw.DrawCardCommand();
                ;
                ;
                ;
            /*
             * End feature
             */


            /* 
             * Discord bot login.
             * Need to put last line.
             */
            // Get Bot Token
            if (!File.Exists("token.txt"))
            {
                // Token File Not Exists
                Console.WriteLine("Please create a file called 'token.txt' and paste your token in it!");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
            discordbotToken = File.ReadAllText("token.txt");
            client.ExecuteAndWait(async () =>
            {
                /* discordBot Token */
                if ( discordbotToken == "" )
                {
                    Console.WriteLine("Please add your discordbot key");
                    Console.WriteLine("URL : https://discordapp.com/developers/applications/me");
                }
                    
                await client.Connect( discordbotToken, TokenType.Bot);
            });
        }

        private void UserInfo()
        {
            /*
             * Detect the user joined or left channel 
             */
            client.UserJoined += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;
                await channel.SendTTSMessage(string.Format("{0}  has joined the channel!", user.NicknameMention));

            };

            client.UserLeft += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;
                await channel.SendTTSMessage(string.Format("{0} has left the channel!", user.NicknameMention));

            };

        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}