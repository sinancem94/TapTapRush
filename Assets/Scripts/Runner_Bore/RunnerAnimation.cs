using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerAnimation
{
    private MonoBehaviour Bore;

    private Vector3 scalerVec;
    private float boreScale;
	public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Coroutine SlideDownBore;

    private static RunnerAnimation instance;
    
    public RunnerAnimation(MonoBehaviour runner)
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("There is already a RunnerAnimation cannot create on more!!!");

        Bore = runner;

        animator = runner.GetComponent<Animator>();
        spriteRenderer = runner.GetComponent<SpriteRenderer>();

        boreScale = runner.transform.localScale.x;
        scalerVec = new Vector3(0.1f, 0.1f, 0f);
    }

    public void AnimationSpeed(float speed)
    {
        animator.SetFloat("speed", speed);
    }

    public void DisableSprite()
    {
        if(!Data.IsDebug)
            spriteRenderer.enabled = false;
    }

    public void ActivateSprite()
    {
        if (!Data.IsDebug)
            spriteRenderer.enabled = true;
    }

    #region BoostEffects

    public void BoreBoostAnimationSlideDown(Vector3 DummyPosition)
    {
        Platform.instance.SetBoreSpeed((int)BoostScript.BoostPhase.AnimationSlideDown);
        SlideDownBore = Bore.StartCoroutine(SlideDownBoreUntilPos(DummyPosition));
    }

    public void BoreBoostAnimationSlideUp()
    {
        Platform.instance.SetBoreSpeed();
        animator.SetBool("isSliding", true);
    }

    public void BoreExitsFromBoost()
    {
        animator.SetBool ("isSliding", false);
    }
    #endregion

    private IEnumerator SlideDownBoreUntilPos(Vector3 Pos)
    {
        float distanceBtwDummy = Bore.transform.position.y - Pos.y;

        float maxSlideSpeed = -2f;
        float slideSpeed = 0f;

        float speed;

        while(Bore.transform.position.y > Pos.y + .5f)
        {
            slideSpeed = (slideSpeed < maxSlideSpeed) ? slideSpeed -= 0.0001f : maxSlideSpeed;

            if (distanceBtwDummy > 5f)
                speed = distanceBtwDummy * slideSpeed;
            else
                speed = 5f * slideSpeed;
                       
            Bore.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);

            distanceBtwDummy = Bore.transform.position.y - Pos.y;

            //if (distanceBtwDummy < Platform.instance.distBetweenBlock * 3f)
                //Platform.instance.SetBoostPhase(BoostScript.BoostPhase.AnimationSlideUp); //Sliding down almost finishes since there is a slowtime function. Set phase to slideUp earlier

            yield return new WaitForSeconds(0.01f);
        }

        Bore.transform.position = Pos;

       Bore.StopCoroutine(SlideDownBore);
    }



    /*  public IEnumerator scaleBore(bool isScaleIncrease)
   {
       if (isScaleIncrease)
       {
           while (gameObject.transform.localScale.x < boreScale + 2f)
           {
               gameObject.transform.localScale += scalerVec;
               yield return new WaitForSeconds(.1f);
           }
       }
       else
       {
           while (gameObject.transform.localScale.x > boreScale)
           {
               gameObject.transform.localScale -= scalerVec;
               yield return new WaitForSeconds(.1f);
           }
       }
   }*/
}