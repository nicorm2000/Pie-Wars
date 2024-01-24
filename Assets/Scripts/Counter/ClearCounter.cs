using UnityEngine;

public class ClearCounter : BaseCounter
{
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
}