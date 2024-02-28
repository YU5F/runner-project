using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    public static int coinAmount = 0;
    private int increaseAmount = 11;

    [SerializeField]
    [Range(1, 5)]
    private int scoreMultiplier = 1;

    [SerializeField]
    [Range(0, 100)]
    public float increaseMultiplierInterval = 1;
    private float patternChangeTimer = 0;
    private int maxMultiplier = 5;

    public static ScoreManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void FixedUpdate()
    {
        currentScore += increaseAmount * scoreMultiplier;
    }

    void Update()
    {
        patternChangeTimer += Time.deltaTime;

        if (patternChangeTimer >= 0.1 && scoreMultiplier < maxMultiplier)
        {
            patternChangeTimer = 0;
            increaseMultiplierInterval += 1f / scoreMultiplier;

            if (increaseMultiplierInterval >= 100)
            {
                increaseMultiplierInterval = 1;
                scoreMultiplier++;
            }
        }
    }

    public void IncreaseMultiplierInterval(float amount)
    {
        if (scoreMultiplier < maxMultiplier)
        {
            increaseMultiplierInterval += amount;
        }
    }
}
