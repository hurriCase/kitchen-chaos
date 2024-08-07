using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private Transform _ingredientIconTemplate;
    [SerializeField] private TextMeshProUGUI _recipeText;
    [SerializeField] private Transform _recipeIngredientList;

    private void Awake()
    {
        _ingredientIconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipe(RecipeSO recipeSO)
    {
        foreach (Transform child in _recipeIngredientList)
        {
            if (child == _ingredientIconTemplate)
                continue;
            
            Destroy(child.gameObject);
        }
        
        _recipeText.text = recipeSO.recipeName;

        foreach (var recipeIngredient in recipeSO.kitchenObjectsSOList)
        {
            var recipeIngredientIcon = Instantiate(_ingredientIconTemplate, _recipeIngredientList);
            recipeIngredientIcon.GetComponent<Image>().sprite = recipeIngredient.sprite;
            recipeIngredientIcon.gameObject.SetActive(true);
        }
        
    }
}
