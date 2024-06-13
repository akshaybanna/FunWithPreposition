using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Prepostion
{


    public class Level1Manager_Preposition : MonoBehaviour
    {

        public static Level1Manager_Preposition instance;

        public GameObject bubblePrefab;
        public List<string> Question = new List<string>();
        public List<string> Otherword = new List<string>();
        public GameObject bullet;
        public int Coin;

        public Animator MsgBox;
        public Animator character;
        public GameObject BaseCharacter;

        // Screen Point 
        // Screen Point 
        public Transform[] randompoint;
        public Transform[] bulletPos;
        public Transform LevelParent;
        public GameObject Instrucation_panel;

        public GameObject coin_object;

       

       public bool IsGameover;
        //private variable
        private bool IsShoot;
        Vector2 DeflautBulletPos;
        public GameObject TargetBubble;
        bool IsRightMove;
        CMS_Data cms_level = new CMS_Data();
        [HideInInspector]public bool IsTimerEnable;
        float CountDowntime=240;  /// if cms not given timeperlevel then we use for ratting star calculate constant value 
        public Transform hand,bubble;
        public int currentBobbleCount;
        
        public Text Timer_text;
        Text msgtext;
        public Text Instruction_text;
        public GameObject KnowledgeFact;

        public ParticleSystem[] eyedrop;

        public Text health;
        private void Awake()
        {
            instance = this;
        }

       
      //  string Preposition_word = "About Above Beneath Beside Between Beyond in into across over on at in frontof under around near next nextto from to towards until up upon since before after among ontop of through by till within along during against aside as Behind Below but onto following down for from off unlike with over";
      //  string Other_word = "School bench Table Beach Bucket Blanket Bed Bottle Pillow Box Candle Chair Garden Park Cloth Bag Door House Pen Bird Paper Child Road Disk Wood Lake Phone Basket Tooth Vehicle Gate Girl Ladder Hat Fish Hand Radio Card Key Oil Boat Rock Salt Ball Bat Clock Bag wire Cycle Box";


     public    List<string> _preposition_list = new List<string>();
      public  List<string> _otherword_list = new List<string>();



        void random_word()
        {
           
            Question.Clear();
            Otherword.Clear();
            //string[] Preposition_list = Preposition_word.Split(' ');
           
            //string[] Other_word_list = Other_word.Split(' ');

            //for (int i = 0; i < Preposition_list.Length; i++)
            //{
            //    _preposition_list.Add(Preposition_list[i]);
            //}

            //for (int i = 0; i < Other_word_list.Length; i++)
            //{
            //    _otherword_list.Add(Other_word_list[i]);
            //}

         

            for (int i = 0; i < _preposition_list.Count; i++)
            {
                int ran = Random.Range(0, _preposition_list.Count);
                Question.Add(_preposition_list[ran]);
                _preposition_list.RemoveAt(ran);
            }
            for (int i = 0; i < _otherword_list.Count; i++)
            {
                int ran = Random.Range(0, _otherword_list.Count);
                Otherword.Add(_otherword_list[ran]);
                _otherword_list.RemoveAt(ran);
            }
          
              

            }
        



        void QuestionAddAccordingToCMS()
        {

            random_word();

        }

       
        // Start is called before the first frame update
        void Start()
        {
             msgtext = MsgBox.GetComponentInChildren<Text>();
            DeflautBulletPos = bullet.transform.localPosition;
           
        }

        public void InstrucationPanal_Active()
        {
            CMSValue(0);
            Instrucation_panel.SetActive(true);

        }

        public void StartGame()
        {

           
            QuestionAddAccordingToCMS();
          
            MsgBox.gameObject.SetActive(false);
            StartCoroutine("BubbleInstatiate");
            // check Cms Timer enable or disable 
            CheckTimerPerLevel();
            // TimerCountDown_Preposition.instance.startTimer(cms_level1.TimePerLevel,Timer_text);
        }

        // Update is called once per frame
        void Update()
        {



           

            if (IsShoot)
            {

               
                    //if (hand.rotation.y == Quaternion.Euler(new Vector3(0, 0, atan2 - 90)).y)
                    //{
                    if (TargetBubble != null)
                    {
                  //  Vector2 v_diff = (TargetBubble.transform.position - hand.position);
                   // float atan2 = Mathf.Atan2(v_diff.y, v_diff.x) * Mathf.Rad2Deg;
                    // hand.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                   // Quaternion targetRoation = Quaternion.Euler(new Vector3(0, 0, atan2 - 90));

                  //  hand.rotation = Quaternion.Lerp(hand.rotation, targetRoation, Time.deltaTime * 50);
                
                   // if (Quaternion.Angle(hand.rotation, targetRoation) <= 0.5f)
                   // {
                        bullet.transform.position = Vector2.MoveTowards(bullet.transform.position, TargetBubble.transform.position, Time.deltaTime * 20);
                        if (Vector2.Distance(bullet.transform.position, TargetBubble.transform.position) < 0.1f)
                        {
                           // print("reach");

                            bullet.SetActive(false);
                            TargetBubble.GetComponent<Animator>().enabled = true;
                            Destroy(TargetBubble,.5f);
                             coin_object.transform.position = TargetBubble.transform.position;

                        TargetBubble = null;
                          StartCoroutine( Right_WrongMsgShow());


                        }
                    //}
                   
                   
                 }
            }
            }
      public  int count;
       public int prepostionword;
        IEnumerator BubbleInstatiate()
        {
            
            List<Transform> _rndpoint = new List<Transform>();
            for (int i = 0; i < randompoint.Length; i++)
            {
                _rndpoint.Add(randompoint[i]);
            }
            currentBobbleCount++;
            prepostionword = 0;
            count = 0;
            bool isright=false;




            for (int i = 0; i < 3; i++)
            {
                int rnd = Random.Range(0, _rndpoint.Count);
                yield return new WaitForSeconds(.1f);
                GameObject g = Instantiate(bubblePrefab, LevelParent.position, Quaternion.identity) as GameObject;


                g.transform.SetParent(LevelParent, false);


                g.transform.position = new Vector3(LevelParent.position.x, LevelParent.position.y, 0);

                int r = Random.Range(0, 2);
                if (i == 2)
                { 
                if (prepostionword < 2 )
                    r = 0;
                else if (count == 0)
                    r = 1;
            }

                if (r==0)
                {
                    g.GetComponentInChildren<Text>().text = Question[count];
                    g.GetComponent<TravelBubbleScritp_Preposition>().IsPreposition = true;
                    isright = true;
                    Question.Remove(Question[count]);
                    prepostionword++;
                }
                else
                {
                    g.GetComponentInChildren<Text>().text = Otherword[count];
                    g.GetComponent<TravelBubbleScritp_Preposition>().IsPreposition = false;
                    Otherword.Remove(Otherword[count]);
                    count++;
                }
                 
                //Question.Remove
                g.GetComponent<TravelBubbleScritp_Preposition>().BubblestartTavel(_rndpoint[rnd].transform.position, _rndpoint[rnd].name);
               // count++;
                // yield return new WaitForSeconds(.5f);
               
                //print(count);
                _rndpoint.Remove(_rndpoint[rnd]);
                
            }
            isclick = false;
        }

        bool findString(string word)
        {
            foreach (var item in _preposition_list)
            {
                if(word== item)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isclick;
        public void OnClickOnBubble(bool IsTrue, GameObject pos, string animation_name)
        {


            if (isclick) return;
            isclick = true;
            character.Play(animation_name);
            switch (animation_name)
            {
                case "1":
                    bullet.transform.position = bulletPos[0].position;
                    bullet.transform.rotation = bulletPos[0].rotation;
                    break;
                case "2":
                    bullet.transform.position = bulletPos[1].position;
                    bullet.transform.rotation = bulletPos[1].rotation;
                    break;
                case "3":
                    bullet.transform.position = bulletPos[2].position;
                    bullet.transform.rotation = bulletPos[2].rotation;
                    break;
                case "4":
                    bullet.transform.position = bulletPos[3].position;
                    bullet.transform.rotation = bulletPos[3].rotation;
                    break;
                case "5":
                    bullet.transform.position = bulletPos[4].position;
                    bullet.transform.rotation = bulletPos[4].rotation;
                    break;
            }
            SoundManager_Preposition.instanace.buttonclickSound();
            StartCoroutine(waitforendAnimaiton(IsTrue, pos));

        }


        IEnumerator waitforendAnimaiton(bool IsTrue, GameObject pos)
        {
            
            character.SetTrigger("Shoot");
            yield return new WaitForSeconds(.4f);
            
            //Vector2 v_diff = (pos.transform.position - hand.position);
            //float atan2 = Mathf.Atan2(v_diff.y, v_diff.x) * Mathf.Rad2Deg;
            //// hand.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
            //hand.rotation = Quaternion.Euler(new Vector3(0, 0, atan2 - 90));
            // bullet.transform.localPosition = DeflautBulletPos;
            TargetBubble = pos;
            bullet.SetActive(true);
            if (IsTrue)
            {
               // print("Right Move");
                IsRightMove = true;

                 
            }
            else
            {
               // print("wrong move");
                IsRightMove = false;
            }
            IsShoot = true;
        }

        IEnumerator Right_WrongMsgShow()
        {
           

            if (IsRightMove)
            {
                msgtext.text = cms_level.RightMove_Msg;
                Coin++;
               
                //isclick = false;
                prepostionword--;
                SoundManager_Preposition.instanace.BubbleBrustplay();
                coin_object.SetActive(true);
            }
            else
            {
                cms_level.MaxWrongMove--;
                updateHeath(cms_level.MaxWrongMove);

                msgtext.text = cms_level.WrongMove_Msg;
                MsgBox.gameObject.SetActive(false);
                MsgBox.gameObject.SetActive(true);
                CancelInvoke("WaitforMsg");
                Invoke("WaitforMsg", 1.5f);

                SoundManager_Preposition.instanace.WrongMovePlay();

                eyedrop[0].Play();
                eyedrop[1].Play();
                CheckGameOverORGameWin();
            }
            yield return new WaitForSeconds(.6f);
            print(prepostionword);
            if (prepostionword <= 0)
            {
                for (int i = 0; i < LevelParent.childCount; i++)
                {
                    LevelParent.GetChild(i).GetComponent<Button>().interactable = false;
                    LevelParent.GetChild(i).GetComponent<Animator>().enabled = true;
                    LevelParent.GetChild(i).GetComponent<Animator>().Play("bubble_transparent");
                    Destroy(LevelParent.GetChild(i).gameObject, 1f);

                }
            }
            else
            {
                if(!eyedrop[0].isPlaying)
                isclick = false;
            }
            

            if (Coin < (cms_level.MaxRightMove))
            {
                if (prepostionword <= 0)
                {
                    if (!IsGameover)
                        StartCoroutine("BubbleInstatiate");
                }
            }
            else
            {
                
                  CheckGameOverORGameWin();
            }
          
           
        }

        void WaitforMsg()
        {
            MsgBox.gameObject.SetActive(false);
            isclick = false;
            
        }

        void CheckGameOverORGameWin()
        {
            if (cms_level.MaxWrongMove < 0)
            {
                GameOver();
                IsGameover = true;
                eyedrop[0].gameObject.SetActive(false);
                eyedrop[1].gameObject.SetActive(false);
            }
            else
            if (Coin >= (cms_level.MaxRightMove))
            {
                GameWin();
                eyedrop[0].gameObject.SetActive(false);
                eyedrop[1].gameObject.SetActive(false);
            }
            
        }



        //======//Gameover function ===============================================
        public void GameOver()
        {
            if (IsGameover) return;
            TimerCountDown_Preposition.instance.StopTimer();
            UIManager_Preposition.instance.OnClick_popupsButton(1);
            IsGameover = true;


        }
///=========================================================//////////////////
        //Gameover function 
        public void GameWin()
        {
            if (IsGameover) return;
            TimerCountDown_Preposition.instance.StopTimer();
            //StarRating();
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

        public void CoinUpdate()
        {
            UIManager_Preposition.instance.Coin_text.text = Coin.ToString();
        }
       



        void RetryGame()
        {
            ResetGame();
            UIManager_Preposition.instance.OnClick_popupsButton(-1);
           // StartGame();

        }
        public void Timerup()
        {
            if (IsGameover) return;
            TimerCountDown_Preposition.instance.StopTimer();
            UIManager_Preposition.instance.OnClick_popupsButton(5);
            IsGameover = true;


        }
        //Reset Game
        public  void ResetGame()
        {

            currentBobbleCount = 0;
            Coin = 0;
            CoinUpdate();
            IsGameover = false;
            // GameObject[] bubble = GameObject.FindGameObjectsWithTag("Bubble");
            Timer_text.text = "00:00";
            StopCoroutine("BubbleInstatiate");
            for (int i = 0; i < bubble.childCount; i++)
            {
                Destroy(bubble.GetChild(i).gameObject);
            }
            eyedrop[0].gameObject.SetActive(true);
            eyedrop[1].gameObject.SetActive(true);
            InstrucationPanal_Active();
            isclick = false;
            UIManager_Preposition.instance.WiningPoupsound();
            coin_object.SetActive(false);
        }

        void updateHeath(int text)
        {
            health.text = (text+1).ToString();
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
            SetBackendData();

        }

        void SetBackendData()
        {
            _preposition_list.Clear();
            _otherword_list.Clear();
            for (int i = 0; i < APIHandler_Preposition.instance._level1Data.RightList.Count; i++)
            {
                _preposition_list.Add(APIHandler_Preposition.instance._level1Data.RightList[i]);
            }

            for (int i = 0; i < APIHandler_Preposition.instance._level1Data.wrongList.Count; i++)
            {
                _otherword_list.Add(APIHandler_Preposition.instance._level1Data.wrongList[i]);
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
