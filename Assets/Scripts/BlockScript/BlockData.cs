using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData
{
    public Color normalColor = new Color32(0, 0, 0, 255);
    public Color reverseColor = Color.green;//new Color32(75, 170, 120, 255);

    public Sprite normalBlock = Resources.Load<Sprite>("Sprites/TapTapBlock2");//, typeof(Texture)) as Texture;
    public Sprite reverseBlock = Resources.Load<Sprite>("Sprites/TapTapBlock2");

    public enum blockType
    {
        normal,
        reverse
    };

    //Changes block type whether its a normal block or reverse block
    public void ChangeBlockType(ref blockType type,SpriteRenderer blockSprite /*,SpriteRenderer blockSprite ,SpriteRenderer childBlockSprite*/)
    {
        if (type == blockType.normal)
        {
            type = blockType.reverse;

            blockSprite.sprite = reverseBlock;
            blockSprite.color = reverseColor;
      
           /* blockSprite.sprite = reverseBlock;
            blockSprite.color = reverseColor;

            childBlockSprite.sprite = reverseBlock;
            childBlockSprite.color = reverseColor; */
        }
        else
        {
            type = blockType.normal;

            blockSprite.sprite = normalBlock;
            blockSprite.color = normalColor;

           /* blockSprite.sprite = normalBlock;
            blockSprite.color = normalColor;

            childBlockSprite.sprite = normalBlock;
            childBlockSprite.color = normalColor; */
        }
    }
}


