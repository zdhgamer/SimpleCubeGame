using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    /// <summary>
    /// 是否可以移动
    /// </summary>
    private bool canMove = false;

    /// <summary>
    /// X轴的宽度
    /// </summary>
    private float xWidth = 5.0f;

    /// <summary>
    /// y轴的宽度
    /// </summary>
    private float yWidth = 5.0f;

    /// <summary>
    /// 移动速度
    /// </summary>
    private float moveSpeed = 1.0f;


    /// <summary>
    /// 当前移动方向
    /// </summary>
    private MoveEnumDirEnum nowMoveDir = MoveEnumDirEnum.XDir;

    public bool CanMove
    {
        get
        {
            return canMove;
        }

        set
        {
            canMove = value;
        }
    }

    public float XWidth
    {
        get
        {
            return xWidth;
        }

        set
        {
            xWidth = value;
        }
    }

    public float YWidth
    {
        get
        {
            return yWidth;
        }

        set
        {
            yWidth = value;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }

        set
        {
            moveSpeed = value;
        }
    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="moveEnumDirEnum"></param>
    public void SetInitData(Vector3 pos,Vector3 size,float moveSpeed,MoveEnumDirEnum moveEnumDirEnum) {
        this.gameObject.transform.localPosition = pos;
        this.gameObject.transform.localScale = size;
        this.moveSpeed = moveSpeed;
        this.nowMoveDir = moveEnumDirEnum;
    }

    private void Update()
    {
        if (canMove) {
            switch (nowMoveDir) {
                //x轴负移动
                case MoveEnumDirEnum.SubXDir:
                    gameObject.transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;
                    if (gameObject.transform.localPosition.x<=-GameConfig.maxXCanMove) {
                        nowMoveDir = MoveEnumDirEnum.XDir;
                    }
                    break;
                //z负移动
                case MoveEnumDirEnum.SubZDir:
                    gameObject.transform.localPosition += Vector3.back * moveSpeed * Time.deltaTime;
                    if (gameObject.transform.localPosition.z <= -GameConfig.maxZCanMove)
                    {
                        nowMoveDir = MoveEnumDirEnum.ZDir;
                    }
                    break;
                //x正移动
                case MoveEnumDirEnum.XDir:
                    gameObject.transform.localPosition += Vector3.right * moveSpeed * Time.deltaTime;
                    if (gameObject.transform.localPosition.x >= GameConfig.maxXCanMove)
                    {
                        nowMoveDir = MoveEnumDirEnum.SubXDir;
                    }
                    break;
                //z正移动
                case MoveEnumDirEnum.ZDir:
                    gameObject.transform.localPosition += Vector3.forward * moveSpeed * Time.deltaTime;
                    if (gameObject.transform.localPosition.z >= GameConfig.maxZCanMove)
                    {
                        nowMoveDir = MoveEnumDirEnum.SubZDir;
                    }
                    break;
            }
        }
    }
}
