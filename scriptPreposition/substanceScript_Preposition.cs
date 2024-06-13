using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prepostion
{
public class substanceScript_Preposition : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
            if (UIManager_Preposition.instance.IsBoyCharacter)
            {
              
                transform.GetChild(0).transform.SetAsFirstSibling();
               
               
            }
            else
            {
                print("aaya");
                transform.GetChild(1).transform.SetAsFirstSibling();
                
            }
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            if (UIManager_Preposition.instance.current_level == 0)
                Level1Manager_Preposition.instance.character = transform.GetChild(0).GetComponent<Animator>();
            else
                Level4Manager_Preposition.instance.Sleepingcharcter = transform.GetChild(0).GetComponent<Animator>();
        }
}
}