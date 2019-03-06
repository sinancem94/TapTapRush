

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingParticleSystem : MonoBehaviour
{
    //public GameObject bore;
    private ParticleSystem nightmareParticleSys;

    private Camera mainCam; //Camera which partsys follows
    //private CameraMovement cameraMovement;

    private float distanceBetweenCamera;

    public float monsterSpeed;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        nightmareParticleSys = gameObject.GetComponent<ParticleSystem>();

        mainCam = Camera.main;
        //cameraMovement = mainCam.GetComponent<CameraMovement>();
        distanceBetweenCamera = 6f;

        transform.position = new Vector3(mainCam.transform.position.x,mainCam.transform.position.y - distanceBetweenCamera,0f);

        //monsterSpeed = Data.monsSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Platform.instance.game.state == GameHandler.GameState.GameRunning)
        {
            if(!Platform.instance.isBoost)
                speed = ((Platform.instance.distanceBtwRunner * 3) / 4) * monsterSpeed; //speed whithout boost 
            else
                speed = ((Platform.instance.distanceBtwRunner) * 4f) * monsterSpeed; // speed with boost

            NightmareChase(speed);
        }
    }

    //When distance between Camera and bore move particle system to closer
	public void NightmareChase(float spd)
    {
          this.transform.Translate(0f, spd * Time.deltaTime, 0f, Space.World);
    }
}