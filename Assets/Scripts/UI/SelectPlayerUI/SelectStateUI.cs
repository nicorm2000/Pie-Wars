using UnityEngine;
using TMPro;
using System;
using static GameInput;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class SelectStateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPressToSelect = null;
    [SerializeField] private GameObject pressToSelectView = null;
    [SerializeField] private GameObject confirmTick = null;

    private Action<int, PLAYER_INPUT> onSelect = null;
    private PLAYER_INPUT playerInput = default;
    private int index = 0;
    private PlayerInput p = null;

    public void Init(Action<int, PLAYER_INPUT> onSelect, int index, PLAYER_INPUT input, PlayerInput p)
    {
        this.onSelect = onSelect;
        this.index = index;
        playerInput = input;
        this.p = p;

        InputAction moveAction = GetMoveAction();

        txtPressToSelect.text = "Press " + moveAction.bindings[1].ToDisplayString() + " to Select";
        ToggleState(false);

        moveAction.performed += SetSelected;

        void SetSelected(CallbackContext callbackContext)
        {
            InputAction moveAction = GetMoveAction();

            Vector2 input = moveAction.ReadValue<Vector2>();

            if (input.y > 0)
            {
                pressToSelectView.GetComponent<Animator>().SetTrigger("TurnOff");
            }

            moveAction.performed -= SetSelected;
        }
    }

    private void ToggleState(bool status)
    {
        pressToSelectView.SetActive(!status);
        confirmTick.SetActive(status);

        if (status)
        {
            onSelect.Invoke(index, playerInput);
        }
    }

    private InputAction GetMoveAction()
    {
        return p.currentActionMap.actions[0];
    }

    public void OnTurnPressTextOff() // animator method
    {
        ToggleState(true);
    }
}
