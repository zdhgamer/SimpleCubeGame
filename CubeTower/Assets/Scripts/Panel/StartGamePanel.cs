using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGamePanel : BasePanel
{

    public override void RegistAction()
    {
        base.RegistAction();
        EventManager.RegistAction(Events.BackMain, this.OnBackMain);
    }

    public override void RemoveAction()
    {
        base.RemoveAction();
        EventManager.RemoveOneAction(Events.BackMain, this.OnBackMain);
    }


    /// <summary>
    /// 开始游戏按钮点击
    /// </summary>
    public void OnStartGameBtnClick() {
        this.gameObject.SetActive(false);
        EventManager.CallAction(Events.StartGame, null);
    }


    /// <summary>
    /// 返回主界面
    /// </summary>
    /// <param name="value"></param>
    public void OnBackMain(object value) {
        gameObject.SetActive(true);
    }


}
