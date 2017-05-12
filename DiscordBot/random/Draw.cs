using Discord.Commands;
using DiscordBot.data.database.draw;


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
    public class Draw : MyBot
    {

		private class Card
		{
            public string name = "" ;
            public string type = "" ;
		}


        private static Random random = new Random(Guid.NewGuid().GetHashCode());
        private static string imagedir = "../../data/images/envelope/";
        private static string bounsSSR = "BounsSSR/";
        private static string dir_SSR = "SSR/";
        private static string dir_SR = "SR/";
        private static string dir_R = "R/";
        private static string version = "version.json";
        private static string jsonstr = "";


        public static void DrawCardCommand()
        {
            int user_drawcount = 0;

            commands.CreateCommand("draw").Parameter("message", ParameterType.Multiple).Do(async (e) =>
            {
                if ( ( user_drawcount = await DrawOptionAsync(e) ) == -1 ) 
					return ;

                Image mergePic = null;
                Image temp_mergePic = null;
                int countSSR = 0, countSR = 0;
				
				for (int i = 0; i < user_drawcount; i++)
				{
					Card card = null ;

                    if (e.Args.Length == 2 && e.Args[1].ToLower().Equals("-bug"))
                        card = GetACard("SSR");
                    else if (i == 9 && countSSR < 1 && countSR < 1) // 保底機制
                        card = GetACard("SR");                      // 保底機制 
                    else
                        card = GetACard("anyone");

                    /* what user gets a card type? */
                    if ( card.type.Equals("SSR") ) countSSR++ ;
					else if ( card.type.Equals("SR") ) countSR++ ;
					
					/* get the image of card and then mergeImages */
					if ( i == 0 )
					{
						mergePic = Image.FromFile( card.name );
					}
					else if ( i == 5 )
					{
						temp_mergePic = mergePic;
						mergePic = Image.FromFile( card.name );
					}
					else if ( i >= 1 )
					{
						Image tempPic = Image.FromFile( card.name );
						mergePic = HorizontalMergeImages(mergePic, tempPic, 20);
						tempPic.Dispose();
					}
				} 
				if ( user_drawcount == 10 ) mergePic = VerticalMergeImages(temp_mergePic, mergePic, 20); // mergePic 是後來抽到的5~10張



                /* UpdateDatabase()*/
                if ( (e.Args.Length == 2 && e.Args[1].ToLower().Equals("-bug")) == false )
                     DatabasePersonalUpdate(e, countSSR, countSR, (user_drawcount-countSR-countSSR), user_drawcount);
                /* SendMessage */
                await e.Channel.SendMessage(e.User.NicknameMention);
                await e.Channel.SendFile(imagedir + "merge.Png", ToStream(mergePic, ImageFormat.Png));
				
                mergePic.Dispose();
                temp_mergePic.Dispose();
            });
            

        }

		private static Card GetACard( string str )
		{
            Card card = new Card() ; 
            List<string> myCardPool = new List<string>();
            int cardType;

            if (str.Equals("SSR"))
                cardType = random.Next(15); // cheat: 0~15 include 0
            else
                cardType = random.Next(1000); // normal: 0~1000 include 0

            if ( cardType >= 0 && cardType <= 5) // 6%
            {
                /* 期間SSR!! */
                // 取得bounsSSR資料夾內所有檔案
                card.type = "SSR";
                foreach (string fname in Directory.GetFiles(imagedir + bounsSSR))
                {
                    myCardPool.Add(fname.Trim());
                }
            }
            else if (cardType >= 6 && cardType <= 14) // 14-6+1 = 9%
            {
                /* SSR!! */
                // 取得SR資料夾內所有檔案
                card.type = "SSR";
                foreach (string fname in Directory.GetFiles(imagedir + dir_SSR))
                {
                    myCardPool.Add(fname.Trim());
                }
            }
            else if ( str.Equals("SR") || ( cardType >= 15 && cardType <= 114 ) )
            {
                /* SR!! */
                // 取得SR資料夾內所有檔案
                card.type = "SR";
                foreach (string fname in Directory.GetFiles(imagedir + dir_SR))
                {
                    myCardPool.Add(fname.Trim());
                }
            }
            else
            {
                /* R */
                // 取得R資料夾內所有檔案
                card.type = "R" ;
                foreach (string fname in Directory.GetFiles(imagedir + dir_R))
                {
                    myCardPool.Add(fname.Trim());
                }
            }

            card.name = myCardPool[random.Next(myCardPool.Count)];
            Console.WriteLine("card :" + card.name);
			myCardPool.Clear();
			return card ;	
		} 
				
        private static async Task<int> DrawOptionAsync(CommandEventArgs e)
        {

            if (e.Args.Length == 0)
            {
                return 1; //執行單抽
            }
            else if (e.Args.Length == 1)
            {
                if (e.Args[0].ToLower().Equals("10"))
                {
                    return 10; //執行10連
                }
                else if (e.Args[0].ToLower().Equals("-v"))
                {
                    try { jsonstr = File.ReadAllText(imagedir + version); } catch { }
                    JObject jsonobj = JObject.Parse(jsonstr);
                    jsonobj.GetValue("cardpoolname");
                    jsonobj.GetValue("date");
                    string versionmessage = "卡池名稱:\n    " + jsonobj.GetValue("cardpoolname") + "\n\n";
                    versionmessage += "日期:\n    " + jsonobj.GetValue("date") + "\n\n";
                    versionmessage += "機率:" + "\nSSR " + jsonobj.GetValue("SSRProb").ToString().PadLeft(8) + "\nSR  " + jsonobj.GetValue("SRProb").ToString().PadLeft(8) + "\nR   " + jsonobj.GetValue("RProb").ToString().PadLeft(8) + "\n";

                    await e.Channel.SendMessage("```" + versionmessage + "```");
                    return -1;
                }
                else if (e.Args[0].ToLower().Equals("-help"))
                {
                    string help_information = "```";
                    help_information += "此為!draw參數說明\n";
                    help_information += "-v      :可以看現在更新的卡池日期\n";
                    help_information += "10      :10連抽\n";
                    help_information += "10 -bug :你可以試試看\n";
                    help_information += "-init   :抽卡資料初始化，至少輸入過一次，沒輸入過將不會記錄任何資料\n";
                    help_information += "-prob   :算出目前抽到的SSR機率 不紀錄使用-bug模式\n";
                    help_information += "```";
                    await e.Channel.SendMessage(help_information);
                    return -1;
                }
                else if (e.Args[0].ToLower().Equals("-init"))
                {
                    DatabasePersonalInit(e);
                    await e.Channel.SendMessage(e.User.NicknameMention + " 您的資料已初始化");
                    return -1;
                }
                else if (e.Args[0].ToLower().Equals("-prob"))
                {
                    await DatabasePersonalProb(e);
                    return -1;
                }
            }
            else if (e.Args.Length == 2 && e.Args[0].ToLower().Equals("10") && e.Args[1].ToLower().Equals("-bug"))
            {
                return 10;
            }


            await e.Channel.SendMessage("```請打 \"!draw -help\" 看那些參數可使用\n```");
            return -1;
            
        }

        public static Stream ToStream(Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        private static Image VerticalMergeImages(Image img1, Image img2, int space)
        {
            Image MergedImage = default(Image);
            Int32 Wide = 0;
            Int32 High = 0;
            High = img1.Height + img2.Height + space;//設定高度          
            if (img1.Width >= img2.Width)
            {
                Wide = img1.Width;
            }
            else
            {
                Wide = img2.Width;
            }
            Bitmap mybmp = new Bitmap(Wide, High);
            Graphics gr = Graphics.FromImage(mybmp);
            //處理第一張圖片
            gr.DrawImage(img1, 0, 0);
            //處理第二張圖片
            gr.DrawImage(img2, 0, img1.Height + space);
            MergedImage = mybmp;
            gr.Dispose();
            return MergedImage;

        }

        private static Image HorizontalMergeImages(Image img1, Image img2, int space)
        {

            Image MergedImage = default(Image);
            Int32 Wide = 0;
            Int32 High = 0;
            Wide = img1.Width + img2.Width + space;//設定寬度           
            if (img1.Height >= img2.Height)
            {
                High = img1.Height;
            }
            else
            {
                High = img2.Height;
            }
            Bitmap mybmp = new Bitmap(Wide, High);
            Graphics gr = Graphics.FromImage(mybmp);
            //處理第一張圖片
            gr.DrawImage(img1, 0, 0);
            //處理第二張圖片
            gr.DrawImage(img2, img1.Width + space, 0);
            MergedImage = mybmp;
            gr.Dispose();
            return MergedImage;
        }

        private static void DatabasePersonalInit(CommandEventArgs e)
        {
            using (var context = new discordbotEntities_draw())
            {
                
                string str = e.Server.Id.ToString();
                var entity = context.personal.Where(input => (input.userid == e.User.NicknameMention && input.channal == str) ).FirstOrDefault();

                if (entity != null)
                {
                    entity.username = e.User.Name;
                    entity.SSR = 0;
                    entity.SR = 0;
                    entity.R = 0;
                    entity.totaldrawcount = 0;
                }
                else
                {
                    context.personal.Add(new personal
                    {
                        userid = e.User.NicknameMention,
                        username = e.User.Name,
                        channal = e.Server.Id.ToString(),
                        SSR = 0,
                        SR = 0,
                        R = 0,
                        totaldrawcount = 0
                    });
                }
                context.SaveChanges();
            }
        }

        private static void DatabasePersonalUpdate(CommandEventArgs e, int countSSR, int countSR, int countR, int count)
        {
            using (var context = new discordbotEntities_draw())
            {
                string str = e.Server.Id.ToString();
                var entity = context.personal.Where(input => (input.userid == e.User.NicknameMention && input.channal == str)).FirstOrDefault();
                if (entity != null)
                {
                    entity.username = e.User.Name;
                    entity.SSR += countSSR;
                    entity.SR += countSR;
                    entity.R += countR;
                    entity.totaldrawcount += count;
                }
                context.SaveChanges();
            }
        }

        private static async Task DatabasePersonalProb(CommandEventArgs e)
        {
            using (var context = new discordbotEntities_draw())
            {
                string str = e.Server.Id.ToString();
                var entity = context.personal.Where(input => (input.userid == e.User.NicknameMention && input.channal == str)).FirstOrDefault();
                if (entity != null)
                {
                    if (entity.totaldrawcount == 0)
                    {
                        await e.Channel.SendMessage(e.User.NicknameMention + " SSR機率約為:" + "0.00%" + " 已抽0張");

                    }
                    else
                    {
                        await e.Channel.SendMessage(e.User.NicknameMention + " SSR機率約為:" + (Math.Round((double)entity.SSR / (double)entity.totaldrawcount, 4) * 100).ToString("f2") + "%" + " 已抽" + entity.totaldrawcount + "張" );
                    }

                }
                else
                {
                    await e.Channel.SendMessage(e.User.NicknameMention + " 您還沒有建立資料請輸入:\n!draw -init");
                }
            }
        }

    }

}
