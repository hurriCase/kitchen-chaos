using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO ;
    [SerializeField] private Transform _counterTopPoint;
    
    private KitchenObject _kitchenObject;

    public void Interact(Player player)
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab, _counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            
        }
        else _kitchenObject.SetKitchenObjectParent(player);
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }
    
    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }
 
    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
