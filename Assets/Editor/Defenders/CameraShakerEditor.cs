using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEditor;
using UnityEngine;

public class CameraShakerEditor : Editor
{
    [CustomEditor(typeof(CameraController))]
    public class ShakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Shake"))
            {
                CameraShaker.Instance.ShakeOnce();
            }
        }
    }
}
