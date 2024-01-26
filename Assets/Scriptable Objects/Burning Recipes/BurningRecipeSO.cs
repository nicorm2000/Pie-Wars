using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public IngredientsSO input;
    public IngredientsSO output;
    public float burningTimerMax;
}