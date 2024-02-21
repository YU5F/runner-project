using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> obstaclePrefabs;

    [SerializeField]
    private List<GameObject> additionalPrefab;
    private int checkCount = 1;
    private float[] spawnPointsX;
    private float spawnPointZ = 20f;
    public GroundSplit lanes;
    private ObjectPool objectPool;

    [SerializeField]
    [Range(10f, 100f)]
    private int maxObject = 30;
    private int activeObjects = 0;

    public enum ObstacleType
    {
        Low,
        Wall
    }

    void Start()
    {
        objectPool = FindObjectOfType<ObjectPool>();

        spawnPointsX = new float[lanes.laneAmount];

        for (int i = 0; i < spawnPointsX.Length; i++)
        {
            spawnPointsX[i] = lanes.GetLanePosition(i);
        }

        StartCoroutine(SpawnObstacles());
    }

    void Update()
    {
        if (activeObjects >= 30)
        {
            spawnPointZ += 10;
            ChangePattern();
            activeObjects = 0;
        }
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            if (objectPool.poolLength < maxObject)
            {
                int[] pattern = GenerateRandomPattern();

                for (int i = 0; i < spawnPointsX.Length; i++)
                {
                    int obstacleTypeIndex = pattern[i];

                    ObstacleType obstacleType = (ObstacleType)obstacleTypeIndex;

                    SpawnObstacle(obstacleType, i);

                    yield return new WaitForSeconds(0.9f);

                    if (checkCount < spawnPointsX.Length)
                    {
                        checkCount++;
                    }
                    else
                    {
                        checkCount = 1;
                        spawnPointZ += 10;
                    }
                    Debug.Log(checkCount);
                }
            }
            else
            {
                yield return new WaitForSeconds(0f);
            }
        }
    }

    void ChangePattern()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject value in obstaclePrefabs)
        {
            temp.Add(value);
        }

        obstaclePrefabs.Clear();
        foreach (GameObject value in additionalPrefab)
        {
            obstaclePrefabs.Add(value);
        }

        additionalPrefab.Clear();
        foreach (GameObject value in temp)
        {
            additionalPrefab.Add(value);
        }

        temp.Clear();
    }

    private int[] GenerateRandomPattern()
    {
        int[] pattern = new int[spawnPointsX.Length];

        pattern[0] = Random.Range(-1, obstaclePrefabs.Count);

        for (int i = 1; i < pattern.Length; i++)
        {
            int obstacleType = Random.Range(-1, obstaclePrefabs.Count);

            if (pattern[i - 1] == 1 && obstacleType == (int)ObstacleType.Wall)
            {
                obstacleType = Random.Range(-1, 1);
            }
            pattern[i] = obstacleType;
        }
        return pattern;
    }

    private void SpawnObstacle(ObstacleType type, int spawnPointIndex)
    {
        if ((int)type == -1)
        {
            return;
        }

        GameObject obstacle = objectPool.GetObject(obstaclePrefabs[(int)type]);
        float ySize = obstacle.GetComponent<Renderer>().bounds.size.y;

        obstacle.transform.position = new Vector3(spawnPointsX[spawnPointIndex], 0 + ySize / 2, spawnPointZ);
        activeObjects++;
    }
}
