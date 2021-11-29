using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLooseHandler : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("mainScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
