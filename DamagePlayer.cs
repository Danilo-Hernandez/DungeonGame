using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
	public int damageToDeal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other) {
    	if(other.gameObject.tag == "Player") {
    		PlayerController.instance.TakeDamage(damageToDeal);
    	}
    }

    void OnCollisionStay2D(Collision2D other) {
    	if(other.gameObject.tag == "Player") {
    		PlayerController.instance.TakeDamage(damageToDeal);
    	}
    }

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.tag == "Player") {
    		PlayerController.instance.TakeDamage(damageToDeal);
    	}
    }
}
