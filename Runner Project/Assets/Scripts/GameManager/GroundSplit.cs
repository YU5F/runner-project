using UnityEngine;

public class GroundSplit : MonoBehaviour
{
    public Transform groundObject;

    [Range(0, 5)]
    public int laneAmount;
    private float[] lanePositions;
    private readonly float scaleMultiplier = 8f;

    void Awake()
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
}
