using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public enum EnemyType {basicSlime, masterSlime, staredown, masterStaredown, staredownSorcerer, masterStaredownSorcerer, staredownCommander, masterStaredownCommander,
	basicShifter, masterShifter, energyCrawler, staredownRefractor};
	public EnemyType behaviour;
	public float range, speed;

	public List<Transform> firePoints;

	public int health, currentHealth, coinDrop, chargesToGive;

	public bool transitioned;

	public float burnCounter, poisonCounter, poisonTime, burnTime;

	public GameObject enemyGO, regularProjectile, chasingProjectile, strongProjectile, effect, poisonIcon, burnIcon, slime, shield, satchetItem;

    // Start is called before the first frame update
    void Start()
    {
    	currentHealth = health;

        switch(behaviour) {
        	case EnemyType.basicSlime:
        		StartCoroutine(BasicSlimeBehaviour());
        		break;
        	case EnemyType.masterSlime:
        		break;
        	case EnemyType.staredown:
        		StartCoroutine(StaredownBehaviour());
        		break;
        	case EnemyType.masterStaredown:
        		break;
        	case EnemyType.staredownSorcerer:
        		StartCoroutine(StaredownSorcererBehaviour());
        		break;
        	case EnemyType.masterStaredownSorcerer:
        		break;
        	case EnemyType.staredownCommander:
        		StartCoroutine(StaredownCommanderBehaviour());
        		break;
        	case EnemyType.masterStaredownCommander:
        		break;
        	case EnemyType.basicShifter:
        		StartCoroutine(ShifterBehaviour());
        		break;
        	case EnemyType.masterShifter:
        		break;
        	case EnemyType.energyCrawler:
        		StartCoroutine(EnergyCrawlerBehaviour());
        		break;
        	case EnemyType.staredownRefractor:
        		StartCoroutine(ReflectorBehaviour());
        		break;
        }
    }

    void Update() {
    	if(behaviour != EnemyType.basicShifter) {
	    	if(GetComponent<Rigidbody2D>().velocity.x > 0) {
	    		transform.localScale = new Vector3(-1, 1, 1);
	    	} else if(GetComponent<Rigidbody2D>().velocity.x < 0) {
	    		transform.localScale = new Vector3(1, 1, 1);
	    	} else {
	    		if(transform.position.x > PlayerController.instance.gameObject.transform.position.x) {
	    			transform.localScale = new Vector3(1, 1, 1);
	    		} else {
	    			transform.localScale = new Vector3(-1, 1, 1);
	    		}
	    	}
	    }

	    if(!PlayerController.instance.isPaused && behaviour == EnemyType.staredownRefractor) {
    		PlayerProjectile[] projectiles = FindObjectsOfType<PlayerProjectile>();

    		bool inRange = false;

    		for(int i = 0; i < projectiles.Length; i++) {
    			if(Vector3.Distance(projectiles[i].gameObject.transform.position, transform.position) <= range) {
    				inRange = true;
    			}
    		}

    		if(inRange) {
    			StartCoroutine(Shield());
    		}
    	}

    	poisonCounter -= Time.deltaTime;
    	burnCounter -= Time.deltaTime;
    	burnTime -= Time.deltaTime;
    	poisonTime -= Time.deltaTime;

    	if(poisonTime > 0 && poisonCounter <= 0) {
    		if(currentHealth > 1) {
    			TakeDamage(1);
    			poisonCounter = 0.5f;
    		}
    	}

    	if(poisonTime > 0) {
    		poisonIcon.SetActive(true);
    	} else {
    		poisonIcon.SetActive(false);
    	}

    	if(burnTime > 0) {
    		burnIcon.SetActive(true);
    	} else {
    		burnIcon.SetActive(false);
    	}

    	if(burnTime > 0 && burnCounter <= 0) {
    		TakeDamage(5);
    		burnCounter = 2f;
    	}
    }

    public IEnumerator BasicSlimeBehaviour() {
    	yield return new WaitForSeconds(0.25f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range) {
    				GetComponent<Rigidbody2D>().velocity = (PlayerController.instance.gameObject.transform.position - transform.position) * speed;
    			} else {
    				Wander();
    				yield return new WaitForSeconds(Random.Range(0f, 1f));
    			}
    		} else {
    			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		}
    		yield return null;
    	}
    }

    public IEnumerator ReflectorBehaviour() {
    	yield return new WaitForSeconds(0.25f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			Wander();
    			yield return new WaitForSeconds(Random.Range(0f, 1f));
    		} else {
    			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		}
    		yield return null;
    	}
    }

    public IEnumerator Shield() {
    	shield.SetActive(true);

    	yield return new WaitForSeconds(1);

    	shield.SetActive(false);
    }

    public IEnumerator ShifterBehaviour() {
    	yield return new WaitForSeconds(0.25f);

    	while(1 == 1) {
    		transform.GetChild(0).GetComponent<Animator>().SetFloat("x", GetComponent<Rigidbody2D>().velocity.x);
    		transform.GetChild(0).GetComponent<Animator>().SetFloat("y", GetComponent<Rigidbody2D>().velocity.y);

    		Vector2 mousePos = new Vector2(PlayerController.instance.transform.position.x - transform.position.x, PlayerController.instance.transform.position.y - transform.position.y);

	        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
	        transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, angle);

    		if(!PlayerController.instance.isPaused) {
    			if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range) {
    				if(!transitioned) {
    					transform.GetChild(0).GetComponent<Animator>().SetTrigger("Transition");
    					transitioned = true;
    					transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    				} else {
    					GetComponent<Rigidbody2D>().velocity = (transform.position - PlayerController.instance.gameObject.transform.position) * speed;
    				}
    			} else {
    				Wander();
    				yield return new WaitForSeconds(Random.Range(1f, 1.5f));

    				if(transitioned) {
    					ShootTowardsPlayer();
    				}
    			}
    		} else {
    			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		}
    		yield return null;
    	}
    }

    public IEnumerator StaredownBehaviour() {
    	yield return new WaitForSeconds(0.25f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range) {
    				if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range/3) {
	    				GetComponent<Rigidbody2D>().velocity = (transform.position - PlayerController.instance.gameObject.transform.position) * speed;
	    			} else {
	    				GetComponent<Rigidbody2D>().velocity = Vector2.zero;

	    				yield return new WaitForSeconds(Random.Range(1f, 1.5f));
    					ShootTowardsPlayer();
	    			}
    			}
    		} else {
    			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		}
    		yield return null;
    	}
    }

    public IEnumerator EnergyCrawlerBehaviour() {
    	yield return new WaitForSeconds(0.25f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range) {
    				if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range/3) {
	    				GetComponent<Rigidbody2D>().velocity = (PlayerController.instance.gameObject.transform.position - transform.position) * speed;
	    				shield.SetActive(false);
	    			} else {
	    				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	    				shield.SetActive(true);

	    				yield return new WaitForSeconds(Random.Range(0.25f, 1f));
    					ShootTowardsPlayer();
	    			}
    			}
    		} else {
    			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		}
    		yield return null;
    	}
    }

    public IEnumerator StaredownSorcererBehaviour() {
    	yield return new WaitForSeconds(0.25f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range) {
    				if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range/3) {
	    				GetComponent<Rigidbody2D>().velocity = (transform.position - PlayerController.instance.gameObject.transform.position) * speed;
	    			} else {
	    				GetComponent<Rigidbody2D>().velocity = Vector2.zero;

	    				yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
    					ShootChasingTowardsPlayer();
	    			}
    			}
    		} else {
    			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		}
    		yield return null;
    	}
    }

    public IEnumerator StaredownCommanderBehaviour() {
    	yield return new WaitForSeconds(0.25f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range) {
    				if(Vector3.Distance(PlayerController.instance.gameObject.transform.position, transform.position) < range/3) {
	    				GetComponent<Rigidbody2D>().velocity = (transform.position - PlayerController.instance.gameObject.transform.position) * speed;
	    			} else {
	    				GetComponent<Rigidbody2D>().velocity = Vector2.zero;

	    				yield return new WaitForSeconds(Random.Range(1f, 1.5f));
    					ShootStrongTowardsPlayer();
	    			}
    			}
    		} else {
    			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		}
    		yield return null;
    	}
    }

    public void Wander() {
    	int rand = Random.Range(0,8);

    	switch(rand) {
    		case 0:
    			GetComponent<Rigidbody2D>().velocity = new Vector2(1,0) * speed;
    			break;
    		case 1: 
    			GetComponent<Rigidbody2D>().velocity = new Vector2(0,1) * speed;
    			break;
    		case 2: 
    			GetComponent<Rigidbody2D>().velocity = new Vector2(-1,0) * speed;
    			break;
    		case 3: 
    			GetComponent<Rigidbody2D>().velocity = new Vector2(0,-1) * speed;
    			break;
    		case 4: 
    			GetComponent<Rigidbody2D>().velocity = new Vector2(1,1) * speed;
    			break;
    		case 5: 
    			GetComponent<Rigidbody2D>().velocity = new Vector2(-1,-1) * speed;
    			break;
    		case 6: 
    			GetComponent<Rigidbody2D>().velocity = new Vector2(-1,1) * speed;
    			break;
    		case 7: 
    			GetComponent<Rigidbody2D>().velocity = new Vector2(1,-1) * speed;
    			break;
    	}
    }

    public void ShootTowardsPlayer() {
		Vector2 offset = new Vector2(PlayerController.instance.gameObject.transform.position.x - firePoints[0].position.x, PlayerController.instance.gameObject.transform.position.y - firePoints[0].position.y);
	    float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

    	Instantiate(regularProjectile, firePoints[0].position, Quaternion.Euler(0, 0, angle));
    }

    public void ShootStrongTowardsPlayer() {
		Vector2 offset = new Vector2(PlayerController.instance.gameObject.transform.position.x - firePoints[0].position.x, PlayerController.instance.gameObject.transform.position.y - firePoints[0].position.y);
	    float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

    	Instantiate(strongProjectile, firePoints[0].position, Quaternion.Euler(0, 0, angle));
    }

    public void ShootChasingTowardsPlayer() {
    	Instantiate(chasingProjectile, firePoints[0].position, Quaternion.identity);
    }

    public void TakeDamage(int dmg) {
    	currentHealth -= dmg;

    	AudioManager.instance.PlaySFX(2);

    	Instantiate(effect, transform.position, Quaternion.Euler(-90, 0, 0));

    	if(currentHealth <= 0) {
    		PlayerController.instance.currency += Mathf.RoundToInt(coinDrop * PlayerController.instance.moneyMultiplier);

    		PlayerController.instance.activeCharges += chargesToGive;

    		if(PlayerController.instance.activeItems.Contains(Item.ItemType.satchet)) {
    			if(Random.Range(0,20) == 0) {
    				Instantiate(satchetItem, transform.position, Quaternion.identity);
    			}
    		}

    		Destroy(enemyGO);
    	}
    }
}
