using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    #region Singleton
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberOfGameSessions = FindObjectsOfType(GetType()).Length;
        if (numberOfGameSessions  > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    private int score;

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int amount)
    {
        score += amount;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
