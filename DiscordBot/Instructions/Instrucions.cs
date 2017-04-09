using Discord;
using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DiscordBot.Instructions
{
    public class Instrucions : MyBot
    {
        // 說明指令Json檔案
        public static string jsonHelper = "../../Instructions/Instructions_Helper.json";
        // BOT回答答案
        public static string[] BotAnswer = { "Yes", "Maybe", "No"};
        // 隨機種子產生器
        public static Random random = new Random(Guid.NewGuid().GetHashCode());
        // ========================== 說明指令集 ===============================
        public static void ResponseMessage_Helper(string messages, MessageEventArgs e)
        {
            string jsonString = "";
            try { jsonString = File.ReadAllText(jsonHelper); } catch { }
            JObject jsonObject = JObject.Parse(jsonString);

            switch (messages)
            {
                case "!Agr_hello":
                    {
                        e.Message.Channel.SendMessage(jsonObject.GetValue("Agr_hello").ToString());
                    }
                    break;
                case "!Agr_卯月":
                    {
                        e.Message.Channel.SendMessage(jsonObject.GetValue("Agr_卯月").ToString());
                    }
                    break;
                case "!Agr_gatya":
                    {
                        e.Message.Channel.SendMessage(jsonObject.GetValue("Agr_gatya").ToString());
                    }
                    break;
                case "!Agr_ask":
                    {
                        e.Message.Channel.SendMessage(jsonObject.GetValue("Agr_ask").ToString());
                    }
                    break;
            }
        }
        // =====================================================================
        // ======================== 單一字詞指令集 =============================
        public static void ResponseMessage_Single(string messages, MessageEventArgs e)
        {
            switch (messages)
            {
                case "!Agr_Hello":
                    {
                        e.Message.Channel.SendMessage("```World!```");
                    }
                    break;
                case "!Agr_卯月":
                    {
                        e.Message.Channel.SendFile("../../envelope/QQ.jpg");
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
                case "!Agr_gatya":
                    {
                        if (Convert.ToInt32(param) > 11) return;
                        int randNum = 0;
                        string output = "";
                        for (int i = 0; i < Convert.ToInt32(param); i++)
                        {
                            randNum = random.Next(100);
                            output += randNum.ToString() + " ";
                        }
                        e.Message.Channel.SendMessage("`" + output + "`");
                    }
                    break;
                case "!Agr_ask":
                    {
                        string output = e.Message.User + " : " + param + " ?\nAnswer Is : " + BotAnswer[random.Next(3)];
                        e.Message.Channel.SendMessage("`" + output + "`");
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
