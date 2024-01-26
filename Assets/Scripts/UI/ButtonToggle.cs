using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color unselectedColor = Color.white;

    private Button button = null;
    private Image image = null;

    public bool IsSelected { get; private set; }

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ToggleSelected(true));

        image = GetComponent<Image>();

        ToggleSelected(false);
    }

    public void ToggleSelected(bool status)
    {
        IsSelected = status;

        image.color = status ? selectedColor : unselectedColor;

        button.interactable = !status;
    }
}
