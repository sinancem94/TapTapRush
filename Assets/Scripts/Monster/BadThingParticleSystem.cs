

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingParticleSystem : MonoBehaviour
{
    //public GameObject bore;
    private Camera mainCam; //Camera which partsys follows
    //private CameraMovement cameraMovement;

    private float distanceBetweenCamera;

    public float monsterSpeed;
    private float speed;

    // Update is called once per frame
    void Update()
    {
        if (Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
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