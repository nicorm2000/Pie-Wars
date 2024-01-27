using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowingSystem : MonoBehaviour
{

    [SerializeField] private GameObject itemToThrow;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private float forceMultiplier;
    [Range(0.2f, 1.25f)] [SerializeField] private float timeHeld = 0;
    private bool isCharging = false;
    private void Awake()
    {
        gameInput.OnThrowPerformed += OnThrowPerformed;
        gameInput.OnThrowCanceled += OnThrowCanceled;
    }

    private void Update()
    {
        if (isCharging)
            timeHeld = Mathf.Clamp(timeHeld + Time.deltaTime, 0.2f, 1.25f);
    }

    void OnThrowPerformed(object sender, System.EventArgs e)
    {
        if (TryGetThrowableChild(out itemToThrow))
            isCharging = true;
        else
            return;
    }

    void OnThrowCanceled(object sender, System.EventArgs e)
    {
        if (itemToThrow == null)
            return;

        Rigidbody itemRb = itemToThrow.GetComponent<Rigidbody>();
        itemRb.isKinematic = false;
        itemRb.AddForce((transform.forward + new Vector3(0, 0.25f, 0)) * timeHeld * forceMultiplier, ForceMode.Impulse);
        itemToThrow.transform.parent = null;

        if (itemToThrow.TryGetComponent<IngredientObject>(out IngredientObject ingredientObject))
            ingredientObject.isFlying = true;
        if (itemToThrow.TryGetComponent<PlateObject>(out PlateObject plateObject))
        {
            //Add GetPlayerNumber to Player.cs
            //plateObject.SetPlayerThrower(GetComponent<Player>().GetPlayerNumber());
            plateObject.SetPlayerThrower(1);
            plateObject.isFlying = true;
        }

        itemToThrow.GetComponent<UnityEngine.Collider>().isTrigger = false;

        GetComponent<Player>().SetIngredientObject(null);
        isCharging = false;
        timeHeld = 0;
    }

    bool TryGetThrowableChild(out GameObject gameObject)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject.layer == 7)
            {
                gameObject = child.gameObject;
                return true;
            }
        }
        gameObject = null;
        return false;
    }
}
