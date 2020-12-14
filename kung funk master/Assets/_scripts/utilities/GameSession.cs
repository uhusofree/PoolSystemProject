using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public int score = 0;
    static GameSession _instance;
    public static GameSession instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameSession>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        ManageSingleton();
    }
    private void ManageSingleton()
    {
        int GameSessions = FindObjectsOfType<GameSession>().Length;
        if(GameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddtoScore(int currentScore)
    {
        score += currentScore;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
