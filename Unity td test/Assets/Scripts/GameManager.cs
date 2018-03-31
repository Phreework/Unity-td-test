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

        GameObject uicanvas = GameObject.Find("Canvas");
        Transform[] UIchildren = GetAllUICom(uicanvas);
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
            }//////////////////////////////////////////
        }
    }





    // Update is called once per frame
    void Update () {
		
	}
    public void SetWave(int wave) {
        m_wave = wave;
        m_txt_wave.text = string.Format("波数：<color=yellow>{0}/{1}</color>", m_wave, m_waveMax);
    }
    public void SetDamage(int damage) {
        m_life -= damage;
        if(m_life <= 0) {
            m_life = 0;
            m_but_try.gameObject.SetActive(true);
        }
        m_txt_life.text = string.Format("生命：<color=yellow>{0}</color>",m_life);
    }
    public bool SetPoint(int point) {
        if (m_point + point < 0) {
            return false;
        }
        m_point += point;
        m_txt_point.text = string.Format("铜钱：<color=yellow>{0}</color>",m_point);
        return true;
    }
    void OnButCreateDefenderDown(BaseEventData data) {
        m_isSelectedSoldierButton = true;
    }
    void OnButCreateDefenderUp(BaseEventData data) {
        GameObject go = data.selectedObject;
        //not yet
    }


    #region Extra Method
    private static Transform[] GetAllUICom(GameObject uicanvas) {
        return uicanvas.GetComponentsInChildren<Transform>();
    }
    #endregion

}
