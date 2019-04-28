using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoreBoostEffects : MonoBehaviour
{
    private Vector3 scalerVec;
    private float boreScale;
	public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Coroutine SlideDownBore;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        boreScale = this.transform.localScale.x;
        scalerVec = new Vector3(0.1f, 0.1f, 0f);
    }


    public IEnumerator scaleBore(bool isScaleIncrease)
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
    }


    public void BoreEntersBoost()
    {
        spriteRenderer.enabled = false;
    }

    public void BoreBoostAnimationSlideDown(Vector3 DummyPosition)
    {
        Platform.instance.SetBoreSpeed((int)BoostScript.BoostPhase.AnimationSlideDown);

        SlideDownBore = StartCoroutine(SlideDownBoreUntilDummy(DummyPosition));
    }

    public void ActivateSprite()
    {
        spriteRenderer.enabled = true;
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

    private IEnumerator SlideDownBoreUntilDummy(Vector3 dummyPos)
    {
        float distanceBtwDummy = this.transform.position.y - dummyPos.y;

        float maxSlideSpeed = -2f;
        float slideSpeed = 0f;

        float speed;

        while(this.transform.position.y > dummyPos.y + .5f)
        {
            slideSpeed = (slideSpeed < maxSlideSpeed) ? slideSpeed -= 0.0001f : maxSlideSpeed;

            if (distanceBtwDummy > 5f)
                speed = distanceBtwDummy * slideSpeed;
            else
                speed = 5f * slideSpeed;
                       
            this.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);

            distanceBtwDummy = this.transform.position.y - dummyPos.y;

            //if (distanceBtwDummy < Platform.instance.distBetweenBlock * 3f)
                //Platform.instance.SetBoostPhase(BoostScript.BoostPhase.AnimationSlideUp); //Sliding down almost finishes since there is a slowtime function. Set phase to slideUp earlier

            yield return new WaitForSeconds(0.01f);
        }

        this.transform.position = dummyPos;

        StopCoroutine(SlideDownBore);
    }

}