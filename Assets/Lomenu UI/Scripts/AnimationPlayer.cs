using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public Animator animator;

    public void PlayFromBegining(string animationName)
    {
        Debug.Log("Animation Player called for " + name);
        if (animator == null)
        {
            return;
        }

        Debug.Log("triggering animation " + animationName);

        animator.Play(animationName, 0, 0f);
    }
}

