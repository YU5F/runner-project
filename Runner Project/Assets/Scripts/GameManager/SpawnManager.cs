using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] obstaclePrefabs;
    private int checkCount = 0;
    private float[] spawnPointsX;
    private float spawnPointZ = 20f;
    public GroundSplit lanes;
    private ObjectPool objectPool;

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

    private IEnumerator SpawnObstacles()
    {
        // if (maxObject > objectPool.poolLength)
        // {
        int[] pattern = GenerateRandomPattern();

        for (int i = 0; i < spawnPointsX.Length; i++)
        {
            int obstacleTypeIndex = pattern[i];

            ObstacleType obstacleType = (ObstacleType)obstacleTypeIndex;

            SpawnObstace(obstacleType, i);

            yield return new WaitForSeconds(0f);

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
        //    }
    }

    private int[] GenerateRandomPattern()
    {
        int[] pattern = new int[spawnPointsX.Length];

        pattern[0] = Random.Range(-1, obstaclePrefabs.Length);

        for (int i = 1; i < pattern.Length; i++)
        {
            int obstacleType = Random.Range(-1, obstaclePrefabs.Length);

            if (pattern[i - 1] == 1 && obstacleType == 1)
            {
                obstacleType = Random.Range(-1, 1);
            }
            pattern[i] = obstacleType;
        }
        return pattern;
    }

    private void SpawnObstace(ObstacleType type, int spawnPointIndex)
    {
        if((int)type == -1){
            return;
        }

        GameObject obstacle = objectPool.GetObject(obstaclePrefabs[(int)type]);
        BoxCollider obstacleCollider = obstacle.GetComponent<BoxCollider>();

        float yOffsetMultiplier = 1.0f;

        switch (type)
        {
            case ObstacleType.Low:
                yOffsetMultiplier = 1.8f;
                break;
            case ObstacleType.Wall:
                yOffsetMultiplier = 4.0f;
                break;
        }

        obstacle.transform.position = new Vector3(
            spawnPointsX[spawnPointIndex],
            obstacleCollider.size.y * 0.5f * yOffsetMultiplier,
            spawnPointZ
        );
    }
}
