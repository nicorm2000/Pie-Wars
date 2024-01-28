using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PLAYER_INPUT { UNDEFINED, WASD, ARROWS, IJKL, NUMPAD }
public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    private PlayerInputActions playerInputActions;
    private PLAYER_INPUT inputType = default;

    public event EventHandler OnThrowPerformed;
    public event EventHandler OnThrowCanceled;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnInteractAction;
    public static event EventHandler OnPauseAction;

    public enum Binding
    { 
        Move_Up, 
        Move_Down, 
        Move_Left, 
        Move_Right, 
        Interact, 
        InteractAlternate, 
        Pause
    }

    public enum Input_Action
    {
        Move,
		Throw,
        Interact,
        InteractAlternate,
        Pause
    }

    private void OnDestroy()
    {
        //GetInputAction(Input_Action.Interact).performed -= Interact_performed;
        //GetInputAction(Input_Action.InteractAlternate).performed -= InteractAlternate_performed;
        //GetInputAction(Input_Action.Pause).performed -= Pause_performed;
        //
        //playerInputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Throw_performed(InputAction.CallbackContext obj)
    {
        OnThrowPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void Throw_canceled(InputAction.CallbackContext obj)
    {
        OnThrowCanceled?.Invoke(this, EventArgs.Empty);
	}

    public void SetInputType(PLAYER_INPUT input)
    {
        inputType = input;

        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        switch (inputType)
        {
            case PLAYER_INPUT.WASD:
                playerInputActions.Player.Enable();
                break;
            case PLAYER_INPUT.ARROWS:
                playerInputActions.Player1.Enable();
                break;
            case PLAYER_INPUT.IJKL:
                playerInputActions.Player2.Enable();
                break;
            case PLAYER_INPUT.NUMPAD:
                playerInputActions.Player3.Enable();
                break;
            default:
                break;
        }

        GetInputAction(Input_Action.Throw).performed += Throw_performed;
        GetInputAction(Input_Action.Throw).canceled += Throw_canceled;
        GetInputAction(Input_Action.Interact).performed += Interact_performed;
        GetInputAction(Input_Action.InteractAlternate).performed += InteractAlternate_performed;
        GetInputAction(Input_Action.Pause).performed += Pause_performed;
    }
	
    public InputAction GetInputAction(Input_Action action)
    {
        switch (inputType)
        {
            case PLAYER_INPUT.WASD:
                switch (action)
                {
                    case Input_Action.Move:
                        return playerInputActions.Player.Move;
                    case Input_Action.Interact:
                        return playerInputActions.Player.Interact;
                    case Input_Action.InteractAlternate:
                        return playerInputActions.Player.InteractAlternate;
                    case Input_Action.Pause:
                        return playerInputActions.Player.Pause;
                    default:
                        return playerInputActions.Player.Interact;
                }
            case PLAYER_INPUT.ARROWS:
                switch (action)
                {
                    case Input_Action.Move:
                        return playerInputActions.Player1.Move;
                    case Input_Action.Interact:
                        return playerInputActions.Player1.Interact;
                    case Input_Action.InteractAlternate:
                        return playerInputActions.Player1.InteractAlternate;
                    case Input_Action.Pause:
                        return playerInputActions.Player1.Pause;
                    default:
                        return playerInputActions.Player1.Interact;
                }
            case PLAYER_INPUT.IJKL:
                switch (action)
                {
                    case Input_Action.Move:
                        return playerInputActions.Player2.Move;
                    case Input_Action.Interact:
                        return playerInputActions.Player2.Interact;
                    case Input_Action.InteractAlternate:
                        return playerInputActions.Player2.InteractAlternate;
                    case Input_Action.Pause:
                        return playerInputActions.Player2.Pause;
                    default:
                        return playerInputActions.Player2.Interact;
                }
            case PLAYER_INPUT.NUMPAD:
                switch (action)
                {
                    case Input_Action.Move:
                        return playerInputActions.Player3.Move;
                    case Input_Action.Interact:
                        return playerInputActions.Player3.Interact;
                    case Input_Action.InteractAlternate:
                        return playerInputActions.Player3.InteractAlternate;
                    case Input_Action.Pause:
                        return playerInputActions.Player3.Pause;
                    default:
                        return playerInputActions.Player3.Interact;
                }
            default:
                return null;
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = GetInputAction(Input_Action.Move).ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return GetInputAction(Input_Action.Move).bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return GetInputAction(Input_Action.Move).bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return GetInputAction(Input_Action.Move).bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return GetInputAction(Input_Action.Move).bindings[4].ToDisplayString();
            case Binding.Interact:
                return GetInputAction(Input_Action.Interact).bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return GetInputAction(Input_Action.InteractAlternate).bindings[0].ToDisplayString();
            case Binding.Pause:
                return GetInputAction(Input_Action.Pause).bindings[0].ToDisplayString();
        }
    }
}