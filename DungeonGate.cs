using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGate : MonoBehaviour
{
	public GameObject canvas;

	public string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerController.instance.transform.position, transform.position) < 1.5f) {
        	canvas.SetActive(true);
        } else {
        	canvas.SetActive(false);
        }
    }

    public void LoadScene() {
    	PlayerController.instance.isPaused = true;
    	PlayerController.instance.transform.position = Vector3.zero;
    	PlayerController.instance.luck -= 5;
    	SceneManager.LoadScene(sceneToLoad);
    }
}
