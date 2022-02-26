using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
	public static UIController instance;

	public Slider playerHealth;

	public Image activeItem, satchetActive;

	public Text playerHealthText, currencyText, keysText, bombsText;

	public bool isKeyboard = true;

	public Slider itemCharges;

	public GameObject loadScreen, itemDisplays, burnIcon, poisonIcon, changeItem, changeSatchetItem, pauseMenu;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null) {
    		instance = this;
    	} else {
    		Destroy(gameObject);
    	}

    	DontDestroyOnLoad(gameObject);
    }

    public void ExitToTitle() {
    	SceneManager.LoadScene("TitleScreen");
		Destroy(CameraController.instance.gameObject);
		Destroy(DebugController.instance.gameObject);
		Destroy(DontDestroy.instance.gameObject);
		Destroy(PlayerController.instance.transform.parent.gameObject);
		Destroy(gameObject);
    }

    public void OpenPauseMenu() {
    	pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu() {
    	pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    	if(PlayerController.instance.burned) {
    		burnIcon.SetActive(true);
    	} else {
    		burnIcon.SetActive(false);
    	}

    	if(PlayerController.instance.poisoned) {
    		poisonIcon.SetActive(true);
    	} else {
    		poisonIcon.SetActive(false);
    	}

    	if(PlayerController.instance.activeItems.Count > 0) {
	    	switch(PlayerController.instance.activeItems[PlayerController.instance.currentActive]) {
	        	case Item.ItemType.philosofersStone: 
	        		activeItem.sprite = PlayerController.instance.actives[0].GetComponent<SpriteRenderer>().sprite;
	        		break;
	        	case Item.ItemType.elixir: 
	        		activeItem.sprite = PlayerController.instance.actives[1].GetComponent<SpriteRenderer>().sprite;
	        		break; 
	        	case Item.ItemType.singularity: 
	        		activeItem.sprite = PlayerController.instance.actives[2].GetComponent<SpriteRenderer>().sprite;
	        		break; 
	        	case Item.ItemType.duplica: 
	        		activeItem.sprite = PlayerController.instance.actives[3].GetComponent<SpriteRenderer>().sprite;
	        		break; 
	        	case Item.ItemType.satchet: 
	        		activeItem.sprite = PlayerController.instance.actives[4].GetComponent<SpriteRenderer>().sprite;
	        		break; 
	        	case Item.ItemType.candyJar: 
	        		activeItem.sprite = PlayerController.instance.actives[10].GetComponent<SpriteRenderer>().sprite;
	        		break; 
	        	case Item.ItemType.deck: 
	        		activeItem.sprite = PlayerController.instance.actives[11].GetComponent<SpriteRenderer>().sprite;
	        		break; 
	        	case Item.ItemType.diceSet: 
	        		activeItem.sprite = PlayerController.instance.actives[12].GetComponent<SpriteRenderer>().sprite;
	        		break; 
	        }

	        switch(PlayerController.instance.activeItems[PlayerController.instance.currentActive]) {
        		case Item.ItemType.philosofersStone: 
	        		itemCharges.maxValue = 45;
	        		break; 
	        	case Item.ItemType.elixir: 
	        		itemCharges.maxValue = 35;
	        		break; 
	        	case Item.ItemType.singularity: 
	        		itemCharges.maxValue = 20;
	        		break; 
	        	case Item.ItemType.duplica: 
	        		itemCharges.maxValue = 100;
	        		break; 
	        	case Item.ItemType.satchet: 
	        		itemCharges.maxValue = 10;
	        		break;
	        	case Item.ItemType.candyJar: 
	        		itemCharges.maxValue = 25;
	        		break;
	        	case Item.ItemType.deck: 
	        		itemCharges.maxValue = 10;
	        		break;
	        	case Item.ItemType.diceSet: 
	        		itemCharges.maxValue = 15;
	        		break; 
        	}

	        activeItem.color = new Color(1,1,1,1);
        } else {
        	activeItem.color = new Color(1,1,1,0);
        }

        if(PlayerController.instance.activeItems.Count > 1) {
        	changeItem.SetActive(true);
        } else {
        	changeItem.SetActive(false);
        }

        changeSatchetItem.SetActive(false);
        satchetActive.transform.parent.gameObject.SetActive(false);
        satchetActive.color = new Color(1,1,1,0);

        if(PlayerController.instance.activeItems.Contains(Item.ItemType.satchet) && PlayerController.instance.currentActive == PlayerController.instance.activeItems.IndexOf(Item.ItemType.satchet)) {
        	satchetActive.transform.parent.gameObject.SetActive(true);

        	if(PlayerController.instance.satchetItems.Count > 1) {
        		changeSatchetItem.SetActive(true);
        	}
        	
        	if(PlayerController.instance.satchetItems.Count > 0) {
        		satchetActive.color = new Color(1,1,1,1);

	        	switch(PlayerController.instance.satchetItems[PlayerController.instance.currentSatchetItem]) {
		        	case PlayerController.SatchetItems.containedSingularity: 
		        		satchetActive.sprite = PlayerController.instance.actives[5].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.juiceOfLife: 
		        		satchetActive.sprite = PlayerController.instance.actives[6].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.soulShard: 
		        		satchetActive.sprite = PlayerController.instance.actives[7].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.cursedDroplet: 
		        		satchetActive.sprite = PlayerController.instance.actives[8].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.magicDoubler: 
		        		satchetActive.sprite = PlayerController.instance.actives[9].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        }
		    } 
        } 

        if(PlayerController.instance.activeItems.Contains(Item.ItemType.deck) && PlayerController.instance.currentActive == PlayerController.instance.activeItems.IndexOf(Item.ItemType.deck)) {
        	satchetActive.transform.parent.gameObject.SetActive(true);

        	if(PlayerController.instance.deckItems.Count > 1) {
        		changeSatchetItem.SetActive(true);
        	}
        	
        	if(PlayerController.instance.deckItems.Count > 0) {
        		satchetActive.color = new Color(1,1,1,1);

	        	switch(PlayerController.instance.deckItems[PlayerController.instance.currentDeckItem]) {
		        	case PlayerController.SatchetItems.cA: 
		        		satchetActive.sprite = PlayerController.instance.actives[13].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.dA: 
		        		satchetActive.sprite = PlayerController.instance.actives[14].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.hA: 
		        		satchetActive.sprite = PlayerController.instance.actives[15].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.sA: 
		        		satchetActive.sprite = PlayerController.instance.actives[16].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        }
		    } 
        } 

        if(PlayerController.instance.activeItems.Contains(Item.ItemType.diceSet) && PlayerController.instance.currentActive == PlayerController.instance.activeItems.IndexOf(Item.ItemType.diceSet)) {
        	satchetActive.transform.parent.gameObject.SetActive(true);

        	if(PlayerController.instance.dieItems.Count > 1) {
        		changeSatchetItem.SetActive(true);
        	}
        	
        	if(PlayerController.instance.dieItems.Count > 0) {
        		satchetActive.color = new Color(1,1,1,1);

	        	switch(PlayerController.instance.dieItems[PlayerController.instance.currentDieItem]) {
		        	case PlayerController.SatchetItems.d4: 
		        		satchetActive.sprite = PlayerController.instance.actives[17].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.d6: 
		        		satchetActive.sprite = PlayerController.instance.actives[18].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.d8: 
		        		satchetActive.sprite = PlayerController.instance.actives[19].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        	case PlayerController.SatchetItems.d20: 
		        		satchetActive.sprite = PlayerController.instance.actives[20].GetComponent<SpriteRenderer>().sprite;
		        		break;
		        }
		    } 
        } 

        itemCharges.value = PlayerController.instance.activeCharges;

        playerHealth.maxValue = PlayerController.instance.maxHealth + PlayerController.instance.healthMod;
        playerHealth.value = PlayerController.instance.currentHealth;

        playerHealthText.text = PlayerController.instance.currentHealth.ToString() + " / " +  (PlayerController.instance.maxHealth + PlayerController.instance.healthMod).ToString();

        currencyText.text = PlayerController.instance.currency.ToString();

        keysText.text = PlayerController.instance.keys.ToString();

        bombsText.text = PlayerController.instance.bombs.ToString();
    }
}
