using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour 
{
    GameObject[] _pooledObject;

    public void BeginPooling(GameObject objectToPool, int amount) 
    {
        _pooledObject = new GameObject[amount];
        for (int i = 0; i < amount; i++) 
        {
            _pooledObject[i] = Instantiate(objectToPool);
            _pooledObject[i].SetActive(false);
        }
    }

    public void BeginPooling(GameObject objectToPool, int amount, LayerMask layer)
    {
        _pooledObject = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            _pooledObject[i] = Instantiate(objectToPool);
            _pooledObject[i].layer = layer;
            _pooledObject[i].SetActive(false);
        }
    }

    public GameObject Fetch() 
    {
        GameObject fetchedObject = null;
        for (int i = 0; i < _pooledObject.Length; i++) 
        {
            if (!_pooledObject[i].activeInHierarchy) 
            {
                fetchedObject = _pooledObject[i];
                break;
            }
        }
        return fetchedObject;
    }
}
