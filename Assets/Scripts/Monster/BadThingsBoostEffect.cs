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

	public void nightmareRadius(float nightmareRad){
		var shapeofNightmareParticle = nightmareParticleSys.shape;
		shapeofNightmareParticle.radius = nightmareRad;
	}

	public void debugBadThing(){
		Debug.Log ("debugbadthing");
	}
}
