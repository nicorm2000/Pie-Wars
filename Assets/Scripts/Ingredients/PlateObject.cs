using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateObject : IngredientObject
{
    [SerializeField] private List<IngredientsSO> validIngredients;
    private List<IngredientsSO> ingredientObjectSOList = new List<IngredientsSO>();

    public bool TryAddIngredient(IngredientsSO ingredient)
    {
        if (!validIngredients.Contains(ingredient))
            return false;
        if (ingredientObjectSOList.Contains(ingredient))
            return false;

        ingredientObjectSOList.Add(ingredient);
        return true;
    }
}
