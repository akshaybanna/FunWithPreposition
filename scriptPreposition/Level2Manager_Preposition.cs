using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;


namespace Prepostion
{
    [System.Serializable]
    public class Situation
    {

        public string[] prepositionWord;
        public GameObject[] object_Preposition;
    }

    public class Level2Manager_Preposition : MonoBehaviour
    {

        public static Level2Manager_Preposition instance;

        [Header("Level2 Situation")]

        public Situation[] _situation;
        public int currentSitutation;
        public bool IsRunning;
        public bool IsSlide;
        public CharacterControllerScript_Preposition _characterControllerScript;

        public GameObject Instrucation_panel;
        public Color PressColor;
        public Color UnPressColor;

        public GameObject Sign_borad;

        public int Coin;

        public Animator MsgBox;



        //level 2 Referances
        [Header("level 2 Referances ")]
        public Button[] PrepositionWord_Button;
        public Text Coin_text;

        //private variable
        private bool IsShoot;
        Vector2 DeflautBulletPos;
        GameObject TargetBubble;
        bool IsRightMove;
        public float Speed;

        // public float char_speed = 0;
        public GameObject coin_object;
        CMS_Data cms_level = new CMS_Data();


        [HideInInspector] public bool IsTimerEnable;
        [HideInInspector] public bool ISGameOver;
        [HideInInspector] public bool IsTriggerCoin;

        [HideInInspector] public string LastPress_button;
        float CountDowntime = 240;  /// if cms not given timeperlevel then we use for ratting star calculate constant value 
        public Text Timer_text;
        public Text Instruction_text;
        public GameObject KnowledgeFact;
        public Text health;


        private void Awake()
        {
            instance = this;
        }




        public void InstrucationPanal_Active()
        {
            CMSValue(1);
            Instrucation_panel.SetActive(true);

        }



        // Start is called before the first frame update
        public void GameStart()
        {

            Speed = 6;
            PrepositionWordActive(true);
            PrepositionWordAssign();
            RandomObjectPick();
            _characterControllerScript.run();
            MsgBox.gameObject.SetActive(false);
            IsRunning = true;

            // check Cms Timer enable or disable 
            CheckTimerPerLevel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PrepositionButt_Animation(bool Istrue)
        {
            foreach (var item in PrepositionWord_Button)
            {
                item.GetComponent<Image>().color = UnPressColor;
                if (Istrue)
                    item.GetComponent<Animator>().Play("ButtonAnimation");
                else
                    item.GetComponent<Animator>().Play("ButtonAnimation_ret");
            }

        }
        void PrepostionObject(bool Istrue)
        {
            foreach (var item in _situation[currentSitutation].object_Preposition)
            {
                item.gameObject.SetActive(Istrue);
            }
        }
       public bool iscomplete;
        public void EnterCoin(string pre_word, GameObject coin)
        {

            //iscomplete = true;
            Text msgtext = MsgBox.GetComponentInChildren<Text>();
            if ((pre_word == LastPress_button))
            {
                coin.SetActive(false);
                msgtext.text = cms_level.RightMove_Msg;

                coin_object.SetActive(true);
                Coin++;
                // CoinUpdate();
                SoundManager_Preposition.instanace.rightmovePlay();


            }
            else
            {
                cms_level.MaxWrongMove--;
                updateHeath(cms_level.MaxWrongMove);
                msgtext.text = cms_level.WrongMove_Msg;
                MsgBox.gameObject.SetActive(false);
                MsgBox.gameObject.SetActive(true);
                CancelInvoke("WaitforMsg");
                Invoke("WaitforMsg", 1f);
                SoundManager_Preposition.instanace.WrongMovePlay();
                if (cms_level.MaxWrongMove <= 0)
                {


                    //print("wrong" + cms_level.WrongMove_Msg);
                    CheckGameOverORGameWin();

                }

            }

            //msg box

            Button_press("");
            LastPress_button = null;

        }

        void Button_press(string text)
        {
            for (int i = 0; i < PrepositionWord_Button.Length; i++)
            {
                PrepositionWord_Button[i].GetComponent<Image>().color = UnPressColor;
                if (text == PrepositionWord_Button[i].GetComponentInChildren<Text>().text)
                {
                    PrepositionWord_Button[i].GetComponent<Image>().color = PressColor;
                }
            }
        }
        public void ClickOnPrepoitionWord(Text name)
        {
            LastPress_button = name.text;

            Button_press(LastPress_button);


            if (!IsTimerEnable) return;
            // Level2Manager_Preposition.instance.PrepositionButt_Animation(false);
            // print(name + "near" + NearByPreposition_object);
            //Text msgtext = MsgBox.GetComponentInChildren<Text>();
            // if ( (NearByPreposition_object == name.text))
            // {
            //     msgtext.text = cms_level.RightMove_Msg;
            //     Coin++;
            //     CoinUpdate();
            //     NearByPreposition_coin.SetActive(false);

            //     //msg box
            //     MsgBox.gameObject.SetActive(false);
            //     MsgBox.gameObject.SetActive(true);
            //     CancelInvoke("WaitforMsg");
            //     Invoke("WaitforMsg", 1f);
            // }
            // else
            // {
            //     cms_level.Retry--;
            //     Life_ChangeUpdate();
            //     msgtext.text = cms_level.WrongMove_Msg;
            //     CheckGameOverORGameWin();
            // }





        }






        //void Right_WrongMsgShow()
        //{


        //}


        bool onlyonetime;
        public void changeSituation()
        {
            if (!_characterControllerScript.IsPlayerRunning() && !_characterControllerScript.IsPlayerIdle()) return;
            if (currentSitutation == 0)
            {
                // print(TimerCountDown_Preposition.instance.timeLeft);
                if ((int)TimerCountDown_Preposition.instance.timeLeft <= (int)(cms_level.TimePerLevel - 65))
                {

                    if (onlyonetime) return;
                    PrepositionButt_Animation(false);
                    PrepostionObject(false);

                    Sign_borad.transform.localPosition = new Vector2(1200, Sign_borad.transform.localPosition.y);
                    Sign_borad.SetActive(true);
                    onlyonetime = true;
                }

            }
            else
                if (currentSitutation == 1)
            {
                //print(TimerCountDown_Preposition.instance.timeLeft);
                if ((int)TimerCountDown_Preposition.instance.timeLeft <= (int)(cms_level.TimePerLevel - (65 * 2)) && (int)TimerCountDown_Preposition.instance.timeLeft <= (int)(cms_level.TimePerLevel - 65))
                {
                    if (onlyonetime) return;
                    PrepositionButt_Animation(false);
                    PrepostionObject(false);
                    Sign_borad.transform.localPosition = new Vector2(1200, Sign_borad.transform.localPosition.y);
                    Sign_borad.SetActive(true);
                    onlyonetime = true;


                }

            }

        }

        public void waitSomeTime()
        {
            //yield return new WaitForSeconds(5f);
            //print("call in waitsec");
            currentSitutation++;
            PrepositionWordAssign();
            PrepositionButt_Animation(true);
            RandomObjectPick();
            onlyonetime = false;


        }

        void WaitforMsg()
        {
            MsgBox.gameObject.SetActive(false);
        }

        void CheckGameOverORGameWin()
        {


            if (cms_level.MaxWrongMove < 0)
            {

                ISGameOver = true;
                _characterControllerScript.Idle();
                GameOver();
            }

        }

        public void TimeUp()
        {
            if ((Coin >= cms_level.MaxRightMove) && (TimerCountDown_Preposition.instance.timeLeft <= 0))
            {
                GameWin();
                ISGameOver = true;
                _characterControllerScript.Idle();
            }
            else
            {
                GameOver();
            }
        }


        // assign button word 
        void PrepositionWordAssign()
        {

            for (int i = 0; i < _situation[currentSitutation].prepositionWord.Length; i++)
            {
                PrepositionWord_Button[i].GetComponentInChildren<Text>().text = _situation[currentSitutation].prepositionWord[i];
            }

        }

        void PrepositionWordActive(bool isactive)
        {

            for (int i = 0; i < _situation[currentSitutation].prepositionWord.Length; i++)
            {
                PrepositionWord_Button[i].gameObject.SetActive(isactive);
            }

        }
        public void ObjectFalse(GameObject g)
        {
            for (int i = 0; i < _situation[currentSitutation].object_Preposition.Length; i++)
            {
                if (g == _situation[currentSitutation].object_Preposition[i].gameObject == g)
                {
                    _situation[currentSitutation].object_Preposition[i].SetActive(false);
                }

            }

        }

        int last_proint, Secondlast_pro;
        public void RandomObjectPick()
        {



            int rnd = Random.Range(0, _situation[currentSitutation].object_Preposition.Length);
            if (last_proint == rnd || Secondlast_pro == rnd)
            {
                RandomObjectPick();
                return;
            }

            Secondlast_pro = last_proint;
            last_proint = rnd;
            int rnd_pos = Random.Range(0, 2);

            //print("RND POS" + rnd_pos);
            switch (rnd_pos)
            {
                case 0:
                    _situation[currentSitutation].object_Preposition[rnd].transform.localPosition = new Vector2(1000, _situation[currentSitutation].object_Preposition[rnd].transform.localPosition.y);
                    break;
                case 1:
                    _situation[currentSitutation].object_Preposition[rnd].transform.localPosition = new Vector2(1200, _situation[currentSitutation].object_Preposition[rnd].transform.localPosition.y);
                    break;
                case 2:
                    _situation[currentSitutation].object_Preposition[rnd].transform.localPosition = new Vector2(1200, _situation[currentSitutation].object_Preposition[rnd].transform.localPosition.y);
                    break;
            }
            //print(_situation[currentSitutation].object_Preposition[rnd].transform.localPosition);
            _situation[currentSitutation].object_Preposition[rnd].SetActive(true);
            //print(rnd);

        }

        //======//Gameover function ===============================================
        public void GameOver()
        {

            TimerCountDown_Preposition.instance.StopTimer();
            UIManager_Preposition.instance.OnClick_popupsButton(1);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < _situation[i].object_Preposition.Length; j++)
                {
                    _situation[i].object_Preposition[j].SetActive(false);
                }
            }

        }
        ///=========================================================//////////////////
        //Gameover function 
        public void GameWin()
        {

            TimerCountDown_Preposition.instance.StopTimer();

            UIManager_Preposition.instance.OnClick_popupsButton(0);
            CheckWinStarRatting();
            UIManager_Preposition.instance.SetLevelLock();
            UIManager_Preposition.instance.LevelUnlockLock();

        }
        void CheckWinStarRatting()
        {
            // print("call");

            UIManager_Preposition.instance.WinStarActive(3);


        }
        public void CoinUpdate()
        {
            Coin_text.text = Coin.ToString();
        }




        public void ResetGame()
        {

            StopAllCoroutines();
            currentSitutation = 0;
            _characterControllerScript.gameObject.transform.GetChild(0).transform.localScale = new Vector2(1, 1);
            _characterControllerScript.gameObject.transform.GetChild(0).transform.localPosition = new Vector2(0, 0);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < _situation[i].object_Preposition.Length; j++)
                {
                    _situation[i].object_Preposition[j].SetActive(false);
                }
            }
            PrepositionWordActive(false);
            TimerCountDown_Preposition.instance.StopTimer();
            TimerCountDown_Preposition.instance.timeLeft = 200;
            LastPress_button = "";
            Button_press("");
            onlyonetime = false;
            Coin = 0;
            Speed = 6;
            CoinUpdate();
            Sign_borad.SetActive(false);
            ISGameOver = false;
            IsRunning = false;
            Timer_text.text = "00:00";
            InstrucationPanal_Active();
            PrepostionObject(false);
            if (_characterControllerScript != null)
                _characterControllerScript.ResetCharacterAnimtion();

            // Instrucation_panel.SetActive(true);
            UIManager_Preposition.instance.WiningPoupsound();
            coin_object.SetActive(false);
        }


        void updateHeath(int text)
        {
            health.text = (text + 1).ToString();
        }
        void CMSValue(int currentLevel)
        {
            cms_level.LevelName = APIHandler_Preposition.instance._CMSDabase[currentLevel].LevelName;

            cms_level.NumberOfQuestion_CMS = APIHandler_Preposition.instance._CMSDabase[currentLevel].NumberOfQuestion_CMS;
            cms_level.MaxRightMove = APIHandler_Preposition.instance._CMSDabase[currentLevel].MaxRightMove;
            cms_level.MaxWrongMove = APIHandler_Preposition.instance._CMSDabase[currentLevel].MaxWrongMove;
            updateHeath(cms_level.MaxWrongMove);

            cms_level.RightMove_Msg = APIHandler_Preposition.instance._CMSDabase[currentLevel].RightMove_Msg;
            cms_level.WrongMove_Msg = APIHandler_Preposition.instance._CMSDabase[currentLevel].WrongMove_Msg;

            cms_level.TimePerLevel = 210;

            cms_level.Instrucation = APIHandler_Preposition.instance._CMSDabase[currentLevel].Instrucation;
            cms_level.Knowledge_Fact = APIHandler_Preposition.instance._CMSDabase[currentLevel].Knowledge_Fact;
            cms_level.How_To_Play = APIHandler_Preposition.instance._CMSDabase[currentLevel].How_To_Play;

            cms_level.RattingStar1_Time = APIHandler_Preposition.instance._CMSDabase[currentLevel].RattingStar1_Time;
            cms_level.RattingStar2_Time = APIHandler_Preposition.instance._CMSDabase[currentLevel].RattingStar2_Time;
            cms_level.RattingStar3_Time = APIHandler_Preposition.instance._CMSDabase[currentLevel].RattingStar3_Time;


            // check Cms Knowledge enable or disable 
            CheckKnowledgeFact();
            //set instrucation win 
            SetInstrucationText();
            // set how to play Text and vedio
            UIManager_Preposition.instance.SetHowToPlayText();


        }

        //=========check timer per level condition ================
        void CheckTimerPerLevel()
        {

            print(cms_level.TimePerLevel);
            if (cms_level.TimePerLevel == 0)
            {
                IsTimerEnable = false;
                Timer_text.transform.parent.gameObject.SetActive(false);
                TimerCountDown_Preposition.instance.startTimer(CountDowntime, true);
            }
            else
            {
                IsTimerEnable = true;
                Timer_text.transform.parent.gameObject.SetActive(true);
                TimerCountDown_Preposition.instance.startTimer(cms_level.TimePerLevel, Timer_text);

            }
        }
        //=====================================================================================
        // check Disable or Enable Knowledge Fact
        void CheckKnowledgeFact()
        {
            if (cms_level.Knowledge_Fact == "")
                KnowledgeFact.SetActive(false);
            else
            {
                KnowledgeFact.SetActive(true);
                UIManager_Preposition.instance.SetknowledgeFactTextAccordingTolevel();
            }

        }
        // check Disable or Enable Knowledge Fact
        void SetInstrucationText()
        {
            Instruction_text.text = cms_level.Instrucation;
        }
        //======//Gameover function ===============================================

    }
}
