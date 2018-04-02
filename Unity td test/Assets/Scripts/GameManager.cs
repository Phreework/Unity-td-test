using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public int m_wave = 1;
    public int m_waveMax = 10;
    public int m_life = 10;
    public int m_point = 30;

    //UI textCOM
    Text m_txt_wave;
    Text m_txt_life;
    Text m_txt_point;
    //UI restart button
    Button m_but_try;
    //flag select button
    bool m_isSelectedSoldierButton = false;

    //pathNode line switch
    public bool m_debug = true;
    //pathnode
    public List<PathNode> m_PathNodes;

    private void Awake() {
        Instance = this;
    }
    void Start () {
        //create UnityAction
        UnityAction<BaseEventData>downAction = 
            new UnityAction<BaseEventData>(OnButCreateDefenderDown);
        UnityAction<BaseEventData>upAction =
            new UnityAction<BaseEventData>(OnButCreateDefenderUp);

        EventTrigger.Entry down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener(downAction);

        EventTrigger.Entry up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener(upAction);


        //Transform[] UIchildren = this.GetComponentsInChildren<Transform>();             remember this emmmmm
        //Transform[] UIchildren2 = this.GetComponentInChildren<Transform>();
        Transform[] UIchildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform t in UIchildren) {
            if (t.name.CompareTo("wave") == 0) {
                m_txt_wave = t.GetComponent<Text>();
                SetWave(1);
            } else if (t.name.CompareTo("life") == 0) {
                m_txt_life = t.GetComponent<Text>();
                m_txt_life.text = string.Format("生命：<color=yellow>{0}</color>", m_life);
            } else if (t.name.CompareTo("point") == 0) {
                m_txt_point = t.GetComponent<Text>();
                m_txt_point.text = string.Format("铜钱：<color=yellow>{0}</color>", m_point);
            } else if (t.name.CompareTo("but_try") == 0) {
                m_but_try = t.GetComponent<Button>();
                m_but_try.onClick.AddListener(delegate () {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                m_but_try.gameObject.SetActive(false);
            }else if (t.name.Contains("but_player")) {
                //add soldier event
                EventTrigger trigger = t.gameObject.AddComponent<EventTrigger>();
                trigger.triggers = new List<EventTrigger.Entry>();
                trigger.triggers.Add(down);
                trigger.triggers.Add(up);
            }
        }
    }






    private void Update() {
        if (m_isSelectedSoldierButton) return;
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        bool press = Input.touches.Length > 0 ? true :false;
        float mx = 0;
        float my = 0;
        if(press) {
            if (Input.GetTouch(0).phase == TouchPhase.Moved) {
                mx = Input.GetTouch(0).deltaPosition.x * 0.01f;
                my = Input.GetTouch(0).deltaPosition.y * 0.01f;
            }
        }
#else 
        bool press = Input.GetMouseButton(0);
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
#endif
        GameCamera.Inst.Control(press, mx, my);
    }

    private void OnDrawGizmos() {
        if (!m_debug || m_PathNodes == null) return;
        Gizmos.color = Color.yellow;
        DrawAdvanceLine();
    }



    public void SetWave(int wave) {
        m_wave = wave;
        m_txt_wave.text = string.Format("波数：<color=yellow>{0}/{1}</color>", m_wave, m_waveMax);
    }
    public void SetDamage(int damage) {
        m_life -= damage;
        if (m_life <= 0) {
            m_life = 0;
            m_but_try.gameObject.SetActive(true);
        }
        m_txt_life.text = string.Format("生命：<color=yellow>{0}</color>", m_life);
    }
    public bool SetPoint(int point) {
        if (m_point + point < 0) {
            return false;
        }
        m_point += point;
        m_txt_point.text = string.Format("铜钱：<color=yellow>{0}</color>", m_point);
        return true;
    }



    void OnButCreateDefenderDown(BaseEventData data) {
        m_isSelectedSoldierButton = true;
    }
    void OnButCreateDefenderUp(BaseEventData data) {
        GameObject go = data.selectedObject;
        //not yet
    }



    [ContextMenu("BuildPath")]
    void BuildPath() {
        m_PathNodes = new List<PathNode>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");
        for(int i = 0;i < objs.Length; i++) {
            m_PathNodes.Add(objs[i].GetComponent<PathNode>());
        }
    }


    #region Extra Method
    private void DrawAdvanceLine() {
        foreach (PathNode node in m_PathNodes) {
            if (node.m_next != null) {
                Gizmos.DrawLine(node.transform.position, node.m_next.transform.position);
            }
        }
    }

    #endregion

}
