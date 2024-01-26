using UnityEngine;

public class SelectPlayersManager : MonoBehaviour
{
    [Header("UI"), Space]
    [SerializeField] private SelectPlayersUI selectPlayersUI = null;
    
    [Header("Player Selection"), Space]
    [SerializeField] private Transform[] holders1v1 = null;
    [SerializeField] private Transform[] holders2v2 = null;
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private KeyCode[] keyCodes = null;

    private int playersReady = 0;
    private int playersToPlay = 0;

    private void Start()
    {
        selectPlayersUI.Init(SetPlayersSelect);
    }

    private void SetPlayersSelect(bool select1v1)
    {
        playersToPlay = select1v1 ? holders1v1.Length : holders2v2.Length;

        for (int i = 0; i < playersToPlay; i++)
        {
            Transform holder = select1v1 ? holders1v1[i] : holders2v2[i];
            GameObject player = Instantiate(playerPrefab, holder);
            player.GetComponentInChildren<SelectStateUI>().Init(OnSelectPlayer, keyCodes[i]);
        }
    }

    private void OnSelectPlayer()
    {
        if (playersReady == 0)
        {
            selectPlayersUI.TunOnPlayBtn();
        }

        playersReady++;
    }
}
