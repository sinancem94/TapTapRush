using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBoostObjects
{

    //Objects used for boost 
    private GameObject BoreDummy; //Enabled when boost starts. Normal runners sprite is disabled and this dummy placed into its place to make it look like runner is stopped.
    private GameObject RoadDummy; //Create a dummy road with sprites. Starting from BoreDummy.transform.position minus few blocks and reaches real roads first block. 


   /* public DummyBoostObjects(GameObject brDum,GameObject rdDum)
    {
        brDum = BoreDummy;
        rdDum = RoadDummy;
    }*/

    public GameObject CreateDummy(RunnerAnimation boreBoostEffects)
    {
        BoreDummy = new GameObject
        {
            name = "RunnerDummy"
        };

        SpriteRenderer d_sprite = BoreDummy.AddComponent<SpriteRenderer>();
        d_sprite.sprite = boreBoostEffects.spriteRenderer.sprite;
        d_sprite.material = boreBoostEffects.spriteRenderer.material;
        d_sprite.sortingLayerID = boreBoostEffects.spriteRenderer.sortingLayerID;

        Animator d_anim = BoreDummy.AddComponent<Animator>();
        d_anim.runtimeAnimatorController = boreBoostEffects.animator.runtimeAnimatorController;

        BoreDummy.transform.localScale = Platform.instance.Runner.transform.localScale;
        BoreDummy.transform.rotation = Platform.instance.Runner.transform.rotation;

        BoreDummy.SetActive(false);

        return BoreDummy;
    }

    public void SetDummy()
    {
        BoreDummy.transform.position = Platform.instance.Runner.transform.position;
        BoreDummy.SetActive(true);
        BoreDummy.GetComponent<Animator>().SetFloat("speed", 10f);
    }

    public void DisableDummy()
    {
        BoreDummy.SetActive(false);
    }

    public GameObject CreateRoadDummy()
    {
        RoadDummy = new GameObject
        {
            name = "RoadReplica"
        };

        RoadDummy.SetActive(false);

        return RoadDummy;
    }

    public void SetRoadDummy()
    {
        //First calculate the distance between last block (pushBlockForward) and dummy bore.
        float roadLength = Platform.instance.platfotmTiles[Platform.instance.pushBlockForward].transform.position.y - BoreDummy.transform.position.y + (5 * Platform.instance.distBetweenBlock);
        //Then calculate how many blocks will be needed in RoadReplica
        int blockCount = (int)(roadLength / Platform.instance.distBetweenBlock);
        //Calculate first blockPos
        Vector3 blockPos = new Vector2(0f, Platform.instance.platfotmTiles[Platform.instance.pushBlockForward].transform.position.y - (Platform.instance.distBetweenBlock * blockCount));

        for (int i = 0; i < blockCount; i++)
        {
            GameObject b = GameObject.Instantiate(Platform.instance.block, RoadDummy.transform);
            b.transform.localScale = Platform.instance.blockScale;
            b.transform.position = blockPos;

            blockPos.y += Platform.instance.distBetweenBlock;
        }
        RoadDummy.SetActive(true);
    }

    public void DestoryRoadDummy()
    {
        foreach (SpriteRenderer s in RoadDummy.GetComponentsInChildren<SpriteRenderer>())
        {
            GameObject.Destroy(s.gameObject);
        }
        RoadDummy.SetActive(false);
    }

}
