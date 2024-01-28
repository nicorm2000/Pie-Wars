using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private IngredientsSO ingredientSO;

    [SerializeField] private LayerMask layer;
    private IIngredientObjectParent ingredientObjectParent;
    public bool isFlying;
    [SerializeField] float rayDistance = 2;

    private void Update()
    {
        if (isFlying)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayDistance, layer))
            {
                if (hit.transform.gameObject.GetComponent<ClearCounter>() != null)
                    hit.transform.gameObject.GetComponent<ClearCounter>().ReceiveItem(this.gameObject);
            }
        }
    }
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

    public bool TryGetPlate(out PlateObject plateObject)
    {
        if (this is PlateObject)
        {
            plateObject = this as PlateObject;
            return true;
        }
        else
        {
            plateObject = null;
            return false;
        }
    }

    public static IngredientObject SpawnIngredientObject(IngredientsSO ingredientsSO, IIngredientObjectParent ingredientObjectParent)
    {
        Transform ingredientTransform = Instantiate(ingredientsSO.prefab);
        IngredientObject ingredientObject = ingredientTransform.GetComponent<IngredientObject>();
        ingredientObject.SetIngredientObjectParent(ingredientObjectParent);

        return ingredientObject;
    }

    public static IngredientObject SpawnIngredientObject(PlateObject ingredientsSO, IIngredientObjectParent ingredientObjectParent)
    {
        Transform ingredientTransform = Instantiate(ingredientsSO).transform;
        IngredientObject ingredientObject = ingredientTransform.GetComponent<IngredientObject>();
        ingredientObject.SetIngredientObjectParent(ingredientObjectParent);

        return ingredientObject;
    }
}