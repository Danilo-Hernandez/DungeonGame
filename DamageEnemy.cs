using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
	public int damageToDeal;
    public bool isPlayerExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other) {
    	if(other.gameObject.tag == "Enemy") {
            if(isPlayerExplosion) {
                other.gameObject.GetComponent<Enemy>().TakeDamage(Mathf.RoundToInt(PlayerController.instance.bombDamage * PlayerController.instance.bombDamageMod));
            } else {
    		    other.gameObject.GetComponent<Enemy>().TakeDamage(damageToDeal);
            }
    	}
    }

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.tag == "Enemy") {
    		if(isPlayerExplosion) {
                other.GetComponent<Enemy>().TakeDamage(Mathf.RoundToInt(PlayerController.instance.bombDamage * PlayerController.instance.bombDamageMod));
            } else {
                other.GetComponent<Enemy>().TakeDamage(damageToDeal);
            }
    	}
    }
}
