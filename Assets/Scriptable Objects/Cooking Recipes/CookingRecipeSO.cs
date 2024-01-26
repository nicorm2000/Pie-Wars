using UnityEngine;

[CreateAssetMenu()]
public class CookingRecipeSO : ScriptableObject
{
    public IngredientsSO input;
    public IngredientsSO output;
    public float cookingTimerMax;
}