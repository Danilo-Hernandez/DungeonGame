using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
   	public bool isValuable;

   	public GameObject effect;

    void Update() {
    	if(Vector3.Distance(PlayerController.instance.transform.position, transform.position) <= 0.5f) {
    		if(isValuable) {
    			PlayerController.instance.currency += Mathf.RoundToInt(Random.Range(1, 4) * PlayerController.instance.moneyMultiplier);
    		}

    		Instantiate(effect, transform.position, Quaternion.identity);

    		AudioManager.instance.PlaySFX(5);

    		Destroy(gameObject);
    	}
    }
}
