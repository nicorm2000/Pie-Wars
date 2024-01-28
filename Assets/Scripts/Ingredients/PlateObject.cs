using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateObject : IngredientObject
{
    public event EventHandler<OnIngredientAddedArgs> OnIngredientAdded;

    int playerThrower;
    public class OnIngredientAddedArgs : EventArgs
    {
        public IngredientsSO ingredientSO;
    }

    [SerializeField] private List<IngredientsSO> validIngredients;
    public List<IngredientsSO> ingredientObjectSOList = new List<IngredientsSO>();
    public bool isCompleted = false;
    [SerializeField] public int pieIngredientsQuantity = 2;

    public bool TryAddIngredient(IngredientsSO ingredient)
    {
        if (!validIngredients.Contains(ingredient))
            return false;
        if (ingredientObjectSOList.Contains(ingredient))
            return false;
        if (ingredientObjectSOList.Count >= pieIngredientsQuantity)
            return false;

        ingredientObjectSOList.Add(ingredient);
        
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

    public void SetPlayerThrower(int player)
    {
        playerThrower = player;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isCompleted)
            return;

        if (playerThrower % 2 != 0)
        {
            //Is thrown by team red
            //if (collision.gameObject.GetComponent<Player>().GetPlayerNumber() % 2 == 0)
            //{
            //    //Hitted an Enemy
            //}
            //else
            //{
            //    //Hitted an Ally
            //}
        }
        else
        {
            //if (collision.gameObject.GetComponent<Player>().GetPlayerNumber() % 2 == 0)
            //{
            //    //Hitted an Ally
            //}
            //else
            //{
            //    //Hitted an Enemy
            //}
        }
    }
}
