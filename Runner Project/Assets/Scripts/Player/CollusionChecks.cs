using UnityEngine;

public class CollusionChecks : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            PlayerHealth.DecreaseHealth();
            collision.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            ScoreManager.Instance.coinAmount++;
        }
    }
}
