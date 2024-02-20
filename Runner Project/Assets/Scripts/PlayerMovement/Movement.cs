using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables

    //Inputs
    [SerializeField]
    private InputController input = null;
    private float direction;
    private bool isJumping;
    private bool rollInput = false;

    //Player properties
    [SerializeField]
    [Range(0f, 50f)]
    private float changeLineSpeed;

    [SerializeField]
    [Range(0f, 50f)]
    private float moveSpeed;

    [SerializeField]
    [Range(0f, 20f)]
    private float jumpForce = 10f;

    [SerializeField]
    [Range(0f, 5f)]
    private float rollDuration = 2f;

    [SerializeField]
    [Range(1f, 100f)]
    private float landSpeed = 50f;

    //Checks
    [SerializeField]
    private GroundSplit laneManager;
    private bool isMoving = false;
    private bool isRolling = false;

    [SerializeField]
    private Rigidbody playerRb;

    [SerializeField]
    private Collider playerCollider;

    private int currentLane = 1;
    private float targetPositionX;

    #endregion

    void Update()
    {
        direction = input.RetrieveHorizontalInput();
        isJumping = input.RetrieveJumpInput();
        rollInput = input.RetrieveRollInput();

        ChangeLane(direction);

        if (isJumping)
        {
            Jump();
        }

        if (rollInput)
        {
            if (IsGrounded())
            {
                Roll();
            }
            else
            {
                StartCoroutine(Land());
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * moveSpeed * Time.fixedDeltaTime;
        playerRb.MovePosition(playerRb.position + forwardMove);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z);
        }
    }

    void Roll()
    {
        if (!isRolling)
        {
            isRolling = true;
            StartCoroutine(StartRoll());
        }
    }

    IEnumerator Land()
    {
        Roll();

        float targetHeight = GetGroundHeight();
        while (transform.position.y > targetHeight)
        {
            float newY = Mathf.MoveTowards(
                transform.position.y,
                targetHeight,
                Time.deltaTime * landSpeed
            );
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }

        playerRb.velocity = Vector3.zero;
    }

    IEnumerator StartRoll()
    {
        Vector3 playerScale = transform.localScale;
        transform.localScale = new Vector3(playerScale.x, playerScale.y / 2f, playerScale.z);

        yield return new WaitForSeconds(rollDuration);

        transform.localScale = playerScale;
        isRolling = false;
    }

    float GetGroundHeight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            return hit.point.y + playerCollider.bounds.extents.y;
        }
        return transform.position.y;
    }

    bool IsGrounded()
    {
        float offset = 0.1f;
        float rayLength = playerCollider.bounds.extents.y + offset;
        return Physics.Raycast(transform.position, Vector3.down, rayLength);
    }

    void ChangeLane(float input)
    {
        if (input < 0 && currentLane > 0 && !isMoving)
        {
            ChangeTargetPosition(currentLane - 1);
        }
        if (direction > 0 && currentLane < laneManager.laneAmount - 1 && !isMoving)
        {
            ChangeTargetPosition(currentLane + 1);
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

    void ChangeTargetPosition(int laneIndex)
    {
        isMoving = true;
        currentLane = laneIndex;
        targetPositionX = laneManager.GetLanePosition(laneIndex);
    }
}
