using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;  // 标记检查点是否已被触发

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && collision.CompareTag("Player"))
        {
            // 第一次触发时更新复活点
            PlayerManager playerManager = collision.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.UpdateRespawnPoint(transform.position);  // 更新为检查点位置
                isActivated = true;  // 标记为已激活，防止再次触发
            }
        }
    }
}


