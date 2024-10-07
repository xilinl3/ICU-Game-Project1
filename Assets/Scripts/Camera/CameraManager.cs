using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using System;

public class CameraManager : MonoBehaviour
{
   public static CameraManager Instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;
    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }
    private Coroutine _lerpYDampingCoroutine;
    private Coroutine _panCameraCoroutine;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;

    private Vector2 _startingTrackedObjectOffset;

   private void Awake()
   {
        if(Instance == null)
        {
            Instance = this;
        }
        for(int i = 0; i< _allVirtualCameras.Length; i++)
        {
            if(_allVirtualCameras[i].enabled)
            {
                //set the current active camera
                _currentCamera = _allVirtualCameras[i];

                //set the framing transposer
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        //set the YDamping amount so it's based on the inspector value
        _normYPanAmount = _framingTransposer.m_YDamping;

        //set the starting positing of the tracked object offest
        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
   }
   #region Lerping Y Damping

   public void LerpYDamping(bool isPlayerFalling)
   {
       _lerpYDampingCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
   }
   private IEnumerator LerpYAction(bool isPlayerFalling)
   {
      IsLerpingYDamping = true;
    
        //grab the string damping amount
      float starDampAmount = _framingTransposer.m_YDamping;
      float endDampAmount = 0f;

     //determine the end damping amount
     if(isPlayerFalling)
      {
        endDampAmount = _fallPanAmount;
        LerpedFromPlayerFalling = true;
      }
      else
      {
        endDampAmount = _normYPanAmount;
      }

      //lerp the damping amount
      float elapsedTime = 0f;
      while(elapsedTime < _fallYPanTime)
      {
         elapsedTime += Time.deltaTime;

         float lerpedPanAmount = Mathf.Lerp(starDampAmount, endDampAmount, elapsedTime / _fallYPanTime);
         _framingTransposer.m_YDamping = lerpedPanAmount;

         yield return null;
      }

      IsLerpingYDamping = false;
   }
    #endregion

    #region Pan Camera
    public void panCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
      _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
      Vector2 endPos = Vector2.zero;
      Vector2 startingPos = Vector2.zero;

      //handle pan from trigger
      if(!panToStartingPos)
      {
        //set the direction and distance
        switch(panDirection)
        {
          case PanDirection.Up:
            endPos = Vector2.up;
            break;
          case PanDirection.Down:
            endPos = Vector2.down;
            break;
          case PanDirection.Left:
            endPos = Vector2.right;
            break;
          case PanDirection.Right:
            endPos = Vector2.left;
            break;
          default:
            break;
        }

        endPos *= panDistance;

        startingPos = _startingTrackedObjectOffset;

        endPos += startingPos;
      }

      //handle the pan back to starting position
      else
      {
        startingPos = _framingTransposer.m_TrackedObjectOffset;
        endPos = _startingTrackedObjectOffset;
      }

      //handle the actual panning of the camera
      float elapsedTime = 0f;
      while(elapsedTime < panTime)
      {
        elapsedTime += Time.deltaTime;

        Vector3 panLerp = Vector3.Lerp(startingPos, endPos, elapsedTime / panTime);
        _framingTransposer.m_TrackedObjectOffset = panLerp;

        yield return null;
      }
    }
    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
      Debug.Log($"Trigger Exit Direction: {triggerExitDirection.x}");
      //if the current camera is the camera on the left and our trigger exit direction was on the right
      //如果当前摄像机位于左侧，而我们的触发器出口方向位于右侧
      if(_currentCamera == cameraFromLeft && triggerExitDirection.x >0f)
      {
        //activate the new camera
        cameraFromRight.enabled = true;

        //deactivate the old camera
        cameraFromLeft.enabled = false;

        //set the new camera as the current camera
        _currentCamera = cameraFromRight;

        //update our composer variable
        _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
      }

      //if the current camera is the camera on the eright and our trigger hit direction was on the left
      else if(_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
      {
        //activate the new camera
        cameraFromLeft.enabled = true;

        //deactivate the old camera
        cameraFromRight.enabled = false;

        //set the new camera as the current camera
        _currentCamera = cameraFromLeft;

        //update our composer variable
        _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
      }
    }
    #endregion
}