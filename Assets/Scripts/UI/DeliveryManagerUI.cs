using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform _waitingRecipeList;
    [SerializeField] private Transform _recipeTemplate;
    

    private void Awake()
    {
        _recipeTemplate.gameObject.SetActive(false);
        
        UpdateVisual();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnWaitingRecipeAdded += DeliveryManagerOnWaitingRecipeAdded;
        DeliveryManager.Instance.OnWaitingRecipeCompleted += DeliveryManagerOnWaitingRecipeCompleted;
    }

    private void DeliveryManagerOnWaitingRecipeAdded(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManagerOnWaitingRecipeCompleted(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child  in _waitingRecipeList)
        {
            if (child == _recipeTemplate)
                continue;
            
            Destroy(child.gameObject);
        }

        foreach (var recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            var recipeTransform = Instantiate(_recipeTemplate, _waitingRecipeList);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipe(recipeSO);
        }
    }
}
