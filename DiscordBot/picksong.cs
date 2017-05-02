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
            //string helpStr = "```請使用!Agr_AddSong建立歌曲, 格式為 !Agr_AddSong [曲名] [屬性]";
            //helpStr += "\n!Agr_EditSong編輯歌曲資訊, 格式為 !Agr_EditSong [曲名] [屬性] [Master等級] [MasterNote數] ... (不含M+)";
            string helpStr = "\n```請使用!Agr_ShowSong顯示歌曲資訊, 格式為 !Agr_ShowSong [曲名] [屬性]";
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
                    if (messages[2] == "pa" || messages[2] == "cu" || messages[2] == "co" || messages[2] == "all")
                        ShowSongInfo(messages[1], messages[2], e);
                }
            }
            //if (e.User.Id != 285422794416586753) return;
            if (messages.Length == 3)
            {
                if (messages[0] == "!Agr_AddSong")
                {
                    if (messages[2] == "pa" || messages[2] == "cu" || messages[2] == "co" || messages[2] == "all")
                        CreateSongs(messages[1], messages[2], e);
                }
            }
            if (messages.Length == 11)
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
                sw.WriteLine("  \"Type\": \"" + type + "\",");
                sw.WriteLine("  \"MLV\": 0,");
                sw.WriteLine("  \"MNOTES\": 0,");
                sw.WriteLine("  \"PLV\": 0,");
                sw.WriteLine("  \"PNOTES\": 0,");
                sw.WriteLine("  \"RLV\": 0,");
                sw.WriteLine("  \"RNOTES\": 0,");
                sw.WriteLine("  \"DLV\": 0,");
                sw.WriteLine("  \"DNOTES\": 0");
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
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonSongs + message[2] + "/" + message[1] + ".json", output);
        }
        public static void ShowSongInfo(string songName, string type, MessageEventArgs e)
        {
            string json = File.ReadAllText(jsonSongs + type + "/" + songName + ".json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            string history = "\n歌曲名稱: " + songName;
            history += "\nMaster等級: " + jsonObj["MLV"] + "    Note數量: " + jsonObj["MNOTES"];
            history += "\nPro等級: " + jsonObj["PLV"] + "    Note數量: " + jsonObj["PNOTES"];
            history += "\nRegular等級: " + jsonObj["RLV"] + "    Note數量: " + jsonObj["RNOTES"];
            history += "\nDebut等級: " + jsonObj["DLV"] + "    Note數量: " + jsonObj["DNOTES"];

            e.Channel.SendMessage(history);
        }
    }
}
