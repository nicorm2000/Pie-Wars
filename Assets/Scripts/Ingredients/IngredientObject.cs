using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private IngredientsSO ingredientSO;

    private IIngredientObjectParent ingredientObjectParent;

    public IngredientsSO GetIngredientObjectSO()
    {
        return ingredientSO;
    }

    public void SetIngredientObjectParent(IIngredientObjectParent ingredientObjectParent)
    {
        if (this.ingredientObjectParent != null)
        {
            this.ingredientObjectParent.ClearIngredientObject();
        }
        
        this.ingredientObjectParent = ingredientObjectParent;

        if (ingredientObjectParent.HasIngredientObject())
        {
            Debug.LogError("Counter already has an ingredient");
        }

        ingredientObjectParent.SetIngredientObject(this);

        transform.parent = ingredientObjectParent.GetIngredientObjectFollowTranform();
        transform.localPosition = Vector3.zero;
    }

    public IIngredientObjectParent GetIngredientObjectParent()
    {
        return ingredientObjectParent;
    }

    public void DestoySelf()
    {
        ingredientObjectParent.ClearIngredientObject();

        Destroy(gameObject);
    }

    public static IngredientObject SpawnIngredientObject(IngredientsSO ingredientsSO, IIngredientObjectParent ingredientObjectParent)
    {
        Transform ingredientTransform = Instantiate(ingredientsSO.prefab);
        IngredientObject ingredientObject = ingredientTransform.GetComponent<IngredientObject>();
        ingredientObject.SetIngredientObjectParent(ingredientObjectParent);

        return ingredientObject;
    }
}