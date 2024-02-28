using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> currentPatternObjects;
    public List<GameObject> nextPatternObjects;
    private int checkCount = 1;
    private float[] spawnPointsX;
    private float spawnPointZ = 20f;
    public GroundSplit lanes;
    public GameObject rampObject;
    private ObjectPool objectPool;

    [SerializeField]
    [Range(10f, 100f)]
    private int maxObject = 30;
    private int activeObjects = 0;
    private static float spaceAmount = 10f;
    float spawnObstacleInterval = 0;

    void Start()
    {
        objectPool = FindObjectOfType<ObjectPool>();

        spawnPointsX = new float[lanes.laneAmount];

        for (int i = 0; i < spawnPointsX.Length; i++)
        {
            spawnPointsX[i] = lanes.GetLanePosition(i);
        }
    }

    void Update()
    {
        SpawnObstacles();
        UpdatePattern();
    }

    private void SpawnObstacles()
    {
        spawnObstacleInterval += Time.deltaTime;

        if (spawnObstacleInterval >= 0.01f)
        {
            spawnObstacleInterval = 0;
            if (objectPool.poolLength < maxObject)
            {
                int[] pattern = GenerateRandomPattern();

                for (int i = 0; i < spawnPointsX.Length; i++)
                {
                    int obstacleTypeIndex = pattern[i];

                    SpawnObstacle(obstacleTypeIndex, i);

                    PutGapBetweenPatterns(spaceAmount);
                }
            }
        }
    }

    private void PutGapBetweenPatterns(float addZAxis)
    {
        if (checkCount < spawnPointsX.Length)
        {
            checkCount++;
        }
        else
        {
            checkCount = 1;
            spawnPointZ += addZAxis;
        }
    }

    private void UpdatePattern()
    {
        if (activeObjects >= MapGeneration.maxPatternObject)
        {
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
            activeObjects = 0;
        }
    }

    private int[] GenerateRandomPattern()
    {
        int[] pattern = new int[spawnPointsX.Length];

        if (
            MapGeneration.patternIndex == 1
            && MapGeneration.patternIndex == 2
            && MapGeneration.patternIndex == 3
        )
        {
            for (int i = 1; i < pattern.Length; i++)
            {
                int obstacleType = Random.Range(-1, currentPatternObjects.Count);
                if (pattern[i - 1] == -1 && obstacleType == -1)
                {
                    obstacleType = 0;
                }
                pattern[i] = obstacleType;
            }
            return pattern;
        }

        pattern[0] = Random.Range(-1, currentPatternObjects.Count);

        for (int i = 1; i < pattern.Length; i++)
        {
            int obstacleType = Random.Range(-1, currentPatternObjects.Count);

            if (pattern[i - 1] == 1 && obstacleType == 1)
            {
                obstacleType = Random.Range(-1, 1);
            }
            pattern[i] = obstacleType;
        }
        return pattern;
    }

    private void SpawnObstacle(int type, int spawnPointIndex)
    {
        if (type == -1)
        {
            return;
        }

        GameObject obstacle = objectPool.GetObject(currentPatternObjects[type]);
        obstacle.SetActive(true);
        float ySize = obstacle.GetComponent<MeshRenderer>().bounds.size.y;

        if (obstacle.CompareTag("Ramp"))
        {
            GameObject ramp = objectPool.GetObject(rampObject);
            ramp.SetActive(true);
            ramp.transform.position = new Vector3(0, 2.5f, spawnPointZ + 23);
        }

        if (obstacle.CompareTag("Obstacle"))
        {
            spaceAmount = 15f;
        }

        if (obstacle.name == "LowObstacle")
        {
            obstacle.transform.GetChild(0).Rotate(Vector3.up, Random.Range(0f, 360f), Space.Self);
            obstacle.transform.position += Vector3.up * .5f;
        }

        if (obstacle.name == "MovingObstacle")
        {
            spawnPointZ += 10f;
        }

        if (obstacle.CompareTag("Coin"))
        {
            obstacle.transform.position += new Vector3(0f, .5f, 0f);
            spawnPointZ += 2f;
            spaceAmount = 2f;
        }

        obstacle.transform.position = new Vector3(
            spawnPointsX[spawnPointIndex],
            0 + ySize / 2,
            spawnPointZ
        );

        activeObjects++;

        if (activeObjects >= MapGeneration.maxPatternObject)
        {
            spaceAmount = 10f;
        }
    }
}
