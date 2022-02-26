using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
	public enum BossType {staredownAberration, staredownAglomeration};
	public BossType type;

	public GameObject regularProjectile, homingProjectile, strongProjectile, effect, egg, spike;

	public List<Transform> firePoints, bounds;

	public int health, currentHealth, coinDrop;

	public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
    	currentHealth = health;

    	healthSlider.maxValue = health;
    	healthSlider.value = currentHealth;

        switch(type) {
        	case BossType.staredownAberration:
        		StartCoroutine(staredownAberrationBehaviour());
        		break;
        	case BossType.staredownAglomeration:
        		StartCoroutine(staredownAglomerationBehaviour());
        		break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int dmg) {
    	currentHealth -= dmg;

    	AudioManager.instance.PlaySFX(2);

    	Instantiate(effect, transform.position, Quaternion.Euler(-90, 0, 0));

    	healthSlider.value = currentHealth;

    	if(currentHealth <= 0) {
    		PlayerController.instance.currency += Mathf.RoundToInt(coinDrop * PlayerController.instance.moneyMultiplier);

    		PlayerController.instance.inBattle = false;

        	PlayerController.instance.activeCharges += 10;

    		Destroy(gameObject);
    	}
    }

    public IEnumerator staredownAberrationBehaviour() {
    	yield return new WaitForSeconds(0.5f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			float rand = Random.Range(0,4);

    			switch(rand) {
    				case 0:
    					rand = Random.Range(0f,2f);

    					for(float i = 0; i < rand; i+=0.25f) {
	    					yield return new WaitForSeconds(0.25f);

	    					ShootAllTowardsPlayer();
	    				}

	    				yield return new WaitForSeconds(2);
    					break;
    				case 1:
    					if(currentHealth > health/2) {
	    					rand = Random.Range(0f,1.5f);

	    					for(float i = 0; i < rand; i+=0.5f) {
		    					yield return new WaitForSeconds(0.5f);

		    					ShootHomingTowardsPlayer();
		    				}

		    				yield return new WaitForSeconds(2);
		    			} else {
		    				yield return null;
		    			}
    					break;
    				case 2: 
    					rand = Random.Range(0f,3f);

    					for(float i = 0; i < rand; i+=0.125f) {
	    					yield return new WaitForSeconds(0.125f);

	    					ShootStrongTowardsPlayer();
	    				}

	    				yield return new WaitForSeconds(2);
    					break;
    				case 3: 
    					if(currentHealth <= health/2) {
	    					GetComponent<Animator>().SetTrigger("Atk1");

	    					yield return new WaitForSeconds(0.5f);

	    					for(int i = 0; i < 3; i++) {
		    					ShootAllStraight();
		    					ShootAllReverse();

		    					yield return new WaitForSeconds(0.5f/3f);
		    				}

		    				yield return new WaitForSeconds(2);
		    			} else {
		    				yield return null;
		    			}
    					break;
    			}
    		}

            yield return null;
    	}
    }

    public IEnumerator staredownAglomerationBehaviour() {
    	yield return new WaitForSeconds(0.5f);

    	while(1 == 1) {
    		if(!PlayerController.instance.isPaused) {
    			float rand = Random.Range(0,4);

    			switch(rand) {
    				case 0:
    					rand = 0.25f;

    					for(float i = 0; i < rand; i+=0.25f) {
	    					yield return new WaitForSeconds(0.25f);

	    					ShootFan(135);
	    				}

	    				yield return new WaitForSeconds(2);
    					break;
    				case 1:
    					if(currentHealth <= health/2) {
	    					rand = Random.Range(0f,2f);

	    					for(float i = 0; i < rand; i+=0.5f) {
		    					yield return new WaitForSeconds(0.5f);

		    					ShootEgg(new Vector2(Random.Range(-3f, 3f), Random.Range(-1f, -4f)), Random.Range(1f, 2f));
		    				}

		    				yield return new WaitForSeconds(2);
		    			} else {
		    				yield return null;
		    			}
    					break;
    				case 2: 
    					rand = Random.Range(0f,1f);

    					for(float i = 0; i < rand; i+=0.125f) {
	    					yield return new WaitForSeconds(0.125f);

	    					ShootStrongTowardsPlayer();
	    				}

	    				yield return new WaitForSeconds(2);
    					break;
    				case 3: 
    					if(currentHealth <= health/2) {
		    				for(int i = 0; i < rand; i++) {
		    					Instantiate(spike, new Vector2(Random.Range(bounds[0].position.x, bounds[1].position.x), Random.Range(bounds[0].position.y, bounds[1].position.y)), Quaternion.identity);
		    				}
		    			} else {
		    				yield return null;
		    			}
    					break;
    			}
    		}

            yield return null;
    	}
    }

    public void ShootAllTowardsPlayer() {
    	Vector2 offset = new Vector2(PlayerController.instance.gameObject.transform.position.x - firePoints[0].position.x, PlayerController.instance.gameObject.transform.position.y - firePoints[0].position.y);
		float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

    	for(int i = 0; i < firePoints.Count; i++) {
	    	Instantiate(regularProjectile, firePoints[i].position, Quaternion.Euler(0, 0, angle));
    	}
    }

    public void ShootAllStraight() {
    	for(int i = 0; i < firePoints.Count; i++) {
	    	Instantiate(regularProjectile, firePoints[i].position, Quaternion.Euler(0, 0, 90));
    	}
    }

    public void ShootAllReverse() {
    	for(int i = 0; i < firePoints.Count; i++) {
	    	Instantiate(regularProjectile, firePoints[i].position, Quaternion.Euler(0, 0, -90));
    	}
    }

    public void ShootFan(int degrees) {
    	float ang = 180 - degrees - 180;

    	for(int i = 0; i < degrees/15 + 2; i++) {
    		Instantiate(regularProjectile, firePoints[0].position, Quaternion.Euler(0,0,ang));

    		ang += degrees/15;
    	}
    }

    public void	ShootEgg(Vector2 direction, float distance) {
    	GameObject newEgg = Instantiate(egg, firePoints[Random.Range(1,firePoints.Count)].position, Quaternion.identity);
    	newEgg.GetComponent<Rigidbody2D>().velocity = direction;
    	newEgg.GetComponent<Egg>().time = distance;
    }

    public void ShootHomingTowardsPlayer() {
    	for(int i = 0; i < 2; i++) {
    		int rand = Random.Range(0, firePoints.Count);

	    	Instantiate(homingProjectile, firePoints[rand].position, Quaternion.identity);
    	}
    }

    public void ShootStrongTowardsPlayer() {
    	Vector2 offset = new Vector2(PlayerController.instance.gameObject.transform.position.x - firePoints[0].position.x, PlayerController.instance.gameObject.transform.position.y - firePoints[0].position.y);
		float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

    	Instantiate(strongProjectile, firePoints[0].position, Quaternion.Euler(0, 0, angle));
    }
}
