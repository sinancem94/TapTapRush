using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostParticleSystem : MonoBehaviour
{
    public ParticleSystem explosionSystemRight;
    public ParticleSystem explosionSystemLeft;
    //private ParticleSystem[] particleSystemArray;

    public GameObject gameObjectRight;
    public GameObject gameObjectLeft;

    // Use this for initialization
    void Start()
    {
        //particleSystemArray = gameObject.GetComponentsInChildren<ParticleSystem>();
        gameObjectRight = GameObject.FindWithTag("RightExplosionSystem");
        gameObjectLeft = GameObject.FindWithTag("LeftExplosionSystem");

        explosionSystemRight = gameObjectRight.GetComponent<ParticleSystem>();
        explosionSystemLeft = gameObjectLeft.GetComponent<ParticleSystem>();
    }

   /* public void Explode(Vector3 explosionArea)
    {
        explosionSystemRight.transform.position = explosionArea;
        explosionSystemLeft.transform.position = explosionArea;
        explosionSystemRight.Play();
        explosionSystemLeft.Play();
    }*/

    public void EnteringBoost()
    {
        Debug.Log("Entering boost");

        float boostPos = Platform.instance.sizeHandler.GetWallPosition();

        Vector3 boostParticlePosRight = new Vector3(boostPos, Platform.instance.runner.transform.position.y + 10f, 0);
        Vector3 boostParticlePosLeft = new Vector3(-boostPos, Platform.instance.runner.transform.position.y + 10f, 0);

        gameObjectRight.transform.position = boostParticlePosRight;
        gameObjectLeft.transform.position = boostParticlePosLeft;

        explosionSystemRight.Play();
        explosionSystemLeft.Play();

    }

    public void ExitingBoost()
    {
        explosionSystemLeft.Stop();
        explosionSystemRight.Stop();
    }
}