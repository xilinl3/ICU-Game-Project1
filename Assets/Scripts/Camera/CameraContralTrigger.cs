using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

public class CameraContralTrigger : MonoBehaviour
{
    public CustomInspectorObject customInspectorObject;

    private Collider2D _coll;
    private float playerEnterXPosition;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            if(customInspectorObject.panCameraOnContact)
            {
                //pan the Camera
                CameraManager.Instance.panCameraOnContact(customInspectorObject.panDistance, customInspectorObject.panTime, customInspectorObject.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {

            Vector2 exitDirection = (collider.transform.position - _coll.bounds.center).normalized;

            if(customInspectorObject.swapCamera && customInspectorObject.cameraOnLeft != null && customInspectorObject.cameraOnRight != null)
            {
                //swap cameras
                CameraManager.Instance.SwapCamera(customInspectorObject.cameraOnLeft, customInspectorObject.cameraOnRight, exitDirection);
            }
            if(customInspectorObject.panCameraOnContact)
            {
                //pan the Camera
                CameraManager.Instance.panCameraOnContact(customInspectorObject.panDistance, customInspectorObject.panTime, customInspectorObject.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObject
{
    public bool swapCamera = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

[CustomEditor(typeof(CameraContralTrigger))]
[CanEditMultipleObjects]  // This attribute allows multi-object editing.
public class MyScriptEditor : Editor
{
    SerializedProperty customInspectorObject;
    SerializedProperty swapCamera;
    SerializedProperty panCameraOnContact;
    SerializedProperty cameraOnLeft;
    SerializedProperty cameraOnRight;
    SerializedProperty panDirection;
    SerializedProperty panDistance;
    SerializedProperty panTime;

    private void OnEnable()
    {
        customInspectorObject = serializedObject.FindProperty("customInspectorObject");
        swapCamera = customInspectorObject.FindPropertyRelative("swapCamera");
        panCameraOnContact = customInspectorObject.FindPropertyRelative("panCameraOnContact");
        cameraOnLeft = customInspectorObject.FindPropertyRelative("cameraOnLeft");
        cameraOnRight = customInspectorObject.FindPropertyRelative("cameraOnRight");
        panDirection = customInspectorObject.FindPropertyRelative("panDirection");
        panDistance = customInspectorObject.FindPropertyRelative("panDistance");
        panTime = customInspectorObject.FindPropertyRelative("panTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // Start updating the serialized object

        DrawDefaultInspector();

        if (swapCamera.boolValue)
        {
            EditorGUILayout.PropertyField(cameraOnLeft, new GUIContent("Camera on Left"));
            EditorGUILayout.PropertyField(cameraOnRight, new GUIContent("Camera on Right"));
        }

        if (panCameraOnContact.boolValue)
        {
            EditorGUILayout.PropertyField(panDirection, new GUIContent("Camera Pan Direction"));
            EditorGUILayout.PropertyField(panDistance, new GUIContent("Pan Distance"));
            EditorGUILayout.PropertyField(panTime, new GUIContent("Pan Time"));
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();  // Apply changes to all selected objects
        }
    }
}
