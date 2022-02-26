using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableSpawner : MonoBehaviour
{
	public List<GameObject> breakables = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {	
    	if(Random.Range(0,4) == 0) {
        	Instantiate(breakables[Random.Range(0, breakables.Count)], transform.position, Quaternion.identity);
    	}

        Destroy(gameObject);
    }
}
