using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private IngredientsSO ingredientSO;

    public IngredientsSO GetIngredientObjectSO()
    {
        return ingredientSO;
    }
}