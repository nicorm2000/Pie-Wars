using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private IngredientsSO ingredient;

    public override void Interact(Player player)
    {
        if (!player.HasIngredientObject())
        {
            Transform ingredientTransform = Instantiate(ingredient.prefab);
            ingredientTransform.GetComponent<IngredientObject>().SetIngredientObjectParent(player);
        }
    }
}