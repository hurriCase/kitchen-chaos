using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounterAnimator : MonoBehaviour
{
    [SerializeField] private CuttingCounter _cuttingCounter;
    
    private const string Cut = "Cut";
        
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _cuttingCounter.OnCut += CuttingCounterOnCut;
    }

    private void CuttingCounterOnCut(object sender, EventArgs e)
    {
        _animator.SetBool(Cut, true);
    }
}
