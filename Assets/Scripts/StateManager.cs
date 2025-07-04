using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public void LoadLevel(int holeNumber)
    {
        if (holeNumber == 0)
        {
            ScoreManager.Instance.ResetScore();
        }
        SceneManager.LoadSceneAsync(holeNumber, LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
