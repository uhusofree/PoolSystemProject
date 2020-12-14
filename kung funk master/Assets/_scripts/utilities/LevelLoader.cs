using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private float delayMenuStart = 2;


    public IEnumerator DelayOnMenu()
    {
        yield return new WaitForSeconds(delayMenuStart);
        SceneManager.LoadScene(1); //todo: change to last scene
    }

    public void LoadStartMenu() 
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(DelayOnMenu());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
