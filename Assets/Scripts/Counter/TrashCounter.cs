using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    public override void Interact(IIngredientObjectParent player)
    {
        if (player.HasIngredientObject())
        {
            player.GetIngredientObject().DestoySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}