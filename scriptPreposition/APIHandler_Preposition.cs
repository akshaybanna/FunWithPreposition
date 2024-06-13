using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Prepostion
{


    [System.Serializable]
    public class CMS_Data
    {
        public string LevelName;
        public string Title;

        public int NumberOfQuestion_CMS;
        public int MaxRightMove;
        public int MaxWrongMove;

        public float TimePerLevel;

        public bool IsTimerEnable;
        public bool IsRattingEnable;

        public float RattingStar1_Time;
        public float RattingStar2_Time;
        public float RattingStar3_Time;

        public string WrongMove_Msg;
        public string RightMove_Msg;

        public string Instrucation;

        public string Reward1;
        public string Reward2;
        public string Reward3;

        public string Knowledge_Fact;
        // public string Instruction_Window;
        public string How_To_Play;

        public int IsExtended;
    }

    [System.Serializable]
    public class Home_Data
    {
        public int Id;
        public string Icon;
        public string Game_Code;
        public string Game_Name;
        public string Intro_Screen;
        public string Home_Screen_Title;
        public List<string> Home_Screen_Text;
        public string version;
        public string is_active;
    }

    public class APIHandler_Preposition : MonoBehaviour
    {

        public bool IsStatusTrue;
        public static APIHandler_Preposition instance;
        // CMS Variable

        public CMS_Data[] _CMSDabase;
        public Home_Data home_data;
        string BASE_URL = "https://demo2server.com/allen-dev/api/";
        string GETGAMEINFO_URL = "get-game-info?game_id=4&version=1";
        string GETGAMEVERSION_URL = "check-game-imei";

        public IntelliLocator_RootObject CMS_Server_Data;

        // public List<string> QuestionList_Level1;
        public Button MessageButton;
        public bool IsDeviceIdActive;


        private void Awake()
        {
            instance = this;
            // StartCoroutine(CheckGameInfo());
        }
        // Start is called before the first frame update
        public void StartGame()
        {
            UIManager_Preposition.instance.messagePopup.SetActive(false);
            //  callUpdateValueCms();
            StartCoroutine(CheckGameInfo());
            print("call");
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator CheckGameInfo()
        {

            yield return new WaitForSeconds(1f);
           // print(CheckInternetConnection());
            if (CheckInternetConnection())
            {

                yield return StartCoroutine(checkdeviceid());
                if (!IsDeviceIdActive) yield break;
                lable:
                UnityWebRequest www = UnityWebRequest.Get(BASE_URL + GETGAMEINFO_URL);
                yield return www.SendWebRequest();

                if (www.downloadHandler.text == "")
                {
                    goto lable;
                    yield break;
                }
                try
                {
                    JsonUtility.FromJsonOverwrite(www.downloadHandler.text, CMS_Server_Data);
                    var json = LitJson.JsonMapper.ToObject(www.downloadHandler.text);
                    // print("server data" + www.text);
                    if (CMS_Server_Data.status == "true")
                    {
                        SetServerData();
                        SetHomeScreenData();
                        if (!checkSecurity()) yield break;
                        //Aditional Data Parse
                       LevelDataSet(json);
                        IsStatusTrue = true;
                    }
                    else
                    {
                        UIManager_Preposition.instance.message_text.text = CMS_Server_Data.message + " Please Try Again";
                        UIManager_Preposition.instance.messagePopup.SetActive(true);
                        ChanageMessageButtonListener("Retry");
                        MessageButton.onClick.AddListener(StartGame);
                    }
                }
                catch (System.Exception e)
                {
                    UIManager_Preposition.instance.message_text.text = "Server Error: code 5055";
                    UIManager_Preposition.instance.messagePopup.SetActive(true);
                    ChanageMessageButtonListener("Retry");
                    MessageButton.onClick.AddListener(StartGame);
                }
            }
            else
            {
                UIManager_Preposition.instance.message_text.text = "Please check your Internet connection. Please Try Again";
                UIManager_Preposition.instance.messagePopup.SetActive(true);
                ChanageMessageButtonListener("Retry");
                MessageButton.onClick.AddListener(StartGame);

            }
        }
        public void OK_genderSelection(Toggle boy_toggle)
        {

            StartCoroutine(UIManager_Preposition.instance.LoadVideo());
            if (boy_toggle.isOn)
                UIManager_Preposition.instance.IsBoyCharacter = true;
            else
                UIManager_Preposition.instance.IsBoyCharacter = false;

           

            StartGame();
        }

        public void copyButton()
        {


            GUIUtility.systemCopyBuffer = (SystemInfo.deviceUniqueIdentifier);
            ChanageMessageButtonListener("Retry");
            MessageButton.onClick.AddListener(StartGame);

        }
        void ChanageMessageButtonListener(string buttonText)
        {
            MessageButton.onClick.RemoveAllListeners();
            MessageButton.GetComponentInChildren<Text>().text = buttonText;
            MessageButton.onClick.AddListener(SoundManager_Preposition.instanace.buttonclickSound);

        }
        bool checkSecurity()
        {
            print("call" + Application.version.ToString());
            if (home_data.is_active.Contains("0"))
            {
                print("call inder");
                UIManager_Preposition.instance.message_text.text = "Game is deactivate.";
                UIManager_Preposition.instance.messagePopup.SetActive(true);
                ChanageMessageButtonListener("Retry");
                MessageButton.onClick.AddListener(StartGame);
                return false;
            }
            else if (home_data.version != Application.version.ToString())
            {
                UIManager_Preposition.instance.message_text.text = "Game is old version.please download new version.";
                UIManager_Preposition.instance.messagePopup.SetActive(true);
                ChanageMessageButtonListener("Download");
                MessageButton.onClick.AddListener(DownloadButton);
                return false;
            }
            else
                return true;
        }

        void DownloadButton()
        {
            Application.OpenURL("http://testing.demo2server.com/allen-dev/" + CMS_Server_Data.data.android_url);
            Caching.ClearCache();
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }

        IEnumerator checkdeviceid()
        {
            lable:
            WWWForm form = new WWWForm();
            form.AddField("imei", SystemInfo.deviceUniqueIdentifier);
            //UnityWebRequest www = UnityWebRequest.Post(BASE_URL + GETGAMEINFO_URL);
            using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + GETGAMEVERSION_URL, form))
            {
                yield return www.SendWebRequest();
                print(www.downloadHandler.text);
                if (www.downloadHandler.text == "")
                {
                    goto lable;
                    yield break;
                }

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    IsDeviceIdActive = true;
                    //var json = LitJson.JsonMapper.ToObject(www.downloadHandler.text);
                    //if (json["status"].ToString() == "false")
                    //{
                    //    UIManager_Preposition.instance.message_text.text = "Your Device is not authorized. your device id is " + SystemInfo.deviceUniqueIdentifier;
                    //    UIManager_Preposition.instance.messagePopup.SetActive(true);
                    //    IsDeviceIdActive = false;
                    //    ChanageMessageButtonListener("Copy");
                    //    MessageButton.onClick.AddListener(copyButton);
                    //}
                    //else
                    //{
                    //    print(true);
                    //    IsDeviceIdActive = true;
                    //}
                    // Debug.Log("Form upload complete!"+ www.downloadHandler.text);
                    yield return null;
                }
            }

            //yield return false;

        }
        public void SetServerData()
        {
            for (int i = 0; i < CMS_Server_Data.data.levels.Count; i++)
            {
                if (CMS_Server_Data.data.levels[i].status == 1)
                {
                    _CMSDabase[i].LevelName = CMS_Server_Data.data.levels[i].title;
                    _CMSDabase[i].Title = CMS_Server_Data.data.levels[i].level_title;

                    _CMSDabase[i].NumberOfQuestion_CMS = CMS_Server_Data.data.levels[i].no_of_questions;
                    _CMSDabase[i].MaxRightMove = CMS_Server_Data.data.levels[i].maximum_correct_moves;
                    _CMSDabase[i].MaxWrongMove = CMS_Server_Data.data.levels[i].allow_wrong_move;

                    _CMSDabase[i].TimePerLevel = float.Parse(CMS_Server_Data.data.levels[i].total_time.ToString());

                    _CMSDabase[i].RattingStar1_Time = float.Parse(CMS_Server_Data.data.levels[i].one_star_time.ToString());
                    _CMSDabase[i].RattingStar2_Time = float.Parse(CMS_Server_Data.data.levels[i].two_star_time.ToString());
                    _CMSDabase[i].RattingStar3_Time = float.Parse(CMS_Server_Data.data.levels[i].three_star_time.ToString());

                    _CMSDabase[i].WrongMove_Msg = CMS_Server_Data.data.levels[i].wrong_move_message;
                    _CMSDabase[i].RightMove_Msg = CMS_Server_Data.data.levels[i].right_move_message;


                    _CMSDabase[i].Knowledge_Fact = CMS_Server_Data.data.levels[i].knowledge_fact;
                    _CMSDabase[i].Instrucation = CMS_Server_Data.data.levels[i].instruction_window;
                    _CMSDabase[i].How_To_Play = CMS_Server_Data.data.levels[i].how_to_play.ToString();

                    _CMSDabase[i].Reward1 = CMS_Server_Data.data.levels[i].one_star_reward.ToString();
                    _CMSDabase[i].Reward2 = CMS_Server_Data.data.levels[i].two_star_reward.ToString();
                   _CMSDabase[i].Reward3 = CMS_Server_Data.data.levels[i].three_star_reward.ToString();


                    _CMSDabase[i].IsExtended = CMS_Server_Data.data.levels[i].is_extended;
                }
            }
        }

        public void SetHomeScreenData()
        {
            home_data.Id = CMS_Server_Data.data.home_page.id;
            home_data.Game_Code = CMS_Server_Data.data.home_page.game_code;
            home_data.Game_Name = CMS_Server_Data.data.home_page.game_name;
            home_data.Intro_Screen = CMS_Server_Data.data.home_page.intro_screen;
            home_data.Home_Screen_Title = CMS_Server_Data.data.home_page.home_screen_title;
            home_data.Home_Screen_Text.Add(CMS_Server_Data.data.home_page.home_screen_text1);
            home_data.Home_Screen_Text.Add(CMS_Server_Data.data.home_page.home_screen_text2);
            home_data.Home_Screen_Text.Add(CMS_Server_Data.data.home_page.home_screen_text3);

            home_data.version = CMS_Server_Data.data.home_page.version;
            home_data.is_active = CMS_Server_Data.data.home_page.is_active;
            List<string> templist = new List<string>();
            for (int i = 0; i < home_data.Home_Screen_Text.Count; i++)
            {

               // print("call");
                //print(home_data.Home_Screen_Text[i].Length);
                if (home_data.Home_Screen_Text[i].Length > 0)
                {
                   // print("aaacall");
                    templist.Add(home_data.Home_Screen_Text[i]);
                    // home_data.Home_Screen_Text.Remove(home_data.Home_Screen_Text[i]);
                }
            }

            home_data.Home_Screen_Text.Clear();
            home_data.Home_Screen_Text = templist;
        }
        public bool CheckInternetConnection()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }
            return true;
        }
        void LevelDataSet(LitJson.JsonData json)
        {
            
            for (int i = 0; i < json["data"]["levels"][0]["additional"].Count; i++)
            {
                if(json["data"]["levels"][0]["additional"][i]["is_preposition"].ToString()=="1")
                _level1Data.RightList.Add(json["data"]["levels"][0]["additional"][i]["preposition"].ToString());
                if (json["data"]["levels"][0]["additional"][i]["is_preposition"].ToString() == "0")
                    _level1Data.wrongList.Add(json["data"]["levels"][0]["additional"][i]["preposition"].ToString());
            }

            for (int i = 0; i < json["data"]["levels"][3]["additional"].Count; i++)
            {
                Data_Deaming _data = new Data_Deaming();
                _data.question = json["data"]["levels"][3]["additional"][i]["sentence"].ToString();
                _data.situtaion = json["data"]["levels"][3]["additional"][i]["situation"].ToString();
                _data.Prepostion_word = json["data"]["levels"][3]["additional"][i]["right_word"].ToString();
                _data.wrong_word1 = json["data"]["levels"][3]["additional"][i]["wrong_word1"].ToString();
                _data.wrong_word2 = json["data"]["levels"][3]["additional"][i]["wrong_word2"].ToString();
                _data.wrong_word3 = json["data"]["levels"][3]["additional"][i]["wrong_word3"].ToString();

                _level4Data._dreamingprepostion.Add(_data);

            }
            for (int i = 0; i < json["data"]["levels"][4]["additional"].Count; i++)
            {
                TimePropositn time = new TimePropositn();
                time.question= json["data"]["levels"][4]["additional"][i]["sentence"].ToString();
                time.type = json["data"]["levels"][4]["additional"][i]["type_of_preposition"].ToString();
                level5Data.Prepostion_type.Add(time);
            }
            }
        public Level1Data _level1Data;
        public Level4Data _level4Data;
        public Level5Data level5Data;

    }
}

#region SERVER DATA ////////////////

[System.Serializable]
public class HomePage
{
    public int id;
    public string icon;
    public string game_code;
    public string game_name;
    public string intro_screen;
    public string home_screen_title;
    public string home_screen_text1;
    public string home_screen_text2;
    public string home_screen_text3;
    public string version;
    public string is_active;
}

[System.Serializable]
public class Level
{
    public int id;
    public string game_code;
    public int game_id;
    public int level;
    public string level_title;
    public string title;
    public string intro_text;
    public string right_move_message;
    public string wrong_move_message;
    public int total_time;
    public int three_star_time;
    public int two_star_time;
    public int one_star_time;
    public int one_star_reward;
    public int two_star_reward;
    public int three_star_reward;
    public int allow_wrong_move;
    public int maximum_correct_moves;
    public int no_of_questions;
    public string instruction_window;
    public string knowledge_fact;
    public string how_to_play;
    public object created_by;
    public object updated_by;
    public int status;
    public int is_extended;
    public string additional_name;
    public string additional_table;
    public string additional_data;
    public string created_at;
    public string updated_at;
    public object deleted_at;
    public object additional;
    public string mission_space;
}

[System.Serializable]
public class Data
{
    public HomePage home_page;
    public string android_url;
    public List<Level> levels;
}

[System.Serializable]
public class IntelliLocator_RootObject
{
    public string status;
    public string message;
    public Data data;
}
[System.Serializable]
public class Level1Data
{
    public List<string> wrongList;
    public List<string> RightList;
}
[System.Serializable]
public class Level4Data
{
    public List<Data_Deaming> _dreamingprepostion;
}

[System.Serializable]
public class Level5Data
{
    public List<TimePropositn> Prepostion_type;
}
[System.Serializable]
public class Data_Deaming
{
    public string question;
    public string Prepostion_word;
    public string wrong_word1;
    public string wrong_word2;
    public string wrong_word3;

    public string situtaion;
}
[System.Serializable]
public class TimePropositn
{
    public string type;
    public string question;
 

   
}
#endregion
