using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
	public GameObject effect;
    
    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Projectile") {
        	Instantiate(effect, transform.position, Quaternion.identity);

        	AudioManager.instance.PlaySFX(6);

        	CameraController.instance.ShakeFunc(SettingsController.instance.rumble, 0.2f, 0);

        	Destroy(gameObject);
        }
    }
}
