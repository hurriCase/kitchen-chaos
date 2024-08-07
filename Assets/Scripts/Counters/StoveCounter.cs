using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State
    {
        Idle,
        Frying,
        Cooked,
        Burned,
    }
    
    public event EventHandler<IHasProgress.OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State State;
    }
    
    [SerializeField] private FryingRecipeSO[] _fryingRecipesSOArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipesSOArray;

    private FryingRecipeSO _fryingRecipeSO;
    private float _fryingTimer;
    private BurningRecipeSO _burningRecipeSO;
    private float _burningTimer;
    
    
    private State _state;
    
    public override void Interact(Player player)
    {
        if (HasKitchenObject() is false
            && player.HasKitchenObject()
            && HasFryingRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            _fryingTimer = 0f;
            _state = State.Frying;
            _fryingRecipeSO = GetFryingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
            
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
            {
                State = _state
            });
        }
        else if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var plateKitchenObject) && HasKitchenObject())
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();
                _state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                {
                    State = _state
                });
            
                OnCuttingProgressChanged?.Invoke(this, new IHasProgress.OnCuttingProgressChangedEventArgs()
                {
                    ProgressNormalized = 0f
                });
            }
                
            
        } 
        
        else if (player.HasKitchenObject() is false && HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            _state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
            {
                State = _state
            });
            
            OnCuttingProgressChanged?.Invoke(this, new IHasProgress.OnCuttingProgressChangedEventArgs()
            {
                ProgressNormalized = 0f
            });
        }
            
    }

    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {

        switch (_state)
        {
            case State.Idle:
                break;
            
            case State.Frying:
                _fryingTimer += Time.deltaTime;
                
                OnCuttingProgressChanged?.Invoke(this, new IHasProgress.OnCuttingProgressChangedEventArgs()
                {
                    ProgressNormalized = _fryingTimer / _fryingRecipeSO.fryingTimeMax
                });
                
                if (_fryingTimer >= _fryingRecipeSO.fryingTimeMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SetKitchenObject(_fryingRecipeSO.output, this);
                    _state = State.Cooked;
                    _burningTimer = 0f;
                    _burningRecipeSO = GetBurnedRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                    {
                        State = _state
                    });
                    
                    
                }
                break;
            
            case State.Cooked:
                _burningTimer += Time.deltaTime;
                
                OnCuttingProgressChanged?.Invoke(this, new IHasProgress.OnCuttingProgressChangedEventArgs()
                {
                    ProgressNormalized = _burningTimer / _burningRecipeSO.burningTimeMax
                });
                
                if (_burningTimer >= _burningRecipeSO.burningTimeMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SetKitchenObject(_burningRecipeSO.output, this);
                    _state = State.Burned;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                    {
                        State = _state
                    });
                    
                }
                    
                break;
            
            case State.Burned:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool HasFryingRecipe(KitchenObjectSO kitchenObjectSO)
    {
        var fryingRecipe = GetFryingRecipeSO(kitchenObjectSO);
        
        if (fryingRecipe != null) 
            return true;
        return false;
    }
    
    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var fryingRecipe in _fryingRecipesSOArray)
            if (fryingRecipe.input == kitchenObjectSO)
                return fryingRecipe;

        return null;
    }
    
    private BurningRecipeSO GetBurnedRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var burningRecipe in _burningRecipesSOArray)
            if (burningRecipe.input == kitchenObjectSO)
                return burningRecipe;

        return null;
    }
}
