using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{

    public Rigidbody2D birdRigidbody2D;
    public float jumpStrength;
    public bool birdIsAlive = true;
    
    private Logic logic;  // Logic class
    
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
        Debug.Log(birdIsAlive.ToString());
    }

    // Update is called once per frame
    void Update()
    {

        if (birdIsAlive && Input.GetKeyDown(KeyCode.Space))
        {
            birdRigidbody2D.velocity = Vector2.up * jumpStrength;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birdIsAlive = false;
    }

}
