using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : IManager
{
    public List<GameObject> poolList = new List<GameObject>();

    public void InsertPool(GameObject thisObject)
    {
        thisObject.gameObject.SetActive(false);
        poolList.Add(thisObject);
    }

    public GameObject RecyclePool(string objectname, Vector3 position, Quaternion rotation) 
    {
        for(int i = 0; i < poolList.Count; i++)
        {
            if(poolList[i].gameObject.name == objectname && !poolList[i].activeSelf)
            {
                poolList[i].gameObject.SetActive(true);
                poolList[i].gameObject.transform.position = position;
                poolList[i].gameObject.transform.rotation = rotation;
                return poolList[i].gameObject;
            }
        }

        return null;
    }

    public void Init()
    {
       
    }

    public void OnUpdate()
    {
       
    }

    public void Clear()
    {
        poolList.Clear();
    }

}
