using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;

    public event EventHandler<IHasProgress.OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;
    
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipesSO;
    private int _cuttingProgress;
    
    
    public override void Interact(Player player)
    {
        if (HasKitchenObject() is false
            && player.HasKitchenObject()
            && HasCuttingRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            _cuttingProgress = 0;
            OnCuttingProgressChanged?.Invoke(this, new IHasProgress.OnCuttingProgressChangedEventArgs()
            {
                ProgressNormalized = _cuttingProgress
            });
        }
        else if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var plateKitchenObject) &&
                 HasKitchenObject())
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                GetKitchenObject().DestroySelf();
            else {}
        
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
        
        OnCuttingProgressChanged?.Invoke(this, new IHasProgress.OnCuttingProgressChangedEventArgs()
        {
            ProgressNormalized = (float)_cuttingProgress / cuttingRecipe.maxCutsToSlice
        });
        
        OnCut?.Invoke(this, EventArgs.Empty);
        
        if (_cuttingProgress != cuttingRecipe.maxCutsToSlice) return;
        
        var slicedKitchenObjectSO = GetOutputFromInput(kitchenObject);
        GetKitchenObject().DestroySelf();
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
        foreach (var cuttingRecipe in _cuttingRecipesSO)
            if (cuttingRecipe.input == kitchenObjectSO)
                return cuttingRecipe;

        return null;
    }
}
