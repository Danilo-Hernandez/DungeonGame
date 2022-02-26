using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	public float time;
	public GameObject creature;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Hatch());
    }

    public IEnumerator Hatch() {
    	yield return null;
    	yield return null;

    	yield return new WaitForSeconds(time);

    	Instantiate(creature, transform.position, Quaternion.identity);

    	Destroy(gameObject);
    }
}
