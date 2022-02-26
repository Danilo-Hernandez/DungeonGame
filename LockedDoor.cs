using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
	public GameObject canvas;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerController.instance.transform.position, transform.position) < 1.5f) {
        	canvas.SetActive(true);
        } else {
        	canvas.SetActive(false);
        }
    }
}
