using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Defender {

    protected override void Init() {
        m_attackArea = 5.0f;
        m_power = 1;
        m_attackInterval = 1.0f;

        CreateModel("archer");
        StartCoroutine(Attack());
    }

    protected override void CauseDamage() {
        Vector3 attackPointPos = this.m_model.transform.FindChild("atkpoint").position;
        Projectile.Create(m_targetEnemy.transform, attackPointPos, (Enemy enemy) => {
            enemy.setDamage(m_power);
            m_targetEnemy = null;
        });
    }
}
