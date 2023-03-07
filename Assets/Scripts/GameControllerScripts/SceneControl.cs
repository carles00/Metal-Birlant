using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPlayClick()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnPlayerLost()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
