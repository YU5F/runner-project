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
        if (objectPool.poolLength < maxObject)
        {
            int[] pattern = GenerateRandomPattern();

            for (int i = 0; i < spawnPointsX.Length; i++)
            {
                int obstacleTypeIndex = pattern[i];
                SpawnObstacle(obstacleTypeIndex, i);
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

        if(MapGeneration.patternIndex == 2){
            for(int i = 0; i < pattern.Length; i++){
                pattern[i] = 0;
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

        GameObject obstacle = objectPool.GetObject(currentPatternObjects[(int)type]);
        obstacle.SetActive(true);
        float ySize = obstacle.GetComponent<MeshRenderer>().bounds.size.y;

        if(obstacle.gameObject.name == "MovingObstacle"){
            spawnPointZ += 20;
        }

        obstacle.transform.position = new Vector3(
            spawnPointsX[spawnPointIndex],
            0 + ySize / 2,
            spawnPointZ
        );

        activeObjects++;
    }
}
