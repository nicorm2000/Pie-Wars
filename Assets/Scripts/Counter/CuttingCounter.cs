using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNnormalized;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

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
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNnormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
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
            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNnormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                IngredientsSO outputIngredientsSO = GetOutputForInput(GetIngredientObject().GetIngredientObjectSO());

                GetIngredientObject().DestoySelf();

                IngredientObject.SpawnIngredientObject(outputIngredientsSO, this);
            }
        }
    }

    private IngredientsSO GetOutputForInput(IngredientsSO inputIngredientSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputIngredientSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private bool HasRecipeWithInput(IngredientsSO inputIngredientSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputIngredientSO);

        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(IngredientsSO inputIngredientSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputIngredientSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}