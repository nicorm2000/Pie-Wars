using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    { 
        MainMenu,
        SelectPlayers,
        Game,
        Level, //Scene With the Level Layout.
        nicorm, //until game scene isn't broken from going to it form main menu
        Credits,
        Loading
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}