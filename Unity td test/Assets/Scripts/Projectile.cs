using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    System.Action<Enemy> onAttack;

    Transform m_target;
    Bounds m_attackCenter;

    public static void Create(Transform target,Vector3 spawnPos, System.Action<Enemy> onAttack) {
        //load arrow model
        GameObject arrowPrefab = Resources.Load<GameObject>("arrow");
        GameObject go = (GameObject)Instantiate(arrowPrefab, spawnPos,
                        Quaternion.LookRotation(target.position - spawnPos));
        //add arrow scr arrow
        Projectile arrowmodel = go.AddComponent<Projectile>();
        //set arrow
        arrowmodel.m_target = target;
        arrowmodel.m_attackCenter = target.GetComponentInChildren<SkinnedMeshRenderer>().bounds;

        arrowmodel.onAttack = onAttack;
        Destroy(go, 3.0f);
    }

	void Update () {
        if (HasTarget()) {
            this.transform.LookAt(m_attackCenter.center);
            if (EnemyIntoAttackRange()) {
                onAttack(m_target.GetComponent<Enemy>());
                Destroy(this.gameObject);
            }
        }
        this.transform.Translate(new Vector3(0, 0, 10 * Time.deltaTime));
    }

    #region Extra Method
    private bool EnemyIntoAttackRange() {
        return Vector3.Distance(this.transform.position, m_attackCenter.center) < 0.5f;
    }

    private bool HasTarget() {
        return m_target != null;
    }
    #endregion
}
