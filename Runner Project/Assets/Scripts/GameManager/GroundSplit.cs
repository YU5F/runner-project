using UnityEngine;

public class GroundSplit : MonoBehaviour
{
    public Transform groundObject;

    [Range(0, 5)]
    public int laneAmount = 3;

    private float[] lanePositions;
    public float scaleMultiplier = 8f;

    void Start()
    {
        //Split ground object into 3 diffrent sections and assign section positions to an array
        float laneWidth = groundObject.localScale.x / laneAmount;
        lanePositions = new float[laneAmount];

        for (int i = 0; i < laneAmount; i++)
        {
            float laneCenterX =
                groundObject.position.x + (i - (laneAmount - 1) / 2f) * laneWidth * scaleMultiplier;
            lanePositions[i] = laneCenterX;
        }
    }

    public float GetLanePosition(int laneIndex)
    {
        if (laneIndex >= 0 && laneIndex < laneAmount)
        {
            return lanePositions[laneIndex];
        }
        else
        {
            Debug.LogError("Invalid lane index");
            return -10;
        }
    }

    void Update()
    {
        Debug.Log(GetLanePosition(0) + " " + GetLanePosition(1) + " " + GetLanePosition(2));
    }
}
