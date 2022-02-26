using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
	public GameObject boss, gate, gate2, doubleBoss;

	public GameObject[] doors;

	public bool initiated, terminated;

	public float range;
    // Start is called before the first frame update
    void Start()
    {
        CameraController.instance.gameObject.transform.position = new Vector3(0,5,-10);
        CameraController.instance.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range && !initiated) {
        	initiated = true;

        	if(PlayerController.instance.curses.Contains(PlayerController.Curses.duplicate)) {
        		doubleBoss.SetActive(true);
        	} else {
        		boss.SetActive(true);
        	}

        	PlayerController.instance.inBattle = true;

        	for(int i = 0; i < doors.Length; i++) {
        		doors[i].SetActive(true);
        	}
        }

        if((boss == null || doubleBoss == null) && !terminated) {
        	int rand = Random.Range(0,100);

        	if(rand < 5 + PlayerController.instance.luck) {
        		gate2.SetActive(true);
        	} else {
        		gate.SetActive(true);
        	}

        	for(int i = 0; i < doors.Length; i++) {
        		doors[i].SetActive(false);
        	}

        	terminated = true;
        }
    }
}
