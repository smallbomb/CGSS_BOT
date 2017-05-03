using Discord;
using System;
using System.IO;

namespace DiscordBot.Instructions
{
    public class PickSong : MyBot
    {
        // 說明指令Json檔案
        public static string jsonSongs = "../../data/songs/";
        // 隨機種子產生器
        public static Random random = new Random(Guid.NewGuid().GetHashCode());

        public static void ResponseMessage_Helper(MessageEventArgs e)
        {
            string helpStr = "```請使用!Agr_AddSong建立歌曲, 格式為 !Agr_AddSong [曲名] [屬性]";
            helpStr += "\n!Agr_EditSong編輯歌曲資訊, 格式為 !Agr_EditSong [曲名] [屬性] [Master等級] [MasterNote數] ... (不含M+)";
            helpStr = "\n```請使用!Agr_ShowSong顯示歌曲資訊, 格式為 !Agr_ShowSong [曲名] [屬性]";
            helpStr += "\n[屬性] 包含 cu  pa  co  all 四種";
            helpStr += "\n抽歌及美化功能製作中...```";
            e.Channel.SendMessage(helpStr);
        }

        public static void ReceiveMessage(string[] messages, MessageEventArgs e)
        {
            if (messages[0] == "!Agr_Song")
            {
                ResponseMessage_Helper(e);
            }
            if (messages.Length == 3)
            {
                if (messages[0] == "!Agr_ShowSong")
                {
                    if (messages[2] == "パッション" || messages[2] == "キュート" || messages[2] == "クール" || messages[2] == "全タイプ")
                        ShowSongInfo(messages[1], messages[2], e);
                }
            }
            if (messages.Length == 3)
            {
                if (messages[0] == "!Agr_AddSong")
                {
                    if (messages[2] == "パッション" || messages[2] == "キュート" || messages[2] == "クール" || messages[2] == "全タイプ")
                        CreateSongs(messages[1], messages[2], e);
                }
            }
            if (messages.Length == 12)
            {
                if (messages[0] == "!Agr_EditSong")
                {
                    SetSongInfo(messages, e);
                }
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
        // ============================ 建構式 =================================
        public static void SetSongs()
        {
            MessageParser();            // 回應文字指令
        }
        // =====================================================================
        public static void CreateSongs(string songName, string type, MessageEventArgs e)
        {
            if (!File.Exists(jsonSongs + type + "/" + songName))
            {
                StreamWriter sw = new StreamWriter(jsonSongs + type + "/" + songName + ".json");
                sw.WriteLine("{");
                sw.WriteLine("  \"TYPE\": \"" + type + "\",");
                sw.WriteLine("  \"MLV\": 0,");
                sw.WriteLine("  \"MNOTES\": 0,");
                sw.WriteLine("  \"PLV\": 0,");
                sw.WriteLine("  \"PNOTES\": 0,");
                sw.WriteLine("  \"RLV\": 0,");
                sw.WriteLine("  \"RNOTES\": 0,");
                sw.WriteLine("  \"DLV\": 0,");
                sw.WriteLine("  \"DNOTES\": 0,");
                sw.WriteLine("  \"AVA\": \"\"");
                sw.Write("}");
                sw.Close();
                e.Channel.SendMessage("建立" + songName + " ，請使用!Agr_EditSong編輯歌曲資訊");
            }
        }
        public static void SetSongInfo(string[] message, MessageEventArgs e)
        {
            string json = "";
            try { json = File.ReadAllText(jsonSongs + message[2] + "/" + message[1] + ".json"); } catch { }
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["MLV"] = Convert.ToInt32(message[9]);
            jsonObj["MNOTES"] = Convert.ToInt32(message[10]);
            jsonObj["PLV"] = Convert.ToInt32(message[7]);
            jsonObj["PNOTES"] = Convert.ToInt32(message[8]);
            jsonObj["RLV"] = Convert.ToInt32(message[5]);
            jsonObj["RNOTES"] = Convert.ToInt32(message[6]);
            jsonObj["DLV"] = Convert.ToInt32(message[3]);
            jsonObj["DNOTES"] = Convert.ToInt32(message[4]);
            jsonObj["AVA"] = message[11];
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonSongs + message[2] + "/" + message[1] + ".json", output);
        }
        public static void ShowSongInfo(string songName, string type, MessageEventArgs e)
        {
            string json = File.ReadAllText(jsonSongs + type + "/" + songName + ".json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            string history = songName + "        " + jsonObj["TYPE"];
            history += "\n\n:regional_indicator_m:  " + GetNumberFullString(Convert.ToInt32(jsonObj["MLV"])) + "  :musical_note:  " + GetNumberFullString(Convert.ToInt32(jsonObj["MNOTES"]));
            history += "\n\n:regional_indicator_p:  " + GetNumberFullString(Convert.ToInt32(jsonObj["PLV"])) + "  :musical_note:  " + GetNumberFullString(Convert.ToInt32(jsonObj["PNOTES"]));
            history += "\n\n:regional_indicator_r:  " + GetNumberFullString(Convert.ToInt32(jsonObj["RLV"])) + "  :musical_note:  " + GetNumberFullString(Convert.ToInt32(jsonObj["RNOTES"]));
            history += "\n\n:regional_indicator_d:  " + GetNumberFullString(Convert.ToInt32(jsonObj["DLV"])) + "  :musical_note:  " + GetNumberFullString(Convert.ToInt32(jsonObj["DNOTES"]));
            history += "\n\n" + jsonObj["AVA"];

            e.Channel.SendMessage(history);
        }
        public static string GetNumberFullString(int num)
        {
            string nums = "";
            int bits = 3;
            if (num < 100) bits = 2;
            int DisplayNumber = 0;
            for (int index = bits - 1; index >= 0; index--)
            {
                DisplayNumber = (num % ((int)Math.Pow(10, bits - index))) / ((int)Math.Pow(10, bits - index - 1));
                nums += GetNumberString(DisplayNumber) + " ";
            }
            return nums;
        }
        public static string GetNumberString(int num)
        {
            string nums = "";
            switch (num)
            {
                case 0:
                    nums = ":zero:";
                    break;
                case 1:
                    nums = ":one:";
                    break;
                case 2:
                    nums = ":two:";
                    break;
                case 3:
                    nums = ":three:";
                    break;
                case 4:
                    nums = ":four:";
                    break;
                case 5:
                    nums = ":five:";
                    break;
                case 6:
                    nums = ":six:";
                    break;
                case 7:
                    nums = ":seven:";
                    break;
                case 8:
                    nums = ":eight:";
                    break;
                case 9:
                    nums = ":nine:";
                    break;
            }
            return nums;
        }
    }
}
