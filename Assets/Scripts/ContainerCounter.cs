using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnContainerCounterOpen;
    
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) return;
        Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        OnContainerCounterOpen?.Invoke(this, EventArgs.Empty);
    }
}
