using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
  public void Playgame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void quitgame()
    {
        Debug.Log("game is quitting");
        Application.Quit();
    }
}
