using Discord;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;

namespace DiscordBot.AgrGetya
{
    public class CGGetya : MyBot
    {
        public static Random random = new Random(Guid.NewGuid().GetHashCode());
        public static string imagedir = "../../envelope/";
        public static string bounsSSR = "bounsSSR/";
        public static string dir_SSR = "SSR/";
        public static string dir_SR = "SR/";
        public static string dir_R = "R/";
        public static void CGGetyaCommand(MessageEventArgs e)
        {
            Image mergePic = null;
            Image temp_mergePic = null;
            List<string> myCardPool = new List<string>();
            int countBounceSSR = 0, countSSR = 0, countSR = 0, countR = 0;
            int getSSR = 0, getSR = 0;
            foreach (string fname in Directory.GetFiles(imagedir + bounsSSR))
            {
                countBounceSSR++;
                myCardPool.Add(fname.Trim());
            }
            foreach (string fname in Directory.GetFiles(imagedir + dir_SSR))
            {
                countSSR++;
                myCardPool.Add(fname.Trim());
            }
            foreach (string fname in Directory.GetFiles(imagedir + dir_SR))
            {
                countSR++;
                myCardPool.Add(fname.Trim());
            }
            foreach (string fname in Directory.GetFiles(imagedir + dir_R))
            {
                countR++;
                myCardPool.Add(fname.Trim());
            }
            for (int i = 0; i < 10; i++)
            {
                int cardGetya = random.Next(1000);
                string card;
                if (cardGetya < 7)
                {
                    getSSR++;
                    card = myCardPool[random.Next(countBounceSSR)];
                }
                else if (cardGetya < 16)
                {
                    getSSR++;
                    card = myCardPool[random.Next(countSSR) + countBounceSSR];
                }
                else if (cardGetya < 116)
                {
                    getSR++;
                    card = myCardPool[random.Next(countSR) + countSSR + countBounceSSR];
                }
                else
                {
                    card = myCardPool[random.Next(countR) + countSR + countSSR + countBounceSSR];
                }

                if (i == 0)
                {
                    mergePic = Image.FromFile(card);
                }
                else if (i == 5)
                {
                    temp_mergePic = mergePic;
                        mergePic = Image.FromFile(card);
                }
                else if (i >= 1 && i <= 8)
                {
                    Image tempPic = Image.FromFile(card);
                    mergePic = HorizontalMergeImages(mergePic, tempPic);
                    tempPic.Dispose();
                }
                else if(i == 9)
                {
                    if(getSSR == 0 && getSR == 0)
                    {
                        card = myCardPool[random.Next(countSR) + countSSR + countBounceSSR];  
                    }
                    Image tempPic = Image.FromFile(card);
                    mergePic = HorizontalMergeImages(mergePic, tempPic);
                    tempPic.Dispose();
                }


            }
            mergePic = VerticalMergeImages(temp_mergePic, mergePic); // mergePic 是後來抽到的5~10張
            e.Channel.SendFile(imagedir + "merge.Png", ToStream(mergePic, ImageFormat.Png));

            if (getSSR > 0)
            {
                e.Channel.SendMessage(":fire::fire::fire::fire::fire::fire::fire::fire::fire:");
            }
            else if (getSR > 1)
            {
                e.Channel.SendMessage(":see_no_evil::see_no_evil:");
            }
            else
            {
                e.Channel.SendFile(imagedir + "QQ.jpg");
            }
            mergePic.Dispose();
            temp_mergePic.Dispose();
        }

        private static Stream ToStream(Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
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