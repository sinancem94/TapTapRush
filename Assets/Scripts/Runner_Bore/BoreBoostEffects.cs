using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoreBoostEffects : MonoBehaviour
{
    private Vector3 scalerVec;
    private float boreScale;
	public Animator animator;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
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

    public void stopBore()
    {
        this.transform.Translate(0f, 0f * speed * Time.deltaTime, 0f, Space.World);
    }

    public void boreStartsSliding()
    {
		animator.SetBool ("isSliding", true);
    }

    public void boreExitsFromBoost()
    {
		animator.SetBool ("isSliding", false);
    }
}