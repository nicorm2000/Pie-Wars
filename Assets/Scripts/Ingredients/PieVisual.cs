using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieVisual : MonoBehaviour
{
    [Serializable]
    public struct IngredientSo_GameObject
    {
        public IngredientsSO ingredientSO;
        public GameObject gameObject;
    }
    [SerializeField] private PlateObject plateObject;
    [SerializeField] private List<IngredientSo_GameObject> ingredientSo_GameObjects;
    private void Awake()
    {
        plateObject.OnIngredientAdded += PlateVisual_OnIngredientAdded;

        foreach (IngredientSo_GameObject ingredient in ingredientSo_GameObjects)
        {
            ingredient.gameObject.SetActive(false);
        }
    }

    private void PlateVisual_OnIngredientAdded(object sender, PlateObject.OnIngredientAddedArgs e)
    {
        foreach (IngredientSo_GameObject ingredient in ingredientSo_GameObjects)
        {
            if (ingredient.ingredientSO == e.ingredientSO)
            {
                ingredient.gameObject.SetActive(true);
            }
        }
    }
}
