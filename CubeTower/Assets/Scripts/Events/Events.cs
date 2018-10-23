using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour {

    /// <summary>
    /// StartGame 开始游戏
    /// </summary>
    public const string StartGame = "StartGame";

    /// <summary>
    /// 游戏结束
    /// </summary>
    public const string GameOver = "GameOver";

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public const string ReStartGame = "ReStartGame";

    /// <summary>
    /// 返回主界面
    /// </summary>
    public const string BackMain = "BackMain";

    /// <summary>
    /// 清理物体
    /// </summary>
    public const string ClearTowerGo = "ClearTowerGo";

    /// <summary>
    /// 新增一个物体
    /// </summary>
    public const string AddTowerGo = "AddTowerGo";

    /// <summary>
    /// 增加物体结束
    /// </summary>
    public const string AddTowerGoFinish = "AddTowerGoFinish";

    /// <summary>
    /// 屏幕点击
    /// </summary>
    public const string SceneTouched = "SceneTouched";

}
