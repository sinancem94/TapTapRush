﻿using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {
    
    public BlockData.blockType type;
    private BlockAnimation blockAnimation;

    private List<SpriteRenderer> BlockSprites;

    private float outlineSize;

    bool isShined;

    public bool isMoving;

   // private SpriteRenderer blockSprite;
   // private SpriteRenderer ChildBlockSprite;
    private Text limitText;

    public int limit;

    private void OnEnable()
    {
        blockAnimation = this.GetComponent<BlockAnimation>();
        BlockSprites = new List<SpriteRenderer>();

        foreach(SpriteRenderer sp in this.GetComponentsInChildren<SpriteRenderer>())
        {
            BlockSprites.Add(sp);

            sp.sprite = BlockData.normalBlock;
            sp.color = BlockData.normalColor;
        }
//        Debug.Log(BlockSprites.Count);
        outlineSize = BlockSprites[0].material.GetFloat("_OutlineSize");

       /* blockSprite = GetComponent<SpriteRenderer>();
        ChildBlockSprite = GetComponentInChildren<SpriteRenderer>();



        blockSprite.sprite = BlockData.normalBlock;
        blockSprite.color = BlockData.normalColor;

        ChildBlockSprite.sprite = BlockData.normalBlock;
        ChildBlockSprite.color = BlockData.normalColor;*/

        limit = 1;

        isShined = false;
        isMoving = false;

        type = BlockData.blockType.normal;

        if (!Data.isAngled) //for mode trial
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if(Mathf.Approximately(this.transform.position.x, 0) && !isShined)
        {
            if(outlineSize < 10f)
            {
                outlineSize += 0.5f;
                foreach (SpriteRenderer sr in BlockSprites)
                {
                    sr.material.SetFloat("_OutlineSize", outlineSize);
                }    
            }
            else //if outlineSize >= 10
            {
                isShined = true;
                foreach (SpriteRenderer sr in BlockSprites)
                {
                    sr.material.SetFloat("_OutlineSize", 10);
                }    
            }
        }
    }

    public void SetBlock()
    {
        
        int r = Random.Range(2, 10);// 2 yaptım başını hiç ters olmasın diye
        //TODO: Create a random reverse generator that deals reverse positions 
        if (type == BlockData.blockType.reverse){
            BlockData.ChangeBlockType(ref type, BlockSprites);
        }
        else if (r < 2){
            BlockData.ChangeBlockType(ref type, BlockSprites);
        }
            
        foreach (SpriteRenderer sr in BlockSprites)
        {
            sr.material.SetFloat("_OutlineSize", 1);
        }
        outlineSize = BlockSprites[0].material.GetFloat("_OutlineSize");

        isShined = false;
    }

    public void MoveTile(float toPosition)
    {
        if(!isMoving)
        {
            isMoving = true;
            StartCoroutine(blockAnimation.MoveTile(toPosition));
        }
    }

    public void ChangeLimit(int num)
    {
        limit += num; 
    }
}
