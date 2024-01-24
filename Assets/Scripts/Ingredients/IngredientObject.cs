using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private IngredientsSO ingredientSO;

    private ClearCounter clearCounter;

    public IngredientsSO GetIngredientObjectSO()
    {
        return ingredientSO;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (this.clearCounter != null)
        {
            this.clearCounter.ClearIngredientObject();
        }
        
        this.clearCounter = clearCounter;

        if (clearCounter.HasIngredientObject())
        {
            Debug.LogError("Counter already has an ingredient");
        }

        clearCounter.SetIngredientObject(this);

        transform.parent = clearCounter.GetIngredientObjectFollowTranform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }
}