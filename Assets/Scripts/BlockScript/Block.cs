using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour
{

    public BlockData.blockType type;
    public BlockData blockData;

    private BlockAnimation blockAnimation;

    private SpriteRenderer BlockSprite;
    private float outlineSize;
    bool isShined;

    public bool isMoving;

    private Vector2 blockScale;

    // private SpriteRenderer blockSprite;
    // private SpriteRenderer ChildBlockSprite;

    private void OnEnable()
    {
        blockData = new BlockData();
        blockAnimation = this.GetComponent<BlockAnimation>();

        BlockSprite = GetComponent<SpriteRenderer>();

        BlockSprite.sprite = blockData.normalBlock;
        BlockSprite.color = blockData.normalColor;

        //Debug.Log(BlockSprites.Count);
        outlineSize = BlockSprite.material.GetFloat("_OutlineSize");
        //BlockSprites[0].material.GetFloat("_OutlineSize");

        isShined = false;
        isMoving = false;

        type = BlockData.blockType.normal;

        blockScale = Platform.instance.blockScale;

       /* if (!Data.isAngled) //for mode trial
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }*/

    }

    private void Update()
    {
        //Blokların ortaya geçince parlaması için..
        if (Mathf.Approximately(this.transform.position.x, 0) && !isShined)
        {

            isShined = true;

            float smalledSize = blockScale.x / 2.5f;
			StartCoroutine (ScaleDownBlock (isShined, smalledSize));

           /* if (outlineSize < 10f)																burayla
            {
                outlineSize += 0.5f;
                BlockSprite.material.SetFloat("_OutlineSize", outlineSize);
            }
            else //if outlineSize >= 10
            {
                isShined = true;
                BlockSprite.material.SetFloat("_OutlineSize", 10);									burası arası outline size kullanılmayacağı için çıkartıldı
            }*/
        }
    }

    public void SetBlock(LevelManager.LevelBlockType bt)
    {
        if(bt == LevelManager.LevelBlockType.Reverse)
        {
            if(type == BlockData.blockType.normal)
                blockData.ChangeBlockType(ref type, BlockSprite);
        }
        else if (bt == LevelManager.LevelBlockType.Mixed)
        {
            int r = Random.Range(0, 10);
            //TODO: Create a random reverse generator that deals reverse positions 
            if (type == BlockData.blockType.reverse || r < 2)
            {
                blockData.ChangeBlockType(ref type, BlockSprite);
            }
        }
        isShined = false;
        StartCoroutine (ScaleDownBlock (isShined, blockScale.x));
        //outlineSize = 1f;
        //BlockSprite.material.SetFloat("_OutlineSize", outlineSize);
    }

    public void MoveTile(float toPosition)
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(blockAnimation.MoveTile(toPosition));
        }
    }

    public void Fall(Vector2 fallTo)
    {
        StartCoroutine(blockAnimation.Fall(fallTo));
    }

	IEnumerator ScaleDownBlock(bool isScaledDown, float scaledTo)
    {
		if (isScaledDown) 
        {
			while (transform.localScale.x > scaledTo) 
            {
				transform.localScale -= new Vector3 (0.08f, 0.08f, 0);

                yield return new WaitForSeconds (0.001f);
			}
            outlineSize = 0;
			BlockSprite.material.SetFloat ("_OutlineSize", outlineSize);
		} 
        else 
        {
            transform.localScale = new Vector2(scaledTo,scaledTo);

            outlineSize = 1;
            BlockSprite.material.SetFloat("_OutlineSize", outlineSize);
        }
	}
}