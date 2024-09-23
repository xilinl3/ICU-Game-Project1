using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
   //开始键
   public void Starting()
    {
        SceneManager.LoadScene(1);
    }

   //退出键
    public void ExitGame()
    {
         Application.Quit();
    }
}
