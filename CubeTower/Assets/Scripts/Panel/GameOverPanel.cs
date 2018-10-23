using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : BasePanel
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
    public void OnReStartClick()
    {
        gameObject.SetActive(false);
        EventManager.CallAction(Events.ReStartGame, null);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="value"></param>
    private void OnStartGame(object value)
    {
        hideSelfPanel();
    }


    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="value"></param>
    private void OnGameOver(object value)
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    private void OnReStartGame(object value)
    {
        hideSelfPanel();
    }

    /// <summary>
    /// 隐藏自己
    /// </summary>
    private void hideSelfPanel() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }
}
