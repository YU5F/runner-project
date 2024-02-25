using UnityEngine;

public class CollusionChecks : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Lose health
            Debug.Log("cart");
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Coin")){
            other.gameObject.SetActive(false);
            Debug.Log("coyin");
        }
    }
}
