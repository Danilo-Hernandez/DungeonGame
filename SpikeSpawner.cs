using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
	public GameObject spike;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSpike());
    }

    public IEnumerator SpawnSpike() {
    	yield return new WaitForSeconds(1);

    	Instantiate(spike, transform.position, Quaternion.identity);

    	Destroy(gameObject);
    }
}
