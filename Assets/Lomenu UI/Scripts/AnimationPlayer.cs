using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public Animator animator;

    public void PlayFromBegining(string animationName)
    {
        if (animator == null)
        {
            return;
        }

        animator.Play(animationName, 0, 0f);
    }
}

