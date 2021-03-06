﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    /// <summary>
    /// 相机上升速度
    /// </summary>
    private float addSpeed = 2.0f;

    /// <summary>
    /// 是否增加
    /// </summary>
    private bool adding = false;

    /// <summary>
    /// 初始的属性
    /// </summary>
    private Vector3 oldPos;

    private float lastY;


    private void Awake()
    {
        oldPos = gameObject.transform.localPosition;
        lastY = oldPos.y;
        EventManager.RegistAction(Events.AddTowerGo, this.OnAddTowerGo);
        EventManager.RegistAction(Events.ClearTowerGo, this.OnClearTowerGo);
    }


    /// <summary>
    /// 当增加一个物体事件
    /// </summary>
    /// <param name="value"></param>
    private void OnAddTowerGo(object value) {
        adding = true;
    }

    /// <summary>
    /// 清理
    /// </summary>
    /// <param name="value"></param>
    private void OnClearTowerGo(object value) {
        gameObject.transform.localPosition = oldPos;
        lastY = oldPos.y;
    }


    private void Update()
    {
        if (adding == true) {
            gameObject.transform.localPosition += Vector3.up * addSpeed * Time.deltaTime;
            if (gameObject.transform.localPosition.y - lastY >= GameConfig.goY) {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,
                    lastY + GameConfig.goY, gameObject.transform.localPosition.z);
                lastY = gameObject.transform.localPosition.y;
                adding = false;
                EventManager.CallAction(Events.AddTowerGoFinish, null);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveOneAction(Events.AddTowerGo, this.OnAddTowerGo);
    }
}
