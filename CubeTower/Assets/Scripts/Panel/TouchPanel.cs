using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPanel : BasePanel
{
    public override void RegistAction()
    {
        base.RegistAction();
        EventManager.RegistAction(Events.GameOver, this.OnGameOver);
        EventManager.RegistAction(Events.ReStartGame, this.OnReStartGame);
        EventManager.RegistAction(Events.StartGame, this.OnStartGame);
    }

    public override void RemoveAction()
    {
        base.RemoveAction();
        EventManager.RemoveOneAction(Events.GameOver, this.OnGameOver);
        EventManager.RemoveOneAction(Events.ReStartGame, this.OnReStartGame);
        EventManager.RemoveOneAction(Events.StartGame, this.OnStartGame);
    }

    /// <summary>
    /// 返回按钮点击事件
    /// </summary>
    public void OnBackClick() {

    }

    /// <summary>
    /// 屏幕点击事件
    /// </summary>
    public void OnSceneTouchClick()
    {
        EventManager.CallAction(Events.SceneTouched, null);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="value"></param>
    private void OnStartGame(object value)
    {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="value"></param>
    private void OnGameOver(object value)
    {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    private void OnReStartGame(object value)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}
