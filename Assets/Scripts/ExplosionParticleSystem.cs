using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleSystem : MonoBehaviour {

	private ParticleSystem explosionSystem;

	// Use this for initialization
	void Start () {
		
		explosionSystem = gameObject.GetComponent<ParticleSystem> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Explode(Vector3 explosionArea){
		explosionSystem.transform.position = explosionArea;
		explosionSystem.Play ();
	}
}
