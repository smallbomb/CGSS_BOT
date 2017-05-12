using Discord.Commands;
using DiscordBot.data.database.draw;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnu.Getopt;

namespace DiscordBot.random
{
    public class DrawRank : MyBot
    {
        private static string rank_type = "top";
        private static int DrawCount = 200;
        public static void DrawCardRankCommand()
        {




            commands.CreateCommand("drawRank").Parameter("message", ParameterType.Multiple).Do(async (e) =>
            {
                if (await DrawCardRankOption(e))
                {
                    
                    await DatabaseRankProb(e, rank_type, DrawCount);

                }
                rank_type = "top";
                DrawCount = 200;
            });
        }
        private static async Task<Boolean> DrawCardRankOption(CommandEventArgs e)
        {
            Getopt g = new Getopt("testprog", e.Args, "hs:c:");
            int c;
            while ((c = g.getopt()) != -1)
            {
                switch (c)
                {
                    case 'h':
                        await HelpInformation(e);
                        return false;

                    case 's':
                        if (g.Optarg.ToLower().Equals("top") || g.Optarg.ToLower().Equals("bottom"))
                        {
                            rank_type = g.Optarg;
                        }
                        else
                        {
                            await HelpInformation(e);
                            return false;
                        }
                        break;

                    case 'c':
                        int result;
                        if (int.TryParse(g.Optarg, out result))
                        {
                            DrawCount = result;
                        }
                        else
                        {
                            await HelpInformation(e);
                            return false;
                        }
                        break;

                    default:
                        await HelpInformation(e);
                        return false;



                }


            }
            return true;


        }

        private static async Task HelpInformation(CommandEventArgs e)
        {
            string help_message = "";
            help_message += "```";
            help_message += "此為!drawRank參數說明\n";
            help_message += "-s         :後面帶上'top'或'bottom'參數，top為取最高5名，bottom取最後5名，default是:\"Top\"，       ex:!drawRank -s Top\n";
            help_message += "-c         :後面帶上'數字的參數'，至少要抽多少數量，default:" + DrawCount + "，                                 ex:!drawRank -c 500\n";

            help_message += "\nexample:\n";
            help_message += "   !drawRank                   :取機率最高且至少" + DrawCount + "抽的5位\n";
            help_message += "   !drawRank -c 100            :取機率最高且至少100抽的5位\n";
            help_message += "   !drawRank -c 300 -s bottom  :取機率最低且至少300抽的5位\n";
            help_message += "大小寫無關，若參數不打時則使用default值";
            help_message += "```";
            await e.Channel.SendMessage(help_message);
        }


        private static async Task DatabaseRankProb(CommandEventArgs e, string type, int count)
        {

            using (var context = new discordbotEntities_draw())
            {

                string str = e.Server.Id.ToString();



                IQueryable<personal> entitylist;
                if (type.ToLower().Equals("bottom"))
                {
                    entitylist = context.personal.Where(input => (input.channal == str && input.totaldrawcount >= count)).OrderBy(t => (Math.Round((double)t.SSR / (double)t.totaldrawcount, 4) * 100));
                }
                else
                {
                    entitylist = context.personal.Where(input => (input.channal == str && input.totaldrawcount >= count)).OrderByDescending(t => (Math.Round((double)t.SSR / (double)t.totaldrawcount, 4) * 100));
                }


                string rank_result = "```\n";
                if (type.ToLower().Equals("bottom"))
                    rank_result += "千川ちひろ模擬抽卡 非洲排行榜QQ.......(至少" + DrawCount + "抽的情況下)\n\n";
                else
                    rank_result += "千川ちひろ模擬抽卡 歐洲排行榜.........(至少" + DrawCount + "抽的情況下)\n\n";
                int i = 1;
                foreach (var entity in entitylist)
                {
                    if (i == 6) break;
                    if (entity.totaldrawcount != 0)
                    {
                        rank_result += ((i + ".").PadRight(3) + (Math.Round((double)entity.SSR / (double)entity.totaldrawcount, 4) * 100).ToString("f2").PadLeft(6) + "%").PadRight(15)+ entity.username + "\n";
                    }
                    else
                    {
                        rank_result += ((i + ".").PadRight(3) + "0.00".PadLeft(6) + "%").PadRight(15) + entity.username + "\n";
                    } 

                    i++;
                }
                rank_result += "```";
                await e.Channel.SendMessage(rank_result);
                
            }
        }
    }
}
