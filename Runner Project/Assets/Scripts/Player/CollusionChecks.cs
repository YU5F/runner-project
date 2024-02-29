using System.Collections;
using UnityEngine;

public class CollusionChecks : MonoBehaviour
{
    public Material playerMaterial;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            PlayerHealth.DecreaseHealth();
            collision.gameObject.SetActive(false);
            ScoreManager.Instance.scoreMultiplier = 1;
            ScoreManager.Instance.increaseMultiplierInterval = 1;
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

    IEnumerator BlinkCharacterMaterial(){
        
        yield return new WaitForSeconds(3f);
    }
}
