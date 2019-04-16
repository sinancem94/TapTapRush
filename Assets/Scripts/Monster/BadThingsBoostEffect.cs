using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingsBoostEffect : MonoBehaviour
{
    private ParticleSystem nightmareParticleSys;

    void Start()
    {
        nightmareParticleSys = gameObject.GetComponent<ParticleSystem>();
    }

    public void nightmareRadius(float nightmareRad)
    {
        var shapeofNightmareParticle = nightmareParticleSys.shape;
        shapeofNightmareParticle.radius = nightmareRad;
    }

    public void debugBadThing()
    {
        Debug.Log("debugbadthing");
    }

    public void badThingBoostEntering()
    {

    }

    public void badThingsBoostExit(float newPos)
    {                       // henüz kullanmadık boostscripte eklenecek.
        Vector3 nightmarePos = new Vector3(0f, newPos - 5f, 0);
        this.transform.position = nightmarePos;
    }
}