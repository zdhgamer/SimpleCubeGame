using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour {

    /// <summary>
    /// 重叠在一起的GameObject
    /// </summary>
    private List<GameObject> mToweredGo = new List<GameObject>();

    /// <summary>
    /// 上一次移动方向
    /// </summary>
    private MoveEnumDirEnum lastStartDir = MoveEnumDirEnum.XDir;

    /// <summary>
    /// 上次的大小
    /// </summary>
    private Vector3 lastSize = new Vector3(GameConfig.goX, GameConfig.goY, GameConfig.goZ);

    /// <summary>
    /// 上次的移动速度
    /// </summary>
    private float lastMoveSpeed = 1.0f;

    /// <summary>
    /// 单例
    /// </summary>
    private static GameLogicManager instance;

    public static GameLogicManager Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        EventManager.RegistAction(Events.GameOver, this.OnGameOver);
        EventManager.RegistAction(Events.ReStartGame, this.OnReStartGame);
        EventManager.RegistAction(Events.StartGame, this.OnStartGame);
        EventManager.RegistAction(Events.AddTowerGoFinish, this.OnAddTowerGoFinish);
        EventManager.RegistAction(Events.SceneTouched, this.OnSceneTouched);

    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="value"></param>
    private void OnStartGame(object value) {
        StartGame();
    }


    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="value"></param>
    private void OnGameOver(object value) {

    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    private void OnReStartGame(object value) {
        StartGame();
    }

    /// <summary>
    /// 新增一个物体结束
    /// </summary>
    /// <param name="value"></param>
    private void OnAddTowerGoFinish(object value) {

    }

    /// <summary>
    /// 屏幕点击
    /// </summary>
    /// <param name="value"></param>
    private void OnSceneTouched(object value) {
        Cube movingCube = GetLastMovingCube();
        if (movingCube!=null) {
            movingCube.CanMove = false;
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void StartGame()
    {
        ClearTowerGo();
        GameObject baseOne = Resources.Load<GameObject>("Cube");
        GameObject one = InstantiateBaseOne(baseOne);
        one.GetComponent<Cube>().CanMove = false;
        GameObject two = InstantiateBaseOne(baseOne);
        Cube cube = two.GetComponent<Cube>();
        InitMoveDir(cube);
    }

    /// <summary>
    /// 初始化移动
    /// </summary>
    /// <param name="cube"></param>
    private void InitMoveDir(Cube cube) {
        cube.CanMove = true;
        if (lastStartDir == MoveEnumDirEnum.XDir)
        {
            lastStartDir = MoveEnumDirEnum.SubZDir;
        }
        else {
            lastStartDir = MoveEnumDirEnum.XDir;
        }

        switch (lastStartDir) {
            //x正方向 右下
            case MoveEnumDirEnum.XDir:
                cube.SetInitData(Vector3.zero + new Vector3(-GameConfig.maxXCanMove, mToweredGo.Count * .5f, 0), lastSize, lastMoveSpeed, MoveEnumDirEnum.XDir);
                break;
            //z负方向 左下
            case MoveEnumDirEnum.SubZDir:
                cube.SetInitData(Vector3.zero + new Vector3(0, mToweredGo.Count * .5f, GameConfig.maxZCanMove), lastSize, lastMoveSpeed, MoveEnumDirEnum.SubZDir);
                break;
        }
    }

    /// <summary>
    /// 生成一个方块
    /// </summary>
    /// <param name="baseOne"></param>
    /// <param name="localPos"></param>
    /// <returns></returns>
    private GameObject InstantiateBaseOne(GameObject baseOne)
    {
        GameObject one = Instantiate(baseOne);
        one.SetActive(true);
        one.transform.localPosition = Vector3.zero + new Vector3(0, mToweredGo.Count* .5f, 0);
        one.transform.localRotation = Quaternion.identity;
        mToweredGo.Add(one);
        return one;
    }

    /// <summary>
    /// 清空已经存在于场景中的物体
    /// </summary>
    private void ClearTowerGo() {
        for (int i=0;i< mToweredGo.Count;i++) {
            DestroyImmediate(mToweredGo[i]);
            mToweredGo[i] = null;
        }
        mToweredGo.Clear();
    }

    /// <summary>
    /// 获取最后一个正在移动的物体
    /// </summary>
    /// <returns></returns>
    private Cube GetLastMovingCube() {
        if (mToweredGo != null)
        {
            return mToweredGo[mToweredGo.Count - 1].GetComponent<Cube>();
        }
        else {
            return null;
        }
    }
    

    private void OnDestroy()
    {
        EventManager.RemoveOneAction(Events.GameOver, this.OnGameOver);
        EventManager.RemoveOneAction(Events.ReStartGame, this.OnReStartGame);
        EventManager.RemoveOneAction(Events.StartGame, this.OnStartGame);
        EventManager.RemoveOneAction(Events.AddTowerGoFinish, this.OnAddTowerGoFinish);
        EventManager.RemoveOneAction(Events.SceneTouched, this.OnSceneTouched);
    }
}
