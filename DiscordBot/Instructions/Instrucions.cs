using Discord;
using System;
using DiscordBot.AgrGetya;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DiscordBot.Instructions
{
    public class Instrucions : MyBot
    {
        // 說明指令Json檔案
        public static string jsonHelper = "../../Instructions/Instructions_Helper.json";
        // BOT回答答案
        public static int BotAnswerCount = 4;
        public static string[] BotAnswer = { "Yes", "Maybe", "No", "I don't know"};
        // 隨機種子產生器
        public static Random random = new Random(Guid.NewGuid().GetHashCode());
        // ========================== 說明指令集 ===============================
        public static void ResponseMessage_Helper(string messages, MessageEventArgs e)
        {
            string jsonString = "";
            try { jsonString = File.ReadAllText(jsonHelper); } catch { }
            JObject jsonObject = JObject.Parse(jsonString);

            e.Channel.SendMessage(jsonObject.GetValue(messages).ToString());
        }
        // =====================================================================
        // ======================== 單一字詞指令集 =============================
        public static void ResponseMessage_Single(string messages, MessageEventArgs e)
        {
            switch (messages)
            {
                case "!Agr_Hello":
                    {
                        e.Channel.SendMessage("```World!```");
                    }
                    break;
                case "!Agr_卯月":
                    {
                        e.Channel.SendFile("../../envelope/QQ.jpg");
                    }
                    break;
                case "!Agr_gatya":
                    {
                        e.Channel.SendMessage(e.User.NicknameMention);
                        CGGetya.CGGetyaCommand(e);
                    }
                    break;
                default:
                    break;
            }
        }
        // =====================================================================
        // ====================== 夾帶單一參數指令集 ===========================
        public static void ResponseMessage_Double(string messages, string param, MessageEventArgs e)
        {
            // Help 說明指令
            if(param == "help")
            {
                ResponseMessage_Helper(messages, e);
                return;
            }
            switch (messages)
            {
                case "!Agr_ask":
                    {
                        string output =
                            e.Message.User + " : " +
                            param + " ?\nAnswer Is : " +
                            BotAnswer[random.Next(BotAnswerCount)];
                        e.Channel.SendMessage("```" + output + "```");
                    }
                    break;
                case "!Agr_cgid":
                    {
                        e.Channel.SendMessage("https://deresute.me/" + param + "/large");
                    }
                    break;
                default:
                    break;
            }
        }
        // =====================================================================
        // ======================== 切割指令做分類 =============================
        public static void ReceiveMessage(string[] messages, MessageEventArgs e)
        {
            Console.WriteLine(messages.Length);
            switch (messages.Length)
            {
                case 1:
                    ResponseMessage_Single(messages[0], e);
                    break;
                case 2:
                    ResponseMessage_Double(messages[0], messages[1], e);
                    break;
                default:
                    break;
            }
        }
        // =====================================================================
        // =========================== 回應指令 ================================
        public static void MessageParser()
        {
            client.MessageReceived += (sender, e) =>
            {
                // 不處理Bot的訊息
                if (e.Message.User.IsBot)
                {
                    return;
                }
                ReceiveMessage(e.Message.Text.Split(' '), e);
            };
        }
        // =====================================================================
        // ========================= 其餘功能放置處 ============================
        public static void OtherFeatures()
        {
        }
        // =====================================================================
        // ============================ 建構式 =================================
        public static void SetInstrucions()
        {
            MessageParser();            // 回應文字指令

            OtherFeatures();            // Bot 其他功能
        }
        // =====================================================================
    }
}
