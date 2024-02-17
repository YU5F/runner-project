using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject lowObstaclePrefab;

    [SerializeField]
    private GameObject wallObstaclePrefab;

    [SerializeField]
    [Range(0f, 10f)]
    private float timeBetweenPatterns = 3f;
    private int patternLength = 3;
    private float[] spawnPointsX;
    private static float spawnPointZ = 10f;
    public GroundSplit lanes;

    private List<GameObject> activeObstacles = new List<GameObject>();

    void Start()
    {
        spawnPointsX = new float[3];

        for (int i = 0; i <= 2; i++)
        {
            spawnPointsX[i] = lanes.GetLanePosition(i);
        }

        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
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
                yield return new WaitForSeconds(0f);
                spawnPointZ += 20;
            }
            yield return null;
        }
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
        GameObject obstacle = Instantiate(
            lowObstaclePrefab,
            new Vector3(spawnPointsX[spawnPointIndex], 0, spawnPointZ),
            Quaternion.identity
        );
        activeObstacles.Add(obstacle);
    }

    private void SpawnWallObstacle(int spawnPointIndex)
    {
        GameObject obstacle = Instantiate(
            wallObstaclePrefab,
            new Vector3(spawnPointsX[spawnPointIndex], 0, spawnPointZ),
            Quaternion.identity
        );
        activeObstacles.Add(obstacle);
    }
}
