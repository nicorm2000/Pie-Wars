using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateObject : IngredientObject
{
    public event EventHandler<OnIngredientAddedArgs> OnIngredientAdded;
    public class OnIngredientAddedArgs : EventArgs
    {
        public IngredientsSO ingredientSO;
    }

    [SerializeField] private List<IngredientsSO> validIngredients;
    private List<IngredientsSO> ingredientObjectSOList = new List<IngredientsSO>();
    private bool isCompleted = false;
    private int pieIngredientsQuantity = 3;

    public bool TryAddIngredient(IngredientsSO ingredient)
    {
        if (!validIngredients.Contains(ingredient))
            return false;
        if (ingredientObjectSOList.Contains(ingredient))
            return false;

        ingredientObjectSOList.Add(ingredient);
        if (ingredientObjectSOList.Count == pieIngredientsQuantity)
        {
            isCompleted = true;
            Debug.Log("Completed Pie");
        }

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedArgs
        {
            ingredientSO = ingredient
        });
        return true;
    }

    public bool GetPieStatus()
    {
        return isCompleted;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCompleted)
            return;

        //If collides with enemy player, sums points
    }
}
