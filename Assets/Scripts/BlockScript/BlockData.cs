using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData
{
    public static Color normalColor = new Color32(159, 77, 77, 255);
    public static Color reverseColor = new Color32(75, 170, 120, 255);

    public static Sprite normalBlock = Resources.Load<Sprite>("Sprites/TapTapBlock2");//, typeof(Texture)) as Texture;
    public static Sprite reverseBlock = Resources.Load<Sprite>("Sprites/TapTapBlock2");

    public enum blockType
    {
        normal,
        reverse
    };

    //Changes block type whether its a normal block or reverse block
    public static void ChangeBlockType(ref blockType type,List<SpriteRenderer> blockSprites /*,SpriteRenderer blockSprite ,SpriteRenderer childBlockSprite*/)
    {
        if (type == blockType.normal)
        {
            type = blockType.reverse;

            foreach(SpriteRenderer sp in blockSprites)
            {
                sp.sprite = reverseBlock;
                sp.color = reverseColor;
            }

           /* blockSprite.sprite = reverseBlock;
            blockSprite.color = reverseColor;

            childBlockSprite.sprite = reverseBlock;
            childBlockSprite.color = reverseColor; */
        }
        else
        {
            type = blockType.normal;

            foreach (SpriteRenderer sp in blockSprites)
            {
                sp.sprite = normalBlock;
                sp.color = normalColor;
            }

           /* blockSprite.sprite = normalBlock;
            blockSprite.color = normalColor;

            childBlockSprite.sprite = normalBlock;
            childBlockSprite.color = normalColor; */
        }
    }
}


