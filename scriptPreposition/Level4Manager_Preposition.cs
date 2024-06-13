using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prepostion
{
    [System.Serializable]
    public class PrepositionActivity
    {

        public List<Question_prepostion> Question = new List<Question_prepostion>();
        public GameObject screen_object;
    }
    
    [System.Serializable]
    public class Question_prepostion
    {

       
        public string Question;
        public string prepositionWord;
        public List<string> wrongword = new List<string>();
       
    }


    public class Level4Manager_Preposition : MonoBehaviour
    {

        public static Level4Manager_Preposition instance;

        public int currentQuestionNo;
        [Header("Level2 Situation")]

        public PrepositionActivity[] _situation;
        public int currentSitutation;
        public bool IsRunning;
        public bool IsSlide;
        public CharacterControllerScript_Preposition _characterControllerScript;
        public List<string> shuffle_Question;
       // public List<string> shuffle_PrepositonWord;
        public GameObject instrucation;
        public Animator DreamingAnimation;
        public GameObject[] rightAnimation;
        public GameObject[] rightAnimation_screen2;
        public int Coin;

        public Animator MsgBox;



        //level 2 Referances
        [Header("level 2 Referances ")]
        public Button[] PrepositionWord_Button;
        public Text Coin_text;
        //screen one
        public Text question_text;
        public Animator[] bubble;
        //screen two
        public Text question_text_Screen2;
        public Animator Racket;
        //screen three
        public Text question_text_Screen3;
        public Animator Racket3;

        public Animator Sleepingcharcter;

        //private variable
        private bool IsShoot;
        Vector2 DeflautBulletPos;
        GameObject TargetBubble;
        bool IsRightMove;
        public Text Timer_text;

        public Sprite[] dreambubblesprite;

        public GameObject dreamingScreen;
        public GameObject SecoundScreen;

        CMS_Data cms_level = new CMS_Data();


        [HideInInspector] public bool IsTimerEnable;
        [HideInInspector] public bool ISGameOver;
        [HideInInspector] public bool IsTriggerCoin;

        [HideInInspector] public string LastPress_button;


        public Animator[] character_animation;
        public Animator bg_scene2;
        public Text Instruction_text;
        public GameObject KnowledgeFact;

        public Text health;

        float CountDowntime = 240;  /// if cms not given timeperlevel then we use for ratting star calculate constant value 

        private void Awake()
        {
            instance = this;
        }




        public void InstrucationPanal_Active()
        {
            CMSValue(3);
            instrucation.SetActive(true);

        }

        // Start is called before the first frame update
        public void GameStart()
        {
          


            for (int i = 0; i < _situation.Length; i++)
            {
                _situation[i].screen_object.gameObject.SetActive(false);
            }
            
         
            _situation[currentSitutation].screen_object.gameObject.SetActive(true);
            RandomObjectPick();

            // check Cms Timer enable or disable 
            CheckTimerPerLevel();

        }

        //void QuestionGenrate()
        //{
        //    int rnd = Random.Range(0, _situation[currentSitutation].Question.Length);
        //    question_text.text = _situation[currentSitutation].Question[rnd];

        //    int rnd1 = Random.Range(0, _situation[currentSitutation].prepositionWord.Length);
        //    question_text.text = _situation[currentSitutation].Question[rnd1];

        //}

       // public  List<string> shuffle_wrongWord = new List<string>();

        public void WrongWord_shuffling()
        {
            //List<string> _wrongWord = new List<string>();
            //_wrongWord.Clear();
            //shuffle_wrongWord.Clear();
            //for (int i = 0; i < _situation[currentSitutation].wrongword.Length; i++)
            //{
            //    _wrongWord.Add(_situation[currentSitutation].wrongword[i]);
            //}

            //for (int i = 0; i < _situation[currentSitutation].wrongword.Length; i++)
            //{
            //    int rnd = Random.Range(0, _wrongWord.Count);
            //    shuffle_wrongWord.Add(_wrongWord[rnd]);
            //    _wrongWord.Remove(_wrongWord[rnd]);


            //}
        }
       

        public void PrepositionButt_Animation(bool Istrue)
        {
            foreach (var item in PrepositionWord_Button)
            {
                item.gameObject.SetActive(Istrue);
            }
        }
        void PrepostionObject(bool Istrue)
        {
            //foreach (var item in _situation[currentSitutation].prepositionWord)
            //{
            //    // item.gameObject.SetActive(Istrue);
            //}
        }


        void ResetTextToNextQuestion()
        {
            switch (currentSitutation)
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                    {
                        bubble[i].GetComponentInChildren<Text>().text = "";

                    }

                    question_text.text = "";

                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        Racket.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text = "";

                    }
                    Racket.Play("Screen2_out");
                    question_text_Screen2.text = "";

                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        Racket3.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text = "";

                    }
                    Racket3.Play("Screen2_out");
                    question_text_Screen3.text = "";

                    break;
            }
        }
        public void Rightwrongmove()
        {

            ResetTextToNextQuestion();
            Text msgtext = MsgBox.GetComponentInChildren<Text>();
            if (IsRightMove)
            {
                msgtext.text = cms_level.RightMove_Msg;
                Coin++;
                CoinUpdate();

                SoundManager_Preposition.instanace.RightMoveplayLevel3_4();

                //msg box
                MsgBox.gameObject.SetActive(false);
                MsgBox.gameObject.SetActive(true);
                //print("right");
                Checkwining();


                //CancelInvoke("WaitforMsg");
                //Invoke("WaitforMsg", 1f);
            }
            else
            {
                cms_level.MaxWrongMove--;
                updateHeath(cms_level.MaxWrongMove );

                SoundManager_Preposition.instanace.WrongMovePlay(); 
                //print(cms_level.Retry);

                msgtext.text = cms_level.WrongMove_Msg;
                Handheld.Vibrate();
               // MsgBox.GetComponent<Animator>().Play("msganim");
                CancelInvoke("WaitforMsg");
               Invoke("WaitforMsg", 2f);
               
            }


           
        }

       

      
        void WaitforMsg()
        {
            //MsgBox.GetComponent<Animator>().Play("return");
           
         
                CheckGameOverORGameWin();
         
        }

        void CheckGameOverORGameWin()
        {


            if (cms_level.MaxWrongMove < 0)
            {
                GameOver();
            }
            else
            {
                NextQuestion();
            }


        }

        void NextQuestion()
        {
           
            genrateQuestion();
        }

        public void Checkwining()
        {
           
            if ((cms_level.MaxRightMove <= currentQuestionNo) )
            {
                GameWin();
            }
            else
            {
                switch (currentSitutation)
                {
                    case 0:

                        switch (currentQuestionNo)
                        {
                            case 0:
                                rightAnimation[currentQuestionNo].GetComponent<Animator>().enabled = true;
                                break;
                            case 1:
                                rightAnimation[currentQuestionNo].GetComponent<Animator>().enabled = true;
                                break;
                            case 2:
                                rightAnimation[currentQuestionNo].GetComponent<Animator>().enabled = true;
                                break;
                            case 3:
                                rightAnimation[currentQuestionNo].gameObject.SetActive(true);
                                break;
                            case 4:
                                rightAnimation[currentQuestionNo].gameObject.SetActive(true);
                                break;
                            default:    print("MAx");
                                break;
                        }
                        break;
                    case 1:

                        switch (currentQuestionNo)
                        {
                            case 0:
                                rightAnimation_screen2[currentQuestionNo].SetActive(true);
                                break;

                           
                        }
                        bg_scene2.Play("wining");
                        Invoke("StopActivity", 2f);
                        break;
                    case 2:
                        character_animation[0].Play("handshake");
                        character_animation[1].Play("handshake");
                       // Invoke("StopAnimation");
                        break;
                }
               
               currentQuestionNo++;       
                print("right move call");
                Invoke("NextQuestion", 2f);
                //Checkwining();
            }

        }

        void StopActivity()
        {
            bg_scene2.Play("idle2");

        }
        int last_situtation;
        public void ClickOK_instrucation()
        {
          currentSitutation = Random.Range(0, 3);
            if (last_situtation == currentSitutation)
            {
                ClickOK_instrucation();
                return;
            }
            last_situtation = currentSitutation;

            print("currentSitutation" + currentSitutation);
            
            DreamingAnimation.transform.GetChild(1).GetComponent<Image>().sprite = dreambubblesprite[currentSitutation];

            Invoke("dreamingAnimation", 1f);
        }
        void dreamingAnimation()
        {
            DreamingAnimation.gameObject.SetActive(true);
        }
        public void Onclick_Dream()
        {
            dreamingScreen.SetActive(false);
            SecoundScreen.SetActive(true);
            GameStart();



        }

        // assign button word 
        void PrepositionWordAssign()
        {

           // for (int i = 0; i < _situation[currentSitutation].Question.Length; i++)
            {
               //PrepositionWord_Button[i].GetComponentInChildren<Text>().text = _situation[currentSitutation].Question[i];
            }

        }
        bool isClick;
        public void bubbleClick(Text word)
        {
            if (isClick) return;
            isClick = true;
            if (word.text == correctword)
            {
                for (int i = 0; i < bubble.Length; i++)
                {
                    
                    if ( bubble[i].transform.GetComponentInChildren<Text>().text == correctword)
                    {
                        
                        switch (i)
                        { 
                            case 0:
                                bubble[i].transform.GetChild(0).GetComponent<Animator>().Play("buuble1animation");
                                print("2");
                                break; 

                            case 1:
                                bubble[i].transform.GetChild(0).GetComponent<Animator>().Play("bubble2animation");
                                print("3");
                                break; 
                            
                            case 2:
                                bubble[i].transform.GetChild(0).GetComponent<Animator>().Play("bubble3animation");
                                print("4");
                                break; 

                            case 3:
                                bubble[i].transform.GetChild(0).GetComponent<Animator>().Play("bubble4animation");
                                print("5");
                                break;
                        }
                        
                    }

                    Invoke("ReturnBaloon", .2f);
                }
                IsRightMove = true;
                //print("right");
                shuffle_Question.RemoveAt(lastQuestion);
                _situation[currentSitutation].Question.RemoveAt(lastQuestion);
                //shuffle_PrepositonWord.RemoveAt(lastQuestion);
            }
            else
            {
                for (int i = 0; i < bubble.Length; i++)
                {
                    bubble[i].Play("return_bubble");
                }
                //print("wrong");
                IsRightMove = false;
            }

            Rightwrongmove();
        }



        public void ReturnBaloon()
        {
            for (int i = 0; i < bubble.Length; i++)
            {
                bubble[i].Play("return_bubble");
            }
        }
        string correctword;
        int numberofquestion;
        public void RandomObjectPick()
        {
            shuffle_Question.Clear();
            //shuffle_PrepositonWord.Clear();
           // List<string> _prepositionword = new List<string>();
           // List<string> _question = new List<string>();
            for (int i = 0; i < _situation[currentSitutation].Question.Count; i++)
            {
               shuffle_Question.Add(_situation[currentSitutation].Question[i].Question);
               // shuffle_PrepositonWord.Add(_situation[currentSitutation].prepositionWord[i]);
            }
            numberofquestion = _situation[currentSitutation].Question.Count;
            genrateQuestion();
        }
        int lastQuestion;
        //int rnd;
        void genrateQuestion()
        {
           // WrongWord_shuffling();
            //print("question");
            int rnd = Random.Range(0, shuffle_Question.Count);
           // print(rnd);
            if (rnd == lastQuestion && shuffle_Question.Count > 1)
            {
                genrateQuestion();
                return;
            }
               

            lastQuestion = rnd;

           // ResetTextToNextQuestion();
            switch (currentSitutation)
            {
                case 0:

                    int rnd_word = Random.Range(0, bubble.Length);
                    question_text.text = shuffle_Question[rnd];
                    bubble[rnd_word].GetComponentInChildren<Text>().text = _situation[currentSitutation].Question[rnd].prepositionWord;
                    correctword = _situation[currentSitutation].Question[rnd].prepositionWord;
                    int count=0;
                   
                    for (int i = 0; i < bubble.Length; i++)
                    {
                     
                        bubble[i].Play("bubbleAnimation");
                    
                        if (bubble[i].GetComponentInChildren<Text>().text == "")
                        {
                         
                            bubble[i].GetComponentInChildren<Text>().text = _situation[currentSitutation].Question[rnd].wrongword[count];
                            count++;
                            print(count);
                        }

                    }
                    break;
                case 1:
                    int rnd_word1 = Random.Range(0, Racket.gameObject.transform.childCount);
                    question_text_Screen2.text = shuffle_Question[rnd];

                    Racket.gameObject.transform.GetChild(rnd_word1).transform.GetComponentInChildren<Text>().text = _situation[currentSitutation].Question[rnd].prepositionWord;
                    int count1 = 0;
                    correctword = _situation[currentSitutation].Question[rnd].prepositionWord;
                    for (int i = 0; i < 3; i++)
                    {
                     
                        Racket.Play("Screen2_in");
                  
                        if (Racket.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text == "")
                        {
                     
                            Racket.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text = _situation[currentSitutation].Question[rnd].wrongword[count1];
                            count1++;
                            print(count1);
                        }

                    }
                    break;
                case 2:

                    int rnd_word2 = Random.Range(0, Racket3.gameObject.transform.childCount);
                   
             
                    question_text_Screen3.text = shuffle_Question[rnd];
                   
                    Racket3.gameObject.transform.GetChild(rnd_word2).transform.GetComponentInChildren<Text>().text = _situation[currentSitutation].Question[rnd].prepositionWord;
                   
                    correctword = _situation[currentSitutation].Question[rnd].prepositionWord;
                    int count2 = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        Racket3.Play("Screen2_in");
                   
                        if (Racket3.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text == "")
                        {
                       
                            Racket3.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text = _situation[currentSitutation].Question[rnd].wrongword[count2];
                            count2++;
                            print(count2);
                        }

                    }
                    break;
            }

            isClick = false;
        }

        //print(_situation[currentSitutation].object_Preposition[rnd].transform.localPosition);
        //_situation[currentSitutation].object_Preposition[rnd].SetActive(true);
        //print(rnd);


       
        //======//Gameover function ===============================================
        public void GameOver()
        {
            if (ISGameOver) return;
            SecoundScreen.SetActive(false);
            dreamingScreen.SetActive(true);
            ISGameOver = true;
            StartCoroutine("GameLossDream");


        }
        ///=========================================================//////////////////
        //Gameover function 
        public void GameWin()
        {
            if (ISGameOver) return;
            SecoundScreen.SetActive(false);
            dreamingScreen.SetActive(true);

            StartCoroutine("GamewinDream");

            


        }

        IEnumerator GamewinDream()
        {
            Sleepingcharcter.Play("simle");
            yield return new WaitForSeconds(1.5f);
          
            TimerCountDown_Preposition.instance.StopTimer();
           
            UIManager_Preposition.instance.OnClick_popupsButton(0);
            CheckWinStarRatting();
            UIManager_Preposition.instance.SetLevelLock();
            UIManager_Preposition.instance.LevelUnlockLock();
        }
        void CheckWinStarRatting()
        {
            // print("call");
            if (cms_level.TimePerLevel == 0)
            {
                if (TimerCountDown_Preposition.instance.timeLeft >= 180)
                    UIManager_Preposition.instance.WinStarActive(3);
                else if (TimerCountDown_Preposition.instance.timeLeft >= 120 && TimerCountDown_Preposition.instance.timeLeft < 180)
                {
                    UIManager_Preposition.instance.WinStarActive(2);
                    // print("star2");
                }
                else
                    UIManager_Preposition.instance.WinStarActive(1);
            }
            else
            {

                if (TimerCountDown_Preposition.instance.timeLeft >= (cms_level.TimePerLevel - cms_level.RattingStar3_Time))
                    UIManager_Preposition.instance.WinStarActive(3);
                else if (TimerCountDown_Preposition.instance.timeLeft >= (cms_level.TimePerLevel - cms_level.RattingStar2_Time) && TimerCountDown_Preposition.instance.timeLeft < (cms_level.TimePerLevel - cms_level.RattingStar3_Time))
                {
                    UIManager_Preposition.instance.WinStarActive(2);
                    // print("star2");
                }
                else
                    UIManager_Preposition.instance.WinStarActive(1);
            }

        }

        public void Timerup()
        {

            ISGameOver = true;
            TimerCountDown_Preposition.instance.StopTimer();
            UIManager_Preposition.instance.OnClick_popupsButton(5);


        }
        IEnumerator GameLossDream()
        {
            Sleepingcharcter.Play("sad");
            yield return new WaitForSeconds(1.5f);
           
            TimerCountDown_Preposition.instance.StopTimer();
            UIManager_Preposition.instance.OnClick_popupsButton(1);
        }



       

        void CoinUpdate()
        {
            Coin_text.text = Coin.ToString();
        }
        

       

       
       public void ResetGame()
        {
            currentSitutation = 0;
            Coin = 0;
            dreamingScreen.SetActive(true);
            SecoundScreen.SetActive(false);
            currentQuestionNo = 0;
            Sleepingcharcter.Play("idle");
           DreamingAnimation.gameObject.SetActive(false);
            MsgBox.GetComponent<Animator>().Play("idle_msg");

            StopCoroutine("GamewinDream");
            StopCoroutine("GameLossDream");

            rightAnimation[1].transform.GetChild(0).gameObject.SetActive(false);
            rightAnimation[1].transform.GetChild(2).gameObject.SetActive(false);
            

            isClick = false;
            for (int i = 0; i < rightAnimation.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        rightAnimation[i].GetComponent<Animator>().enabled = false;
                        break;
                    case 1:
                        rightAnimation[i].GetComponent<Animator>().enabled = false;
                        break;
                    case 2:
                        rightAnimation[i].GetComponent<Animator>().enabled = false;
                        break;
                    case 3:
                        rightAnimation[i].gameObject.SetActive(false);
                        break;
                    case 4:
                        rightAnimation[i].gameObject.SetActive(false);
                        break;
                }
            }
            for (int i = 0; i < rightAnimation_screen2.Length; i++)
            {
                
                {
                    rightAnimation_screen2[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < Racket3.gameObject.transform.childCount; i++)
            {
                Racket3.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text = "";
            }
            for (int i = 0; i < Racket.gameObject.transform.childCount; i++)
            {
                Racket.gameObject.transform.GetChild(i).transform.GetComponentInChildren<Text>().text = "";
            }
            for (int i = 0; i < bubble.Length; i++)
            {
                bubble[i].GetComponentInChildren<Text>().text = "";
            }
          
                CoinUpdate();
            ISGameOver = false;
            IsRunning = false;
            //print("call reset");
            InstrucationPanal_Active();
            Timer_text.text = "00:00";
            UIManager_Preposition.instance.WiningPoupsound();

        }


        void updateHeath(int text)
        {
            health.text = (text+1).ToString();
        }

        void CMSValue(int currentLevel)
        {
            cms_level.LevelName = APIHandler_Preposition.instance._CMSDabase[currentLevel].LevelName;

            cms_level.NumberOfQuestion_CMS = APIHandler_Preposition.instance._CMSDabase[currentLevel].NumberOfQuestion_CMS;
            cms_level.MaxRightMove = 4;
            cms_level.MaxWrongMove = APIHandler_Preposition.instance._CMSDabase[currentLevel].MaxWrongMove;
            updateHeath(cms_level.MaxWrongMove);

            cms_level.RightMove_Msg = APIHandler_Preposition.instance._CMSDabase[currentLevel].RightMove_Msg;
            cms_level.WrongMove_Msg = APIHandler_Preposition.instance._CMSDabase[currentLevel].WrongMove_Msg;

            cms_level.TimePerLevel = APIHandler_Preposition.instance._CMSDabase[currentLevel].TimePerLevel;

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

            // aditional
            SetLevelData();

        }

        void SetLevelData()
        {

            _situation[0].Question.Clear();
            _situation[1].Question.Clear();
            _situation[2].Question.Clear();
            for (int i = 0; i < APIHandler_Preposition.instance._level4Data._dreamingprepostion.Count; i++)
            {

                if (APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].situtaion == "Boy's Birthday Celebration with calendar displayed")
                {
                    Question_prepostion _questio = new Question_prepostion();
                    _questio.Question = APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].question;
                    _questio.prepositionWord = APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].Prepostion_word;
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word1);
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word2);
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word3);
                    _situation[0].Question.Add(_questio);
                }
                else if (APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].situtaion == "Club activities with reference of place")
                {
                    Question_prepostion _questio = new Question_prepostion();
                    _questio.Question = APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].question;
                    _questio.prepositionWord = APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].Prepostion_word;
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word1);
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word2);
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word3);
                    _situation[1].Question.Add(_questio);
                }
                else if (APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].situtaion == "Kids Information")
                {
                    Question_prepostion _questio = new Question_prepostion();
                    _questio.Question = APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].question;
                    _questio.prepositionWord = APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].Prepostion_word;
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word1);
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word2);
                    _questio.wrongword.Add(APIHandler_Preposition.instance._level4Data._dreamingprepostion[i].wrong_word3);
                    _situation[2].Question.Add(_questio);
                }
            }
           
        }

        //=========check timer per level condition ================
        void CheckTimerPerLevel()
        {
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
