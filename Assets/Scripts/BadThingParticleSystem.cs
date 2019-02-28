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

    float speed;

    // Start is called before the first frame update
    void Start()
    {
		nightmareParticleSys = gameObject.GetComponent<ParticleSystem> ();
        mainCam = Camera.main;
        cameraMovement = mainCam.GetComponent<CameraMovement>();
        distanceBetweenCamera = 5f;

        tempVec = mainCam.transform.position;
        tempVec.y = tempVec.y - distanceBetweenCamera;
        tempVec.z = 0;
        transform.position = tempVec;

        speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
       /* tempVec = mainCam.transform.position;
        tempVec.y = tempVec.y - distanceBetweenCamera;
        tempVec.z = 0;*/
		NightmareChase (tempVec);
    }

    //When distance between Camera and bore move particle system to closer
	public void NightmareChase(Vector3 nightmarePos)
    {
       // if(bore.transform.position.y - transform.position.y > 10f)
		    //transform.position = nightmarePos;
        if(Platform.instance.game.state == GameHandler.GameState.GameRunning)
            this.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);
	}
}
