using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IIngredientObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField] private Transform counterTopPoint;

    private IngredientObject ingredientObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate()");
    }

    public Transform GetIngredientObjectFollowTranform()
    {
        return counterTopPoint;
    }

    public void SetIngredientObject(IngredientObject ingredientObject)
    {
        this.ingredientObject = ingredientObject;

        if (ingredientObject != null) 
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public IngredientObject GetIngredientObject()
    {
        return ingredientObject;
    }

    public void ClearIngredientObject()
    {
        ingredientObject = null;
    }

    public bool HasIngredientObject()
    {
        return ingredientObject != null;
    }
}