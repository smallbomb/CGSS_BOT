using Discord.Commands;

using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DiscordBot.random
{
    public class Draw : MyBot
    {
        

        public static void DrawCardCommand()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string imagedir = "../../envelope/";
            string bounsSSR = "bounsSSR/";
            string dir_SSR = "SSR/";
            string dir_SR = "SR/";
            string dir_R = "R/";
            string version = "version.txt";

            commands.CreateCommand("draw").Parameter("message", ParameterType.Multiple).Do(async (e) =>
            {

                if ( e.Args.Length == 0 )
                {
                    ; //正常執行10連
                }
                else if ( e.Args[0].ToLower().Equals("-v") )
                {
                    if (!File.Exists(imagedir + version))
                        Console.WriteLine("!drew File open error");
                    else 
                        await e.Channel.SendMessage( "```" + File.ReadAllText( imagedir+version ) + "```" );
                    return;
                }
                else if ( e.Args[0].ToLower().Equals("-help") )
                {
                    string help_information = "```";
                    help_information += "此為!draw參數說明\n";
                    help_information += "-v      :可以看現在更新的卡池日期\n";
                    help_information += "```";
                    await e.Channel.SendMessage(help_information);
                    return;
                }
                else
                {
                    await e.Channel.SendMessage("```請打 \"!draw -help\" 看那些參數可使用\n```");
                    return;
                }


                    //Console.WriteLine("e.User.NicknameMention: " + e.User.NicknameMention);
                    await e.Channel.SendMessage(e.User.NicknameMention);
                Image mergePic = null;
                int countSSR = 0, countSR = 0;

                for (int i = 0; i < 10; i++)
                {
                    List<string> myCardPool = new List<string>();
                    int cardType = random.Next(1000);
                    if (cardType >= 1 && cardType <= 6)
                    {

                        /* 期間SSR!! */
                        // 取得bounsSSR資料夾內所有檔案
                        countSSR++;
                        foreach (string fname in Directory.GetFiles(imagedir + bounsSSR))
                        {
                            myCardPool.Add(fname.Trim());
                        }
                    }
                    else if (cardType >= 7 && cardType <= 15)
                    {
                        /* SSR!! */
                        // 取得SR資料夾內所有檔案
                        countSSR++;
                        foreach (string fname in Directory.GetFiles(imagedir + dir_SSR))
                        {
                            myCardPool.Add(fname.Trim());
                        }
                    }
                    else if (cardType >= 16 && cardType <= 115)
                    {
                        /* SR!! */
                        // 取得SR資料夾內所有檔案
                        countSR++;
                        foreach (string fname in Directory.GetFiles(imagedir + dir_SR))
                        {
                            myCardPool.Add(fname.Trim());
                        }
                    }
                    else
                    {
                        /* R */
                        // 取得R資料夾內所有檔案
                        foreach (string fname in Directory.GetFiles(imagedir + dir_R))
                        {
                            myCardPool.Add(fname.Trim());
                        }
                    }


                    Console.WriteLine("myCardPool.count :" + myCardPool.Count);
                    string card = myCardPool[random.Next(myCardPool.Count)];

                    Console.WriteLine("card :" + card);

                    if (i == 0 )
                    {
                        mergePic = Image.FromFile(card);
                    }
                    else if ( i == 5 )
                    {
                        mergePic.Save(imagedir + "merge.jpg");
                        mergePic.Dispose();
                        mergePic = Image.FromFile(card);
                    }
                    else if (i >= 1)
                    {
                        Image tempPic = Image.FromFile(card);
                        mergePic = HorizontalMergeImages(mergePic, tempPic);
                        tempPic.Dispose();
                    }


                    // await e.Channel.SendFile( card );

                }

                
                Image merge1 = Image.FromFile(imagedir + "merge.jpg");
                mergePic = VerticalMergeImages(merge1, mergePic); // mergePic 是後來抽到的5~10張

                merge1.Dispose();
                mergePic.Save(imagedir + "merge.jpg");
                mergePic.Dispose();

                if (countSSR >= 1)
                {
                    await e.Channel.SendFile(imagedir + "merge.jpg");
                    await e.Channel.SendMessage(":fire::fire::fire::fire::fire::fire::fire::fire::fire:");
                }
                else if (countSR >= 2)
                {
                    await e.Channel.SendFile(imagedir + "merge.jpg");
                    await e.Channel.SendMessage(":see_no_evil::see_no_evil:");
                }
                else
                {
                    await e.Channel.SendFile(imagedir + "QQ.jpg");
                }

            });
        }

        private static Image VerticalMergeImages(Image img1, Image img2)
        {
            Image MergedImage = default(Image);
            Int32 Wide = 0;
            Int32 High = 0;
            int space = 20; // definition
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

        private static Image HorizontalMergeImages(Image img1, Image img2)
        {

            Image MergedImage = default(Image);
            Int32 Wide = 0;
            Int32 High = 0;
            int space = 20; // definition
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
    }




}
