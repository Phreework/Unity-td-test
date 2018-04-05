using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : Enemy {

	void Update () {
        RotateTo();
        MoveTo();
        Fly();
	}
    public void Fly() {
        float flyspeed = 0;
        if (this.transform.position.y <= 2.1f) {
            flyspeed = 1.0f;
        }
        this.transform.Translate(new Vector3(0, flyspeed * Time.deltaTime, 0));
    }
    protected override void SetBarLocalPosAndScale() {
        m_lifebarObj.localPosition = new Vector3(0, 2f, 0);
        m_lifebarObj.localScale = new Vector3(0.02f, 0.02f, 0.02f);
    }
}
