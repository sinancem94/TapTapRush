using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour
{
    //Objects used for boost 
    private GameObject BoreDummy; //Enabled when boost starts. Normal runners sprite is disabled and this dummy placed into its place to make it look like runner is stopped.
    private GameObject RoadDummy; //Create a dummy road with sprites. Starting from BoreDummy.transform.position minus few blocks and reaches real roads first block. 

    private DummyBoostObjects dummyObjects;

    //public ParticleSystem explosionParticleSys;
    private BoostParticleSystem boostParticleSystem;
	private PostProcessingChange postProcessingChange;
	private RunnerAnimation boreBoostEffects;
	private BadThingsAnimation badThingsBoostEffect;

    private Coroutine timeSlower;
    private bool TimeSlowed = false;

    private Coroutine boostAnimation;

    private float currMonsSpeed;

    private BoostEndMode endMode = BoostEndMode.CamChase;


    public enum BoostPhase
    {
        None,
        OnBoost,
        AnimationSlideDown,
        AnimationSlideUp
    };

    enum BoostEndMode
    {
        Time,
        CamChase
    };

    //public Color newVignetteColor;
    //public Color oldVignetteColor;

    private void Start()
    {
        boostParticleSystem = (BoostParticleSystem)FindObjectOfType(typeof(BoostParticleSystem));
		postProcessingChange = (PostProcessingChange)FindObjectOfType (typeof(PostProcessingChange));

        boreBoostEffects = Platform.instance.Runner.GetComponent<Runner>().BoreAnimationController;
        badThingsBoostEffect = Platform.instance.Nightmare.GetComponent<BadThingParticleSystem>().BadThingsAnimationController;

        dummyObjects = new DummyBoostObjects();  

        BoreDummy = dummyObjects.CreateDummy(boreBoostEffects); //Create dummy bore for boost to use. Using boreBoostEffect to use its spriteRenderer parameter
        RoadDummy = dummyObjects.CreateRoadDummy();
        //CreateRoadReplica();
	}



    public void StartBoost() //Phase 1 initiate
    {
        //float timeChangeSpeed = 20f;
        timeSlower = StartCoroutine(SlowTime(0.3f,0.4f,BoostPhase.OnBoost));

        //explosionParticleSys.gameObject.SetActive (true);
        boostParticleSystem.EnteringBoost();

        StartCoroutine(postProcessingChange.BoostPostProcessingSettings(true));
        //  StartCoroutine (boreBoostEffects.scaleBore (true));  //boreboosteffectte var büyütüp küçültüyo. boreyi durduracağımız için yoruma aldım işlevsiz olacak büyük iht
        //boreBoostEffects.stopBore ();     // only for testing animation right now.
        currMonsSpeed = badThingsBoostEffect.BadThingBoostEntering();
        boreBoostEffects.DisableSprite();
        dummyObjects.SetDummy();

        //boreBoostEffects.BoreStartsSliding();   								
    }


    public void BoostFinish()
    {
        //float timeChangeSpeed = 100f;
        boostAnimation = StartCoroutine(BoostAnimatonPhase());
    }


    public void StopBoost()
    {
        //float timeChangeSpeed = 20f;
        timeSlower = StartCoroutine(SlowTime(0.4f, 0.4f, BoostPhase.None)); //Not setting to None before time slowed up because of player input issue.
        //explosionParticleSys.gameObject.SetActive (false);

        boostParticleSystem.ExitingBoost();

        StartCoroutine(postProcessingChange.BoostPostProcessingSettings(false));

        dummyObjects.DestoryRoadDummy();
        //StartCoroutine (boreBoostEffects.scaleBore (false));   //boreboosteffectte var büyütüp küçültüyo. boreyi durduracağımız için yoruma aldım işlevsiz olacak büyük iht
        //badThingsBoostEffect.nightmareRadius (1f);
		boreBoostEffects.BoreExitsFromBoost(); 									// only for testing animation right now.
        badThingsBoostEffect.BadThingsBoostExit(currMonsSpeed);
    }


    /// <summary>
    /// Boost animation starts from when player input is cutted and camera starts rolls back until last position of dummy bore. 
    /// This iterator calls BoreBoostAnimationStarts and waits until camera slides after that it calls bores sliding animation and finally finishes that to. 
    /// </summary>
    /// <returns>The animator.</returns>
    private IEnumerator BoostAnimatonPhase() 
    {
        timeSlower = StartCoroutine(SlowTime(0.4f, 0.4f, BoostPhase.AnimationSlideDown)); //First slow time to make player understand boost is finished than start sliding down

        yield return new WaitUntil(() => TimeSlowed);

        dummyObjects.SetRoadDummy();
        boreBoostEffects.BoreBoostAnimationSlideDown(BoreDummy.transform.position); //Start sliding down the camera

        yield return new WaitUntil(() => Platform.instance.Runner.transform.position.y <= BoreDummy.transform.position.y + 0.5f);//Platform.instance.GetBoostPhase() == BoostPhase.AnimationSlideUp);

        Platform.instance.SetBoostPhase(BoostScript.BoostPhase.AnimationSlideUp);
        boreBoostEffects.ActivateSprite();
        dummyObjects.DisableDummy();
        //float timeChangeSpeed = 100f;
        timeSlower = StartCoroutine(SlowTime( 0.4f, 0.15f, BoostPhase.AnimationSlideUp)); //first slow time to make player understand camera slidid until bore than bore starts sliding

        yield return new WaitUntil(() => TimeSlowed);

        badThingsBoostEffect.BadThingsAnimationEnter(currMonsSpeed);
        boreBoostEffects.BoreBoostAnimationSlideUp();

        yield return new WaitUntil(() => Platform.instance.straightRoadLenght < Platform.instance.distBetweenBlock);

        StopBoost();
    }

    //Defined two SlowTime corountines because if isBoost setted to false directly before time slowed and information of boost end passed to player. 
    //Player will not be able to understand boost end & will die.

    private IEnumerator SlowTime( float minTime , float waitTime, BoostPhase phase)
    {
        TimeSlowed = false;
      /*  while (Time.timeScale > minTime)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, minTime - 0.1f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);
        }
        */
        Time.timeScale = minTime;

        yield return new WaitForSeconds(waitTime);

      /*  while (Time.timeScale < 1f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1.1f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);
        }*/
        Time.timeScale = 1f;

        TimeSlowed = true;

        if(phase != Platform.instance.GetBoostPhase())
        {
            Platform.instance.SetBoostPhase(phase);
        }

        StopCoroutine(timeSlower);
    }

}
