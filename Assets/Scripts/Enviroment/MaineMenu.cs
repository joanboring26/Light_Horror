using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaineMenu : MonoBehaviour
{
    public void PlayLvl1()
    {
        SceneManager.LoadScene("level_1");
    }

    public void QuitGame()
    {

        Application.Quit();
    }

}
