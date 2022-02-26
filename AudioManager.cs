using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource[] sFX;
	public static AudioManager instance;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int index) {
    	sFX[index].Play();
    }
}
