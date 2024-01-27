using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IIngredientObjectParent
{
    public static Player Instance { get; private set; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public event EventHandler OnPickSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public bool isSlipper = false;
    public bool isSlow = false;
    public float moveSpeedMultiplier;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float playerRadius;
    [SerializeField] private float playerHeight;
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform ingredientHoldPoint;
    [SerializeField] private float slipperyFactor;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private IngredientObject ingredientObject;
    public Rigidbody rb;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 Player Instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        rb = GetComponent<Rigidbody>();
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        //HandleMovement();
        //HandleMovement2();
        HandleInteractions();
    }

    private void FixedUpdate()
    {
        HandleMovement3();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
            lastInteractDir = moveDir;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    //private void HandleMovement()
    //{
    //    Vector2 inputVector = gameInput.GetMovementVectorNormalized();
    //
    //    Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
    //    float moveDistance = moveSpeed * Time.deltaTime;
    //    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
    //
    //    if (!canMove)
    //    {
    //        Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
    //        canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
    //
    //        if (canMove)
    //        {
    //            moveDir = moveDirX;
    //        }
    //        else
    //        {
    //            Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
    //            canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
    //
    //            if (canMove)
    //            {
    //                moveDir = moveDirZ;
    //            }
    //            else
    //            {
    //                //Can not move in any direction
    //            }
    //        }
    //    }
    //
    //    if (canMove)
    //        transform.position += moveDir * moveSpeed * Time.deltaTime;
    //
    //    isWalking = moveDir != Vector3.zero;
    //
    //    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    //}


    //[SerializeField] private float playerStopDistance;
    //private void HandleMovement2()
    //{
    //    Vector2 inputVector = gameInput.GetMovementVectorNormalized();
    //    Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
    //    bool isHit = Physics.Raycast(transform.position, moveDir, playerStopDistance);
    //
    //    if (isHit)
    //    {
    //
    //        moveDir = Vector3.zero;
    //    }
    //
    //    transform.position += moveDir * moveSpeed * Time.deltaTime;
    //    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    //}

    private void HandleMovement3()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        Vector3 targetVelocity = Vector3.zero;

        if (!isSlow)
        {
            targetVelocity = moveDir * MoveSpeed;
        }
        else if (isSlow)
        {
            targetVelocity = moveDir * MoveSpeed * moveSpeedMultiplier;
        }

        Vector3 velocityChange = targetVelocity - rb.velocity;

        if (isSlipper)
        {
            rb.AddForce(velocityChange * slipperyFactor, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        if (moveDir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, newRotation, rotateSpeed * Time.fixedDeltaTime));
        }
    }

    public void TriggerStateChange(float time)
    {
        StartCoroutine(WaitForTriggerStateChange(time));
    }

    public IEnumerator WaitForTriggerStateChange(float time)
    {
        yield return new WaitForSeconds(time);
        isSlipper = false;
        isSlow = false;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetIngredientObjectFollowTranform()
    {
        return ingredientHoldPoint;
    }

    public void SetIngredientObject(IngredientObject ingredientObject)
    {
        this.ingredientObject = ingredientObject;

        if (ingredientObject != null)
        {
            OnPickSomething?.Invoke(this, EventArgs.Empty);
        }
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