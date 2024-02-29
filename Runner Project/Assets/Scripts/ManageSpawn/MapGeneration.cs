using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    private static Dictionary<string, GameObject> patternObjects =
        new Dictionary<string, GameObject>();

    private static int prevPattern = -1;
    private static int nonObstacleCheck = 0;
    public static int maxPatternObject;
    public static int patternIndex;

    public enum PatternTypes
    {
        ObstaclePattern = 0,
        CoinPattern = 1,
        RampPattern = 2,
        IncomingObstaclesPattern = 3
    }

    void Awake()
    {
        GameObject objects = GameObject.Find("PatternObjects");

        patternObjects.Clear();

        patternObjects.Add("LowObstacle", objects.transform.Find("LowObstacle").gameObject);
        patternObjects.Add("WallObstacle", objects.transform.Find("WallObstacle").gameObject);
        patternObjects.Add("Coin", objects.transform.Find("Coin").gameObject);
        patternObjects.Add("Ramp", objects.transform.Find("Ramp").gameObject);
        patternObjects.Add("MovingObstacle", objects.transform.Find("MovingObstacle").gameObject);
        patternObjects.Add("HoleObstacle", objects.transform.Find("HoleObstacle").gameObject);
    }

    public static List<GameObject> GeneratePattern()
    {
        List<GameObject> pattern = new List<GameObject>();

        patternIndex = Random.Range(0, System.Enum.GetValues(typeof(PatternTypes)).Length);

        while (patternIndex == prevPattern)
        {
            patternIndex = Random.Range(0, System.Enum.GetValues(typeof(PatternTypes)).Length);
        }

        if (patternIndex != (int)PatternTypes.ObstaclePattern && nonObstacleCheck >= 2)
        {
            nonObstacleCheck = 0;
            patternIndex = (int)PatternTypes.ObstaclePattern;
        }

        if (patternIndex != (int)PatternTypes.ObstaclePattern)
        {
            nonObstacleCheck++;
        }

        switch (patternIndex)
        {
            case (int)PatternTypes.ObstaclePattern:
                pattern.Add(patternObjects["LowObstacle"]);
                pattern.Add(patternObjects["WallObstacle"]);
                pattern.Add(patternObjects["HoleObstacle"]);
                maxPatternObject = 15;
                nonObstacleCheck = 0;
                break;
            case (int)PatternTypes.CoinPattern:
                pattern.Add(patternObjects["Coin"]);
                maxPatternObject = 25;
                break;
            case (int)PatternTypes.RampPattern:
                pattern.Add(patternObjects["Ramp"]);
                maxPatternObject = 1;
                break;
            case (int)PatternTypes.IncomingObstaclesPattern:
                pattern.Add(patternObjects["MovingObstacle"]);
                maxPatternObject = 3;
                break;
        }

        prevPattern = patternIndex;

        return pattern;
    }
}
