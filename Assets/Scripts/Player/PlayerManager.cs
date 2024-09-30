using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPoint;  // 使用 Vector3 来存储位置

    private SpriteRenderer playerRenderer;
    private Rigidbody2D playerRigidbody;
    private PlayerMovement playerMovement;

    void Start()
    {
        // 将玩家的初始位置作为重生点
        respawnPoint = transform.position;

        // 获取组件
        playerRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // 更新复活点（使用位置）
    public void UpdateRespawnPoint(Vector3 newRespawnPosition)
    {
        respawnPoint = newRespawnPosition;  // 只更新位置
        Debug.Log("重生点已更新为：" + respawnPoint);
    }

    // 复活玩家
    public void RespawnPlayer()
    {
        transform.position = respawnPoint;  // 将玩家传送到最新的复活点

        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;  // 显示玩家
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;  // 恢复物理
            playerRigidbody.velocity = Vector2.zero;  // 确保速度为零
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;  // 恢复玩家移动
        }

        Debug.Log("玩家已复活并移动到复活点：" + respawnPoint);
    }

    // 禁用玩家功能
    public void DisablePlayer()
    {
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;  // 隐藏玩家
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;  // 停止任何当前的移动
            playerRigidbody.isKinematic = true;  // 禁用物理影响
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;  // 禁用玩家移动
        }

        Debug.Log("玩家已被禁用，等待复活。");
    }
}
