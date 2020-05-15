using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TestScene : MonoBehaviour
{
    public void BackButtonClick()
    {
        SceneManager.LoadScene("Loader");
    }
}
