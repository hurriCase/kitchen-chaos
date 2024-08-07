using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter _platesCounter;
    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;
    [SerializeField] private Transform _counterTopPoint;

    private List<GameObject> _platesVisualGameObject;

    private void Awake()
    {
        _platesVisualGameObject = new List<GameObject>();
    }

    private void Start()
    {
        _platesCounter.OnPlateSpawn += PlatesCounterOnPlateSpawn;
        _platesCounter.OnPlateRemove += PlatesCounterOnPlateRemove;
    }

    private void PlatesCounterOnPlateRemove(object sender, EventArgs e)
    {
        var plateGameObject = _platesVisualGameObject[^1];
        _platesVisualGameObject.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounterOnPlateSpawn(object sender, EventArgs e)
    {
        var playerVisualTransform = Instantiate(_plateKitchenObjectSO.prefab, _counterTopPoint);

        const float platesOffsetY = .1f;
        
        playerVisualTransform.localPosition = new Vector3(0, platesOffsetY * _platesVisualGameObject.Count, 0);

        _platesVisualGameObject.Add(playerVisualTransform.gameObject);
    }
}
