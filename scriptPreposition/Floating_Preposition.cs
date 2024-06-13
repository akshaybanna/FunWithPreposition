using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating_Preposition : MonoBehaviour
{
    Vector2 floatY;
    float originalY;

    public float floatStrength;

    private void OnEnable()
    {
        this.originalY = this.transform.position.y;
        time = 0;
    }

    void Start()
    {
       
    }
    float time;
    void Update()
    {
        /* Old code:
        floatY = transform.position;
        floatY.y = originalY + (Mathf.Sin(Time.time) * floatStrength);
        transform.position = floatY;
        */
        time += Time.deltaTime;
        transform.position = new Vector2(transform.position.x,
            originalY +(float) (System.Math.Sin(time) * floatStrength));
    }
}
