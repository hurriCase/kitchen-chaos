using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (HasKitchenObject() is false) 
            if (player.HasKitchenObject())
                player.GetKitchenObject().SetKitchenObjectParent(this);
            else {}
        
        else if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                GetKitchenObject().DestroySelf();
        } 
        
        else if (player.HasKitchenObject() is false) 
            GetKitchenObject().SetKitchenObjectParent(player);
        
        else if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                player.GetKitchenObject().DestroySelf();
        }
    }
}
