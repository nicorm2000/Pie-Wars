using System;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SelectPlayersUI : MonoBehaviour
{
    [Header("Party Selection"), Space]
    [SerializeField] private GameObject holderPartySelectionUI = null;
    [SerializeField] private ButtonToggle toggle1v1 = null;
    [SerializeField] private ButtonToggle toggle2v2 = null;
    [SerializeField] private Button btnMenu = null;
    [SerializeField] private Button btnConfirm = null;

    [Header("Player Selection"), Space]
    [SerializeField] private Button btnPlay = null;

    private Action<bool> onSetPlayersSelect = null;

    public void Init(Action<bool> onSetPlayersSelect)
    {
        this.onSetPlayersSelect = onSetPlayersSelect;

        btnConfirm.onClick.AddListener(() => SetPlayersSelect());

        btnMenu.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });

        btnPlay.onClick.AddListener(() => 
        {
            Loader.Load(Loader.Scene.nicorm);
        });

        btnPlay.gameObject.SetActive(false);
    }

    public void TunOnPlayBtn()
    {
        btnPlay.gameObject.SetActive(true);
    }

    private void SetPlayersSelect()
    {
        bool select1v1 = toggle1v1.IsSelected;

        onSetPlayersSelect.Invoke(select1v1);

        holderPartySelectionUI.SetActive(false);
    }
}
