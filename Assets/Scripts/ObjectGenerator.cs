using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator
{
    ObjectPooler.ObjectPoolItem eye;

    public ObjectPooler.ObjectPoolItem block;


    public ObjectGenerator(Transform blockParent,Transform eyeParent)
    {
        eye.objectToPool = Resources.Load<GameObject>("Prefabs/Eyes");
        eye.parent = eyeParent;
        eye.amountToPool = 5;
        eye.shouldExpand = false;
        eye.objectTag = eye.objectToPool.tag;

        block.objectToPool = blockParent.GetChild(0).gameObject;
        block.parent = blockParent;
        block.amountToPool = 30;
        block.shouldExpand = true;
        block.objectTag = block.objectToPool.tag;
    }

    public void GenerateObjects(ref List<GameObject> Blocks, ref List<GameObject> Eyes, ref GameObject checkPointZone,ref GameObject roadReplica ,int BlockCount = 0)
    {
        Eyes = ObjectPooler.instance.PooltheObjects(eye, eye.parent); //Pool 5 eyes

        if(BlockCount != 0)
            block.amountToPool = BlockCount;  //if block amount is NOT empty (0) change amount to that

        Blocks = ObjectPooler.instance.PooltheObjects(block, block.parent); //Pool blocks

        //Generate CheckPoint
        GenerateCheckPoint(ref checkPointZone);
        roadReplica = GenerateRoadReplica();
    }


    void GenerateCheckPoint(ref GameObject cp)
    {
        GameObject CPBOrder = new GameObject("CPBORDER");

        SpriteRenderer borderRenderer = CPBOrder.AddComponent<SpriteRenderer>();
        borderRenderer.sortingLayerID = block.objectToPool.GetComponent<SpriteRenderer>().sortingLayerID;
        borderRenderer.sprite = Resources.Load<Sprite>("Sprites/Square");

        ObjectPooler.instance.PooltheObjects(CPBOrder, 2, cp.transform); //First Set Borders
        GameObject.Destroy(CPBOrder);

        block.amountToPool = 5; //Change amount to pool for checkpoint blocks. 
        ObjectPooler.instance.PooltheObjects(block, cp.transform, true);
    }

    GameObject GenerateRoadReplica()
    {
        GameObject repRoad = new GameObject
        {
            name = "RoadReplica"
        };

        ObjectPooler.instance.PooltheObjects(block.objectToPool, 10, repRoad.transform,true);

        repRoad.SetActive(false);

        return repRoad;
    }
}
