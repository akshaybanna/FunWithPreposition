using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prepostion
{
public class characterRunLevelMenu_Preposition : MonoBehaviour
{
      public Transform target;
       
    // Start is called before the first frame update
    void Awake()
    {
           
                if (UIManager_Preposition.instance.IsBoyCharacter)
                {
                    transform.GetChild(0).transform.SetAsFirstSibling();
                    
                }
                else
                {
                    transform.GetChild(1).transform.SetAsFirstSibling();
                   
                }
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            //if (UIManager_Preposition.instance.current_level != 0 && UIManager_Preposition.instance.Character_Logo.activeSelf==false) 
            //  transform.position = target.position;
    }

}
}
