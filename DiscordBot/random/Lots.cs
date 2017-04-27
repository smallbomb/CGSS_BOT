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

                if (ask.Equals("大凶")) 
                    lots = "大凶.png";
                else if (ask.Equals("吉")) 
                    lots = "吉.png";
                else if (ask.Equals("中吉"))
                    lots = "中吉.png";
                else if (ask.Equals("兇"))
                    lots = "兇.png";
                else if (ask.Equals("小吉"))
                    lots = "小吉.png";
                else if (ask.Equals("大吉"))
                    lots = "大吉.png";
                else if(lotsNumber >= 0 && lotsNumber <= 8) // 8-0+1 = 9%
                    lots = "大凶.png";
                else if (lotsNumber >= 9 && lotsNumber <= 32) // 32-9+1 = 24%
                    lots = "吉.png";
                else if (lotsNumber >= 33 && lotsNumber <= 48)
                    lots = "中吉.png";
                else if (lotsNumber >= 49 && lotsNumber <= 69)
                    lots = "兇.png";
                else if (lotsNumber >= 70 && lotsNumber <= 82)
                    lots = "小吉.png";
                else if (lotsNumber >= 83 && lotsNumber <= 99)
                    lots = "大吉.png";

                Image img = Image.FromFile(imagedir + lots);


                await e.Channel.SendMessage(e.User.Mention + "想問: \"" + ask + "\"");
                await e.Channel.SendFile(lots, Draw.ToStream(img, ImageFormat.Png));
                img.Dispose();
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
                help_information += "!lots \"吉\"\n";
                help_information += "!lots \"大凶\"...可以直接選該簽\n";
                help_information += "\n感謝Cior圖源提供\n";
                help_information += "```";
                await e.Channel.SendMessage(help_information);
                return null;
            }

            // message combine
            string str = "";
            for ( int i = 0; i < e.Args.Length; i++ ) 
                str += e.Args[i].ToString()+" ";

            
            return str.Trim();
        }

 


    }
}
