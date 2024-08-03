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
        else if (player.HasKitchenObject() is false) 
            GetKitchenObject().SetKitchenObjectParent(player);
    }
}
