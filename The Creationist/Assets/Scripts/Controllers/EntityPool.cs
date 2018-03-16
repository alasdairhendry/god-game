using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPool : MonoBehaviour {

    public static EntityPool singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    private Dictionary<int, EntityPoolData> poolingData = new Dictionary<int, EntityPoolData>();

    public void CreatePoolData(int ID, string resourcePath, int preloadCount, Action onPool, Action onDepool)
    {
        EntityPoolData data = new EntityPoolData(resourcePath, preloadCount, onPool, onDepool);
        poolingData.Add(ID, data);
    }

    public void Preload()
    {
        foreach (KeyValuePair<int, EntityPoolData> data in poolingData)
        {
            data.Value.Preload();
        }
    }

    public GameObject Instantiate(int ID)
    {
        GameObject go = poolingData[ID].Dequeue();

        if(go == null)
        {
            // Spawn new
            Debug.Log("No entities in pool - Creating New");
            return null;
        }
        else
        {
            go.transform.parent = null;
            go.SetActive(true);
            return go;
        }        
    }

    public void Destroy(int ID, GameObject go)
    {
        go.GetComponent<IPoolable>().OnDestroy();
        poolingData[ID].Enqueue(go);
        go.SetActive(false);
        go.transform.parent = GameObject.Find("ObjectPool").transform;
    }

    public class EntityPoolData
    {
        private Queue<GameObject> pool = new Queue<GameObject>();        

        private string resourcePath = "";
        private int preloadCount = 0;

        private Action onPool;
        private Action onDepool;

        public EntityPoolData(string resourcePath, int preloadCount, Action onPool, Action onDepool)
        {
            this.resourcePath = resourcePath;
            this.preloadCount = preloadCount;
            this.onPool = onPool;
            this.onDepool = onDepool;
        }

        public void Preload()
        {
            for (int i = 0; i < preloadCount; i++)
            {                
                GameObject go = Instantiate(Resources.Load(resourcePath)) as GameObject;
                go.transform.parent = GameObject.Find("ObjectPool").transform;
                pool.Enqueue(go);
                go.SetActive(false);
            }
        }

        private void CreateNew()
        {
            GameObject go = Instantiate(Resources.Load(resourcePath)) as GameObject;
            go.transform.parent = GameObject.Find("ObjectPool").transform;
            pool.Enqueue(go);
            go.SetActive(false);            
        }

        public void Enqueue(GameObject go)
        {
            pool.Enqueue(go);
        }

        public GameObject Dequeue()
        {
            if (pool.Count <= 0)
            {
                Debug.Log("Pool Count ID " + resourcePath + " empty. Creating New Object.");
                CreateNew();
            }

            GameObject go = pool.Dequeue();
            go.GetComponent<IPoolable>().OnInstantiate();
            return go;
        }
    }
}
