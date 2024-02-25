using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnObject : MonoBehaviour
{
    private ObjectPool objectPool;
    private GameObject player;

    [SerializeField]
    [Range(0f, 20f)]
    private float maxDistance = 3f;

    void Start()
    {
        objectPool = GameObject.Find("GameManager").GetComponent<ObjectPool>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (transform.position.z + maxDistance < player.transform.position.z)
        {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (objectPool != null)
        {
            objectPool.ReturnGameObject(this.gameObject);
        }
    }
}
