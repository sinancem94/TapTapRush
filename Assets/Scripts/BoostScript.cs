using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour
{
	//public ParticleSystem explosionParticleSys;
	private BoostParticleSystem boostParticleSystem;
	private PostProcessingChange postProcessingChange;
	private BoreBoostEffects boreBoostEffects;
	private BadThingsBoostEffect badThingsBoostEffect;

	//public Color newVignetteColor;
	//public Color oldVignetteColor;

	private void Start(){
        boostParticleSystem = (BoostParticleSystem)FindObjectOfType(typeof(BoostParticleSystem));
		postProcessingChange = (PostProcessingChange)FindObjectOfType (typeof(PostProcessingChange));
		boreBoostEffects = (BoreBoostEffects)FindObjectOfType (typeof(BoreBoostEffects));
		badThingsBoostEffect = (BadThingsBoostEffect)FindObjectOfType (typeof(BadThingsBoostEffect));
	}

    public void StartBoost(float timeChangeSpeed)
    {
		badThingsBoostEffect.nightmareRadius (10f);
        StartCoroutine(SlowTime(timeChangeSpeed, true));
        boostParticleSystem.EnteringBoost();
		StartCoroutine (postProcessingChange.BoostVignetteSettings (true));
		StartCoroutine (boreBoostEffects.scaleBore (true));
    }
   

    public void StopBoost(float timeChangeSpeed)
    {
        StartCoroutine(SlowTime(timeChangeSpeed, false));
        boostParticleSystem.ExitingBoost();
		StartCoroutine (postProcessingChange.BoostVignetteSettings (false));
		StartCoroutine (boreBoostEffects.scaleBore (false));
		badThingsBoostEffect.nightmareRadius (1f);
    }

    private IEnumerator SlowTime(float changeSpeed, bool isStarted)
    {
        if(isStarted)
        {
            Platform.instance.SetBoost(true); // setted true if start boost called this
        }

        while(Time.timeScale > 0.4f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.3f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);  
        }

        Time.timeScale = 0.4f;

        yield return new WaitForSeconds(.4f);

        while(Time.timeScale < 1f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1.1f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);  
        }
        Time.timeScale = 1f;

        if(!isStarted)
        {
            Platform.instance.SetBoost(false); //Setted False after time speeds again
        }
          
        StopCoroutine(SlowTime(changeSpeed,isStarted));
    }
}
