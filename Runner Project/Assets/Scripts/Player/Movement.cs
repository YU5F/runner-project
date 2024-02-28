using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables

    //Inputs
    [SerializeField]
    private InputController input = null;

    [SerializeField]
    private Animator playerAnimator;
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

    [SerializeField]
    [Range(0.1f, 10f)]
    private float nearMissDistance = 2f;
    private float nearMissRayLength;
    private float nearMissMaxRayLength = 10f;

    //Checks
    [SerializeField]
    private GroundSplit laneManager;
    private bool isMoving = false;
    private bool isRolling = false;

    [SerializeField]
    private Rigidbody playerRb;

    [SerializeField]
    private CapsuleCollider playerCollider;

    private int currentLane = 1;
    private float targetPositionX;
    private float increaseSpeedTimer = 0;
    private float increaseSpeedInterval = 5f;

    #endregion

    void Start()
    {
        nearMissRayLength = nearMissMaxRayLength;
    }

    void Update()
    {
        playerRb.velocity = new Vector3(0, playerRb.velocity.y, moveSpeed);

        IncreaseSpeed();

        direction = input.RetrieveHorizontalInput();
        isJumping = input.RetrieveJumpInput();
        rollInput = input.RetrieveRollInput();

        ChangeLane(direction);

        if (isJumping)
        {
            Jump();
        }

        if (IsGrounded())
        {
            playerAnimator.SetBool("IsGrounded", true);
            playerAnimator.SetBool("IsJumping", false);
        }
        else
        {
            playerAnimator.SetBool("IsGrounded", false);
            playerAnimator.SetBool("IsJumping", true);
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

    void IncreaseSpeed()
    {
        increaseSpeedTimer += Time.deltaTime;
        if (increaseSpeedTimer >= increaseSpeedInterval)
        {
            increaseSpeedTimer = 0;
            moveSpeed += 0.24f;
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z);
        }
    }

    private bool CheckIfNearMiss()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.forward;
        Vector3 rayOrigin =
            transform.position + (Vector3.down * playerCollider.bounds.extents.y / 2);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, nearMissRayLength))
        {
            float distance = Vector3.Distance(rayOrigin, hit.point);
            Debug.DrawRay(rayOrigin, rayDirection * distance, Color.green);

            if (hit.collider.CompareTag("Obstacle"))
            {
                nearMissRayLength = distance;
            }

            if (distance < nearMissDistance && hit.collider.CompareTag("Obstacle"))
            {
                NearMiss();
                return true;
            }
        }
        else
        {
            nearMissRayLength = nearMissMaxRayLength;
        }
        return false;
    }

    private void NearMiss()
    {
        ScoreManager.Instance.IncreaseMultiplierInterval(20f);
        Debug.Log("near miss");
    }

    void Roll()
    {
        if (!isRolling)
        {
            isRolling = true;
            playerAnimator.SetBool("IsRolling", true);
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
        playerCollider.height /= 2;

        yield return new WaitForSeconds(rollDuration);

        playerCollider.height *= 2;

        isRolling = false;
        playerAnimator.SetBool("IsRolling", false);
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
        CheckIfNearMiss();
        playerAnimator.SetTrigger("IsChangingLane");
        isMoving = true;
        currentLane = laneIndex;
        targetPositionX = laneManager.GetLanePosition(laneIndex);
    }
}
