using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasIngredientObject())
        {
            player.GetIngredientObject().DestoySelf();
        }
    }
}