using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField][Range(0f, 100f)] private float rotationSpeed = 10f;

    void Update(){
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
