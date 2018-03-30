using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TileObject))]
public class TileEditor : Editor {

    protected bool editMode = false;    //edit mode flag
    protected TileObject tileObject;    //affected script
    private void OnEnable() {
        tileObject = (TileObject)target;
    }
    public void OnSceneGUI() {
        if (editMode) {
            //cancel select func
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            //show data(help line) in editor
            tileObject.debug = true;
            //get input event
            Event e = Event.current;

            if (e.button == 0 && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && !e.alt) {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray,out hitinfo, 2000, tileObject.tileLayer)) {
                    tileObject.setDataFromPosition(hitinfo.point.x, hitinfo.point.z, tileObject.dataID);
                }
            }
        }
        HandleUtility.Repaint();
    }

    //Inspector UI window
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        GUILayout.Label("Tile Editor");
        editMode = EditorGUILayout.Toggle("Edit", editMode);
        tileObject.debug = EditorGUILayout.Toggle("Debug", tileObject.debug);

        string[] editDataStr = { "Dead", "Road", "Guard" };
        tileObject.dataID = GUILayout.Toolbar(tileObject.dataID, editDataStr);

        EditorGUILayout.Separator();
        if (GUILayout.Button("Reset")) {
            tileObject.Reset();
        }
        DrawDefaultInspector();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
