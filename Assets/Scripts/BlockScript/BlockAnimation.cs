using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAnimation : MonoBehaviour {



    public IEnumerator Fall(Vector2 distance)
    {
        this.transform.Translate(distance, Space.World);
        Vector3 xshrink = new Vector2(-0.1f, 0f);
        Vector3 yshrink = new Vector2(0f, -0.1f);

        while (this.transform.localScale.x > 0)
        {
            this.transform.localScale += xshrink;
            yield return new WaitForSecondsRealtime(.02f);
            if (this.transform.localScale.x <= 0.5f)
            {
                this.transform.localScale += yshrink;
            }
        }

        this.gameObject.SetActive(false);
    }

    public IEnumerator MoveTile(float toPosition)
    {
        float distance = toPosition - this.transform.position.x;
        Vector3 translationVector = new Vector3(distance, 0f, 0f);

        bool slided = false;

        int from = (distance < 0) ? -1 : 1;

        while(!slided)
        {
			
            this.transform.position += translationVector * 0.15f;
            distance = toPosition - this.transform.position.x;

            if((from == 1 && distance < toPosition ) || (from == -1 && distance > toPosition))
            {
                slided = true;
            }

            yield return new WaitForSeconds(0.01f);
        }
        this.transform.position = new Vector3(0f,transform.position.y,0f);

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
