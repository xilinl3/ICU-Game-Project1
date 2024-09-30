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

    //继续键
    public void Continue()
    {
        GameObject.Find("暂停菜单").SetActive(false);
        Time.timeScale = 1;  
    }

    //重新开始
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
