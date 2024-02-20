using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public static bool pCheck = false;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(transform.parent.gameObject, 1f);
        }
    }
}
