using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private IngredientsSO ingredient;

    public override void Interact(Player player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                //Player is carrying something
                player.GetIngredientObject().SetIngredientObjectParent(this);
            }
            else
            {
                //Player has nothing in hands
            }
        }
        else
        {
            //There is an ingredient here
            if (player.HasIngredientObject())
            {
                //Player is carrying somehting
            }
            else
            {
                //Player is not carrying anything
                GetIngredientObject().SetIngredientObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasIngredientObject())
        {
            GetIngredientObject().DestoySelf();

            Transform ingredientTransform = Instantiate(ingredient.prefab);
            Debug.Log(ingredientTransform);
            ingredientTransform.GetComponent<IngredientObject>().SetIngredientObjectParent(this);
        }
    }
}