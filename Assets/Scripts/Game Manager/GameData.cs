using UnityEngine.InputSystem;

public class GameData
{
    private PLAYER_INPUT[] players = null;
    private PlayerInput[] playersInputs = null;

    public void DefineAmountOfPlayers(int amountPlayers)
    {
        players = new PLAYER_INPUT[amountPlayers];
        playersInputs = new PlayerInput[amountPlayers];

        for (int i = 0; i < amountPlayers; i++)
        {
            players[i] = PLAYER_INPUT.UNDEFINED;
        }
    }

    public void AddPlayerInput(int index, PLAYER_INPUT input, PlayerInput playerInput)
    {
        players[index] = input;
        playersInputs[index] = playerInput;
    }

    public PLAYER_INPUT[] GetPlayersInputsType()
    {
        return players;
    }

    public PlayerInput[] GetPlayersInputs()
    {
        return playersInputs;
    }
}
