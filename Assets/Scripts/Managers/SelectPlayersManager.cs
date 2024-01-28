using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectPlayersManager : MonoBehaviour
{
    [Header("UI"), Space]
    [SerializeField] private SelectPlayersUI selectPlayersUI = null;
    
    [Header("Player Selection"), Space]
    [SerializeField] private Transform[] holders1v1 = null;
    [SerializeField] private Transform[] holders2v2 = null;
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private PLAYER_INPUT[] inputs1v1 = null;
    [SerializeField] private PLAYER_INPUT[] inputs2v2 = null;

    private int playersReady = 0;
    private int playersToPlay = 0;

    private List<PlayerInput> playerInputs = new List<PlayerInput>();

    private void Start()
    {
        selectPlayersUI.Init(SetPlayersSelect);
    }

    private void SetPlayersSelect(bool select1v1)
    {
        playersToPlay = select1v1 ? holders1v1.Length : holders2v2.Length;
        PLAYER_INPUT[] inputs = select1v1 ? inputs1v1 : inputs2v2;

        GameManager.GameData.DefineAmountOfPlayers(playersToPlay);

        for (int i = 0; i < playersToPlay; i++)
        {
            PlayerInput p = PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
            p.SwitchCurrentActionMap("Player" + ((int)inputs[i] - 1 == 0 ? "" : (int)inputs[i] - 1));
            playerInputs.Add(p);

            Transform holder = select1v1 ? holders1v1[i] : holders2v2[i];
            p.gameObject.transform.SetParent(holder);
            p.gameObject.transform.localPosition = Vector3.zero;
            p.gameObject.transform.localRotation = Quaternion.identity;

            p.GetComponentInChildren<SelectStateUI>().Init(OnSelectPlayer, i, inputs[i], p);

            p.GetComponentInChildren<PlayerSkinManager>().ChangeMaterial(GameManager.Instance.Skins[i]);
        }
    }

    private void OnSelectPlayer(int index, PLAYER_INPUT playerInput)
    {
        if (playersReady == 0)
        {
            selectPlayersUI.TunOnPlayBtn();
        }

        GameManager.GameData.AddPlayerInput(index, playerInput, playerInputs[index]);

        playersReady++;
    }
}
