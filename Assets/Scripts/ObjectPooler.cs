﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler instance;

    public struct ObjectPoolItem
    {
        public int amountToPool;
        public GameObject objectToPool;
        public bool shouldExpand;
        public string objectTag;
        public Transform parent;

    }

    //public List<GameObject> pooledObjects;
    // public GameObject objectToPool;
    //public int amountToPool;

    private void Awake()
    {
        instance = this;
    }


    /*void Start()
    {
        pooledObjects = new List<GameObject>();
        for(int i = 0; i< amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }*/

    public GameObject GetPooledObject(List<GameObject> pooledObjects)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public List<GameObject> PooltheObjects(ObjectPoolItem itemToPool) 
    {
        List<GameObject> pooledObjects;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < itemToPool.amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(itemToPool.objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        return pooledObjects;
    }

    public List<GameObject> PooltheObjects(ObjectPoolItem itemToPool,Transform parent)
    {
        List<GameObject> pooledObjects;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < itemToPool.amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(itemToPool.objectToPool,parent);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        return pooledObjects;
    }

}
