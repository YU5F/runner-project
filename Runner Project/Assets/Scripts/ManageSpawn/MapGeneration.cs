using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    private static Dictionary<string, GameObject> patternObjects =
        new Dictionary<string, GameObject>();

    public enum PatternTypes
    {
        ObstaclePattern = 0,
        CoinPattern = 1,
        RampPattern = 2,
        IncomingObstaclesPattern = 3
    }

    private static int prevPattern = -1;
    public static int maxPatternObject;
    public static int patternIndex;

    void Awake()
    {
        GameObject objects = GameObject.Find("PatternObjects");

        patternObjects.Add("LowObstacle", objects.transform.Find("LowObstacle").gameObject);
        patternObjects.Add("WallObstacle", objects.transform.Find("WallObstacle").gameObject);
        patternObjects.Add("Coin", objects.transform.Find("Coin").gameObject);
        patternObjects.Add("Ramp", objects.transform.Find("Ramp").gameObject);
        patternObjects.Add("MovingObstacle", objects.transform.Find("MovingObstacle").gameObject);
    }

    public static List<GameObject> GeneratePattern()
    {
        List<GameObject> pattern = new List<GameObject>();

        patternIndex = Random.Range(0, System.Enum.GetValues(typeof(PatternTypes)).Length);

        while (patternIndex == prevPattern)
        {
            patternIndex = Random.Range(0, System.Enum.GetValues(typeof(PatternTypes)).Length);
        }
        
        switch (patternIndex)
        {
            case (int)PatternTypes.ObstaclePattern:
                pattern.Add(patternObjects["LowObstacle"]);
                pattern.Add(patternObjects["WallObstacle"]);
                maxPatternObject = 30;
                break;
            case (int)PatternTypes.CoinPattern:
                pattern.Add(patternObjects["Coin"]);
                maxPatternObject = 20;
                break;
            case (int)PatternTypes.RampPattern:
                pattern.Add(patternObjects["Ramp"]);
                maxPatternObject = 10;
                break;
            case (int)PatternTypes.IncomingObstaclesPattern:
                pattern.Add(patternObjects["MovingObstacle"]);
                maxPatternObject = 5;
                break;
        }

        prevPattern = patternIndex;

        return pattern;
    }
}
