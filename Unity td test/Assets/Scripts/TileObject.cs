using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour {

    public static TileObject Instance = null;
    public LayerMask tileLayer;
    public float tileSize = 1;
    public int xTileCount = 2;
    public int zTileCount = 2;
    public int[] data;              //0 is locked,1 is enemy road,2 is player road
    [HideInInspector]
    public int dataID = 0;
    [HideInInspector]
    public bool debug = false;
    private void Awake() {
        Instance = this;
    }
    public void Reset() {
        data = new int[xTileCount * zTileCount];
    }
    private void OnDrawGizmos() {
        if (!debug) return;
        if (data == null) {
            Debug.Log("Please reset data first");
            return;
        }
        Vector3 pos = transform.position;
        DrawZdirHelpLine(pos);
        DrawXdirHelpLine(pos);
    }



    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int getDataFromPosition(float posx, float posz) {
        int index = getIndexByPos(posx, posz);
        if (isIndexError(index)) return 0;
        return data[index];
    }
    public void setDataFromPosition(float posx, float posz, int number) {
        int index = getIndexByPos(posx, posz);
        if (isIndexError(index)) return;
        data[index] = number;
    }





    #region Extra Method

    private int getIndexByPos(float posx, float posz) {
        return (int)((posx - transform.position.x) / tileSize) * zTileCount + (int)((posz - transform.position.z) / tileSize);
    }
    private bool isIndexError(int index) {
        return index < 0 || index >= data.Length;
    }

    private void DrawZdirHelpLine(Vector3 pos) {
        for (int i = 0; i < xTileCount; i++) {
            Gizmos.color = new Color(0, 0, 1, 1);
            Gizmos.DrawLine(pos + new Vector3(tileSize * i, pos.y, 0),
                            transform.TransformPoint(tileSize * i, pos.y, tileSize * zTileCount));

            for (int k = 0; k < zTileCount; k++) {
                if ((i * zTileCount + k) < data.Length && data[i * zTileCount + k] == dataID) {
                    Gizmos.color = new Color(1, 0, 0, 0.3f);
                    Gizmos.DrawCube(new Vector3(pos.x + i * tileSize + tileSize * 0.5f,
                                    pos.y, pos.z + k * tileSize + tileSize * 0.5f),
                                    new Vector3(tileSize, 0.2f, tileSize));
                }
            }
        }
    }

    private void DrawXdirHelpLine(Vector3 pos) {
        for (int k = 0; k < zTileCount; k++) {
            Gizmos.color = new Color(0, 0, 1, 1);
            Gizmos.DrawLine(pos + new Vector3(0, pos.y, tileSize * k),
                           transform.TransformPoint(tileSize * xTileCount, pos.y, tileSize * k));
        }
    }
    #endregion
}
