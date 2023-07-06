using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーン遷移用

public class Title : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Main");//メインに遷移
    }
}
