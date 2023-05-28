using Unity.Netcode;
using UnityEngine.SceneManagement;

/*******************************************************************************

   Nom du fichier: Loader.cs
   
   Contexte: Cette classe sert a gérer quelle scène doit être chargée
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public static class Loader
{
    public enum Scene
    {
        MenuScene,
        GameScene,
        Lobby,
        EndOfGameScene
    }

    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}
