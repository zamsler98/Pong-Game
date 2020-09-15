using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void EasyGame()
    {
        GameManager.StartGame(GameType.EASY);
    }

    public void MediumGame()
    {
        GameManager.StartGame(GameType.MEDIUM);
    }

    public void HardGame()
    {
        GameManager.StartGame(GameType.HARD);
    }

    public void MultiPlayer()
    {
        GameManager.StartGame(GameType.MULTIPLAYER);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
