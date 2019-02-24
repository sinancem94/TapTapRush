using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingParticleSystem : MonoBehaviour
{
	public GameObject bore;
	private ParticleSystem nightmareParticleSys;

    // Start is called before the first frame update
    void Start()
    {
		nightmareParticleSys = gameObject.GetComponent<ParticleSystem> ();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 tempVec = bore.transform.position;
		tempVec.y = tempVec.y - 3f;
		nightmareChase (tempVec);
    }

	public void nightmareChase(Vector3 nightmarePos){
		nightmareParticleSys.transform.position = nightmarePos;
	}
}
