using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetIngredientObject().GetIngredientObjectSO()))
                {
                    //Player carrying sommething that can be cut
                    player.GetIngredientObject().SetIngredientObjectParent(this);
                }
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
        if (HasIngredientObject() && HasRecipeWithInput(GetIngredientObject().GetIngredientObjectSO()))
        {
            IngredientsSO outputIngredientsSO = GetOutputForInput(GetIngredientObject().GetIngredientObjectSO());
            GetIngredientObject().DestoySelf();

            IngredientObject.SpawnIngredientObject(outputIngredientsSO, this);
        }
    }

    private IngredientsSO GetOutputForInput(IngredientsSO inputIngredientSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputIngredientSO)
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(IngredientsSO inputIngredientSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputIngredientSO)
            {
                return true;
            }
        }
        return false;
    }
}