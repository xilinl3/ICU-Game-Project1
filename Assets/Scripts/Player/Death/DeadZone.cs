using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("玩家进入死域");

            // 获取玩家的 PlayerManager 组件
            PlayerManager playerManager = collision.GetComponent<PlayerManager>();

            if (playerManager != null)
            {
                // 禁用玩家，并延迟复活
                playerManager.DisablePlayer();

                // 可以使用协程或延迟恢复玩家
                Debug.Log("在这里播放玩家死亡的动画 default为1秒");
                Invoke("RespawnPlayer", 1.0f);  // 2秒后复活
            }
        }
    }

    void RespawnPlayer()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.RespawnPlayer();  // 调用复活方法
        }
    }
}
