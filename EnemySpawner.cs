using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject basic, advanced, master;

    // Start is called before the first frame update
    void Start()
    {
        float rand = Random.Range(0f,100f);

        if(rand < 0.1 + DungeonGenerator.instance.difficultyMod) {
        	Destroy(basic);
        	Destroy(advanced);
        } else if(rand < 2 + DungeonGenerator.instance.difficultyMod) {
        	Destroy(basic);
        	Destroy(master);
        } else {
        	Destroy(master);
        	Destroy(advanced);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
