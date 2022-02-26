using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOutline : MonoBehaviour
{
    public bool closeWhenEntered;

    public GameObject[] doors;

    public GameObject pickupSpawner;

    //public List<GameObject> enemies = new List<GameObject>();

    [HideInInspector]
    public bool roomActive;

    public GameObject mapHider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* if(enemies.Count > 0 && roomActive && openWhenEnemiesCleared)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] == null)
                {
                    enemies.RemoveAt(i);

                    i--;
                }
            }

            if(enemies.Count == 0)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(false);

                    closeWhenEntered = false;
                }
            }
        } */
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }

        if(Random.Range(0,5) == 0) {
        	Instantiate(pickupSpawner, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !roomActive)
        {
            CameraController.instance.ChangeTarget(transform);

            if(closeWhenEntered)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;

            mapHider.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && !roomActive)
        {
            CameraController.instance.ChangeTarget(transform);

            if(closeWhenEntered)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;

            mapHider.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            roomActive = false;
        }
    }
}

