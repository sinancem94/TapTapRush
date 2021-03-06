﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAnimation : MonoBehaviour {

    private Block block;

    private void Start()
    {
        block = this.GetComponent<Block>();
    }

    private float MoveTowards(float to)
    {
        return Mathf.MoveTowards(transform.position.x, to, 0.1f);
    }

    public IEnumerator Fall(Vector2 toPos,bool RollBack)
    {
       
        Vector3 initScale = this.transform.localScale;
        Vector3 initPos = this.transform.position;

        Vector3 xshrink = new Vector2(-0.1f, 0f);
        Vector3 yshrink = new Vector2(0f, -0.1f);
        

        while (this.transform.localScale.x > 0)
        {
            if(!Mathf.Approximately(transform.position.x,toPos.x))
                this.transform.Translate(toPos * Time.deltaTime * 3, Space.World);

            this.transform.localScale += xshrink;
            yield return new WaitForSecondsRealtime(.02f);
            if (this.transform.localScale.x <= 0.5f)
            {
                this.transform.localScale += yshrink;
            }
        }

        if(!RollBack)
            this.gameObject.SetActive(false);
        else //
        {
            while (this.transform.localScale.x < initScale.x)
            {
                if (!Mathf.Approximately(transform.position.x, initPos.x))
                    this.transform.Translate(-toPos * Time.deltaTime * 3, Space.World);

                this.transform.localScale -= xshrink;
                yield return new WaitForSecondsRealtime(.02f);
                if (this.transform.localScale.x >= 0.5f)
                {
                    this.transform.localScale -= yshrink / 2;
                }
            }

            transform.position = initPos;
            transform.localScale = initScale;
        }
    }

    public IEnumerator MoveTile(float toPosition)
    {
        float distance = toPosition - this.transform.position.x;
        Vector3 translationVector = new Vector3(distance, 0f, 0f);

        bool slided = false;

        int from = (distance < 0) ? -1 : 1;
        while(!slided)
        {
            //Debug.Log(transform.position + "  " + slided + " " + gameObject.name + "  " + distance);
            //this.transform.Translate(translationVector * Time.deltaTime);
            this.transform.position += translationVector * 0.15f;


            distance = toPosition - this.transform.position.x;

            if((from == 1 && distance <= 0 ) || (from == -1 && distance >= 0))
            {
                slided = true;
            }
            //Debug.Log(transform.position + "  " + slided);
            yield return new WaitForSeconds(0.01f);
        }
        this.transform.position = new Vector3(toPosition,transform.position.y,0f);

        block.isMoving = false;
        StopCoroutine(MoveTile(toPosition));
    }

    /*private Sprite blockSprite;

	void Start () 
    {
        blockSprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(Fall(Vector2.zero));
	}*/


   /* void ChangeSprite()
    {
        Vector2[] spriteVertices = blockSprite.vertices;

        for (int i = 0; i < spriteVertices.Length; i++)
        {
            Debug.Log(i);
            spriteVertices[i].x = Mathf.Clamp(
                (blockSprite.vertices[i].x - blockSprite.bounds.center.x -
                 (blockSprite.textureRectOffset.x / blockSprite.texture.width) + blockSprite.bounds.extents.x) /
                (2.0f * blockSprite.bounds.extents.x) * blockSprite.rect.width,
                0.0f, blockSprite.rect.width);

            spriteVertices[i].y = Mathf.Clamp(
                (blockSprite.vertices[i].y - blockSprite.bounds.center.y -
                     (blockSprite.textureRectOffset.y / blockSprite.texture.height) + blockSprite.bounds.extents.y) /
                (2.0f * blockSprite.bounds.extents.y) * blockSprite.rect.height,
                0.0f, blockSprite.rect.height);
            
            // Make a small change to the second vertex
            if (i == 3)
            {
                //Make sure the vertices stay inside the Sprite rectangle
                if (spriteVertices[i].x < blockSprite.rect.size.x - .1f)
                {
                    spriteVertices[i].x = spriteVertices[i].x + 0.1f;
                }
                else spriteVertices[i].x = 0;

                if(spriteVertices[0].y < blockSprite.rect.size.y + .1f)
                {
                    spriteVertices[0].y = spriteVertices[0].y - 0.1f;
                }
                else spriteVertices[0].y = 0;
            }
        }
        //Override the geometry with the new vertices
        blockSprite.OverrideGeometry(spriteVertices, blockSprite.triangles);
    }*/
}
