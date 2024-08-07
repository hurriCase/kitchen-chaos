using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlateRemove;
    
    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;
    [SerializeField] private int _maxPlates;
    [SerializeField] private float _timeForPlateSpawn;
    
    private float _plateSpawnTimer;
    private int _platesSpawnAmount;

    private void Update()
    {
        _plateSpawnTimer += Time.deltaTime;
        
        if ((_plateSpawnTimer >= _timeForPlateSpawn) is false) return;
        _plateSpawnTimer = 0;
        
        if (_platesSpawnAmount >= _maxPlates) return;
            
        OnPlateSpawn?.Invoke(this, EventArgs.Empty);
        _platesSpawnAmount++;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() || _platesSpawnAmount <= 0) return;
        
        _platesSpawnAmount--;
        KitchenObject.SetKitchenObject(_plateKitchenObjectSO, player);
        
        OnPlateRemove?.Invoke(this, EventArgs.Empty);
    }
}
