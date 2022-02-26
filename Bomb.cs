using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public float time;
	public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitTillExplosion());
    }

    public IEnumerator WaitTillExplosion() {
    	yield return new WaitForSeconds(time);

    	Instantiate(explosion, transform.position, Quaternion.identity);
    	Destroy(gameObject);
    }
}
