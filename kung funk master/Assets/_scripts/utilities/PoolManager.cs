using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;

    }
   

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;



    static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        InitPool();
    }
    public void InitPool()
    {
        
        foreach (Pool pool in pools)
        {
            GameObject holder = new GameObject(pool.tag + " holder");
            holder.transform.parent = transform;

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                obj.transform.parent = holder.transform;
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject ReuseObject(string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "not found");
            return null;
        }

        GameObject objectToReuse = poolDictionary[tag].Dequeue();

        objectToReuse.SetActive(true);
        objectToReuse.transform.position = pos;
        objectToReuse.transform.rotation = rot;

        IPoolInterface pooledobj = objectToReuse.GetComponent<IPoolInterface>();

        if (pooledobj != null)
        {
            pooledobj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToReuse);

        return objectToReuse;
    }
}
