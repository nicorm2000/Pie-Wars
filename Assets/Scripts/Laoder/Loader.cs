using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    { 
        MainMenu,
        Game,
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