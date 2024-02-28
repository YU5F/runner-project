using UnityEngine;

public class ChangeTrigger : MonoBehaviour
{
    private Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void TriggerSideRollAnim(){
        animator.SetTrigger("IsChangingLane");
    }
}
