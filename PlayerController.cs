using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	//items player currently carries
	public List<Item.ItemType> items = new List<Item.ItemType>(), activeItems = new List<Item.ItemType>();

	//active items in player's possesion
	public List<GameObject> actives = new List<GameObject>(), familiars = new List<GameObject>();

	//float miscellaneous values
	public float speed, bulletSpeed, shotSpeed, damageMod = 1, speedMod = 1, bulletSpeedMod = 1, shotSpeedMod = 1, moneyMultiplier = 1, burnTime, poisonTime, bombDamageMod,
	luck;

	//time counters
	private float shotCounter, damageCounter, interactionCounter, activeCounter, poisonCounter, burnCounter, switchCounter, pauseCounter;

	//booleans
	public bool isPaused = true, inBattle, doubleShot, burned, poisoned, hasPoison, hasFire, tripleShot;

	//the wand used to fire
	public Transform wand;

	//curses that can be obtained
	public enum Curses {duplicate, crystal, eternal, parasite, infected};

	//items that can be put in a satchet
	public enum SatchetItems {containedSingularity, soulShard, magicDoubler, cursedDroplet, juiceOfLife, hA, dA, cA, sA, d4, d6, d8, d20};

	//items in player's satchet
	public List<SatchetItems> satchetItems = new List<SatchetItems>(), deckItems = new List<SatchetItems>(), dieItems = new List<SatchetItems>();

	//curses currently in player
	public List<Curses> curses = new List<Curses>();

	//projectile types
	public GameObject projectile, doubleProjectile, fireProjectile, doubleFireProjectile, poisonProjectile, doublePoisonProjectile, fire_PoisonProjectile,
	doubleFire_PoisonProjectile, cursedItemSpawner, crystalProjectile, fireCrystalProjectile, fire_PoisonCrystalProjectile, poisonCrystalProjectile, doubleCrystalProjectile,
	doubleFireCrystalProjectile, doublePoisonCrystalProjectile, doubleFire_PoisonCrystalProjectile, itemSpawner, shopItemSpawner, tripleProjectile, tripleCrystalProjectile,
	tripleFireProjectile, tripleFireCrystalProjectile, triplePoisonProjectile, triplePoisonCrystalProjectile, tripleFire_PoisonCrystalProjectile, tripleFire_PoisonProjectile,
	bomb;

	//hit effect
	public GameObject effect;

	//integer miscellaneous values
	public int damageToDeal, maxActives = 1, currentActive, activeCharges, currentSatchetItem, maxHealth, healthMod, currentHealth, currency, keys, lives, currentDeckItem, 
	currentDieItem, bombs, bombDamage;

	void Awake() {
		//checks if player already exists
		if(instance == null) {
    		instance = this;
    	} else {
    		Destroy(transform.parent.gameObject);
    	}
		DontDestroyOnLoad(transform.parent);
	}

    void Start()
    {
    	//sets health to max and pauses the game
        currentHealth = maxHealth;
        isPaused = true;
    }

    public void TakeDamage(int dmg) {

    	//checks if player can recieve damage
    	if(damageCounter <= 0) {
	    	currentHealth -= dmg;

	    	AudioManager.instance.PlaySFX(4);

	    	Instantiate(effect, transform.position, Quaternion.Euler(-90, 0, 0));

	    	damageCounter = 0.5f;

	    	//checks if player is dead
	    	if(currentHealth <= 0) {
	    		if(lives == 0) {
		    		currentHealth = 0;
		    		SceneManager.LoadScene("TitleScreen");
		    		Destroy(CameraController.instance.gameObject);
		    		Destroy(DebugController.instance.gameObject);
		    		Destroy(DontDestroy.instance.gameObject);
		    		Destroy(UIController.instance.gameObject);
		    		Destroy(transform.parent.gameObject);
		    	} else if(lives > 0) {
		    		if(curses.Contains(Curses.eternal)) {
		    			maxHealth = 1;
		    			healthMod = 0;
		    		}
		    		currentHealth = maxHealth + healthMod;
		    		transform.position = Vector3.zero;
		    		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		    		lives --;
		    	}
	    	}
	    }
    }

    void Update()
    {	
    	//checks if health is above max
    	if(currentHealth > maxHealth + healthMod) {
	    	currentHealth = maxHealth + healthMod;
	    }

	    // checks if max health is under 0
	    if(maxHealth + healthMod < 1) {
	    	healthMod = 0;
	    	maxHealth = 1;
	    	currentHealth = 1;
	    }

	    //checks if active items are higher than max
	    if(activeItems.Count > maxActives) {
	    	activeItems.RemoveAt(activeItems.Count - 1);
	    }

	    //execute when not paused
    	if(!isPaused) {
    		shotCounter += Time.deltaTime;
    		damageCounter -= Time.deltaTime; 
    		interactionCounter -= Time.deltaTime;
    		activeCounter -= Time.deltaTime;
    		poisonCounter -= Time.deltaTime;
    		burnCounter -= Time.deltaTime;
    		burnTime -= Time.deltaTime;
    		poisonTime -= Time.deltaTime;
    		switchCounter -= Time.deltaTime;
    		pauseCounter -= Time.deltaTime;

    		if(poisoned && poisonCounter <= 0 && poisonTime > 0) {
    			if(currentHealth > 1) {
    				currentHealth--;
    				poisonCounter =0.5f;
    			}
    		} else if(poisonTime <= 0) {
    			poisoned = false;
    		}

    		if(burned && burnCounter <= 0 && burnTime > 0) {
	    		currentHealth -= 5;
	    		burnCounter = 2;
    		} else if(burnTime <= 0) {
    			burned = false;
    		}

    		if(activeCharges > 100) {
    			activeCharges = 100;
    		}
    	}
    }

    //removes charges from active item
    public IEnumerator EmptyCharges(int amount) {
    	yield return null;

    	activeCharges -= amount;
    }

    //gets input from player to move, only executes if not paused
    public void Movement(InputAction.CallbackContext context) {
    	if(!isPaused) {
    		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    		
	    	Vector2 vel = context.ReadValue<Vector2>();

	    	GetComponent<Animator>().SetFloat("x", context.ReadValue<Vector2>().x);
	    	GetComponent<Animator>().SetFloat("y", context.ReadValue<Vector2>().y);

		    vel.Normalize();

		    GetComponent<Rigidbody2D>().velocity = vel * speed * speedMod;
		} else {
    		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    	}
    }

    public void PauseUnPause(InputAction.CallbackContext context) {
    	if(pauseCounter <= 0) {
	    	if(!isPaused) {
	    		UIController.instance.OpenPauseMenu();
	    	} else {
	    		UIController.instance.ClosePauseMenu();
	    	}

	    	isPaused = !isPaused;

	    	pauseCounter = 1;
	    }
    }

    //handles input for the cursor, only executes if not paused
    public void AimingMouse(InputAction.CallbackContext context) {
    	if(!isPaused) {
    		Vector2 mousePos = context.ReadValue<Vector2>();

    		Vector2 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);
			Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);


	        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
	        wand.rotation = Quaternion.Euler(0, 0, angle);
	    }
    }

    //handles input for firing, only ececuted if not paused
    public void Shoot(InputAction.CallbackContext context) {
    	if(!isPaused) {
	    	if(shotCounter > shotSpeed * shotSpeedMod) {
	    		if(items.Contains(Item.ItemType.crystalCluster)) {
	    			if(tripleShot) {
		    			if(!hasPoison && !hasFire) {
		    				Instantiate(tripleCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire && hasPoison) {
		    				Instantiate(tripleFire_PoisonCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasPoison) {
		    				Instantiate(triplePoisonCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire) {
		    				Instantiate(tripleFireCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			}
		    		} else if(doubleShot) {
		    			if(!hasPoison && !hasFire) {
		    				Instantiate(doubleCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire && hasPoison) {
		    				Instantiate(doubleFire_PoisonCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasPoison) {
		    				Instantiate(doublePoisonCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire) {
		    				Instantiate(doubleFireCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			}
		    		} else {
		    			if(!hasPoison && !hasFire) {
		    				Instantiate(crystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire && hasPoison) {
		    				Instantiate(fire_PoisonCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasPoison) {
		    				Instantiate(poisonCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire) {
		    				Instantiate(fireCrystalProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			}
		    		}
	    		} else {
	    			if(tripleShot) {
		    			if(!hasPoison && !hasFire) {
		    				Instantiate(tripleProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire && hasPoison) {
		    				Instantiate(tripleFire_PoisonProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasPoison) {
		    				Instantiate(triplePoisonProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire) {
		    				Instantiate(tripleFireProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			}
		    		} else if(doubleShot) {
		    			if(!hasPoison && !hasFire) {
		    				Instantiate(doubleProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire && hasPoison) {
		    				Instantiate(doubleFire_PoisonProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasPoison) {
		    				Instantiate(doublePoisonProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire) {
		    				Instantiate(doubleFireProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			}
		    		} else {
		    			if(!hasPoison && !hasFire) {
		    				Instantiate(projectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire && hasPoison) {
		    				Instantiate(fire_PoisonProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasPoison) {
		    				Instantiate(poisonProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			} else if(hasFire) {
		    				Instantiate(fireProjectile, wand.GetChild(0).transform.position, wand.rotation);
		    			}
		    		}
		    	}
		        
		        shotCounter = 0;
		    }
		}
    }

    //returns player to satrting room
    public void ReturnToStart(InputAction.CallbackContext context) {
    	if(!isPaused) {
	    	transform.position = Vector3.zero;
	    }
    }

    //randomizes all items in the current floor
    public void Transmute() {
    	Item[] itemsToTransmute = FindObjectsOfType<Item>();

		for(int i = 0; i < itemsToTransmute.Length; i++) {
			if(itemsToTransmute[i].isPickup && itemsToTransmute[i].canTransmute) {
				if(itemsToTransmute[i].isPurchaseable) {
						Instantiate(itemsToTransmute[i].itemSpawnerShop, itemsToTransmute[i].gameObject.transform.position, Quaternion.identity);
						itemsToTransmute[i].GetComponent<Item>().DestroyAfter(0.1f);
				} else if(itemsToTransmute[i].canTransmute) {
						Instantiate(itemsToTransmute[i].itemSpawner, itemsToTransmute[i].gameObject.transform.position, Quaternion.identity);
						itemsToTransmute[i].GetComponent<Item>().DestroyAfter(0.1f);
				}
			} else if(itemsToTransmute[i].canTransmute) {
				Instantiate(itemsToTransmute[i].itemSpawner, itemsToTransmute[i].gameObject.transform.position, Quaternion.identity);
				itemsToTransmute[i].GetComponent<Item>().DestroyAfter(0.1f); 
			}
		}
    }

    //kills all enemies in the room
    public void KillAllEnemies() {
    	Enemy[] enemiestoDamage = FindObjectsOfType<Enemy>();

		for(int i = 0; i < enemiestoDamage.Length; i++) {
			if(enemiestoDamage[i].gameObject.activeInHierarchy) {
				enemiestoDamage[i].TakeDamage(10000000);
			}
		}
    }

    public void PlaceBomb(InputAction.CallbackContext context) {
    	if(bombs >= 1) {
    		Instantiate(bomb, transform.position, Quaternion.identity);
    		bombs--;
    	}
    }

    //activates selected active item
    public void ActiveItemTrigger(InputAction.CallbackContext context) {
    	if(!isPaused) {
    		if(activeCounter <= 0) {
    			activeCounter = 0.5f;
		    	if(activeItems.Count > 0) {
		        	switch(activeItems[currentActive]) {
		        		case Item.ItemType.philosofersStone:
		        			if(activeCharges >= 45) {
		        				StartCoroutine(EmptyCharges(45));
				        		Transmute();
		        			}
		        			break;
		        		case Item.ItemType.singularity:
		        			if(activeCharges >= 20) {
		        				StartCoroutine(EmptyCharges(20));
				        		KillAllEnemies();
		        			}
		        			break;
		        		case Item.ItemType.elixir:
		        			if(activeCharges >= 35) {
		        					StartCoroutine(EmptyCharges(35));
				        			StartCoroutine(DoubleShot(10));
		        				}
		        			break;
		        		case Item.ItemType.duplica:
		        			if(activeCharges >= 100) {
		        				StartCoroutine(EmptyCharges(100));	

		        				Instantiate(cursedItemSpawner, transform.position, Quaternion.identity);
		        			}
		        			break;
		        		case Item.ItemType.satchet:
		        			if(activeCharges >= 10) {
		        				StartCoroutine(EmptyCharges(10));	

		        				if(satchetItems.Count > 0) {
			        				switch(satchetItems[currentSatchetItem]) {
			        					case SatchetItems.containedSingularity:
			        						KillAllEnemies();
			        						break;
			        					case SatchetItems.soulShard:
			        						Transmute();
			        						break;
			        					case SatchetItems.magicDoubler:
			        						StartCoroutine(DoubleShot(10));
			        						break;
			        					case SatchetItems.cursedDroplet:
			        						Instantiate(cursedItemSpawner, transform.position, Quaternion.identity);
			        						break;
			        					case SatchetItems.juiceOfLife:
			        						currentHealth = maxHealth + healthMod;
			        						break;
			        				}

			        				satchetItems.RemoveAt(currentSatchetItem);
			        				currentSatchetItem = 0;
			        			}
		        			}
		        			break;
		        		case Item.ItemType.candyJar:
		        			if(activeCharges >= 25) {
		        				StartCoroutine(EmptyCharges(25));	

		        				currentHealth = maxHealth + healthMod;		        			
		        			}
		        			break;
		        		case Item.ItemType.deck:
		        			if(activeCharges >= 10) {
		        				StartCoroutine(EmptyCharges(10));	

		        				if(deckItems.Count > 0) {
			        				switch(deckItems[currentDeckItem]) {
			        					case SatchetItems.sA:
			        						bombs *= 2;
			        						break;
			        					case SatchetItems.hA:
			        						healthMod += 50;
			        						break;
			        					case SatchetItems.dA:
			        						currency *= 2;
			        						break;
			        					case SatchetItems.cA:
			        						keys *= 2;
			        						break;
			        				}

			        				deckItems.RemoveAt(currentDeckItem);
			        				currentDeckItem = 0;
			        			}
		        			}
		        			break;
		        		case Item.ItemType.diceSet:
		        			if(activeCharges >= 10) {
		        				StartCoroutine(EmptyCharges(10));	

		        				int rand = 0;

		        				if(dieItems.Count > 0) {
			        				switch(dieItems[currentDieItem]) {
			        					case SatchetItems.d4:
			        						rand = Random.Range(0,4);

			        						if(rand == 0) {
			        							healthMod += 10;
			        						} else if(rand == 1) {
			        							keys++;
			        						} else if(rand == 2) {
			        							currency += 10;
			        						} else if(rand == 3) {
			        							bombs++;
			        						}
			        						break;
			        					case SatchetItems.d6:
			        						rand = Random.Range(0,6);

			        						if(rand == 0) {
			        							healthMod += 10;
			        						} else if(rand == 1) {
			        							keys++;
			        						} else if(rand == 2) {
			        							currency += 10;
			        						} else if(rand == 3) {
			        							bombs++;
			        						} else if(rand == 4) {
			        							currency += 50;
			        						} else if(rand == 5) {
			        							healthMod += 50;
			        						}
			        						break;
			        					case SatchetItems.d8:
			        						rand = Random.Range(0,8);

			        						if(rand == 0) {
			        							healthMod += 10;
			        						} else if(rand == 1) {
			        							keys++;
			        						} else if(rand == 2) {
			        							currency += 10;
			        						} else if(rand == 3) {
			        							bombs++;
			        						} else if(rand == 4) {
			        							currency += 50;
			        						} else if(rand == 5) {
			        							healthMod += 50;
			        						} else if(rand == 6) {
			        							currentHealth += 50;
			        						} else if(rand == 7) {
			        							bombs += 5;
			        							keys += 5;
			        						}
			        						break;
			        					case SatchetItems.d20:
			        						rand = Random.Range(0,20);

			        						if(rand == 0) {
			        							healthMod += 10;
			        						} else if(rand == 1) {
			        							keys++;
			        						} else if(rand == 2) {
			        							currency += 10;
			        						} else if(rand == 3) {
			        							bombs++;
			        						} else if(rand == 4) {
			        							currency += 50;
			        						} else if(rand == 5) {
			        							healthMod += 50;
			        						} else if(rand == 6) {
			        							currentHealth += 50;
			        						} else if(rand == 7) {
			        							bombs += 5;
			        							keys += 5;
			        						} else if(rand == 8) {
			        							Instantiate(itemSpawner, transform.position, Quaternion.identity);
			        						} else if(rand == 9) {
			        							Instantiate(shopItemSpawner, transform.position, Quaternion.identity);
			        						} else if(rand == 10) {
			        							currentHealth -= 10;
			        						} else if(rand == 11) {
			        							currentHealth -= 50;
			        						} else if(rand == 12) {
			        							currency = 9999;
			        						} else if(rand == 13) {
			        							currency = 0;
			        						} else if(rand == 14) {
			        							bombs = 0;
			        							keys = 0;
			        						} else if(rand == 15) {
			        							healthMod -= 20;
			        						} else if(rand == 16) {
			        							curses.Add((Curses)Random.Range(0,5));
			        						} else if(rand == 17) {
			        							int temp = bombs;
			        							bombs = keys;
			        							keys = temp;
			        						} else if(rand == 18) {
			        							lives++;
			        						} else if(rand == 19) {
			        							damageMod += 0.5f;
			        						}
			        						break;
			        				}

			        				dieItems.RemoveAt(currentDieItem);
			        				currentDieItem = 0;
			        			}
		        			}
		        			break;
		        	}
	        	}
	        }   
	    }
	}

	//triggers double shot for given time
	public IEnumerator DoubleShot(float time) {
		if(!doubleShot) {
			doubleShot = true;

			yield return new WaitForSeconds(time);

			doubleShot = false;
		} else {
			tripleShot = true;

			yield return new WaitForSeconds(time);

			tripleShot = false;
		}
	}

	//switches current active item
	public void SwitchActive(InputAction.CallbackContext context) {
		if(switchCounter <= 0) {
			if(currentActive < activeItems.Count-1) {
				currentActive++;
			} else if(currentActive == activeItems.Count-1) {
				currentActive = 0;
			}

			switchCounter = 0.2f;
		}
	}

	//switches selected satchet item
	public void SwitchSatchet(InputAction.CallbackContext context) {
		if(switchCounter <= 0 && activeItems.Count > 0) {
			if(currentActive == activeItems.IndexOf(Item.ItemType.satchet)) {
				if(currentSatchetItem < satchetItems.Count-1) {
					currentSatchetItem++;
				} else if(currentSatchetItem == satchetItems.Count-1) {
					currentSatchetItem = 0;
				}
			}

			if(currentActive == activeItems.IndexOf(Item.ItemType.deck)) {
				if(currentDeckItem < deckItems.Count-1) {
					currentDeckItem++;
				} else if(currentDeckItem == deckItems.Count-1) {
					currentDeckItem = 0;
				}
			}

			if(currentActive == activeItems.IndexOf(Item.ItemType.diceSet)) {
				if(currentDieItem < dieItems.Count-1) {
					currentDieItem++;
				} else if(currentDieItem == dieItems.Count-1) {
					currentDieItem = 0;
				}
			}

			switchCounter = 0.2f;
		}
	}

	//handles interaction input
    public void Interact(InputAction.CallbackContext context) {

    	if(interactionCounter <= 0) {
	    	Interactable[] interactables = FindObjectsOfType<Interactable>();

	    	for(int i = 0; i < interactables.Length; i++) {

	    		//interactions with locked doors
	    		if(interactables[i].gameObject.GetComponent<LockedDoor>() != null) {
	    			if(PlayerController.instance.keys >= 1 && Vector3.Distance(transform.position, interactables[i].gameObject.transform.position) < 1.5f) {
        				keys --;

        				AudioManager.instance.PlaySFX(3);

	        			Destroy(interactables[i].gameObject);
        			}
	    		}

	    		//interactions with dungeon gates
	    		if(interactables[i].gameObject.GetComponent<DungeonGate>() != null && Vector3.Distance(transform.position, interactables[i].gameObject.transform.position) < 1.5f) {
	    			if(!SettingsController.instance.isDemo) {
	    				interactables[i].gameObject.GetComponent<DungeonGate>().LoadScene();
	    			} else {
	    				currentHealth = 0;
	    			}
	    		}

	    		//interaction with items
	    		if(interactables[i].gameObject.GetComponent<Item>() != null) {
	    			if(Vector3.Distance(transform.position, interactables[i].gameObject.transform.position) < 1.5f) {
	    				if(!interactables[i].gameObject.GetComponent<Item>().isPurchaseable) {
			        		if(!interactables[i].gameObject.GetComponent<Item>().isPickup && !interactables[i].gameObject.GetComponent<Item>().isActive) {
			        			if(!interactables[i].gameObject.GetComponent<Item>().isTrinket) {
			        				PlayerController.instance.items.Add(interactables[i].gameObject.GetComponent<Item>().type);
			        			}
			        		} else if(interactables[i].gameObject.GetComponent<Item>().isActive) {
			        			if(activeItems.Count >= maxActives) {
			        				//spawns the removed active when picking up a new one
			        				switch(activeItems[currentActive]) {
			        					case Item.ItemType.philosofersStone: 
			        						Instantiate(actives[0], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.elixir: 
			        						Instantiate(actives[1], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.singularity: 
			        						Instantiate(actives[2], transform.position, Quaternion.identity);
			        						break;   
			        					case Item.ItemType.duplica: 
			        						Instantiate(actives[3], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.satchet: 
			        						Instantiate(actives[4], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.candyJar: 
			        						Instantiate(actives[10], transform.position, Quaternion.identity);
			        						break; 
			        					case Item.ItemType.deck: 
			        						Instantiate(actives[11], transform.position, Quaternion.identity);
			        						break;  
			        					case Item.ItemType.diceSet: 
			        						Instantiate(actives[12], transform.position, Quaternion.identity);
			        						break;       				
			        				} 

			        				activeItems[currentActive] = interactables[i].gameObject.GetComponent<Item>().type;
			        			} else {
			        				activeItems.Add(interactables[i].gameObject.GetComponent<Item>().type);
			        			}

			        			//sets charges to the corresponding amount
			        			switch(interactables[i].gameObject.GetComponent<Item>().type) {
			        				case Item.ItemType.philosofersStone: 
			        					activeCharges = 45;
			        					break;
			        				case Item.ItemType.singularity: 
			        					activeCharges = 20;
			        					break;
			        				case Item.ItemType.elixir: 
			        					activeCharges = 35;
			        					break;
			        				case Item.ItemType.duplica: 
			        					curses.Add(Curses.duplicate);
			        					activeCharges = 100;
			        					break;
			        				case Item.ItemType.satchet:
			        					activeCharges = 10;
			        					break;
			        				case Item.ItemType.candyJar:
			        					activeCharges = 25;
			        					break;
			        				case Item.ItemType.deck:
			        					activeCharges = 10;
			        					break;
			        				case Item.ItemType.diceSet:
			        					activeCharges = 15;
			        					break;
			        			}
			        		}

			        		AudioManager.instance.PlaySFX(3);

			        		//displays the item in the screen
			        		if(interactables[i].gameObject.GetComponent<Item>().display) {
			        			GameObject newDisplay = Instantiate(interactables[i].gameObject.GetComponent<Item>().itemDisplayGO);
			        			newDisplay.transform.SetParent(UIController.instance.itemDisplays.transform);
			        			newDisplay.transform.GetChild(0).GetComponent<Image>().sprite = interactables[i].gameObject.GetComponent<SpriteRenderer>().sprite;
			        		}

			        		//selects item effect
				        	switch(interactables[i].gameObject.GetComponent<Item>().type) {
				        		case Item.ItemType.ironSoul:
				        			healthMod += 10;
				        			damageMod += 0.2f;
				        			break;
								case Item.ItemType.enhancedApple:
				        			healthMod += 25;
				        			currentHealth = maxHealth + healthMod;
				        			break;
				        		case Item.ItemType.key:
				        			keys ++;
				        			break;
				        		case Item.ItemType.hpPotion:
				        			currentHealth += 20;
				        			break;
				        		case Item.ItemType.midasHand:
				        			moneyMultiplier += 0.5f;
				        			break;
				        		case Item.ItemType.eternalFire:
				        			hasFire = true;
				        			break;
				        		case Item.ItemType.uraniumPellets:
				        			hasPoison = true;
				        			break;
				        		case Item.ItemType.spiritArrow:
				        			Instantiate(familiars[0], transform.position, Quaternion.identity);
				        			break;
				        		case Item.ItemType.crystalCluster:
				        			bulletSpeedMod -= 0.5f;
				        			healthMod -= 75;
				        			currentHealth = maxHealth + healthMod;
				        			curses.Add(Curses.crystal);
				        			break;
				        		case Item.ItemType.eternity:
					        		lives += 5;
					        		curses.Add(Curses.eternal);
				        			break;
				        		case Item.ItemType.ouroboros:
					        		damageMod += 1;
					        		curses.Add(Curses.parasite);
				        			break;
				        		case Item.ItemType.theDrop:
					        		speedMod += 0.3f;
					        		shotSpeedMod -= 0.2f;
					        		curses.Add(Curses.infected);
				        			break;
				        		case Item.ItemType.goldenIdol:
					        		lives ++;
				        			break;
				        		case Item.ItemType.powerSuit:
					        		healthMod += 50;
					        		damageMod += 0.25f;
				        			break;
				        		case Item.ItemType.cursedDroplet:
					        		satchetItems.Add(SatchetItems.cursedDroplet);
				        			break;
				        		case Item.ItemType.soulShard:
					        		satchetItems.Add(SatchetItems.soulShard);
				        			break;
				        		case Item.ItemType.juiceOfLife:
					        		satchetItems.Add(SatchetItems.juiceOfLife);
				        			break;
				        		case Item.ItemType.magicDoubler:
					        		satchetItems.Add(SatchetItems.magicDoubler);
				        			break;
				        		case Item.ItemType.containedSingularity:
					        		satchetItems.Add(SatchetItems.containedSingularity);
				        			break;
				        		case Item.ItemType.hiddenCompartment:
					        		maxActives++;
				        			break;
				        		case Item.ItemType.sA:
					        		deckItems.Add(SatchetItems.sA);
				        			break;
				        		case Item.ItemType.dA:
					        		deckItems.Add(SatchetItems.dA);
				        			break;
				        		case Item.ItemType.hA:
					        		deckItems.Add(SatchetItems.hA);
				        			break;
				        		case Item.ItemType.cA:
					        		deckItems.Add(SatchetItems.cA);
				        			break;
				        		case Item.ItemType.d4:
					        		dieItems.Add(SatchetItems.d4);
				        			break;
				        		case Item.ItemType.d6:
					        		dieItems.Add(SatchetItems.d6);
				        			break;
				        		case Item.ItemType.d8:
					        		dieItems.Add(SatchetItems.d8);
				        			break;
				        		case Item.ItemType.d20:
					        		dieItems.Add(SatchetItems.d20);
				        			break;
				        		case Item.ItemType.puffBallFlute:
					        		int rando = Random.Range(1,4);
					        		Instantiate(familiars[rando], transform.position, Quaternion.identity);
				        			break;
				        		case Item.ItemType.bomb:
					        		bombs++;
				        			break;
				        		case Item.ItemType.goldenBrew:
					        		doubleShot = true;
				        			break;
				        		case Item.ItemType.goldenBomb:
					        		bombs += 5;
					        		bombDamageMod += 0.5f;
				        			break;
				        		case Item.ItemType.silverBrew:
					        		luck++;
				        			break;
				        		case Item.ItemType.rainbowBrew:
					        		Instantiate(familiars[4], transform.position, Quaternion.identity);
				        			break;
				        	}

				        	Destroy(interactables[i].gameObject);
			        	} else if(interactables[i].gameObject.GetComponent<Item>().isPurchaseable && currency >= interactables[i].gameObject.GetComponent<Item>().price) {
			        		if(!interactables[i].gameObject.GetComponent<Item>().isPickup && !interactables[i].gameObject.GetComponent<Item>().isActive) {
			        			if(!interactables[i].gameObject.GetComponent<Item>().isTrinket) {
			        				PlayerController.instance.items.Add(interactables[i].gameObject.GetComponent<Item>().type);
			        			}
			        		} else if(interactables[i].gameObject.GetComponent<Item>().isActive) {
			        			if(activeItems.Count >= maxActives) {
			        				switch(activeItems[currentActive]) {
			        					case Item.ItemType.philosofersStone: 
			        						Instantiate(actives[0], transform.position, Quaternion.identity);
			        						break;     
			        					case Item.ItemType.elixir: 
			        						Instantiate(actives[1], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.singularity: 
			        						Instantiate(actives[2], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.duplica: 
			        						Instantiate(actives[3], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.satchet: 
			        						Instantiate(actives[4], transform.position, Quaternion.identity);
			        						break;
			        					case Item.ItemType.candyJar: 
			        						Instantiate(actives[10], transform.position, Quaternion.identity);
			        						break; 
			        					case Item.ItemType.deck: 
			        						Instantiate(actives[11], transform.position, Quaternion.identity);
			        						break;  
			        					case Item.ItemType.diceSet: 
			        						Instantiate(actives[12], transform.position, Quaternion.identity);
			        						break;   
			          				} 

			          				activeItems[currentActive] = interactables[i].gameObject.GetComponent<Item>().type;
			        			} else {
			        				activeItems.Add(interactables[i].gameObject.GetComponent<Item>().type);
			        			}

			        			switch(interactables[i].gameObject.GetComponent<Item>().type) {
			        				case Item.ItemType.philosofersStone: 
			        					activeCharges = 45;
			        					break;
			        				case Item.ItemType.singularity: 
			        					activeCharges = 20;
			        					break;
			        				case Item.ItemType.elixir: 
			        					activeCharges = 35;
			        					break;
			        				case Item.ItemType.duplica: 
			        					activeCharges = 100;
			        					break;
			        				case Item.ItemType.satchet:
			        					activeCharges = 10;
			        					break;
			        				case Item.ItemType.candyJar:
			        					activeCharges = 25;
			        					break;
			        				case Item.ItemType.deck:
			        					activeCharges = 10;
			        					break;
			        				case Item.ItemType.diceSet:
			        					activeCharges = 15;
			        					break;
			        			}
			        		}

			        		currency -= interactables[i].gameObject.GetComponent<Item>().price;

			        		AudioManager.instance.PlaySFX(3);

			        		if(interactables[i].gameObject.GetComponent<Item>().display) {
			        			GameObject newDisplay = Instantiate(interactables[i].gameObject.GetComponent<Item>().itemDisplayGO);
			        			newDisplay.transform.SetParent(UIController.instance.itemDisplays.transform);
			        			newDisplay.transform.GetChild(0).GetComponent<Image>().sprite = interactables[i].gameObject.GetComponent<SpriteRenderer>().sprite;
			        		}

				        	switch(interactables[i].gameObject.GetComponent<Item>().type) {
				        		case Item.ItemType.ironSoul:
				        			healthMod += 10;
				        			damageMod += 0.2f;
				        			break;
								case Item.ItemType.enhancedApple:
				        			healthMod += 25;
				        			currentHealth = PlayerController.instance.maxHealth + PlayerController.instance.healthMod;
				        			break;
				        		case Item.ItemType.key:
				        			keys ++;
				        			break;
				        		case Item.ItemType.hpPotion:
				        			currentHealth += 20;
				        			break;
				        		case Item.ItemType.midasHand:
				        			moneyMultiplier += 0.5f;
				        			break;
				        		case Item.ItemType.eternalFire:
				        			hasFire = true;
				        			break;
				        		case Item.ItemType.uraniumPellets:
				        			hasPoison = true;
				        			break;
				        		case Item.ItemType.spiritArrow:
				        			Instantiate(familiars[0], transform.position, Quaternion.identity);
				        			break;
				        		case Item.ItemType.crystalCluster:
				        			bulletSpeedMod -= 0.5f;
				        			healthMod -= 75;
				        			currentHealth = maxHealth + healthMod;
				        			curses.Add(Curses.crystal);
				        			break;
				        		case Item.ItemType.eternity:
					        		lives += 5;
					        		curses.Add(Curses.eternal);
				        			break;
				        		case Item.ItemType.ouroboros:
					        		damageMod += 1;
					        		curses.Add(Curses.parasite);
				        			break;
				        		case Item.ItemType.theDrop:
					        		speedMod += 0.3f;
					        		shotSpeedMod += 0.2f;
					        		curses.Add(Curses.infected);
				        			break;
				        		case Item.ItemType.goldenIdol:
					        		lives ++;
				        			break;
				        		case Item.ItemType.powerSuit:
					        		healthMod += 50;
					        		damageMod += 0.25f;
				        			break;
				        		case Item.ItemType.cursedDroplet:
					        		satchetItems.Add(SatchetItems.cursedDroplet);
				        			break;
				        		case Item.ItemType.soulShard:
					        		satchetItems.Add(SatchetItems.soulShard);
				        			break;
				        		case Item.ItemType.juiceOfLife:
					        		satchetItems.Add(SatchetItems.juiceOfLife);
				        			break;
				        		case Item.ItemType.magicDoubler:
					        		satchetItems.Add(SatchetItems.magicDoubler);
				        			break;
				        		case Item.ItemType.containedSingularity:
					        		satchetItems.Add(SatchetItems.containedSingularity);
				        			break;
				        		case Item.ItemType.hiddenCompartment:
					        		maxActives++;
				        			break;
				        		case Item.ItemType.sA:
					        		deckItems.Add(SatchetItems.sA);
				        			break;
				        		case Item.ItemType.dA:
					        		deckItems.Add(SatchetItems.dA);
				        			break;
				        		case Item.ItemType.hA:
					        		deckItems.Add(SatchetItems.hA);
				        			break;
				        		case Item.ItemType.cA:
					        		deckItems.Add(SatchetItems.cA);
				        			break;
				        		case Item.ItemType.d4:
					        		dieItems.Add(SatchetItems.d4);
				        			break;
				        		case Item.ItemType.d6:
					        		dieItems.Add(SatchetItems.d6);
				        			break;
				        		case Item.ItemType.d8:
					        		dieItems.Add(SatchetItems.d8);
				        			break;
				        		case Item.ItemType.d20:
					        		dieItems.Add(SatchetItems.d20);
				        			break;
				        		case Item.ItemType.puffBallFlute:
					        		int rando = Random.Range(1,4);
					        		Instantiate(familiars[rando], transform.position, Quaternion.identity);
				        			break;
				        		case Item.ItemType.bomb:
					        		bombs++;
				        			break;
				        		case Item.ItemType.goldenBrew:
					        		doubleShot = true;
				        			break;
				        		case Item.ItemType.goldenBomb:
					        		bombs += 5;
					        		bombDamageMod += 0.5f;
				        			break;
				        		case Item.ItemType.silverBrew:
					        		luck++;
				        			break;
				        		case Item.ItemType.rainbowBrew:
					        		Instantiate(familiars[4], transform.position, Quaternion.identity);
				        			break;
				        	}

				        	Destroy(interactables[i].gameObject);
			        	}
	    			}
	    		}
	    	} 
	    }

	    interactionCounter = 0.5f;
    }
}
