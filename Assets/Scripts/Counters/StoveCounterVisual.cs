using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject _particleSystem;
    [SerializeField] private GameObject _visualGameObject;
    [SerializeField] private StoveCounter _stoveCounter;

    private void Start()
    {
        _stoveCounter.OnStateChanged += StoveCounterOnCook;
    }

    private void StoveCounterOnCook(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        var showVisual = e.State is (StoveCounter.State.Frying or StoveCounter.State.Cooked);
        _particleSystem.SetActive(showVisual);
        _visualGameObject.SetActive(showVisual);
    }
}
