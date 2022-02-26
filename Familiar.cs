using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{
	public float range, speed;
	public int damage, audioToPlay;
	public bool damages, canAtk = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(PlayerController.instance.transform.parent, true);
    }

    // Update is called once per frame
    void Update()
    {
    	if(damages) {
    		Enemy[] enemies = FindObjectsOfType<Enemy>();

    		if(enemies.Length > 0) {
    			if(Vector3.Distance(transform.position, enemies[0].gameObject.transform.position) > range) {
		        	Vector2 vel = (enemies[0].gameObject.transform.position - transform.position);
		        	vel.Normalize();
		       		GetComponent<Rigidbody2D>().velocity = vel * speed;
		        } else {
		        	if(canAtk) {
		        		enemies[0].TakeDamage(50);

			        	AudioManager.instance.PlaySFX(audioToPlay);

			        	GetComponent<Rigidbody2D>().velocity = Vector2.zero;

			        	StartCoroutine(AtkCooldown(5));
		        	}
		        }
    		} else {
    			if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) > range * 4) {
		        	Vector2 vel = (PlayerController.instance.gameObject.transform.position - transform.position);
		        	vel.Normalize();
		       		GetComponent<Rigidbody2D>().velocity = vel * speed;
		        } else {
		        	GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		        }
    		}
    	} else {
    		if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) > range * 4) {
		       	Vector2 vel = (PlayerController.instance.gameObject.transform.position - transform.position);
		       	vel.Normalize();
		       	GetComponent<Rigidbody2D>().velocity = vel * speed;
		    } else {
		       	GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		    }
    	}
    }

    public IEnumerator AtkCooldown(float secs) {
    	canAtk = false;

    	yield return new WaitForSeconds(secs);

    	canAtk = true;
    }
}
