using Discord;
using System;

namespace DiscordBot.Instructions
{
    public class Instrucions : MyBot
    {
        public static Random random = new Random(Guid.NewGuid().GetHashCode());
        // ======================== 單一字詞指令集 =============================
        public static void ResponseMessage_Single(string messages, MessageEventArgs e)
        {
            switch (messages)
            {
                case "!hello2":
                    {
                        e.Message.Channel.SendMessage("World!");
                    }
                    break;
                case "!卯月":
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
            switch (messages)
            {
                case "!gatya":
                    {
                        if (Convert.ToInt32(param) > 11) return;
                        int randNum = 0;
                        string output = "";
                        for (int i = 0; i < Convert.ToInt32(param); i++)
                        {
                            randNum = random.Next(100);
                            output += randNum.ToString() + " ";
                        }
                        e.Message.Channel.SendMessage(output);
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
            // 建立新文字頻道的預設訊息
            client.ChannelCreated += (sender, e) =>
            {
                if (e.Channel.Type == ChannelType.Text && !e.Channel.IsPrivate)
                {
                    e.Channel.SendMessage("一個新頻道已經建立，說點話吧!!");
                }
            };
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
