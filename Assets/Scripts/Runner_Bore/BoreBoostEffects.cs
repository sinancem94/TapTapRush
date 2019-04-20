using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoreBoostEffects : MonoBehaviour
{
    private Vector3 scalerVec;
    private float boreScale;
	public Animator animator;
    private SpriteRenderer spriteRenderer;

    private GameObject Dummy;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        CreateDummy();

        boreScale = this.transform.localScale.x;
        scalerVec = new Vector3(0.1f, 0.1f, 0f);
    }

    void CreateDummy()
    {
        Dummy = new GameObject();
        SpriteRenderer d_sprite = Dummy.AddComponent<SpriteRenderer>();
        d_sprite.sprite = spriteRenderer.sprite;
        d_sprite.material = spriteRenderer.material;
        d_sprite.sortingLayerID = spriteRenderer.sortingLayerID;

        Dummy.transform.localScale = this.transform.localScale;
        Dummy.transform.rotation = this.transform.rotation;

        Dummy.SetActive(false);
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


    public void BoreEntersBoost()
    {
        Dummy.transform.position = this.transform.position;
        Dummy.SetActive(true);

        spriteRenderer.enabled = false;
    }
    
    public void BoreStartsSliding()
    {
		animator.SetBool ("isSliding", true);
    }

    public void BoreExitsFromBoost()
    {
        Dummy.SetActive(false);
        spriteRenderer.enabled = true;

        animator.SetBool ("isSliding", false);
    }
}