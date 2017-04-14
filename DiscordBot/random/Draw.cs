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
    public class Draw : MyBot
    {

		//private class Card
		//{
		//	public string name = "" ;
  //          public string type = "" ;
		//}


  //      private static Random random = new Random(Guid.NewGuid().GetHashCode());
  //      private static string imagedir = "../../data/images/envelope/";
  //      private static string bounsSSR = "BounsSSR/";
  //      private static string dir_SSR = "SSR/";
  //      private static string dir_SR = "SR/";
  //      private static string dir_R = "R/";
  //      private static string version = "version.json";
  //      private static string jsonstr = "";

  //      public static void DrawCardCommand()
  //      {
  //          int user_drawcount = 0 ;

  //          commands.CreateCommand("draw").Parameter("message", ParameterType.Multiple).Do(async (e) =>
  //          {

  //              if ( ( user_drawcount = await DrawOptionAsync(e) ) == -1 ) 
		//			return ;

                

  //              Image mergePic = null;
  //              Image temp_mergePic = null;
  //              int countSSR = 0, countSR = 0;
				
		//		for (int i = 0; i < user_drawcount; i++)
		//		{
		//			Card card = GetACard() ;
						
		//			/* what user gets a card type? */
		//			if ( card.type.Equals("SSR") ) countSSR++ ;
		//			else if ( card.type.Equals("SR") ) countSR++ ;
					
		//			/* get the image of card and then mergeImages */
		//			if ( i == 0 )
		//			{
		//				mergePic = Image.FromFile( card.name );
		//			}
		//			else if ( i == 5 )
		//			{
		//				temp_mergePic = mergePic;
		//				mergePic = Image.FromFile( card.name );
		//			}
		//			else if ( i >= 1 )
		//			{
		//				Image tempPic = Image.FromFile( card.name );
		//				mergePic = HorizontalMergeImages(mergePic, tempPic);
		//				tempPic.Dispose();
		//			}
		//		} 
		//		if ( user_drawcount == 10 ) mergePic = VerticalMergeImages(temp_mergePic, mergePic); // mergePic 是後來抽到的5~10張


  //              /* SendMessage */
  //              await e.Channel.SendMessage(e.User.NicknameMention);
  //              if (user_drawcount == 1)
		//		{
  //                  await e.Channel.SendFile(imagedir + "merge.Png", ToStream(mergePic, ImageFormat.Png));
  //                  if (countSSR == 1)
		//				await e.Channel.SendMessage(":fire:");
		//			else if (countSR == 1)
		//				await e.Channel.SendMessage(":see_no_evil:");
                    
  //              } 
		//		else if (user_drawcount == 10)
		//		{
		//			if (countSSR >= 1)
		//			{
  //                      await e.Channel.SendFile(imagedir + "merge.Png", ToStream(mergePic, ImageFormat.Png));
  //                      await e.Channel.SendMessage(":fire::fire::fire::fire::fire::fire::fire::fire::fire::fire::fire::fire:");
  //                  }
		//			else if (countSR >= 2)
		//			{
  //                      await e.Channel.SendFile(imagedir + "merge.Png", ToStream(mergePic, ImageFormat.Png));
  //                      await e.Channel.SendMessage(":see_no_evil::see_no_evil:");
  //                  } 
		//			else
		//			{
		//				await e.Channel.SendFile(imagedir + "QQ.jpg");
		//			}
		//		}
		//		else
		//			Console.WriteLine("Bug:\n Draw.cs:  " + "user_drawcount" );
				

				
  //              mergePic.Dispose();
  //              temp_mergePic.Dispose();
  //          });
  //      }

		//private static Card GetACard()
		//{
  //          Card card = new Card() ; 
  //          List<string> myCardPool = new List<string>();
  //          int cardType = random.Next(1000);
  //          if (cardType >= 1 && cardType <= 6)
  //          {
  //              /* 期間SSR!! */
  //              // 取得bounsSSR資料夾內所有檔案
  //              card.type = "SSR";
  //              foreach (string fname in Directory.GetFiles(imagedir + bounsSSR))
  //              {
  //                  myCardPool.Add(fname.Trim());
  //              }
  //          }
  //          else if (cardType >= 7 && cardType <= 15)
  //          {
  //              /* SSR!! */
  //              // 取得SR資料夾內所有檔案
  //              card.type = "SSR";
  //              foreach (string fname in Directory.GetFiles(imagedir + dir_SSR))
  //              {
  //                  myCardPool.Add(fname.Trim());
  //              }
  //          }
  //          else if (cardType >= 16 && cardType <= 115)
  //          {
  //              /* SR!! */
  //              // 取得SR資料夾內所有檔案
  //              card.type = "SR";
  //              foreach (string fname in Directory.GetFiles(imagedir + dir_SR))
  //              {
  //                  myCardPool.Add(fname.Trim());
  //              }
  //          }
  //          else
  //          {
  //              /* R */
  //              // 取得R資料夾內所有檔案
		//						card.type = "R" ;
  //              foreach (string fname in Directory.GetFiles(imagedir + dir_R))
  //              {
  //                  myCardPool.Add(fname.Trim());
  //              }
  //          }

  //          Console.WriteLine("myCardPool.count :" + myCardPool.Count);
						
  //          card.name = myCardPool[random.Next(myCardPool.Count)];
  //          Console.WriteLine("card :" + card.name);
		//				myCardPool.Clear();
		//				return card ;	
		//} 
				
  //      private static Stream ToStream( Image image, ImageFormat format)
  //      {
  //          var stream = new System.IO.MemoryStream();
  //          image.Save(stream, format);
  //          stream.Position = 0;
  //          return stream;
  //      }

  //      private static async Task<int> DrawOptionAsync(CommandEventArgs e)
  //      {

  //          if (e.Args.Length == 0)
  //          {
  //              return 1; //執行單抽
  //          }

  //          else if (e.Args[0].ToLower().Equals("10"))
  //          {
  //              return 10; //執行10連
  //          }
  //          else if (e.Args[0].ToLower().Equals("-v"))
  //          {
  //              try { jsonstr = File.ReadAllText(imagedir + version); } catch { }
  //              JObject jsonobj = JObject.Parse(jsonstr);
  //              jsonobj.GetValue("cardpoolname");
  //              jsonobj.GetValue("date");
  //              string versionmessage = "卡池名稱:\n    " + jsonobj.GetValue("cardpoolname") + "\n\n";
  //              versionmessage += "日期:\n    " + jsonobj.GetValue("date") + "\n\n";
  //              versionmessage += "機率:" + "\nSSR " + jsonobj.GetValue("SSRProb").ToString().PadLeft(8) + "\nSR  " + jsonobj.GetValue("SRProb").ToString().PadLeft(8) + "\nR   " + jsonobj.GetValue("RProb").ToString().PadLeft(8) + "\n";

  //              await e.Channel.SendMessage("```" + versionmessage + "```");
  //              return -1;
  //          }
  //          else if (e.Args[0].ToLower().Equals("-help"))
  //          {
  //              string help_information = "```";
  //              help_information += "此為!draw參數說明\n";
  //              help_information += "-v      :可以看現在更新的卡池日期\n";
  //              help_information += "10      :10連抽\n";
  //              help_information += "```";
  //              await e.Channel.SendMessage(help_information);
  //              return -1;
  //          }
  //          else
  //          {
  //              await e.Channel.SendMessage("```請打 \"!draw -help\" 看那些參數可使用\n```");
  //              return -1;
  //          }
  //      }

  //      private static Image VerticalMergeImages(Image img1, Image img2)
  //      {
  //          Image MergedImage = default(Image);
  //          Int32 Wide = 0;
  //          Int32 High = 0;
  //          int space = 20; // definition
  //          High = img1.Height + img2.Height + space;//設定高度          
  //          if (img1.Width >= img2.Width)
  //          {
  //              Wide = img1.Width;
  //          }
  //          else
  //          {
  //              Wide = img2.Width;
  //          }
  //          Bitmap mybmp = new Bitmap(Wide, High);
  //          Graphics gr = Graphics.FromImage(mybmp);
  //          //處理第一張圖片
  //          gr.DrawImage(img1, 0, 0);
  //          //處理第二張圖片
  //          gr.DrawImage(img2, 0, img1.Height + space);
  //          MergedImage = mybmp;
  //          gr.Dispose();
  //          return MergedImage;

  //      }

  //      private static Image HorizontalMergeImages(Image img1, Image img2)
  //      {

  //          Image MergedImage = default(Image);
  //          Int32 Wide = 0;
  //          Int32 High = 0;
  //          int space = 20; // definition
  //          Wide = img1.Width + img2.Width + space;//設定寬度           
  //          if (img1.Height >= img2.Height)
  //          {
  //              High = img1.Height;
  //          }
  //          else
  //          {
  //              High = img2.Height;
  //          }
  //          Bitmap mybmp = new Bitmap(Wide, High);
  //          Graphics gr = Graphics.FromImage(mybmp);
  //          //處理第一張圖片
  //          gr.DrawImage(img1, 0, 0);
  //          //處理第二張圖片
  //          gr.DrawImage(img2, img1.Width + space, 0);
  //          MergedImage = mybmp;
  //          gr.Dispose();
  //          return MergedImage;
  //      }
    }

}
