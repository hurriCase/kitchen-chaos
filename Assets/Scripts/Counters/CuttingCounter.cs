using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler OnCut;
    public event EventHandler<OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;

    public class OnCuttingProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }
    
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipes;
    private int _cuttingProgress;
    
    
    public override void Interact(Player player)
    {
        if (HasKitchenObject() is false
            && player.HasKitchenObject()
            && HasCuttingRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            _cuttingProgress = 0;
            OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs()
            {
                ProgressNormalized = _cuttingProgress
            });
        }
        
        else if (player.HasKitchenObject() is false && HasKitchenObject()) 
            GetKitchenObject().SetKitchenObjectParent(player);
    }

    public override void InteractAlternate()
    {
        if (HasKitchenObject() is false) return;
        
        var kitchenObject = GetKitchenObject().GetKitchenObjectSO();
        var cuttingRecipe = GetCuttingRecipeSO(kitchenObject);
        
        if (HasCuttingRecipe(kitchenObject) is false) return;
        
        _cuttingProgress++;
        
        OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs()
        {
            ProgressNormalized = (float)_cuttingProgress / cuttingRecipe.maxCutsToSlice
        });
        
        OnCut?.Invoke(this, EventArgs.Empty);
        
        if (_cuttingProgress != cuttingRecipe.maxCutsToSlice) return;
        
        var slicedKitchenObjectSO = GetOutputFromInput(kitchenObject);
        Destroy(GetKitchenObject().gameObject);
        KitchenObject.SetKitchenObject(slicedKitchenObjectSO, this);
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO kitchenObjectSO)
    {
        var cuttingRecipe = GetCuttingRecipeSO(kitchenObjectSO);
        
        if (cuttingRecipe != null) 
            return cuttingRecipe.output;
        return null;
    }

    private bool HasCuttingRecipe(KitchenObjectSO kitchenObjectSO)
    {
        var cuttingRecipe = GetCuttingRecipeSO(kitchenObjectSO);
        
        if (cuttingRecipe != null) 
            return true;
        return false;
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipe in _cuttingRecipes)
            if (cuttingRecipe.input == kitchenObjectSO)
                return cuttingRecipe;

        return null;
    }
}
