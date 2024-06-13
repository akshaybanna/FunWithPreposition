using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prepostion
{

    public class TravelBubbleScritp_Preposition : MonoBehaviour
    {
        // Start is called before the first frame update

        public bool IsPreposition;
        bool IsTravel;
        public string position_index;
        Vector2 TargetPos;
        Vector2 startpos;
        float speed;
        void Start()
        {
           // BubblestartTavel();
            speed = 5;


        }


        // Update is called once per frame
        void Update()
        {
            if (IsTravel)
            {

                transform.position = Vector2.MoveTowards(transform.position, TargetPos, Time.deltaTime * speed);
                if (Vector2.Distance(transform.position, TargetPos) < .01f)
                {
                    IsTravel = false;
                    transform.GetComponent<Floating_Preposition>().enabled = true;
                   // BubblestartTavel();
                    //speed = .5f;
                    // print("reacj");
                }
            }
        }

        // call to find random point on the screen for bubble
        void randomPoint()
        {
            //startpos=transform
          //  float xpos = Random.Range(Level1Manager_Preposition.instance.topLeft_Point.position.x, Level1Manager_Preposition.instance.topRight_Point.position.x);
            //float Ypos = Random.Range(Level1Manager_Preposition.instance.topLeft_Point.position.y, Level1Manager_Preposition.instance.DownRight_Point.position.y);
           // TargetPos = new Vector2(xpos, Ypos);

            // print(TargetPos);
        }



        public void BubblestartTavel(Vector2 target, string name)
        {
            //print("name");
            TargetPos = target;
            position_index = name;
            IsTravel = true;


        }

        public void OnClick()
        {
            SoundManager_Preposition.instanace.buttonclickSound();
            Level1Manager_Preposition.instance.OnClickOnBubble(IsPreposition, gameObject, position_index);
        }



    }
}
