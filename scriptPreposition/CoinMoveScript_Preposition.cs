using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prepostion
{
public class CoinMoveScript_Preposition : MonoBehaviour
{

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (Level1Manager_Preposition.instance.IsGameover) return;
        transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 10);
        if (Vector2.Distance(transform.position, target.position) < 0.01f)
        {
            gameObject.SetActive(false);
                Level1Manager_Preposition.instance.CoinUpdate();
                Level2Manager_Preposition.instance.CoinUpdate();
                SoundManager_Preposition.instanace.pennyCollectPlay();
        }
    }
}
}