using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private CuttingCounter _cuttingCounter;

    private void Start()
    {
        _cuttingCounter.OnCuttingProgressChanged += CuttingCounterOnCuttingProgressChanged;
        
        Hide();
    }

    private void CuttingCounterOnCuttingProgressChanged(object sender, CuttingCounter.OnCuttingProgressChangedEventArgs e)
    {
        if (e.ProgressNormalized is 0f or 1f) Hide();
        else Show();
        
        _progressBar.fillAmount = e.ProgressNormalized;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
