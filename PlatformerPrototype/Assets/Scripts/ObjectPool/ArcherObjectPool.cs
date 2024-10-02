using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Rizos Eleftherios
public class ArcherObjectPool : MonoBehaviour
{
    public static ArcherObjectPool instance;

    private List<GameObject> pool = new List<GameObject>();
    private int itemsInPool = 10;

    [SerializeField]
    private GameObject arrow;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < itemsInPool; i++)
        {
            GameObject obj = Instantiate(arrow);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }

        }

        return null;
    }
}
