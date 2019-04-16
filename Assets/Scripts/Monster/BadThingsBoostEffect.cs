using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingsBoostEffect : MonoBehaviour
{
	public GameObject bore;
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

	public void badThingBoostEntering(){
	
	}

	public void badThingsBoostExit(){    					// henüz kullanmadık boostscripte eklenecek.
		Vector3 nightmarePos = new Vector3(0f, bore.transform.position.y - 5, 0);
		this.transform.position = nightmarePos;
	}
}
