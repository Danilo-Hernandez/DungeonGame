using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just unpauses the game
public class UnPauser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
