using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public PathNode m_currentNode;
    public int m_life = 15;
    public int m_maxlife = 15;
    public float m_speed = 1;
    public System.Action<Enemy> onDeath;

    protected Transform m_lifebarObj;
    UnityEngine.UI.Slider m_lifebar;

    private void Start() {
        GameManager.Instance.m_EnemyList.Add(this);

        //load lifebar prefab
        GameObject prefab = (GameObject)Resources.Load("Canvas3D");
        m_lifebarObj = ((GameObject)Instantiate(prefab, Vector3.zero, Camera.main.transform.rotation, this.transform)).transform;
        SetBarLocalPosAndScale();
        m_lifebar = m_lifebarObj.GetComponentInChildren<UnityEngine.UI.Slider>();
        StartCoroutine(UpdateLifebar());

    }


    void Update () {
        RotateTo();
        MoveTo();
	}

    public void RotateTo() {
        var position = m_currentNode.transform.position - transform.position;
        position.y = 0;
        var targetRotation = Quaternion.LookRotation(position);
        float next = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, 240 * Time.deltaTime);
        this.transform.eulerAngles = new Vector3(0, next, 0);
    }
    public void MoveTo() {
        Vector3 pos1 = this.transform.position;
        Vector3 pos2 = m_currentNode.transform.position;
        float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));
        if (dist < 0.3f) {
            if(m_currentNode.m_next == null) {
                GameManager.Instance.SetDamage(1);
                DestroyMe();
            } else {
                Debug.Log(m_currentNode.name);
                m_currentNode = m_currentNode.m_next;
            }
            
        }
        this.transform.Translate(new Vector3(0, 0, m_speed * Time.deltaTime));
    }

    public void DestroyMe() {
        GameManager.Instance.m_EnemyList.Remove(this);
        onDeath(this);
        Destroy(this.gameObject);
    }

    public void setDamage(int damage) {
        m_life -= damage;
        if(m_life <= 0) {
            m_life = 0;
            GameManager.Instance.SetPoint(5);
            DestroyMe();
        }
    }

    IEnumerator UpdateLifebar() {
        //renew life value
        m_lifebar.value = (float)m_life / (float)m_maxlife;
        //face camera
        m_lifebarObj.transform.eulerAngles = Camera.main.transform.eulerAngles;
        yield return 0; //no waiting
        StartCoroutine(UpdateLifebar());
    }

    protected virtual void SetBarLocalPosAndScale() {
        m_lifebarObj.localPosition = new Vector3(0, 150f, 40f);
        m_lifebarObj.localScale = new Vector3(2f, 2f, 2f);
    }
}
