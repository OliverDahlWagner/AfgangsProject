using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingPar : MonoBehaviour
{
    // Start is called before the first frame update


    private void Update()
    {
        if (Input.anyKey)
        {
            GetComponent<ParticleSystem>().Play();
        }
    }
}
