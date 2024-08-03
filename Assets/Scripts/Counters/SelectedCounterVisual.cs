using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjectArray;
    
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerOnOnSelectedCounterChanged;
    }

    private void PlayerOnOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.SelectedCounter == _baseCounter) Show();
        else Hide();
    }

    private void Show()
    {
        foreach (var visualGameObject in _visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
        
    }
    
    private void Hide()
    {
        foreach (var visualGameObject in _visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
