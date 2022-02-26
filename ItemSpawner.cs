using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public List<GameObject> s, a, b, c;
	private GameObject item;
	private int iterations = 0;
	public bool isShopItem;
    // Start is called before the first frame update
    void Start()
    {
    	int rand = Random.Range(0,1000);

    	if(rand < 5 + PlayerController.instance.luck * 10) {
    		rand = Random.Range(0, s.Count);
    		item = s[rand];
    	} else if(rand < 55 + PlayerController.instance.luck * 10) {
    		rand = Random.Range(0, a.Count);
    		item = a[rand];
    	} else if(rand < 455 + PlayerController.instance.luck * 10) {
    		rand = Random.Range(0, b.Count);
    		item = b[rand];
    	} else {
    		rand = Random.Range(0, c.Count);
    		item = c[rand];
    	}

        while((PlayerController.instance.items.Contains(item.GetComponent<Item>().type) || PlayerController.instance.activeItems.Contains(item.GetComponent<Item>().type)) && iterations < 500) {
        	rand = Random.Range(0,1000);

	    	if(rand < 5 + PlayerController.instance.luck * 10) {
	    		rand = Random.Range(0, s.Count);
	    		item = s[rand];
	    	} else if(rand < 55 + PlayerController.instance.luck * 10) {
	    		rand = Random.Range(0, a.Count);
	    		item = a[rand];
	    	} else if(rand < 455 + PlayerController.instance.luck * 10) {
	    		rand = Random.Range(0, b.Count);
	    		item = b[rand];
	    	} else {
	    		rand = Random.Range(0, c.Count);
	    		item = c[rand];
	    	}

	    	iterations++;
        }

        if(PlayerController.instance.items.Contains(item.GetComponent<Item>().type) || PlayerController.instance.activeItems.Contains(item.GetComponent<Item>().type)) {
        	Debug.Log("No items found in pool"); 
        } else {
        	if(isShopItem) {
        		GameObject newItem = Instantiate(item, transform.position, Quaternion.identity);
        		newItem.GetComponent<Item>().isPurchaseable = true;
        		newItem.transform.SetParent(transform);
        		newItem.GetComponent<Item>().itemToDestroy = gameObject;
        	} else {
        		GameObject newItem = Instantiate(item, transform.position, Quaternion.identity);
        		newItem.transform.SetParent(transform);
        		newItem.GetComponent<Item>().itemToDestroy = gameObject;
        	}
        }
    }
}
