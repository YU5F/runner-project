using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private float distance;
    private float minDistance = 20f;
    [SerializeField][Range(0f, 50f)]private float moveSpeed = 10f;

    void Update(){
        distance = Vector3.Distance(playerTransform.position, transform.position);

        if(distance <= minDistance){
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }
}
