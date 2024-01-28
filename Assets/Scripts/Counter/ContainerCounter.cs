using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] public IngredientsSO ingredient;

    public override void Interact(IIngredientObjectParent player)
    {
        if (!player.HasIngredientObject())
        {
            Transform ingredientTransform = Instantiate(ingredient.prefab);
            ingredientTransform.GetComponent<IngredientObject>().SetIngredientObjectParent(player);
        }
    }
}