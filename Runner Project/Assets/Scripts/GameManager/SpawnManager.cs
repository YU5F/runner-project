using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject lowObstaclePrefab;

    [SerializeField]
    private GameObject wallObstaclePrefab;
    private int checkCount = 0;
    private float[] spawnPointsX;
    private float spawnPointZ = 20f;
    public GroundSplit lanes;
    private int maxObject = 29;
    private ObjectPool objectPool;

    void Start()
    {
        objectPool = FindObjectOfType<ObjectPool>();

        spawnPointsX = new float[3];

        for (int i = 0; i <= 2; i++)
        {
            spawnPointsX[i] = lanes.GetLanePosition(i);
        }

        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        // if (maxObject > objectPool.poolLength)
        // {
            int[] pattern = GenerateRandomPattern();

            for (int i = 0; i < spawnPointsX.Length; i++)
            {
                int obstacleType = pattern[i];

                if (obstacleType == 1)
                {
                    SpawnLowObstacle(i);
                }
                else if (obstacleType == 2)
                {
                    SpawnWallObstacle(i);
                }

                yield return new WaitForSeconds(0.1f);

                if (checkCount < spawnPointsX.Length)
                {
                    checkCount++;
                }
                else
                {
                    checkCount = 0;
                    spawnPointZ += 10;
                }
            }
            StartCoroutine(SpawnObstacles());
            yield return null;
    //    }
    }

    private int[] GenerateRandomPattern()
    {
        int[] pattern = new int[spawnPointsX.Length];

        pattern[0] = Random.Range(0, 3);

        for (int i = 1; i < spawnPointsX.Length; i++)
        {
            int obstacleType = Random.Range(0, 3);

            if (pattern[i - 1] == 2 && obstacleType == 2)
            {
                obstacleType = Random.Range(0, 2);
            }
            pattern[i] = obstacleType;
        }
        return pattern;
    }

    private void SpawnLowObstacle(int spawnPointIndex)
    {
        GameObject obstacle = objectPool.GetObject(lowObstaclePrefab);
        BoxCollider obstacleCollider = obstacle.GetComponent<BoxCollider>();
        obstacle.transform.position = new Vector3(
            spawnPointsX[spawnPointIndex],
            obstacleCollider.size.y * 0.5f * 1.8f,
            spawnPointZ
        );
    }

    private void SpawnWallObstacle(int spawnPointIndex)
    {
        GameObject obstacle = objectPool.GetObject(wallObstaclePrefab);
        BoxCollider obstacleCollider = obstacle.GetComponent<BoxCollider>();
        obstacle.transform.position = new Vector3(
            spawnPointsX[spawnPointIndex],
            obstacleCollider.size.y * 0.5f * 4,
            spawnPointZ
        );
    }
}
