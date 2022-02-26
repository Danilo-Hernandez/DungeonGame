using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	public static CameraController instance;

    public Transform target;

    public float moveSpeed;

	public Camera mainCamera, miniMapCamera;
	public CinemachineVirtualCamera vCam;
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

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Update is called once per frame
    public IEnumerator Shake(float intensity, float time, float delay)
    {
    	yield return new WaitForSeconds(delay);

        vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;

        yield return new WaitForSeconds(time);

        vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

        transform.position = new Vector3(transform.position.x,transform.position.y,-10);
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    public void ShakeFunc(float intensity, float time, float delay)
    {
    	StartCoroutine(Shake(intensity, time, delay));
    }
}
