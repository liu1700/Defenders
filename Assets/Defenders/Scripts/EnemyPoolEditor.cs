using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyPoolEditor : Editor
{

    [CustomEditor(typeof(EnemyPool))]
    public class ObjectBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EnemyPool pool = (EnemyPool)target;
            if (GUILayout.Button("Generate Level props"))
            {
                pool.GenerateEnemyInfoPerLv();
            }
        }
    }
}
