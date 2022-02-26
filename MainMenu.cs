using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject optionsMenu, particles, particles2, particles3, credits;

	public bool started;

	public Slider volumeSlider, rumbleSlider;
    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = SettingsController.instance.volume;
        rumbleSlider.value = SettingsController.instance.rumble;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRun() {
    	if(!started) {
    		SceneManager.LoadScene("Floor1");
    	}

    	started = true;
    }

    public void Quit() {
    	Application.Quit();
    }

    public void OpenOptions() {
    	optionsMenu.SetActive(true);

    	particles.SetActive(false);
    	particles2.SetActive(false);
    	particles3.SetActive(false);
    }

    public void CloseOptions() {
    	optionsMenu.SetActive(false);

    	particles.SetActive(true);
    	particles2.SetActive(true);
    	particles3.SetActive(true);
    }

    public void OpenCredits() {
    	credits.SetActive(true);
    	
    	particles.SetActive(false);
    	particles2.SetActive(false);
    	particles3.SetActive(false);
    }

    public void CloseCredits() {
    	credits.SetActive(false);
    	
    	particles.SetActive(true);
    	particles2.SetActive(true);
    	particles3.SetActive(true);
    }

    public void SetVolume(float vol) {
    	SettingsController.instance.volume = (int)vol;
    	SavingLoading.instance.Save();
    }

    public void SetRumble(float rumb) {
    	SettingsController.instance.rumble = (int)rumb;
    	SavingLoading.instance.Save();
    }

    public void OpenFeedback() {
    	Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSd5s6mZcDNY6zqDulfRd7hvExFczZG_i_-kCqDr0jcaz1QwgQ/viewform?usp=sf_link");
    }
}
