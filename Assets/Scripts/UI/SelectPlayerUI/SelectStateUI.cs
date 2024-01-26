using UnityEngine;
using TMPro;
using System;

public class SelectStateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPressToSelect = null;
    [SerializeField] private GameObject pressToSelectView = null;
    [SerializeField] private GameObject confirmTick = null;
    [SerializeField] private KeyCode keyToPress = default;

    private Action onSelect = null;

    private void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            pressToSelectView.GetComponent<Animator>().SetTrigger("TurnOff");
        }
    }

    public void Init(Action onSelect, KeyCode keyCode)
    {
        this.onSelect = onSelect;

        keyToPress = keyCode;
        txtPressToSelect.text = "Press " + keyCode.ToString() + " to Select";
        ToggleState(false);
    }

    private void ToggleState(bool status)
    {
        pressToSelectView.SetActive(!status);
        confirmTick.SetActive(status);

        if (status)
        {
            onSelect.Invoke();
        }
    }

    public void OnTurnPressTextOff() // animator methods
    {
        ToggleState(true);
    }
}
