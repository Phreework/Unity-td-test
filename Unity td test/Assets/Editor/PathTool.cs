using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathTool : ScriptableObject {
    static PathNode m_parent = null;
    static int PathNodeNumber = 0;
    [MenuItem("PathTool/Create PathNode")]
    static void CreatePathNode() {
        //create a new pathnode
        GameObject go = new GameObject();
        go.AddComponent<PathNode>();
        go.name = "pathnode";
        go.name = "pathnode" + PathNodeNumber;
        go.tag = "pathnode";
        //select pathnode
        Selection.activeTransform = go.transform;
        SetSelfNextNode();
        SetSelfParentNode();
        PathNodeNumber++;
    }

    private static void SetSelfParentNode() {
        //set self parent
        m_parent = Selection.activeGameObject.GetComponent<PathNode>();
    }

    private static void SetSelfNextNode() {
        if (PathNodeNumber != 0) {
            m_parent.setNext(Selection.activeGameObject.GetComponent<PathNode>());
        }
    }

    [MenuItem("PathTool/Set Parent %q")]
    static void SetParent() {
        if (!Selection.activeGameObject || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1) return;
        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0) {
            m_parent = Selection.activeGameObject.GetComponent<PathNode>();
        }
    }
    [MenuItem("PathTool/Set Next %w")]
    static void SetNextChild() {
        if (!Selection.activeGameObject || m_parent == null || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1) return;
        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0) {
            m_parent.setNext(Selection.activeGameObject.GetComponent<PathNode>());
            m_parent = null;
        }
    }
    [MenuItem("PathTool/name zero %e")]
    static void SetPathnodenumberZero() {
        PathNodeNumber = 0;
        }


}
