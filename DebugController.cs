using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public bool showConsole, showHelp = true;
    public string input;

    public GameObject[] items, enemies;

    public static DebugController instance;

    public static DebugCommand<string> SPAWN, SET_COINS, SET_HEALTH, SET_KEYS, LOAD_SCENE;

    public static DebugCommand KILL;

    public List<string> spawnables = new List<string>();

    public List<object> commandList = new List<object>();

    public void ToggleConsole(InputAction.CallbackContext context) {
    	if(SettingsController.instance.cheatsOn) {
    		if(showConsole) {
    			PlayerController.instance.isPaused = false;
    		}

    		showConsole = !showConsole;
    	}
    }

    public void OnReturn(InputAction.CallbackContext context) {
    	if(showConsole) {
    		HandleInput();
    		input = "";
    	}
    }

    void Awake() {
    	if(instance == null) {
    		instance = this;
    	} else {
    		Destroy(gameObject);
    	}

    	DontDestroyOnLoad(gameObject);

    	SPAWN = new DebugCommand<string>("spawn", "spawns an object", "spawn <object_to_spawn>", (x) => {
    		GameObject newItem;
    		switch(x) {
    			case "iron_soul":
    				newItem = Instantiate(items[0], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "enhanced_apple":
    				newItem = Instantiate(items[1], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "hp_potion":
    				newItem = Instantiate(items[2], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "key":
    				newItem = Instantiate(items[3], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "midas_hand":
    				newItem = Instantiate(items[4], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "philosofers_stone":
    				newItem = Instantiate(items[5], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "singularity":
    				newItem = Instantiate(items[6], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "elixir":
    				newItem = Instantiate(items[7], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "eternal_fire":
    				newItem = Instantiate(items[8], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "uranium_pellets":
    				newItem = Instantiate(items[9], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "spirit_arrow":
    				newItem = Instantiate(items[13], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "duplica":
    				newItem = Instantiate(items[14], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "crystal_cluster":
    				newItem = Instantiate(items[16], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "ouroboros":
    				newItem = Instantiate(items[17], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "eternity":
    				newItem = Instantiate(items[18], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "the_drop":
    				newItem = Instantiate(items[19], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "power_suit":
    				newItem = Instantiate(items[20], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "satchet":
    				newItem = Instantiate(items[21], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "golden_idol":
    				newItem = Instantiate(items[22], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "contained_singularity":
    				newItem = Instantiate(items[23], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "soul_shard":
    				newItem = Instantiate(items[24], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "juice_of_life":
    				newItem = Instantiate(items[25], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "cursed_droplet":
    				newItem = Instantiate(items[26], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "magic_doubler":
    				newItem = Instantiate(items[27], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "hidden_compartment":
    				newItem = Instantiate(items[29], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "candy_jar":
    				newItem = Instantiate(items[30], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "deck":
    				newItem = Instantiate(items[31], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "dice_case":
    				newItem = Instantiate(items[32], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "ace_of_clubs":
    				newItem = Instantiate(items[33], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "ace_of_diamonds":
    				newItem = Instantiate(items[34], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "ace_of_hearts":
    				newItem = Instantiate(items[35], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "ace_of_spades":
    				newItem = Instantiate(items[36], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "d4":
    				newItem = Instantiate(items[37], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "d6":
    				newItem = Instantiate(items[38], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "d8":
    				newItem = Instantiate(items[39], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "d20":
    				newItem = Instantiate(items[40], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "puff_ball_flute":
    				newItem = Instantiate(items[43], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "rainbow_brew":
    				newItem = Instantiate(items[44], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "silver_brew":
    				newItem = Instantiate(items[45], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "golden_brew":
    				newItem = Instantiate(items[46], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "golden_bomb":
    				newItem = Instantiate(items[47], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;
    			case "bomb":
    				newItem = Instantiate(items[48], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				newItem.GetComponent<Item>().canTransmute = false;
    				break;


    			case "pickup":
    				newItem = Instantiate(items[10], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "shop_item":
    				newItem = Instantiate(items[11], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "treasure_room_item":
    				newItem = Instantiate(items[12], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "cursed_item":
    				newItem = Instantiate(items[15], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "satchet_item":
    				newItem = Instantiate(items[28], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "deck_item":
    				newItem = Instantiate(items[41], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "die_item":
    				newItem = Instantiate(items[42], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;


    			case "slime":
    				Instantiate(enemies[0], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "staredown":
    				Instantiate(enemies[1], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "staredown_sorcerer":
    				Instantiate(enemies[2], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "staredown_mutant":
    				Instantiate(enemies[5], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "staredown_commander":
    				Instantiate(enemies[3], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "staredown_aberration":
    				Instantiate(enemies[4], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "explosive_barrel":
    				Instantiate(enemies[6], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "molten_staredown":
    				Instantiate(enemies[7], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "staredown_aglomeration":
    				Instantiate(enemies[8], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "shifter":
    				Instantiate(enemies[9], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "staredown_reflector":
    				Instantiate(enemies[10], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "energy_crawler":
    				Instantiate(enemies[11], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "gulping_slime":
    				Instantiate(enemies[12], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;
    			case "crimson_slime":
    				Instantiate(enemies[13], PlayerController.instance.gameObject.transform.position, Quaternion.identity);
    				break;

    		}
    	});

		KILL = new DebugCommand("kill", "kills the player", "kill", () => {
			PlayerController.instance.TakeDamage(99999999);
		});

		SET_COINS = new DebugCommand<string>("set_coins", "sets coins to given amount", "set_coins <coins_to_set>", (x) => {
			PlayerController.instance.currency = Int32.Parse(x);
		});

		SET_HEALTH = new DebugCommand<string>("set_health", "sets health to given amount", "set_health <health_to_set>", (x) => {
			PlayerController.instance.maxHealth = Int32.Parse(x);
			PlayerController.instance.currentHealth = Int32.Parse(x);
		});

		SET_KEYS = new DebugCommand<string>("set_keys", "sets keys to given amount", "set_keys <keys_to_set>", (x) => {
			PlayerController.instance.keys = Int32.Parse(x);
		});

		LOAD_SCENE = new DebugCommand<string>("load_scene", "loads the said scene", "load_scene <scene_to_load>", (x) => {
			PlayerController.instance.transform.position = Vector3.zero;
			showConsole = false;
			PlayerController.instance.isPaused = true;
			SceneManager.LoadScene(x);
		});

    	commandList.Add(SPAWN);
    	commandList.Add(KILL);
    	commandList.Add(SET_COINS);
    	commandList.Add(SET_KEYS);
    	commandList.Add(SET_HEALTH);
    	commandList.Add(LOAD_SCENE);
    }

    private void OnGUI() {
    	if(!showConsole) {
    		return;
    	} 

    	GUIStyle style = new GUIStyle();

    	style.fontSize = 20;
    	style.alignment = TextAnchor.MiddleLeft;
    	style.normal.textColor = Color.white;

    	GUI.backgroundColor = new Color(1,1,1,0.5f);

    	PlayerController.instance.isPaused = true;

    	float y = 0;

    	GUI.Box(new Rect(0, y, Screen.width, 60), "");
    	input = GUI.TextField(new Rect(10, y + 5, Screen.width-20, 40), input, style);

    	y += 60;

    	string[] properties = new string[0];

    	if(input.Length > 0) {
    		properties = input.Split(' ');
    	}

    	if(showHelp) {
    		Vector2 scroll = Vector2.zero;
    		
    		GUI.Box(new Rect(0, y, Screen.width, 100), "", style);

    		Rect viewport = new Rect(0,0,Screen.width-30,20*commandList.Count + 20 * spawnables.Count);

    		scroll = GUI.BeginScrollView(new Rect(0, y+5, Screen.width, 200), scroll, viewport, false, true);

    		int k = 0;

    		for(int i = 0; i < commandList.Count; i++) {
    			DebugCommandBase command = commandList[i] as DebugCommandBase;

    			if(input.Length > 0 && properties.Length == 1) {
	    			if(command.commandFormat[0] == input[0] || input[0] == ' ') {
	    				string label = $"{command.commandFormat} - {command.commandDesc}";

	    				Rect labelRect = new Rect(5, 20 * k, viewport.width - 100, 20);

	    				GUI.Label(labelRect, label, style);

	    				k++;
	    			}
	    		} else if(input.Length > 0 && properties.Length > 1) {
	    			if(properties[1].Length > 0) {
		    			for(int e = 0; e < spawnables.Count; e++) {
			    			if(command.commandID == "spawn" && command.commandID == properties[0] && properties[1][0] == spawnables[e][0]) {	
			    				string label = $"{command.commandID} {spawnables[e]}";

			    				Rect labelRect = new Rect(5, 20 * k, viewport.width - 100, 20);

			    				GUI.Label(labelRect, label, style);

			    				k++;
			    			}
			    		}
			    	} else {
			    		for(int e = 0; e < spawnables.Count; e++) {
			    			if(command.commandID == "spawn" && command.commandID == properties[0]) {	
			    				string label = $"{command.commandID} {spawnables[e]}";

			    				Rect labelRect = new Rect(5, 20 * k, viewport.width - 100, 20);

			    				GUI.Label(labelRect, label, style);

			    				k++;
			    			}
			    		}
			    	}
	    		}
    		}

    		GUI.EndScrollView();
    	}
    }

    private void HandleInput() {
    	string[] properties = input.Split(' ');

    	for(int i = 0; i < commandList.Count; i++) {
    		DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

    		if(input.Contains(commandBase.commandID)) {
    			if(commandList[i] as DebugCommand != null) {
    				(commandList[i] as DebugCommand).Invoke();
    			} else if(commandList[i] as DebugCommand<string> != null) {
    				(commandList[i] as DebugCommand<string>).Invoke(properties[1]);
    			}
    		}
    	}
    }
}
