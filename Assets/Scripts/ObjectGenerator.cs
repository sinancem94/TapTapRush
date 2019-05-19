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

    public void GenerateObjects(ref List<GameObject> Blocks, ref List<GameObject> Eyes,int BlockCount = 0)
    {
        Eyes = ObjectPooler.instance.PooltheObjects(eye, eye.parent);

        if(BlockCount != 0)
            block.amountToPool = BlockCount;

        Blocks = ObjectPooler.instance.PooltheObjects(block, block.parent);
    }
}
