using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScenekkk : MonoBehaviour
{
  public void MainMenu()
    {
        SceneManager.LoadScene("1 MainMenu");
    }
    public void Playy()
    {
        SceneManager.LoadScene("MultiPlayer");
    }
}
