using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
	public GameObject effect, slime;

	public bool poison, fire, isFragment;

    void Start()
    {
    	//sets velocity to players bullet speed
        GetComponent<Rigidbody2D>().velocity = transform.right * PlayerController.instance.bulletSpeed * PlayerController.instance.bulletSpeedMod;

        AudioManager.instance.PlaySFX(1);
    }

    // Update is called once per frame
    void Update()
    {	
    	//checks if paused
        if(PlayerController.instance.isPaused) {
        	GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        } else {
        	GetComponent<Rigidbody2D>().velocity = transform.right * PlayerController.instance.bulletSpeed * PlayerController.instance.bulletSpeedMod;
        }
    }

    //checks for collision
    void OnCollisionEnter2D(Collision2D other) {
    	//checks for enemy component and deals damage
    	if(other.gameObject.GetComponent<Enemy>() != null) {
    		if(!isFragment) {
    			other.gameObject.GetComponent<Enemy>().TakeDamage(Mathf.RoundToInt(PlayerController.instance.damageToDeal * PlayerController.instance.damageMod));
    		} else {
    			other.gameObject.GetComponent<Enemy>().TakeDamage(Mathf.RoundToInt((PlayerController.instance.damageToDeal * PlayerController.instance.damageMod)/3));
    		}

    		if(poison) {
    			//poisons enemy
    			other.gameObject.GetComponent<Enemy>().poisonTime = 5;
    		}

    		if(fire) {
    			//burns enemy
    			other.gameObject.GetComponent<Enemy>().burnTime = 4;
    		}
    	}

    	//checks for boss component and deals damage
    	if(other.gameObject.GetComponent<Boss>() != null) {
    		other.gameObject.GetComponent<Boss>().TakeDamage(Mathf.RoundToInt(PlayerController.instance.damageToDeal * PlayerController.instance.damageMod));
    	}

    	Instantiate(effect, transform.position, transform.rotation);

    	//checks if player has parasite curse
    	if(PlayerController.instance.curses.Contains(PlayerController.Curses.parasite) && other.gameObject.GetComponent<Enemy>() == null && 
    	other.gameObject.GetComponent<Boss>() == null && !isFragment) {
    		Instantiate(slime, transform.position, Quaternion.identity);
    	}

    	Destroy(gameObject);
    }
}
