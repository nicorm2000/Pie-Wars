using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    { 
        MainMenu,
        Game,
        nicorm, //until game scene isn't broken from going to it form main menu
        Level, //Scene With the Level Layout.
        SelectPlayers,
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