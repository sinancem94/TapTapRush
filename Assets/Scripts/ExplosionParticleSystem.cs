using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleSystem : MonoBehaviour
{
    public GameObject bore;
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
        //Debug.Log(explosionSystemRight);
        explosionSystemLeft = gameObjectLeft.GetComponent<ParticleSystem>();
        //Debug.Log(explosionSystemLeft);

        //Debug.Log("particleSystemArrayLength= " + particleSystemArray.Length);
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
        Debug.Log("entering boost");

        Vector3 boostParticlePosRight = new Vector3(2f, bore.transform.position.y + 10, 0);
        Vector3 boostParticlePosLeft = new Vector3(-2f, bore.transform.position.y + 10, 0);
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