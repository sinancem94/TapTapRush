using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public ObjectGenerator Generator;

    Transform EyeParent;
    public List<GameObject> Eyes;

    Transform BlockParent;
    public List<GameObject> Blocks;

    public ObjectManager(int BlockCount = 0)
    {
        BlockParent = GameObject.FindWithTag("Blocks").transform;
        EyeParent = GameObject.FindWithTag("Eyes").transform;

        Generator = new ObjectGenerator(BlockParent,EyeParent);

        Generator.GenerateObjects(ref Blocks,ref Eyes);
    }


    public List<GameObject> SetBlocks(float yDistance ,float[] blockPosArray,bool isFiveLine,ref float blockYPos,ref int straightRoad)
    {
        int StartStraightLine = 5; // start from third block to give full road at the beginning at level
        float distance = -5f;

        for (int i = 0; i < StartStraightLine; i++)
        {
            distance += yDistance;
            Blocks[i].transform.position = new Vector2(0f, distance);
            Blocks[i].SetActive(true);
        }

        //Platform tiles da buluncak toplam blok sayısından ilk baştaki düz blokları çıkar 
        //int remainingBlock = Generator.block.amountToPool - StartStraightLine;

        for (int i = StartStraightLine; i < Generator.block.amountToPool; i++)
        {
            distance += yDistance;
            Vector2 blockPos = new Vector2(blockPosArray[BlockPosition(blockPosArray.Length, isFiveLine)], distance);
            Blocks[i].transform.position = blockPos;
            Blocks[i].SetActive(true);
        }

        straightRoad = StartStraightLine;
        blockYPos = distance;
        return Blocks;
    }


    private int currBlockPos = 3;
    private int sameLineLength = 0;

    // blokları konumlandıran fonksiyon
    public int BlockPosition(int maxValue,bool isFiveLine)
    {
        int tempEx = currBlockPos;

        currBlockPos = MathCalculation.RandomPosition(currBlockPos, sameLineLength, maxValue, isFiveLine);
        sameLineLength = (tempEx == currBlockPos) ? sameLineLength += 1 : sameLineLength = 0;

        return currBlockPos;
    }

    public void CreateEye()
    {
        GameObject eye = ObjectPooler.instance.GetPooledObject(Eyes);
        eye.SetActive(true);
    }

}
