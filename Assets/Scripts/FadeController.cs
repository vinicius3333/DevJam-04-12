using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private Animator animator;
    public bool isFadeComplete;
    public bool isFadeInComplete;
    private void Start() 
    {
        animator = GetComponent<Animator>();
    }

    public void OnFadeComplete() //chamado na animacao
    {
        isFadeComplete = true;
    }

    public void FadeIn()
    {
        isFadeComplete = false;
        animator.SetTrigger("fadeIn");
    }

    public void FadeOut()
    {
        isFadeComplete = false;
        animator.SetTrigger("fadeOut");
    }
}
