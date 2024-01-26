public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                //Player is carrying something
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
                    if(GetIngredientObject().TryGetPlate(out plateObject))
                    {
                        //Counter Has a plate
                        if(plateObject.TryAddIngredient(player.GetIngredientObject().GetIngredientObjectSO()))
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
}