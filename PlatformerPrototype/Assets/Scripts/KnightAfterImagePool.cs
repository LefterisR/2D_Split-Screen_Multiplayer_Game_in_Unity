using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAfterImagePool : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject knightAfterImageEffect;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static KnightAfterImagePool Instance { get;private set; } //Singlenton

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool() 
    {
        for (int i = 0; i < 10; i++)
        {
            var newImage = Instantiate(knightAfterImageEffect);
            newImage.transform.SetParent(transform);
            AddToPool(newImage);
        }
        
    }

    public void AddToPool(GameObject image)
    {
        image.SetActive(false);
        availableObjects.Enqueue(image);
    }

    public GameObject GetFromPool()
    {
        if(availableObjects.Count == 0)
        {
            GrowPool();
        }
        GameObject image = availableObjects.Dequeue();
        image.SetActive(true);
        return image;
    }


}
