using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private Animator animator;
    private string scaredAnimationName = "Ghostscaredan";
    private string scaredtransformAnimationName = "Ghostscaredtransforman";
    private string frontAnimationName = "Ghost1frontan";
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayScaredAnimation(float fadeDuration = 0.1f)
    {
        animator.CrossFade(scaredAnimationName, fadeDuration);
    }
    public void PlayScaredTransformAnimation(float fadeDuration = 0.1f)
    {
        animator.CrossFade(scaredtransformAnimationName, fadeDuration);
    }

    public void PlayNormalAnimation(float fadeDuration = 0.1f)
    {
        animator.CrossFade(frontAnimationName, fadeDuration);
    }

   
}
