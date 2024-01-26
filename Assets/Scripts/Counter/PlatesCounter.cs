using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private IngredientsSO plate;
    [SerializeField] private float plateSpawnCoolDown;
    private float spawnPlateTimer;
    private int spawnedPlatesAmount = 0;
    [SerializeField] private int maxSpawnablePlates;

    // Update is called once per frame
    void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > plateSpawnCoolDown)
        {
            spawnPlateTimer = 0f;
            if (spawnedPlatesAmount < maxSpawnablePlates)
            {
                spawnedPlatesAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasIngredientObject())
        {
            if (spawnedPlatesAmount > 0)
            {
                spawnedPlatesAmount--;
                Transform ingredientTransform = Instantiate(plate.prefab);
                ingredientTransform.GetComponent<IngredientObject>().SetIngredientObjectParent(player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
