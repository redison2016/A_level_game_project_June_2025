using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("InGameScene");
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
