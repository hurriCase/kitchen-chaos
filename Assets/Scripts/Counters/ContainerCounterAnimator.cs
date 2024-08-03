using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimator : MonoBehaviour
{
    [SerializeField] private ContainerCounter _containerCounter;
    
    private const string OpenClose = "OpenClose";
        
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _containerCounter.OnContainerCounterOpen += ContainerCounterOpen;
    }

    private void ContainerCounterOpen(object sender, EventArgs e)
    {
        _animator.SetBool(OpenClose, true);
    }
}
