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
            //Is thrown by team blue
            if (collision.gameObject.GetComponent<Player>().playerNumber % 2 == 0)
            {
                //Hitted an Enemy
                GameManager.Instance.AddPoints(GameManager.Team.Blue, 1);
            }
            else
            {
                GameManager.Instance.AddPoints(GameManager.Team.Blue, -1);
                //Hitted an Ally
            }
        }
        else
        {
            if (collision.gameObject.GetComponent<Player>().playerNumber % 2 != 0)
            {
                //Hitted an Enemy
                GameManager.Instance.AddPoints(GameManager.Team.Red, 1);
            }
            else
            {
                //Hitted an Ally
                GameManager.Instance.AddPoints(GameManager.Team.Red, -1);
            }
        }
    }
}
