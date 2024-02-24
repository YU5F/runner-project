using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int increaseAmount = 11;

    [SerializeField]
    [Range(1, 5)]
    private int scoreMultiplier = 1;

    [SerializeField]
    [Range(0, 100)]
    private float increaseScoreInterval = 1;
    private float patternChangeTimer = 0;

    void FixedUpdate()
    {
        currentScore = currentScore + increaseAmount * scoreMultiplier;
        Debug.Log(currentScore);
    }

    void Update()
    {
        patternChangeTimer += Time.deltaTime;

        if (patternChangeTimer >= 0.1 && scoreMultiplier < 5)
        {
            patternChangeTimer = 0;
            increaseScoreInterval += 3f / scoreMultiplier;

            if (increaseScoreInterval >= 100)
            {
                increaseScoreInterval = 1;
                scoreMultiplier++;
            }
        }
    }
}
