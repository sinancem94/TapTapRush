using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public ObjectGenerator Generator;

    Transform EyeParent;
    List<GameObject> Eyes;

    Transform BlockParent;
    List<GameObject> Blocks;

    GameObject CheckPointZone; // 5 blocks to be used on checkpoint buffer zones and starting of game.
    GameObject RoadReplica;

    public ObjectManager(int BlockCount = 0)
    {
        BlockParent = GameObject.FindWithTag("Blocks").transform;
        EyeParent = GameObject.FindWithTag("Eyes").transform;
        CheckPointZone = GameObject.FindWithTag("CheckPointZone");

        Generator = new ObjectGenerator(BlockParent,EyeParent);

        Generator.GenerateObjects(ref Blocks,ref Eyes,ref CheckPointZone,ref RoadReplica);
    }

    public float SetCheckPoint(float firstBlockPos, float yDistance, Vector2 b_scale)
    {
        //Set First border of Checkpoint
        Transform firstBorder = CheckPointZone.transform.GetChild(0).transform;

        firstBorder.localPosition = new Vector2(0f, firstBlockPos - (yDistance / 2f));
        firstBorder.localScale = new Vector2(b_scale.x * 1.8f, b_scale.x / 28f);
        firstBorder.gameObject.SetActive(true);

        foreach (Block block in CheckPointZone.GetComponentsInChildren<Block>()) //Set blocks of checkpoint
        {

            block.transform.localPosition = new Vector2(0f, firstBlockPos);
            block.InitiliazeBlock(b_scale);
            firstBlockPos += yDistance;
        }

        firstBlockPos -= yDistance; //since its incremented in loop decrease it before sending parameter to platform.

        //Set second border of checkpoint
        Transform secondBorder = CheckPointZone.transform.GetChild(1).transform;

        secondBorder.localPosition = new Vector2(0f, firstBlockPos + (yDistance /2f));
        secondBorder.localScale = new Vector2(b_scale.x * 1.8f, b_scale.x / 28f);
        secondBorder.gameObject.SetActive(true);

        return firstBlockPos;
    }

    public float MoveCheckPoint(float toPos, float yDistance)
    {
        float checkPointSize = CheckPointZone.GetComponentsInChildren<Block>().Length;

        CheckPointZone.transform.position = new Vector2(0f, toPos + ( (checkPointSize - 1) * yDistance) );

        return CheckPointZone.transform.position.y;
    }

    public List<GameObject> SetNewLevelBlocks(float yDistance, float[] blockPosArray, bool isFiveLine, ref float blockYPos,Vector2 bScale)
    {
        float tmpBlockYPos = blockYPos;

        SetRoadReplica(blockYPos, yDistance, bScale);

        blockYPos = tmpBlockYPos;

        SetBlocks(yDistance, blockPosArray, isFiveLine,ref blockYPos);

        return Blocks;
    }

    void SetRoadReplica(float blockYPos,float yDistance,Vector2 bScale)
    {
        RoadReplica.SetActive(true);

        blockYPos -= 5 * yDistance;

        foreach(Block b in RoadReplica.GetComponentsInChildren<Block>())
        {
            blockYPos -= yDistance;

            Vector2 pos = new Vector2(0f, blockYPos);
            b.transform.position = pos;

            b.InitiliazeBlock(bScale, Platform.instance.levelManager.levelBlockType, true);
        }
    }

    public List<GameObject> SetBlocks(float yDistance ,float[] blockPosArray,bool isFiveLine,ref float blockYPos)
    {
        //Platform tiles da buluncak toplam blok sayısından ilk baştaki düz blokları çıkar 
        //int remainingBlock = Generator.block.amountToPool - StartStraightLine;
        currBlockPos = 3;
        sameLineLength = 0;

        for (int i = 0; i < Blocks.Count; i++)
        {
            Vector2 blockPos = new Vector2(blockPosArray[BlockPosition(blockPosArray.Length, isFiveLine)], blockYPos);
            Blocks[i].transform.position = blockPos;
            Blocks[i].SetActive(true);

            blockYPos += yDistance;
        }

        return Blocks;
    }

    public List<Block> InitiliazeBlockScripts(Vector2 blockScale ,LevelManager.LevelBlockType BlockType)
    {
        List<Block> blockScripts = new List<Block>();

        foreach (GameObject block in Blocks)
        {
            blockScripts.Add(block.GetComponent<Block>());
            blockScripts[blockScripts.Count - 1].InitiliazeBlock(blockScale, BlockType);
        }

        return blockScripts;
    }


    private int currBlockPos;
    private int sameLineLength;

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
