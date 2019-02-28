using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingParticleSystem : MonoBehaviour
{
	public GameObject bore;
	private ParticleSystem nightmareParticleSys;
    private Camera mainCam; //Camera which partsys follows
    private CameraMovement cameraMovement;

    Vector3 tempVec;
    private float distanceBetweenCamera;

    // Start is called before the first frame update
    void Start()
    {
		nightmareParticleSys = gameObject.GetComponent<ParticleSystem> ();
        mainCam = Camera.main;
        cameraMovement = mainCam.GetComponent<CameraMovement>();
        distanceBetweenCamera = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        tempVec = mainCam.transform.position;
        tempVec.y = tempVec.y - distanceBetweenCamera;
		NightmareChase (tempVec);
    }

    //When distance between Camera and bore move particle system to closer
	public void NightmareChase(Vector3 nightmarePos)
    {
       // if(bore.transform.position.y - transform.position.y > 10f)
		    transform.position = nightmarePos;
	}
}
