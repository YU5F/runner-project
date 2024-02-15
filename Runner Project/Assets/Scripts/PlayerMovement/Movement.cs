using UnityEditor.Callbacks;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private InputController input = null;

    [SerializeField]
    [Range(0f, 50f)]
    private float changeLineSpeed;

    [SerializeField]
    [Range(0f, 50f)]
    private float moveSpeed;

    [SerializeField]
    private GroundSplit laneManager;

    [SerializeField]
    private Rigidbody playerRb;
    private int currentLane = 1;
    private float direction;
    private bool isMoving = false;
    private float targetPositionX;

    void Update()
    {
        direction = input.RetrieveHorizontalInput();

        if (direction < 0 && currentLane > 0 && !isMoving)
        {
            MoveToLane(currentLane - 1);
        }
        if (direction > 0 && currentLane < laneManager.laneAmount - 1 && !isMoving)
        {
            MoveToLane(currentLane + 1);
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(targetPositionX, transform.position.y, transform.position.z),
                changeLineSpeed * Time.deltaTime
            );
            if (transform.position.x == targetPositionX)
            {
                isMoving = false;
            }
        }
    }

    void FixedUpdate(){
        Vector3 forwardMove = transform.forward * moveSpeed * Time.fixedDeltaTime;
        playerRb.MovePosition(playerRb.position + forwardMove);
    }

    void MoveToLane(int laneIndex)
    {
        isMoving = true;
        currentLane = laneIndex;
        targetPositionX = laneManager.GetLanePosition(laneIndex);
    }
}
