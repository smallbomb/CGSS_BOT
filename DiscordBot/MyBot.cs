using Discord;
using Discord.Commands;

using DiscordBot.random;
using DiscordBot.Instructions;
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
        public static DiscordClient client { get; private set; }
        public static CommandService commands { get; private set; }
        


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
                    
                    await e.Channel.SendMessage("こんにちは　" + e.User.NicknameMention );


                });

            commands.CreateCommand("help").Do(async (e) =>
                {
                    string help_information = "```";
                    help_information += "現在指令有\n";
                    help_information += "!draw       : 抽卡\n";
                    help_information += "!lots       : 求籤\n";
                    help_information += "!Agr_cgid   : 名片功能\n";
                    help_information += "!Agr_ask    : 跟8ball一樣\n";
                    help_information += "\n\n而每個指令如\n!draw -help\n則有該指令的詳細說明\n"; // 暫定需要做到的功能
                    help_information += "Authors:\nrockon590\n";
                    help_information += "Agreerga\n";
                    help_information += "歡迎任何人加入\n";
                    help_information += "```";

                    await e.Channel.SendMessage(help_information);
                });
            commands.CreateCommand("千川ちひろ").Do(async (e) =>
            {
                await e.Channel.SendMessage("rockon590が大好き^_^");
            });

            /*
             *  End Default info
             */

            /*
             * Add feature 
             */
            UserInfo();
            Draw.DrawCardCommand();
            Instrucions.SetInstrucions();
            Lots.DrawLotsCommand() ;
                ;
                ;
            /*
             * End feature
             */


            /* 
             * Discord bot login.
             * Need to put last line.
             */
            client.ExecuteAndWait(async () =>
            {
                // Get discordBot Token from json
                string jsonstr = "";
                try { jsonstr = File.ReadAllText("../../../Certificate.json"); }
                catch
                {
                    Console.WriteLine("ERROR: 沒有找到" + "Certificate.json");
                    Console.ReadLine();
                    return;
                }

                /* discordBot Token */
                JObject jsonobj = JObject.Parse(jsonstr);
				string discordbotToken = jsonobj.GetValue("token").ToString();
                if ( discordbotToken == "" )
                {
                    Console.WriteLine("Please check your token from \"Certificate.json\" file");
                    Console.WriteLine("URL : https://discordapp.com/developers/applications/me");
					Console.ReadLine(); //Pause
					return ;
                }
                    
                await client.Connect( discordbotToken, TokenType.Bot);
                client.SetGame(new Game( jsonobj.GetValue("game").ToString() ));
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
                await channel.SendMessage(string.Format("{0}  has joined the channel!", user.NicknameMention));

            };

            client.UserLeft += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;
                await channel.SendMessage(string.Format("{0} has left the channel!", user.NicknameMention));

            };

        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}