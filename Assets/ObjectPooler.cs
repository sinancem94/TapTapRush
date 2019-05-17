using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler instance;

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

    public List<GameObject> PooltheObjects(GameObject objectToPool, int amountToPool) 
    {
        List<GameObject> pooledObjects;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        return pooledObjects;
    }
}
