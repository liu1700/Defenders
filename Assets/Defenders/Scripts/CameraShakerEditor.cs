using System.Collections;
using System.Collections.Generic;
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

            CameraController ctrl = (CameraController)target;
            if (GUILayout.Button("Shake"))
            {
                ctrl.ShakeIt();
            }
        }
    }
}
