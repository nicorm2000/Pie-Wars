public class ClearCounter : BaseCounter
{
    public override void Interact(IIngredientObjectParent player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                //Player is carrying something
                player.GetIngredientObject().GetComponent<UnityEngine.Rigidbody>().isKinematic = true;
                player.GetIngredientObject().GetComponent<UnityEngine.Collider>().isTrigger = true;
                player.GetIngredientObject().SetIngredientObjectParent(this);
            }
            else
            {
                //Player has nothing in hands
            }
        }
        else
        {
            //There is an ingredient here
            if (player.HasIngredientObject())
            {
                //Player is carrying somehting
                if (player.GetIngredientObject().TryGetPlate(out PlateObject plateObject))
                {
                    //Player is Holding a plate
                    if (plateObject.TryAddIngredient(GetIngredientObject().GetIngredientObjectSO()))
                        GetIngredientObject().DestoySelf();
                }
                else
                {
                    //Player is Not Holding a plate but something else
                    if (GetIngredientObject().TryGetPlate(out plateObject))
                    {
                        //Counter Has a plate
                        if (plateObject.TryAddIngredient(player.GetIngredientObject().GetIngredientObjectSO()))
                        {
                            player.GetIngredientObject().DestoySelf();
                        }
                    }
                }
            }
            else
            {
                //Player is not carrying anything
                GetIngredientObject().SetIngredientObjectParent(player);
            }
        }
    }
    public void ReceiveItem(UnityEngine.GameObject gameObject)
    {
        if (!HasIngredientObject())
        {
            gameObject.GetComponent<UnityEngine.Rigidbody>().isKinematic = true;
            gameObject.GetComponent<UnityEngine.Collider>().isTrigger = true;
            if (gameObject.TryGetComponent<IngredientObject>(out IngredientObject ingredient))
            {
                ingredient.SetIngredientObjectParent(this);
                ingredient.isFlying = false;
            }
            if (gameObject.TryGetComponent<PlateObject>(out PlateObject plate))
            {
                plate.SetIngredientObjectParent(this);
                plate.isFlying = false;
            }
        }
        else
        {
            //Player is Not Holding a plate but something else
            if (GetIngredientObject().TryGetPlate(out PlateObject plateObject))
            {
                //Counter Has a plate
                gameObject.TryGetComponent<IngredientObject>(out IngredientObject ingredient);
                if (plateObject.TryAddIngredient(ingredient.GetIngredientObjectSO()))
                {
                    ingredient.DestoySelf();
                }
            }
        }
    }
}