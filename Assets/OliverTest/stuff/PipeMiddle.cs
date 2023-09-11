using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMiddle : MonoBehaviour
{

    private Logic logic;  // Logic class

    private void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)   // layer 3 is the bird layer, will only trigger if it hit something on that layer
        {
            logic.AddScore(1);
        }
        
    }

}
