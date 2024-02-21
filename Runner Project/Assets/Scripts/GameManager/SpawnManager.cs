using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> currentPatternObjects;

    [SerializeField]
    private List<GameObject> nextPatternObjects;
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
        StartCoroutine(ChangePattern());
    }

    void Update()
    {
        //     if (activeObjects >= 30)
        //     {
        //         spawnPointZ += 10;
        //         ChangePattern(MapGeneration.GeneratePattern());
        //         activeObjects = 0;
        //     }
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
                    activeObjects++;

                    yield return new WaitForSeconds(0f);

                    if (checkCount < spawnPointsX.Length)
                    {
                        checkCount++;
                    }
                    else
                    {
                        checkCount = 1;
                        spawnPointZ += 10;
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(0f);
            }
        }
    }

    IEnumerator ChangePattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            List<GameObject> patternObjects = MapGeneration.GeneratePattern();

            nextPatternObjects.Clear();
            foreach (GameObject value in patternObjects)
            {
                nextPatternObjects.Add(value);
            }

            currentPatternObjects.Clear();
            foreach (GameObject value in nextPatternObjects)
            {
                currentPatternObjects.Add(value);
            }
        }
    }

    private int[] GenerateRandomPattern()
    {
        int[] pattern = new int[spawnPointsX.Length];

        pattern[0] = Random.Range(-1, currentPatternObjects.Count);

        for (int i = 1; i < pattern.Length; i++)
        {
            int obstacleType = Random.Range(-1, currentPatternObjects.Count);

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

        GameObject obstacle = objectPool.GetObject(currentPatternObjects[(int)type]);
        obstacle.SetActive(true);
        float ySize = obstacle.GetComponent<Renderer>().bounds.size.y;

        obstacle.transform.position = new Vector3(
            spawnPointsX[spawnPointIndex],
            0 + ySize / 2,
            spawnPointZ
        );
    }
}
