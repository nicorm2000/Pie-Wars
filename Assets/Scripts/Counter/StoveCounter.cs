using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public CookingState cookingState;
    }

    public enum CookingState
    {
        Idle,
        Cooking,
        Cooked,
        Burned
    }

    [SerializeField] private CookingRecipeSO[] cookingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private CookingState cookingState;
    private float cookingTimer;
    private float burningTimer;
    private CookingRecipeSO cookingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private PlateObject plateInput;
    [SerializeField] private IngredientsSO pieDough;
    private void Start()
    {
        cookingState = CookingState.Idle;
    }

    private void Update()
    {
        if (HasIngredientObject())
        {
            switch (cookingState)
            {
                case CookingState.Idle:
                    break;
                case CookingState.Cooking:
                    cookingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = cookingTimer / cookingRecipeSO.cookingTimerMax
                    });

                    if (cookingTimer > cookingRecipeSO.cookingTimerMax)
                    {
                        //Cooked
                        GetIngredientObject().DestoySelf();

                        IngredientObject.SpawnIngredientObject(cookingRecipeSO.output, this);

                        cookingState = CookingState.Cooked;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            cookingState = cookingState
                        });
                    }
                    break;
                case CookingState.Cooked:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        //Cooked
                        GetIngredientObject().DestoySelf();

                        IngredientObject.SpawnIngredientObject(burningRecipeSO.output, this);

                        cookingState = CookingState.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            cookingState = cookingState
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case CookingState.Burned:
                    break;
            }
        }
    }

    public override void Interact(IIngredientObjectParent player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                //Player is carrying something
                //if (HasRecipeWithInput(player.GetIngredientObject().GetIngredientObjectSO()))
                //{
                //Player carrying sommething that can be cooked

                if (player.GetIngredientObject().gameObject.TryGetComponent<PlateObject>(out plateInput))
                {
                    //Player is carrying a plate
                    if (plateInput.ingredientObjectSOList.Contains(pieDough) && plateInput.ingredientObjectSOList.Count == plateInput.pieIngredientsQuantity)
                    {
                        player.GetIngredientObject().SetIngredientObjectParent(this);
                        cookingRecipeSO = GetCookingRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

                        cookingState = CookingState.Cooking;
                        cookingTimer = 0f;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            cookingState = cookingState
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = cookingTimer / cookingRecipeSO.cookingTimerMax
                        });
                    }
                }
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
                //if (player.GetIngredientObject().TryGetPlate(out PlateObject plateObject))
                //{
                //    //Player is Holding a plate
                //    if (plateObject.TryAddIngredient(GetIngredientObject().GetIngredientObjectSO()))
                //        GetIngredientObject().DestoySelf();

                //    cookingState = CookingState.Idle;

                //    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                //    {
                //        cookingState = cookingState
                //    });

                //    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                //    {
                //        progressNormalized = 0f
                //    });
                //}
            }
            else
            {
                //Player is not carrying anything
                if (cookingState == CookingState.Cooked)
                {
                    plateInput.isCompleted = true;
                    Debug.Log("Finished Pie");
                }
                else
                    Debug.Log("BurnedPie");

                GetIngredientObject().SetIngredientObjectParent(player);

                cookingState = CookingState.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    cookingState = cookingState
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private IngredientObject TakePlateOut()
    {
        if (cookingState == CookingState.Cooked)
        {
            plateInput.isCompleted = true;
            Debug.Log("Finished Pie");
        }

        return plateInput;
    }
    private IngredientsSO GetOutputForInput(IngredientsSO inputIngredientSO)
    {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputIngredientSO);

        if (cookingRecipeSO != null)
        {
            return cookingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private bool HasRecipeWithInput(IngredientsSO inputIngredientSO)
    {
        CookingRecipeSO cookingRecipeSO = GetCookingRecipeSOWithInput(inputIngredientSO);

        return cookingRecipeSO != null;
    }

    private CookingRecipeSO GetCookingRecipeSOWithInput(IngredientsSO inputIngredientSO)
    {
        foreach (CookingRecipeSO cookingRecipeSO in cookingRecipeSOArray)
        {
            if (cookingRecipeSO.input == inputIngredientSO)
            {
                return cookingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(IngredientsSO inputIngredientSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputIngredientSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsCooked()
    {
        return cookingState == CookingState.Cooked;
    }
    public bool IsIddle()
    {
        return cookingState == CookingState.Idle;
    }
}