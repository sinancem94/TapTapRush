using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleSystem : MonoBehaviour
{
	public GameObject bore;
    private ParticleSystem explosionSystemRight;
	private ParticleSystem explosionSystemLeft;
	private ParticleSystem[] particleSystemArray;

    // Use this for initialization
    void Start()
    {
		particleSystemArray = gameObject.GetComponentsInChildren<ParticleSystem> ();
		explosionSystemRight = particleSystemArray [0];
		explosionSystemLeft = particleSystemArray [2];
		Debug.Log ("particleSystemArrayLength= " + particleSystemArray.Length);
    }

    public void Explode(Vector3 explosionArea)
    {
        explosionSystemRight.transform.position = explosionArea;
		explosionSystemLeft.transform.position = explosionArea;
        explosionSystemRight.Play();
		explosionSystemLeft.Play ();
    }

	public void EnteringBoost(){
		Debug.Log ("entering boost");

		Vector3 boostParticlePosRight = new Vector3 (2f, bore.transform.position.y + 10, 0);
		Vector3 boostParticlePosLeft = new Vector3 (-2f, bore.transform.position.y + 10, 0);
		explosionSystemRight.transform.position = boostParticlePosRight;
		explosionSystemLeft.transform.position = boostParticlePosLeft;

		explosionSystemRight.Play ();
		explosionSystemLeft.Play ();

	}

	public void ExitingBoost(){
		explosionSystemLeft.Stop ();
		explosionSystemRight.Stop ();
	}

	/*public IEnumerator BoostEntering(){
		
		Vector3 boostParticlePos = new Vector3 (2f, bore.transform.position.y + 10, 0);
		explosionSystem.Play ();
			for (int i = 0; i < 1; i++) {
				explosionSystem.transform.position = boostParticlePos;
				Debug.Log ("explosionPos  " + explosionSystem.transform.position);
				boostParticlePos.y = boostParticlePos.y + 2;
				yield return new WaitForSeconds (.08f);
			}

	}*/
}