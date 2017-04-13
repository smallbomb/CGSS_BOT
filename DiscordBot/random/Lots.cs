using Discord.Commands;

using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;

namespace DiscordBot.random
{
    public class Lots : MyBot
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());
        private static string imagedir = "../../data/images/lots/";

        public static void DrawLotsCommand()
        {
            commands.CreateCommand("lots").Parameter("message", ParameterType.Multiple).Do(async (e) =>
            {
                string ask = "";
                if ((ask = await LotsOptionAsync(e)) == null)
                    return;

                int lotsNumber = random.Next(100);
                string lots = "";
                
                if (lotsNumber >= 1 && lotsNumber <= 4)
                    lots = "1.png";
                else if (lotsNumber >= 5 && lotsNumber <= 28)
                    lots = "5.png";
                else if (lotsNumber >= 29 && lotsNumber <= 39)
                    lots = "29.png";
                else if (lotsNumber >= 40 && lotsNumber <= 55)
                    lots = "40.png";
                else if (lotsNumber >= 56 && lotsNumber <= 70)
                    lots = "56.png";
                else if (lotsNumber >= 71 && lotsNumber <= 83)
                    lots = "71.png";
                else if (lotsNumber >= 84 && lotsNumber <= 100)
                    lots = "84.png";

                await e.Channel.SendFile( imagedir + lots );
                await e.Channel.SendMessage(ask);


            });

        }


        private static async Task<string> LotsOptionAsync(CommandEventArgs e)
        {
            if ( e.Args.Length == 0 )
            {
                await e.Channel.SendMessage("```請打 \"!lots -help\" 看指令說明\n```");
                return null;
            }

            else if (e.Args[0].ToLower().Equals("-help"))
            {
                string help_information = "```";
                help_information += "!lots \"你想祈求的內容\"\n";
                help_information += "```";
                await e.Channel.SendMessage(help_information);
                return null;
            }

            // message combine
            string str = "";
            for ( int i = 0; i < e.Args.Length; i++ ) 
                str += e.Args[i].ToString();

            
            return str;
        }

 


    }
}
