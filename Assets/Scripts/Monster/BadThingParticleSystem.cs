using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingParticleSystem : MonoBehaviour
{
    //public GameObject bore;
    //private CameraMovement cameraMovement;

    public float BoostBrake; // 0 or 1. when boost is on set to zero
    public float monsterSpeed;
    private float speed;

    public BadThingsAnimation BadThingsAnimationController;

    private void Start()
    {
        BadThingsAnimationController = new BadThingsAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            speed = ((Platform.instance.distanceBtwRunner * 3) / 4) * monsterSpeed; //speed whithout boost 
            NightmareChase(speed);
        }
    }

    //When distance between Camera and bore move particle system to closer
    private void NightmareChase(float spd)
    {
        this.transform.Translate(BadThingsAnimationController.TranslationVector(spd), Space.World);
    }

    //public void StopOrStartMonster

}
