using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    private struct KitchenObjectSOGameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private List<KitchenObjectSOGameObject> _kitchenObjectSoGameObjectsList;
    
    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
        
        foreach (var kitchenObjectSoGameObject in _kitchenObjectSoGameObjectsList)
        {
            kitchenObjectSoGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var kitchenObjectSoGameObject in _kitchenObjectSoGameObjectsList)
        {
            if (kitchenObjectSoGameObject.kitchenObjectSO == e.KitchenObjectSO) 
                kitchenObjectSoGameObject.gameObject.SetActive(true);
        }
    }
}
