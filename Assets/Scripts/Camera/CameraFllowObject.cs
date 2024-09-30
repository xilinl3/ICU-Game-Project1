using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFllowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _PlayerTransform;

    [Header("Flip Rotation States")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;
    private PlayerMovement Player;
    private bool IsRight;
    private void Awake()
    {
        Player = _PlayerTransform.gameObject.GetComponent<PlayerMovement>();
        IsRight = Player.IsRight;
    }

    private void Update()
    {
        //让 CameraFollowObeject 跟随玩家的位置
        transform.position = _PlayerTransform.position;
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _flipYRotationTime)
        {
           elapsedTime += Time.deltaTime;

           //lerp the y rotation
           yRotation = Mathf.Lerp(startRotation, endRotationAmount, elapsedTime / _flipYRotationTime);
           transform.rotation = Quaternion.Euler(0f,yRotation,0f);

           yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        IsRight = !IsRight;
        return IsRight ? 180 : 0;
    }
}
