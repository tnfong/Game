using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]

public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumsInputFields();
        EditorGUILayout.Space();

        if (ShapeDataInstance.board != null && ShapeDataInstance.colums > 0 && ShapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            ShapeDataInstance.Clear();
        }
    }

    private void DrawColumsInputFields()
    {
        var columsTemp = ShapeDataInstance.colums;
        var rowsTemp = ShapeDataInstance.rows;

        ShapeDataInstance.colums = EditorGUILayout.IntField("Colums", ShapeDataInstance.colums);
        ShapeDataInstance.rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.rows);

        if ((ShapeDataInstance.colums != columsTemp || ShapeDataInstance.rows != rowsTemp) && ShapeDataInstance.colums > 0 && ShapeDataInstance.rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumStyle = new GUIStyle();
        headerColumStyle.fixedWidth = 65;
        headerColumStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        headerColumStyle.fixedWidth = 25;
        headerColumStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (var row = 0; row < ShapeDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumStyle);

            for (var colum = 0; colum < ShapeDataInstance.colums; colum++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);

                var data = EditorGUILayout.Toggle(ShapeDataInstance.board[row].colum[colum], dataFieldStyle);
                ShapeDataInstance.board[row].colum[colum] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();

        }
    }
}
