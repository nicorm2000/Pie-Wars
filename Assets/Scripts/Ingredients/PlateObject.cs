using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateObject : IngredientObject
{
    public event EventHandler<OnIngredientAddedArgs> OnIngredientAdded;

    int playerThrower;

    [SerializeField] private GameObject bananaPieComplete, blueberriesPieComplete, pieOfCreamComplete, plate, plateWithDough;
    public class OnIngredientAddedArgs : EventArgs
    {
        public IngredientsSO ingredientSO;
    }

    [SerializeField] private List<IngredientsSO> validIngredients;
    public List<IngredientsSO> ingredientObjectSOList = new List<IngredientsSO>();
    public bool isCompleted = false;
    [SerializeField] public int pieIngredientsQuantity = 2;

    [SerializeField] private IngredientsSO banana, blueberries, cream, dough;
    [SerializeField] private Material normal, cooked, burned;

    public void ChangePlateState(bool isBurned)
    {
        //plate.SetActive(false);

        //if (ingredientObjectSOList.Contains(banana))
        //    bananaPieComplete.SetActive(true);
        //else if (ingredientObjectSOList.Contains(blueberries))
        //    blueberriesPieComplete.SetActive(true);
        //else if (ingredientObjectSOList.Contains(cream))
        //    pieOfCreamComplete.SetActive(true);
        if (isBurned)
        {
            isCompleted = false;
            bananaPieComplete.gameObject.transform.Find("Mix").GetComponent<MeshRenderer>().material = burned;
            blueberriesPieComplete.gameObject.transform.Find("Mix").GetComponent<MeshRenderer>().material = burned;
            pieOfCreamComplete.gameObject.transform.Find("Mix").GetComponent<MeshRenderer>().material = burned;
        }
        else
        {
            isCompleted = true;
            bananaPieComplete.gameObject.transform.Find("Mix").GetComponent<MeshRenderer>().material = cooked;
            blueberriesPieComplete.gameObject.transform.Find("Mix").GetComponent<MeshRenderer>().material = cooked;
            pieOfCreamComplete.gameObject.transform.Find("Mix").GetComponent<MeshRenderer>().material = cooked;
        }
    }

    public bool TryAddIngredient(IngredientsSO ingredient)
    {
        if (!validIngredients.Contains(ingredient))
            return false;
        if (ingredientObjectSOList.Contains(ingredient))
            return false;
        if (ingredientObjectSOList.Count >= pieIngredientsQuantity)
            return false;

        if (ingredientObjectSOList.Contains(dough))
        {
            plateWithDough.SetActive(false);

            if (ingredient == banana)
                bananaPieComplete.SetActive(true);
            else if (ingredient == blueberries)
                blueberriesPieComplete.SetActive(true);
            else if (ingredient == cream)
                pieOfCreamComplete.SetActive(true);
        }
        else
        {
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedArgs
            {
                ingredientSO = ingredient
            });
        }
        ingredientObjectSOList.Add(ingredient);

        if (ingredient == dough)
        {
            plate.SetActive(false);

            if (ingredientObjectSOList.Count == 1)
                plateWithDough.SetActive(true);
            else
            {
                if (ingredientObjectSOList.Contains(banana))
                    bananaPieComplete.SetActive(true);
                else if (ingredientObjectSOList.Contains(blueberries))
                    blueberriesPieComplete.SetActive(true);
                else if (ingredientObjectSOList.Contains(cream))
                    pieOfCreamComplete.SetActive(true);

                transform.GetComponentInChildren<PieVisual>().gameObject.SetActive(false);
            }
        }



        return true;
    }

    public bool GetPieStatus()
    {
        return isCompleted;
    }

    public IngredientsSO GetDough()
    {
        return dough;
    }

    public void SetPlayerThrower(int player)
    {
        playerThrower = player;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isFlying)
            return;

        int scoreSum = 1;

        if (isCompleted)
            scoreSum = 5;

        Debug.Log("Collided");
        if (playerThrower % 2 != 0)
        {
            if (collision.gameObject.GetComponent<Player>() == null)
                return;

            //Is thrown by team blue
            if (collision.gameObject.GetComponent<Player>().playerNumber % 2 == 0)
            {
                //Hitted an Enemy
                GameManager.Instance.AddPoints(GameManager.Team.Blue, scoreSum);
                this.DestoySelf();
            }
            else
            {
                //GameManager.Instance.AddPoints(GameManager.Team.Blue, -1);
                //Hitted an Ally
            }
        }
        else
        {
            if (collision.gameObject.GetComponent<Player>().playerNumber % 2 != 0)
            {
                //Hitted an Enemy
                GameManager.Instance.AddPoints(GameManager.Team.Red, scoreSum);
                this.DestoySelf();
            }
            else
            {
                //Hitted an Ally
                //GameManager.Instance.AddPoints(GameManager.Team.Red, -1);
            }
        }
        //this.gameObject.layer = 9;
    }

    public static PlateObject SpawnPlateObject(PlateObject ingredientsSO, IIngredientObjectParent ingredientObjectParent)
    {
        Transform ingredientTransform = Instantiate(ingredientsSO).transform;
        PlateObject ingredientObject = ingredientTransform.GetComponent<PlateObject>();
        ingredientObject.SetIngredientObjectParent(ingredientObjectParent);

        return ingredientObject;
    }
}
