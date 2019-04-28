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

    public float BadThingBoostEntering()
    {
        float monsSpeed = Platform.instance.GetMonsterSpeed();
        Platform.instance.SetMonsterSpeed(0);

        return monsSpeed;
    }

    public void BadThingsAnimationEnter(float spd)
    {
        Platform.instance.SetMonsterSpeed(spd * 4);
    }

    public void BadThingsBoostExit(float spd)
    {                      
        Platform.instance.SetMonsterSpeed(spd);
      //  Vector3 nightmarePos = new Vector3(0f, newPos - 5f, 0);
       // this.transform.position = nightmarePos;
    }
}