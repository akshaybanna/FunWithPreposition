using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Level3;
using UnityEngine.Video;
using TMPro;
using System.IO;

namespace Prepostion
{
    [Serializable]
    public class lockStuff
    {
        public Text heding;
        public Image button_image;
        public GameObject lockimage;
    }
    public class UIManager_Preposition : MonoBehaviour
    {

        public static UIManager_Preposition instance;

        [Header("Panels Reference")]
        [SerializeField]
        private Panel[] panels;

        [Header("PopUps Reference")]
        [SerializeField]
        public PopUp[] popsUps;

        public bool IsBoyCharacter;

        // loading referances
        [Header("loading Reference")]
        public Image loading_bar;
        float loading_time = 0;

        // GamePlay TopBar referances
        [Header("Top bar in GamePlay Reference")]
        public GameObject Timer_toppanelGameplay;
        public GameObject Retry_life;
        public GameObject[] Star;
        public Text Coin_text;


        public int current_level;

        public Vector2[] levelPointVec2;
        public RectTransform ContentObj;
        public Animator CharacterLevelAnim;
        public GameObject CharacterLevel;
        public Transform Deflaut_CharacterLevel;

        public VideoClip[] GameVideo_boy;
        public VideoClip[] GameVideo_girl;
        public VideoPlayer Videoplayer;
        public Button[] levelbutton;
        public GameObject GameWin_button;

        public float[] LevelValue;
        public Transform Content;
        public Transform [] level_Point;
        public GameObject Fade;
        public GameObject Character_Logo;
        public Sprite[] BoySprite;

        public Text KnowlegdeFact_text;
        public Text HowToPlayText;
        public VideoPlayer HowToplayVedio;
        public VideoClip[] howToPlayClip;

        // cmsData
        public Animator MsgBox_levelMenu;
        public GameObject MsgButtonTap;
        public Text[] Level_name;
        public Button setting_butt;
        public Color[] LockedColor;
        public lockStuff [] lock_stuff;
        //public GameObject []lock_image;

        //===========


        AssetBundleCreateRequest bundle;
        AssetBundle assetbundle;
      public  IEnumerator LoadVideo()
        {
            string filename = "4_vid";

            string tempPath = (Application.persistentDataPath + "/AssetData");
            tempPath = Path.Combine(tempPath, filename + ".unity");

            if (File.Exists(tempPath))
            {

                bundle = AssetBundle.LoadFromFileAsync(tempPath);
                yield return bundle;

                assetbundle = bundle.assetBundle;

                Debug.Log(assetbundle == null ? "failed" : "Success");


                print("assetbundle.isStreamedSceneAssetBundle");
                string[] scenePaths = assetbundle.GetAllAssetNames();


                print(scenePaths[0]);
                //  var myLoadedAssetBundle = AssetBundle.LoadFromFile(tempPath);

                string[] videonames = assetbundle.GetAllAssetNames();


                GameVideo_boy[0] = assetbundle.LoadAsset<VideoClip>(videonames[0]);
                GameVideo_boy[1] = assetbundle.LoadAsset<VideoClip>(videonames[1]);
                GameVideo_boy[2] = assetbundle.LoadAsset<VideoClip>(videonames[2]);
                GameVideo_boy[3] = assetbundle.LoadAsset<VideoClip>(videonames[3]);
                GameVideo_girl[0] = assetbundle.LoadAsset<VideoClip>(videonames[4]);
                GameVideo_girl[1] = assetbundle.LoadAsset<VideoClip>(videonames[5]);
                GameVideo_girl[2] = assetbundle.LoadAsset<VideoClip>(videonames[6]);
                GameVideo_girl[3] = assetbundle.LoadAsset<VideoClip>(videonames[7]);
                howToPlayClip[0] = assetbundle.LoadAsset<VideoClip>(videonames[8]);
                howToPlayClip[1] = assetbundle.LoadAsset<VideoClip>(videonames[9]);
                howToPlayClip[2] = assetbundle.LoadAsset<VideoClip>(videonames[10]);
                howToPlayClip[3] = assetbundle.LoadAsset<VideoClip>(videonames[11]);
                howToPlayClip[4] = assetbundle.LoadAsset<VideoClip>(videonames[12]);
            }
        }
        #region Global Message Popup

        public GameObject messagePopup;
        public TextMeshProUGUI message_text;

        public GameObject swipeAnimation;




        #endregion
        void SetDataLevelScreen()
        {

            for (int i = 0; i < Level_name.Length; i++)
            {
                Level_name[i].text = APIHandler_Preposition.instance._CMSDabase[i].LevelName;
            }

            if (APIHandler_Preposition.instance.home_data.Home_Screen_Text.Count >= 1)
            {
                ChangeText();
                MsgButtonTap.SetActive(true);
                //StartCoroutine("ChangeText");
            }
            else
            {
                MsgBox_levelMenu.gameObject.SetActive(false);
                MsgButtonTap.SetActive(false);
            }
        }
        int index;
        public void ChangeText()
        {

            
            if (index < APIHandler_Preposition.instance.home_data.Home_Screen_Text.Count)
            {
                MsgBox_levelMenu.gameObject.SetActive(false);
                MsgBox_levelMenu.transform.GetComponentInChildren<Text>().text = APIHandler_Preposition.instance.home_data.Home_Screen_Text[index];

                MsgBox_levelMenu.gameObject.SetActive(true);
                index++;
            }
            else
            {
                MsgBox_levelMenu.gameObject.SetActive(false);
                MsgButtonTap.SetActive(false);
            }


            //if (IsBoyCharacter)
            //    Character_Logo.GetComponent<Image>().sprite = BoySprite[0];
            //else
            //    Character_Logo.GetComponent<Image>().sprite = BoySprite[1];


        }

        public void Firsttimemsg()
        {
            swipeAnimation.SetActive(false);
            PlayerPrefs.SetString("first","done");
        }

        public void SetLevelLock()
        {
            int level = current_level + 1;
            if (PlayerPrefs.GetString("LevelLock").Contains(level.ToString()) == false)
            {
                PlayerPrefs.SetString("LevelLock", PlayerPrefs.GetString("LevelLock") + (level + ","));
            }

        }

        public void LevelUnlockLock()
        {
            string text = PlayerPrefs.GetString("LevelLock");
            print(text);
            for (int i = 1; i < 5; i++)
            {
                if (text.Contains(i.ToString()))
                {
                    print("call");
                    lock_stuff[i-1].button_image.color = LockedColor[1];
                    lock_stuff[i-1].heding.color = LockedColor[1];
                    lock_stuff[i-1].lockimage.SetActive(false);
                  
                    Level_name[i].transform.parent.GetComponent<Button>().interactable = true;
                    Level_name[i].transform.parent.GetComponent<Image>().color = LockedColor[1];
                }
                else
                {

                    lock_stuff[i-1].button_image.color = LockedColor[0];
                    lock_stuff[i-1].heding.color = LockedColor[0];
                    lock_stuff[i-1].lockimage.SetActive(true);
                   
                    Level_name[i].transform.parent.GetComponent<Button>().interactable = false;
                    Level_name[i].transform.parent.GetComponent<Image>().color = LockedColor[0]; 
                }
            }
        }


        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
           // PlayerPrefs.DeleteKey("first");
            if (!PlayerPrefs.HasKey("first"))
                swipeAnimation.SetActive(true);
            else
                swipeAnimation.SetActive(false);


           // PlayerPrefs.DeleteKey("LevelLock");
            LevelUnlockLock();
            // ActivatePanelByIndex(0); // first call loading panel 
            Input.multiTouchEnabled = false;
           StartCoroutine(WaitForLoadingTime()); 

        }


        // first start loading panal and get data from api 
        IEnumerator WaitForLoadingTime()
        {

            while (loading_time<1)
            {
                //print(loading_time);
                yield return new WaitForSeconds(.0001f);
                loading_time += .01f ;
                loading_bar.fillAmount = loading_time;
            }
            yield return new WaitForSeconds(1f);
            if (APIHandler_Preposition.instance.IsStatusTrue)
            {
                OnClickButton(2);
                SetDataLevelScreen();
            }
            else
            {
                StartCoroutine(WaitForLoadingTime());
            }
            //yield return null;
        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitButton();
            }
        }
        public void exitButton()
        {
            popsUps[4].popUpPanel.SetActive(true);
            OnPause();
        }

        public void Yes_Exit()
        {
            // Application.Quit();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        public void SetknowledgeFactTextAccordingTolevel()
        {
            KnowlegdeFact_text.text = APIHandler_Preposition.instance._CMSDabase[current_level].Knowledge_Fact;
        }
        public void SetHowToPlayText()
        {
            RenderTexture.active = HowToplayVedio.targetTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
            HowToplayVedio.clip = howToPlayClip[current_level];
            HowToPlayText.text = APIHandler_Preposition.instance._CMSDabase[current_level].How_To_Play;
        }
        void ActivatePanelByIndex(int index)
        {
            foreach (Panel panel in panels)
            {
                if (index == panel._panelId)
                {
                    panel.panel.SetActive(true);
                }
                else
                {
                    panel.panel.SetActive(false);
                }
            }
        }

        void ActivatePopByIndex(int index)
        {
            foreach(PopUp popUp in popsUps)
            {
                //Debug.Log(index);
                if(index == popUp._popUpId)
                {
                    popUp.popUpPanel.SetActive(true);
                }
                else
                {
                    popUp.popUpPanel.SetActive(false);
                }
            }
        }


        public void OnPause()
        {
            Time.timeScale = 0;
        }
        public void OnPlay()
        {
            Time.timeScale = 1;
        }

        public void OnClickButton(int id)
        {
            ActivatePanelByIndex(id);
        }


        public void SetLastWinLevel()
        {
            Content.GetComponent<RectTransform>().localPosition = new Vector2(LevelValue[current_level], Content.GetComponent<RectTransform>().localPosition.y);
        }
        public void OnClick_popupsButton(int id)
        {
            ActivatePopByIndex(id);
        }
        bool isflag;
        IEnumerator characterRun()
        {

            if (current_level == 0 )
            {
                CharacterLevel.transform.position = Character_Logo.transform.position;
            }
            else
                CharacterLevel.transform.position = Deflaut_CharacterLevel.transform.position;

            Character_Logo.SetActive(false);
            CharacterLevel.SetActive(true);
            CharacterLevel.GetComponentInChildren<Animator>().Play("CharacterRun");
            isflag = true;
            print("aaya");
            Vector2 target = new Vector2(levelbutton[current_level ].transform.localPosition.x, CharacterLevel.transform.localPosition.y);
            while (isflag)
            {
                CharacterLevel.transform.localPosition = Vector2.MoveTowards(CharacterLevel.transform.localPosition, target, Time.deltaTime * 600);
                if (Vector2.Distance(CharacterLevel.transform.localPosition, target) < 100f)
                {
                    isflag = false;
                    CharacterLevel.GetComponentInChildren<Animator>().Play("idle");
                    Fade.SetActive(true);
                }
                yield return new WaitForSeconds(.001f);
            }
            yield return new WaitForSeconds(.5f);
            CharacterLevel.SetActive(false);
            index_level[current_level] = true;
            LevelLoad(current_level);
            yield return new WaitForSeconds(.5f);
            Fade.SetActive(false);
            setting_butt.interactable = true;
        }

        bool []  index_level = new bool[5];
        // level menu click


        void LevelLoad(int level)
        {
            switch (level)
            {
                case 0:
                    Level1Manager_Preposition.instance.ResetGame();
                  //  Level1Manager_Preposition.instance.InstrucationPanal_Active();
                    ActivatePanelByIndex(3);
                    break;
                case 1:
                    Level2Manager_Preposition.instance.ResetGame();

                   Level2Manager_Preposition.instance.InstrucationPanal_Active();
                    ActivatePanelByIndex(4);
                    break;
                case 2:
                    ActivatePanelByIndex(5);
                    KidsLevel.instance.ResetData();
                   /// Level3Manager.instance.ResetData();
                   Level3SubLevelManager.instance_.InstrucationPanal_Active();

                    break;
                case 3:
                    Level4Manager_Preposition.instance.ResetGame();
                   // Level4Manager_Preposition.instance.InstrucationPanal_Active();
                    ActivatePanelByIndex(6);
                    break;
                case 4:
                    Level5Manager_Preposition.instance.ResetGame();
                    // Level5Manager_Preposition.instance.InstrucationPanal_Active();
                    ActivatePanelByIndex(7);
                    break;
            }
            for (int i = 0; i < index_level.Length; i++)
            {
                index_level[i] = false;
            }
            isclick = false;
        }
        bool isclick;
       public  void SetCurrentLevel(int i )
        {
            swipeAnimation.SetActive(false);
            setting_butt.interactable = false;
            MsgButtonTap.SetActive(false);
            if (isclick) return;
            isclick = true;

            current_level = i;
            //print("i"+i);
                      
            switch (current_level)
            {
                case 0:
                   
                    if(!index_level[current_level])
                    {
                        StartCoroutine("characterRun");

                    }
                   
                   
                    
                    break;
                case 1:

                    if (!index_level[current_level ])
                    {
                        StartCoroutine("characterRun");
                        
                    }
                  

                  //  onClick_VideoPlay();
                    break;
                case 2:
                    // Level3Manager.instance.ResetData();
                    // Level3Manager.instance.Instrucation_panel.SetActive(true);
                    if (!index_level[current_level])
                    {
                        StartCoroutine("characterRun");
                       
                    }
                   
                   
                    break;
                case 3:
                    if (!index_level[current_level])
                    {
                        StartCoroutine("characterRun");
                    }
                   
                   
                    break;
                case 4:
                    if (!index_level[current_level])
                    {
                        StartCoroutine("characterRun");
                    }
                   
                    break;

            }

            TimerCountDown_Preposition.instance.StopTimer();

            
                popsUps[0].popUpPanel.GetComponent<AudioSource>().clip = SoundManager_Preposition.instanace.winingPopupSound;
          
           
           
        }


        public void skipButton()
        {
            OnClickButton(3);
          //  _Levels[current_level].popUpPanel.SetActive(true);
          //  _Levels[current_level].Script_object.GetComponent<Level1Manager_Preposition>().StartGame();


        }

        IEnumerator CallFader()
        {
            Fade.SetActive(true);
            yield return new WaitForSeconds(.3f);
            switch (current_level)
            {
                case 0:
                    Level1Manager_Preposition.instance.ResetGame();

                    break;
                case 1:
                    Level2Manager_Preposition.instance.ResetGame();
                   Level2Manager_Preposition.instance.Instrucation_panel.SetActive(false);
                    break;
                case 2:
                    Level3Manager_Preposition.instance.ResetData();
                   // Level3Manager.instance.Instrucation_panel.SetActive(true);
                   // Level3SubLevelManager.instance_.InstrucationPanal_Active();
                    // KidsLevel.instance.ResetData();
                    break;
                case 3:
                    Level4Manager_Preposition.instance.ResetGame();
                    break;
                case 4:
                    Level5Manager_Preposition.instance.ResetGame();
                    break;

            }
            TimerCountDown_Preposition.instance.StopTimer();
            print(current_level);
            SetLastWinLevel();
            OnClickButton(2);
            Character_Logo.SetActive(true);
            ActivatePopByIndex(-1);
             yield return new WaitForSeconds(1f);
            Fade.SetActive(false);
        }
        public void GoTomenu()
        {

            StartCoroutine("CallFader");
           
            //print("call");
           
            
        }

        public void Retry()
        {
            //UIManager_Preposition.instance.OnClick_popupsButton(-1);
            switch (current_level)
            {
                case 0:
                    Level1Manager_Preposition.instance.ResetGame();
                    Level1Manager_Preposition.instance.Instrucation_panel.SetActive(true);
                    break;
                case 1:
                    Level2Manager_Preposition.instance.ResetGame();
                    Level2Manager_Preposition.instance.Instrucation_panel.SetActive(true);
                    break;
                case 2:
                    Level3Manager_Preposition.instance.ResetData();
                    //KidsLevel.instance.ResetData();

                   Level3SubLevelManager.instance_.InstrucationPanal_Active();
                    break;
                case 3:
                    Level4Manager_Preposition.instance.ResetGame();
                    Level4Manager_Preposition.instance.instrucation.SetActive(true);
                   break;
                case 4:
                    Level5Manager_Preposition.instance.ResetGame();
                    Level5Manager_Preposition.instance.instrucation.SetActive(true);
                    break;

            }
            TimerCountDown_Preposition.instance.StopTimer();

        }
       

        public void OnNextLevel()
        {
            if(IsBoyCharacter)
            Videoplayer.clip = GameVideo_boy[current_level];
            else
                Videoplayer.clip = GameVideo_girl[current_level];
            popsUps[3].popUpPanel.SetActive(true);
            current_level++;
            LevelLoad(current_level);
            
            if (current_level == 2)
            {
                Level3Manager_Preposition.instance.ResetData();
            }
            // GoTomenu();
            // RunLevel();
            TimerCountDown_Preposition.instance.StopTimer();
        }

        public void ResetStart()
        {
            for (int i = 0; i < 3; i++)
            {
                Star[i].SetActive(false);
            }
        }
        public void WinStarActive(int num)
        {
            StarReset();
            StartCoroutine(startEnum(num));
        }

        IEnumerator startEnum(int num)
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < num; i++)
            {

                Star[i].SetActive(true);

                yield return new WaitForSeconds(0.5f);
               // popsUps[0].popUpPanel.GetComponentInChildren<Animator>().Play("winpanelShaker", -1, 0f);
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void StarReset()
        {
            for (int i = 0; i < Star.Length; i++)
            {
                Star[i].SetActive(false);
            }

        }



        public void onVideoPlay()
        {
            if (IsBoyCharacter)
                Videoplayer.clip = GameVideo_boy[current_level];
            else
                Videoplayer.clip = GameVideo_girl[current_level];
            popsUps[3].popUpPanel.SetActive(true);
           
        }
        public void onClick_VideoPlay()
        {
            //print(current_level);
           
           
        }

        public void RunLevel()
        {
            StartCoroutine(GoLevelEnum());
        }

        IEnumerator GoLevelEnum()
        {
             if (current_level < 4 || current_level==0)
            {
                ContentObj.anchoredPosition = levelPointVec2[current_level];
                //====================================
                CharacterLevel.transform.GetChild(0).GetComponent<Animator>().Play("CharacterRun", -1, 0f);
                yield return null;
                //ContentObj.anchoredPosition = levelPointVec2[num];
                Vector2 tempvec;
                while ((ContentObj.anchoredPosition.x - levelPointVec2[current_level].x) > 0.1f)
                {
                    //print(ContentObj.anchoredPosition + " | " + levelPointVec2[current_level ]);
                    tempvec = Vector2.MoveTowards(ContentObj.anchoredPosition, levelPointVec2[current_level], Time.deltaTime * 500);
                    ContentObj.anchoredPosition = new Vector2(tempvec.x, ContentObj.anchoredPosition.y);
                    yield return null;
                }

                CharacterLevel.transform.GetChild(0).GetComponent<Animator>().Play("idle", -1, 0f);

                yield return new WaitForSeconds(2f);

                onVideoPlay();
                SetCurrentLevel(current_level);
            }
            else
            {

                ContentObj.anchoredPosition = levelPointVec2[0];
                yield return new WaitForSeconds(2f);
               
            }
         
        }
        public void WiningPoupsound()
        {
            if (current_level == 4)
            {
                popsUps[0].popUpPanel.GetComponent<AudioSource>().clip = SoundManager_Preposition.instanace.GamewinSound;
            }
            else
            {
                popsUps[0].popUpPanel.GetComponent<AudioSource>().clip = SoundManager_Preposition.instanace.winingPopupSound;
            }
        }

    }


    [Serializable]
    public class Panel
    {
        public string _panelName;
        public int _panelId;
        public GameObject panel;
    }

    [Serializable]
    public class PopUp
    {
        public string _popUpName;
        public int _popUpId;
        public GameObject popUpPanel;
    }

    [Serializable]
    public class Levels
    {
        public GameObject popUpPanel;
        public GameObject Script_object;
    }


     



}



    