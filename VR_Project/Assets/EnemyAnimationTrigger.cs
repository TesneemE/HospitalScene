using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Animator anim;

    void OnEnable()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        anim.Play("Mutant Breathing Idle", 0, 0f); // Replace "Walk" with your Mixamo animation clip name
    }
}
