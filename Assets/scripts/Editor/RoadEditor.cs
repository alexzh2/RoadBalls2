﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Road))]
public class RoadEditor : Editor
{
    Road road;
    void OnEnable()
    {
        road = (Road)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Update road"))
        {
            Undo.RecordObject(road, "update_road");
            road.UpdateRoad();
        }
        else if (GUILayout.Button("Generate points"))
        {
            Undo.RecordObject(road, "generate_points");
            road.GeneratePoints(5);
        }
        if (GUILayout.Button("Add point"))
        {
            Undo.RecordObject(road, "add_point");
            road.AddPoint();
        }
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI()
    {
        Draw();
        
    }

    void Draw()
    {
        Handles.color = Color.blue;
        var points = road.GetMainPoints();
        for (int i = 0; i < points.Count; i++)
        {
            Handles.FreeMoveHandle(points[i], Quaternion.identity, 0.5f, Vector3.zero, Handles.SphereHandleCap);
        }

        var path = road.GetPath();
        for (int i = 0; i < path.Count; i++)
        {
            Handles.color = Color.green;
            Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);

            if (i > 0)
            {
                Vector3 forward = path[i] - path[i - 1];
                var left = Vector3.Cross(Vector3.up, forward).normalized;
                var right = Vector3.Cross(forward, Vector3.up).normalized;

                Handles.FreeMoveHandle(path[i]+left, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
                Handles.FreeMoveHandle(path[i]+right, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
            }
        }
    }
}
