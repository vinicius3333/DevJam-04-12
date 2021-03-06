﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private Animator animator;
    public bool isFadeComplete;
    private void Start() 
    {
        animator = GetComponent<Animator>();
        FadeOut();
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
