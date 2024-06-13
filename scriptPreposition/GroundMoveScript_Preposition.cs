using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prepostion
{


public class GroundMoveScript_Preposition : MonoBehaviour
{

        // public GameObject coin;
        // float speedtoMove=3;
        bool isupdate;
        private void OnEnable()
        {
            isupdate = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (Level2Manager_Preposition.instance.IsRunning == false) return;
           if (Level2Manager_Preposition.instance.ISGameOver) return;

            
            transform.Translate(Vector2.left * Time.deltaTime * Level2Manager_Preposition.instance.Speed);
            if (transform.tag == "Preposition")
            {
                if ((int)transform.GetComponent<RectTransform>().localPosition.x <= -800 && isupdate==false)
                {
                    isupdate = true;
                    Level2Manager_Preposition.instance.RandomObjectPick();
                }

                    if ((int)transform.GetComponent<RectTransform>().localPosition.x <= -2000)
                {


                    Level2Manager_Preposition.instance.ObjectFalse(gameObject);

                }
            }
            else
        if ((int)transform.GetComponent<RectTransform>().localPosition.x <= -2250)
        {

                if (transform.name == "Singboard")
                {
                    gameObject.SetActive(false);
                }
                transform.GetComponent<RectTransform>().localPosition = new Vector2(transform.GetComponent<RectTransform>().localPosition.x+4500, transform.GetComponent<RectTransform>().localPosition.y);
                coinTrueFalse(true);
        }

    }

        public void SlideAction()
        {
           
        }
        public void coinTrueFalse(bool istrue)
        {
            //if (coin != null)
            //{
            //    coin.SetActive(istrue);
            //}
        }

}
}
