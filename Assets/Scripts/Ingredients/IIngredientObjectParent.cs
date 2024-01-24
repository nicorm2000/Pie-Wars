using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIngredientObjectParent
{
    public Transform GetIngredientObjectFollowTranform();

    public void SetIngredientObject(IngredientObject ingredientObject);

    public IngredientObject GetIngredientObject();

    public void ClearIngredientObject();

    public bool HasIngredientObject();
}