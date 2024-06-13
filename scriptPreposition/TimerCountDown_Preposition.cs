using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prepostion;
using Level3;
namespace Prepostion
{
    public class TimerCountDown_Preposition : MonoBehaviour
    {
        public static TimerCountDown_Preposition instance;
        public float timeLeft = 300.0f;
        public bool stop = true;

        private float minutes;
        private float seconds;

        public Text text;
        bool IsRattigCalculation;

        private void Awake()
        {
            instance = this;
        }
        public void startTimer(float from,Text Time)
        {
            IsRattigCalculation = false;
            stop = false;
            timeLeft = from;
            text = Time;
            //print(timeLeft);
            //Update();
            //StartCoroutine(updateCoroutine());
        }

        public void startTimer(float from, bool _IsRattigCalculation)
        {
            stop = false;
            timeLeft = from;
            IsRattigCalculation = _IsRattigCalculation;
            //Update();
            //StartCoroutine(updateCoroutine());
        }

        void Update()
        {
            if (stop) return;
            timeLeft -= Time.deltaTime;

            if(UIManager_Preposition.instance.current_level== 1)
            {
                Level2Manager_Preposition.instance.changeSituation();
            }
         
            minutes = Mathf.Floor(timeLeft / 60);
            seconds = timeLeft % 60;

            if (seconds > 59) seconds = 59;

            if (minutes < 0)
            {
                stop = true;
                minutes = 0;
                seconds = 0;
                TimeEnd();
            }
            text.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            //        fraction = (timeLeft * 100) % 100;
        }

        public void StopTimer()
        {
            stop = true;

        }
        void TimeEnd()
        {

            if(0 == UIManager_Preposition.instance.current_level)
            {
               
                    Level1Manager_Preposition.instance.Timerup();
            }
            if (1 == UIManager_Preposition.instance.current_level)
            {
                Level2Manager_Preposition.instance.TimeUp();
            }
            if (2 == UIManager_Preposition.instance.current_level)
            {
                Level3SubLevelManager.instance_.TimeUp();
            }
            if (3 == UIManager_Preposition.instance.current_level)
            {
                Level4Manager_Preposition.instance.Timerup();
            }
            if (4 == UIManager_Preposition.instance.current_level)
            {
                Level5Manager_Preposition.instance.Timerup();
            }
        }


    }
}