using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    } 
    
    [SerializeField] private KitchenObjectSO[] _validKitchenObjectsSOArray; 
    
    private List<KitchenObjectSO> _kitchenObjectsSOList;

    private void Awake()
    {
        _kitchenObjectsSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        
        if (_validKitchenObjectsSOArray.Contains(kitchenObjectSO) is false 
            || _kitchenObjectsSOList.Contains(kitchenObjectSO)) 
            return false;
        
        _kitchenObjectsSOList.Add(kitchenObjectSO);
        
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs()
        {
            KitchenObjectSO = kitchenObjectSO
        });
        
        return true;
    }

    public List<KitchenObjectSO> GetKitchetObjectSOList()
    {
        return _kitchenObjectsSOList;
    }
}
