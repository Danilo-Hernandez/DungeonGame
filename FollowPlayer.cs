using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(PlayerController.instance.gameObject.transform.position.x, PlayerController.instance.gameObject.transform.position.y, transform.position.z);
    }
}
