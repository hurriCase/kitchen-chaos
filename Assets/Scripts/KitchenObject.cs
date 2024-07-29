using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }
    
    public void SetKitchenObjectParent(IKitchenObjectParent kicKitchenObjectParent)
    {
        if (_kitchenObjectParent != null) _kitchenObjectParent.ClearKitchenObject();

        _kitchenObjectParent = kicKitchenObjectParent;

        if (kicKitchenObjectParent.HasKitchenObject()) Debug.LogError("KitchenObjectParent already has a KitchenObject!");
        
        kicKitchenObjectParent.SetKitchenObject(this);
        
        transform.parent = kicKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }
}
