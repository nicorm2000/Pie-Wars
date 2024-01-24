using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private IngredientsSO ingredient;

    private IngredientObject ingredientObject;

    public void Interact()
    {
        if (ingredientObject == null)
        {
            Transform ingredientTransform = Instantiate(ingredient.prefab, counterTopPoint);
            ingredientTransform.localPosition = Vector3.zero;

            ingredientObject = ingredientTransform.GetComponent<IngredientObject>();
        }
    }
}