using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour, ISaveable
{
	public static SettingsController instance;

	public bool usingController, usingKeyboard, cheatsOn, isDemo;

	public int rumble, volume;

	public AudioMixer mainMixer;

	public bool[] unlocked;
    // Start is called before the first frame update
    void Awake()
    {	
    	//checks if an instance already exists
    	if(instance == null) {
        	instance = this;
    	} else {
    		Destroy(gameObject);
    	}

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    	//sets volume
        mainMixer.SetFloat("MasterVolume", volume);
    }

    //saves data
    public object CaptureState() {
    	return new SaveData {
    		vol = volume,
    		rumb = rumble,
    		unlocks = unlocked
    	};
    }

    //loads data
    public void RestoreState(object state) {
    	var saveData = (SaveData)state;

    	volume = saveData.vol;
    	rumble = saveData.rumb;
    	unlocked = saveData.unlocks;
    }

    //data template
    [System.Serializable]
    public struct SaveData {
    	public int vol, rumb;

    	public bool[] unlocks;
    }
}
