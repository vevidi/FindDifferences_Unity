using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TestLoader : MonoBehaviour
{
    public void WitoutMirrorClick()
    {
        SceneManager.LoadScene("MainMenu_exp");
    }

    public void WithMirrorClick()
    {
        SceneManager.LoadScene("MainMenu_exp_mirror");
    }
}
