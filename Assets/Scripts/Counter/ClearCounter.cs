using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private IngredientsSO ingredient;

    public void Interact()
    {
        Debug.Log("Interact");
        Transform ingredientTransform = Instantiate(ingredient.prefab, counterTopPoint);
        ingredientTransform.localPosition = Vector3.zero;
    }
}