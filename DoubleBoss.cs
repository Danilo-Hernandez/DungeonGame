using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBoss : MonoBehaviour
{
   	public GameObject boss1, boss2;

    // Update is called once per frame
    void Update()
    {
        if(boss1 == null && boss2 == null) {
        	Destroy(gameObject);
        }
    }
}
