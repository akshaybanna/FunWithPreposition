using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Prepostion;

namespace Level3
{
    public class Level3Manager_Preposition : MonoBehaviour
    {
        public static Level3Manager_Preposition instance;

        [System.Serializable]
        public class QuestionData
        {
            public string itemName;
            public string proposition;
            public Sprite itemSprite;
            public GameObject itemObject;
        }

        [System.Serializable]
        public class LevelData
        {
            public int currentQuestionCount;
            public int totalQuestionCount;
            public int worngQuestionCount;
            public int rightQuestionCount;
            public int totalWrongQuestionCount;
        }


        [Header("Question Data")]
        public QuestionData[] questionData;

        [Header("Level Data")]
        public LevelData leveldata;

        [Header("Completed Question Data")]
        public List<int> completedQuestion = new List<int>();

        [Header("Game Question")]
        public QuestionItemList itemQuestion;
        public QuestionItemList propositionQuestion;

        //public DragItem questionImage;

        [Header("Level UI")]
        public Canvas gameCanvas;

        //public RectTransform startPos;

        public GameObject Instrucation_panel;

        void OnEnable()
        {
            instance = this;
            StartGame();
        }
        private void Awake()
        {
           // 
        }

        public virtual void StartGame()
        {
            for (int i=0;i< questionData.Length;i++)
            {
                questionData[i].itemObject.GetComponent<DragItem>().StopGlow();
                if (!questionData[i].itemObject.GetComponent<DragItem>().isDone)
                {
                   // questionData[i].itemObject.SetActive(false);
                }
            }

            leveldata.currentQuestionCount++;
            if (leveldata.currentQuestionCount >= leveldata.totalQuestionCount)
            {
                Level3SubLevelManager.ChangeLevel();
                ResetData();
                return;
            }
            int questionNumber = getQuestionNumber();
            QuestionData question = questionData[questionNumber];

            itemQuestion.questionKey = question.itemName;
            propositionQuestion.questionKey = question.proposition;

            question.itemObject.GetComponent<DragItem>().StartGlow();
            question.itemObject.GetComponent<DragItem>().correctAnswer = question.itemName;
            question.itemObject.GetComponent<DragItem>().isDone = false;
            //question.itemObject.GetComponent<RectTransform>().anchoredPosition = startPos.anchoredPosition;
            question.itemObject.SetActive(true);

            if (!itemQuestion.gameObject.activeSelf)
            {
                itemQuestion.gameObject.SetActive(true);
            }

            if (!propositionQuestion.gameObject.activeSelf)
            {
                propositionQuestion.gameObject.SetActive(true);
            }
            
        }

        public int getQuestionNumber()
        {
            while (true)
            {
                int rand = Random.Range(0, questionData.Length);
                if (completedQuestion.Contains(rand))
                {
                    continue;
                }
                completedQuestion.Add(rand);
                return rand;
            }
        }

        public virtual void OnCorrectAnswer(bool isCorrect)
        {
            if (isCorrect)
            {
                SoundManager_Preposition.instanace.RightMoveplayLevel3_4();

                leveldata.rightQuestionCount++;

                if (leveldata.currentQuestionCount >= leveldata.totalQuestionCount)
                {
                    Level3SubLevelManager.ChangeLevel();
                    ResetData();
                }
                else
                {
                    StartGame();
                }



            }
            else
            {
                SoundManager_Preposition.instanace.WrongMovePlay();
                leveldata.worngQuestionCount++;
              Level3SubLevelManager.instance_.updateHeath((leveldata.totalWrongQuestionCount - leveldata.worngQuestionCount));
                if (leveldata.worngQuestionCount > leveldata.totalWrongQuestionCount)
                {
                    //print("losose popup");
                    Level3SubLevelManager.OnLoose();
                    ResetData();

                }
            }
        }

        public void Reset()
        {
            ResetData();
        }

        public virtual void ResetData()
        {
            leveldata.currentQuestionCount = 0;
            leveldata.worngQuestionCount = 0;
            leveldata.rightQuestionCount = 0;
            completedQuestion.Clear();
           // print("call");
            Level3SubLevelManager.instance_.Timer_text.text = "00:00";
            for (int i = 0; i < questionData.Length; i++)
            {

                questionData[i].itemObject.GetComponent<RectTransform>().anchoredPosition = questionData[i].itemObject.GetComponent<DragItem>().rectPos;
                questionData[i].itemObject.GetComponent<RectTransform>().localScale = questionData[i].itemObject.GetComponent<DragItem>().size;
                questionData[i].itemObject.transform.localScale = questionData[i].itemObject.GetComponent<DragItem>().size;
                questionData[i].itemObject.GetComponent<DragItem>().StopGlow();
                questionData[i].itemObject.GetComponent<DragItem>().glow.transform.GetChild(0).gameObject.SetActive(false);
                questionData[i].itemObject.GetComponent<DragItem>().isDone = true;
               
                //questionData[i].itemObject.SetActive(false);
            }
           
            KidsLevel.instance.ResetData();
            UIManager_Preposition.instance.WiningPoupsound();

            // Level3SubLevelManager.instance_.InstrucationPanal_Active();
        }

        public void StartNewGame()
        {

        }
    }
}


