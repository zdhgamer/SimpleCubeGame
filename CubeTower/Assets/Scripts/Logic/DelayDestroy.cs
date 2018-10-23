using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //从出现开始计时，10秒后销毁自己
        Destroy(this.gameObject, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
