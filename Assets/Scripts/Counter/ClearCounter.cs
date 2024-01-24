using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private IngredientsSO ingredient;

    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;

    private IngredientObject ingredientObject;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T))
        {
            if (ingredientObject != null)
            {
                ingredientObject.SetClearCounter(secondClearCounter);
            }
        }
    }

    public void Interact()
    {
        if (ingredientObject == null)
        {
            Transform ingredientTransform = Instantiate(ingredient.prefab, counterTopPoint);
            ingredientTransform.GetComponent<IngredientObject>().SetClearCounter(this);
        }
        else
        {
            Debug.Log(ingredientObject.GetClearCounter());
        }
    }

    public Transform GetIngredientObjectFollowTranform()
    {
        return counterTopPoint;
    }

    public void SetIngredientObject(IngredientObject ingredientObject)
    {
        this.ingredientObject = ingredientObject;
    }

    public IngredientObject GetIngredientObject()
    {
        return ingredientObject;
    }

    public void ClearIngredientObject()
    {
        ingredientObject = null;
    }

    public bool HasIngredientObject()
    {
        return ingredientObject != null;
    }
}