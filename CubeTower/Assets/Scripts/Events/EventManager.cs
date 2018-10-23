using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    private static Dictionary<string, List<Action<object>>> eventsDic = new Dictionary<string, List<Action<object>>>();

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void RegistAction(string key,Action<object> value) {
        if (eventsDic.ContainsKey(key))
        {
            List<Action<object>> actions = eventsDic[key];
            if (actions == null) {
                actions = new List<Action<object>>();
            }
            if (!actions.Contains(value))
            {
                actions.Add(value);
            }
        }
        else {
            List<Action<object>> actions = new List<Action<object>>();
            actions.Add(value);
            eventsDic.Add(key, actions);
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void CallAction(string key,object value) {
        if (eventsDic.ContainsKey(key)) {
            List<Action<object>> actions = eventsDic[key];
            if (actions!=null) {
                for (int i=0;i< actions.Count;i++) {
                    actions[i](value);
                }
            }
        }
    }

    /// <summary>
    /// 移除一个事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void RemoveOneAction(string key, Action<object> value) {
        if (eventsDic.ContainsKey(key)) {
            List<Action<object>> actions = eventsDic[key];
            if (actions != null) {
                if (actions.Contains(value)) {
                    actions.Remove(value);
                }
            }
            if (actions == null || actions.Count<=0) {
                eventsDic.Remove(key);
            }
        }
    }

    /// <summary>
    /// 清理一个key下面的所有事件
    /// </summary>
    /// <param name="key"></param>
    public static void ClearByKey(string key) {
        eventsDic.Remove(key);
    }
}
