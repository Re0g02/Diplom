using UnityEngine;

public class AnimTest : MonoBehaviour
{
    private Animator animator;
    private bool animationState = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animationState = animationState ? false : true;
            animator.SetBool("IsMoving", animationState);
        }
    }
}
