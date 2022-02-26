using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
	public enum ItemType {ironSoul, enhancedApple, key, hpPotion, midasHand, philosofersStone, singularity, elixir, eternalFire, uraniumPellets, spiritArrow, duplica,
	crystalCluster, eternity, theDrop, ouroboros, powerSuit, goldenIdol, satchet, magicDoubler, containedSingularity, soulShard, juiceOfLife, cursedDroplet, hiddenCompartment,
	candyJar, deck, diceSet, d4, d6, d8, d20, sA, hA, cA, dA, puffBallFlute, bomb, goldenBrew, silverBrew, rainbowBrew, goldenBomb};
	public enum Class {s, a, b, c};
	public ItemType type;
	public Class itemClass;
	public int price;
	public GameObject canvas, storePanel, itemDisplayGO, itemSpawner, itemSpawnerShop, itemToDestroy;
	public Text name, description, priceText;
	public string nameText, descriptionText;
	public int id;

	public bool isPurchaseable, display, isPickup, isActive, canTransmute = true, isTrinket;

    // Start is called before the first frame update
    void Start()
    {
        name.text = nameText;
        description.text = descriptionText;

        switch(itemClass) {
        	case Class.s:
        		price = Random.Range(8, 12) * 10;
        		break;		
        	case Class.a:
        		price = Random.Range(4, 8) * 10;
        		break;
        	case Class.b:
        		price = Random.Range(2, 4) * 10;
        		break;
        	case Class.c:
        		price = Random.Range(15, 21);
        		break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    	if(PlayerController.instance.items.Contains(type) || !SettingsController.instance.unlocked[id] || 
    	PlayerController.instance.activeItems.Contains(type)) {
        	Instantiate(itemSpawner, transform.position, Quaternion.identity);
        	Destroy(itemToDestroy);
        }

        if(Vector3.Distance(PlayerController.instance.transform.position, transform.position) < 1.5f) {
        	canvas.SetActive(true);

        	if(isPurchaseable) {
        		storePanel.SetActive(true);

        		priceText.text = price.ToString();
        	}	
        } else {
        	canvas.SetActive(false);
        }
    }

    public void DestroyAfter(float time) {
    	StartCoroutine(DestroySecs(time));
    }

    public IEnumerator DestroySecs(float time) {
    	yield return new WaitForSeconds(time);

    	Destroy(itemToDestroy);

    	Debug.Log("Working");
    }
}


