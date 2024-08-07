using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnWaitingRecipeAdded;
    public event EventHandler OnWaitingRecipeCompleted;
    
    [SerializeField] private RecipeListSO _recipeSOList;
    [SerializeField] private int _maxSpawnRecipeTime = 4;
    [SerializeField] private int _maxRecipeAmount = 4;

    private List<RecipeSO> _waitingRecipeSOList;
    private float _timerForNextRecipe;

    private void Awake()
    {
        _waitingRecipeSOList = new List<RecipeSO>();
        Instance = this;
    }

    private void Update()
    {
        _timerForNextRecipe += Time.deltaTime;
        
        if (_timerForNextRecipe >= _maxSpawnRecipeTime is false) 
            return; 
        
        _timerForNextRecipe = 0f;

        if (_waitingRecipeSOList.Count >= _maxRecipeAmount) return;
        
        _waitingRecipeSOList.Add(_recipeSOList.recipeSOList[Random.Range(0, _recipeSOList.recipeSOList.Count)]);

        OnWaitingRecipeAdded?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        var properRecipe = false;
        
        foreach (var waitingRecipe in _waitingRecipeSOList)
        { 
            properRecipe = plateKitchenObject
                               .GetKitchetObjectSOList()
                               .All(kitchenObjectSO => waitingRecipe.kitchenObjectsSOList.Contains(kitchenObjectSO)) 
                           && waitingRecipe
                               .kitchenObjectsSOList.Count == plateKitchenObject
                               .GetKitchetObjectSOList()
                               .Count;

            if (!properRecipe) continue;
            
            _waitingRecipeSOList.Remove(waitingRecipe);
            break;
        }

        if (properRecipe)
            OnWaitingRecipeCompleted?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return _waitingRecipeSOList;
    }
}
