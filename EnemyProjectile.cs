using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	public float speed;

	public GameObject effect;

	public enum Type {regular, homing};
	public Type type;

	public bool isPoison, isBurn, isReflected;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        AudioManager.instance.PlaySFX(0);

        if(isReflected) {
        	GetComponent<DamagePlayer>().damageToDeal = Mathf.RoundToInt(PlayerController.instance.damageToDeal * PlayerController.instance.damageMod * 1.5f);
        }

        if(PlayerController.instance.curses.Contains(PlayerController.Curses.infected)) {
        	isPoison = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(type == Type.homing) {
        	Vector2 offset = new Vector2(PlayerController.instance.gameObject.transform.position.x - transform.position.x, PlayerController.instance.gameObject.transform.position.y - transform.position.y);
			float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if(PlayerController.instance.isPaused) {
        	GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        } else {
        	GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
    	Instantiate(effect, transform.position, transform.rotation);

    	if(other.gameObject.tag == "Player") {
    		if(isPoison) {
    			PlayerController.instance.poisonTime = 10;
    			PlayerController.instance.poisoned = true;
    		}

    		if(isBurn) {
    			PlayerController.instance.burnTime = 10;
    			PlayerController.instance.burned = true;
    		}
    	}

    	Destroy(gameObject);
    }
}
