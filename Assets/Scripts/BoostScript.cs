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

    private Coroutine timeSlower;

	//public Color newVignetteColor;
	//public Color oldVignetteColor;

	private void Start()
    {
        boostParticleSystem = (BoostParticleSystem)FindObjectOfType(typeof(BoostParticleSystem));
		postProcessingChange = (PostProcessingChange)FindObjectOfType (typeof(PostProcessingChange));
		boreBoostEffects = (BoreBoostEffects)FindObjectOfType (typeof(BoreBoostEffects));
		badThingsBoostEffect = (BadThingsBoostEffect)FindObjectOfType (typeof(BadThingsBoostEffect));
	}



    public void StartBoost(float timeChangeSpeed) //Phase 1 initiate
    {
        Platform.instance.SetBoost(true);

        timeSlower =  StartCoroutine(SlowTime(timeChangeSpeed));

        //explosionParticleSys.gameObject.SetActive (true);
        boostParticleSystem.EnteringBoost();

        StartCoroutine(postProcessingChange.BoostPostProcessingSettings(true));

        //  StartCoroutine (boreBoostEffects.scaleBore (true));  //boreboosteffectte var büyütüp küçültüyo. boreyi durduracağımız için yoruma aldım işlevsiz olacak büyük iht
        //boreBoostEffects.stopBore ();     // only for testing animation right now.


        boreBoostEffects.BoreEntersBoost();

        //boreBoostEffects.BoreStartsSliding();   								
    }


    public void BoostPhaseOneFinished()
    {

    }


    public void StopBoost(float timeChangeSpeed)
    {
        timeSlower = StartCoroutine(SlowTime(timeChangeSpeed,true));
        //explosionParticleSys.gameObject.SetActive (false);

        boostParticleSystem.ExitingBoost();

        StartCoroutine(postProcessingChange.BoostPostProcessingSettings(false));

        badThingsBoostEffect.badThingsBoostExit(Platform.instance.runner.transform.position.y);

        //StartCoroutine (boreBoostEffects.scaleBore (false));   //boreboosteffectte var büyütüp küçültüyo. boreyi durduracağımız için yoruma aldım işlevsiz olacak büyük iht
        //badThingsBoostEffect.nightmareRadius (1f);
		boreBoostEffects.BoreExitsFromBoost(); 									// only for testing animation right now.
    }


    //Defined two SlowTime corountines because if isBoost setted to false directly before time slowed and information of boost end passed to player. 
    //Player will not be able to understand boost end & will die.

    //Just slows time does not change any parameter

    private IEnumerator SlowTime(float changeSpeed)
    {
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

        StopCoroutine(timeSlower);
    }


    //This is used when boost is Ended if boostEnd parameter is true at the end of the corountine isBoost is setted to false.

    private IEnumerator SlowTime(float changeSpeed,bool boostEnd)
    {
        while (Time.timeScale > 0.4f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.3f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);
        }

        Time.timeScale = 0.4f;

        yield return new WaitForSeconds(.4f);

        while (Time.timeScale < 1f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1.1f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);
        }
        Time.timeScale = 1f;

        if(boostEnd)
        {
            Platform.instance.SetBoost(false);
        }

        StopCoroutine(timeSlower);
    }

}
