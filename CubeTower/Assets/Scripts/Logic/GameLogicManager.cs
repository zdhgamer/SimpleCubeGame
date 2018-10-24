using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour {

    /// <summary>
    /// 重叠在一起的GameObject
    /// </summary>
    private List<GameObject> mToweredGoList = new List<GameObject>();

    /// <summary>
    /// 废弃的GameObject 集合
    /// </summary>
    private List<GameObject> mDisGoList = new List<GameObject>();

    /// <summary>
    /// 上一次移动方向
    /// </summary>
    private MoveEnumDirEnum lastMoveDir = MoveEnumDirEnum.XDir;

    /// <summary>
    /// 上次的大小
    /// </summary>
    private Vector3 lastSize = new Vector3(GameConfig.goX, GameConfig.goY, GameConfig.goZ);

    /// <summary>
    /// 上次的移动速度
    /// </summary>
    private float lastMoveSpeed = 1.0f;

    /// <summary>
    /// 上一次放置上去的位置
    /// </summary>
    private Vector3 lastCenter = Vector3.zero;

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
        EventManager.RegistAction(Events.ReStartGame, this.OnReStartGame);
        EventManager.RegistAction(Events.StartGame, this.OnStartGame);
        EventManager.RegistAction(Events.AddTowerGoFinish, this.OnAddTowerGoFinish);
        EventManager.RegistAction(Events.SceneTouched, this.OnSceneTouched);
        EventManager.RegistAction(Events.BackMain, this.OnBackToMain); 
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="value"></param>
    private void OnStartGame(object value) {
        StartGame();
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
        GameObject cubeGo = InstantiateBaseOne();
        Cube cube = cubeGo.GetComponent<Cube>();
        mToweredGoList.Add(cubeGo);
        InitMoveDir(cube);

    }

    /// <summary>
    /// 屏幕点击
    /// </summary>
    /// <param name="value"></param>
    private void OnSceneTouched(object value) {

        Cube movingCube = GetLastMovingCube();
        Cube notMove = GetLastTowerCube();

        if (movingCube!=null) {
            movingCube.CanMove = false;
        }

        if (CanPutDown())
        {
            Vector3 putCenterTemp;
            float one, two;
            GetCanPutCubeSizePos(movingCube, notMove, out putCenterTemp, out one, out two);
            //生成可以放置的cube
            Vector3 putSize = NewCanPutCube(movingCube, putCenterTemp, one, two);
            //计算废弃的那个的位置和大小
            Vector3 desTemp = GetCanDisCubePos(movingCube, one);
            //计算废弃的那个cube的大小
            NewDisCube(one, desTemp);
            mToweredGoList.Remove(movingCube.gameObject);
            Destroy(movingCube.gameObject);
            EventManager.CallAction(Events.AddTowerGo, null);
            lastSize = putSize;
            Debug.Log("上一次的大小："+ lastSize);
            Debug.Log("上一次的位置：" + lastCenter);
            Debug.Log("list的大小为："+mToweredGoList.Count);
        }
        else {//游戏结束
            movingCube.CanDownMove = true;
            EventManager.CallAction(Events.GameOver, mToweredGoList.Count-2);
        }
    }

    /// <summary>
    /// 返回主界面
    /// </summary>
    /// <param name="value"></param>
    private void OnBackToMain(object value) {
        ClearTowerGo();
    }

    /// <summary>
    /// 生成废弃的那个cube
    /// </summary>
    /// <param name="one"></param>
    /// <param name="desTemp"></param>
    private void NewDisCube(float one, Vector3 desTemp)
    {
        float three = 0.0f;
        float four = 0.0f;
        GameObject newDisgo = InstantiateBaseOne();
        newDisgo.transform.localPosition = desTemp;
        Cube newDisCube = newDisgo.GetComponent<Cube>();
        newDisCube.CanMove = false;
        newDisCube.CanDownMove = true;

        if (lastMoveDir == MoveEnumDirEnum.SubXDir || lastMoveDir == MoveEnumDirEnum.XDir)
        {
            three = lastSize.x - one;
            four = lastSize.z;
            newDisCube.transform.localScale = new Vector3(three, GameConfig.goY, four);
        }
        else
        {
            three = lastSize.z - one;
            four = lastSize.x;
            newDisCube.transform.localScale = new Vector3(four, GameConfig.goY, three);
        }
        mDisGoList.Add(newDisgo);
    }

    /// <summary>
    /// 获取可以废弃的那个cube的位置
    /// </summary>
    /// <param name="movingCube"></param>
    /// <param name="one"></param>
    /// <returns></returns>
    private static Vector3 GetCanDisCubePos(Cube movingCube, float one)
    {
        Vector3 desTemp = movingCube.transform.localPosition;
        switch (movingCube.NowMoveDir)
        {
            case MoveEnumDirEnum.SubXDir:
                desTemp += new Vector3(one * 0.5f, 0, 0);
                break;
            case MoveEnumDirEnum.XDir:
                desTemp -= new Vector3(one * 0.5f, 0, 0);
                break;
            case MoveEnumDirEnum.SubZDir:
                desTemp += new Vector3(0, 0, one * 0.5f);
                break;
            case MoveEnumDirEnum.ZDir:
                desTemp -= new Vector3(0, 0, one * 0.5f);
                break;
        }

        return desTemp;
    }

    /// <summary>
    /// 生成新的那个可以放置的cube
    /// </summary>
    /// <param name="movingCube"></param>
    /// <param name="putCenterTemp"></param>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    private Vector3 NewCanPutCube(Cube movingCube, Vector3 putCenterTemp, float one, float two)
    {
        movingCube.gameObject.SetActive(false);
        GameObject newPut = InstantiateBaseOne();
        Cube newPutCube = newPut.GetComponent<Cube>();
        mToweredGoList.Add(newPut);
        newPutCube.CanMove = false;
        //真正的放置位置
        Vector3 putCenter = new Vector3(putCenterTemp.x, movingCube.transform.localPosition.y, putCenterTemp.z);
        lastCenter = putCenter;
        newPut.transform.localPosition = putCenter;
        Vector3 putSize = Vector3.zero;
        if (lastMoveDir == MoveEnumDirEnum.SubXDir || lastMoveDir == MoveEnumDirEnum.XDir)
        {
            putSize = new Vector3(one, GameConfig.goY, two);
            newPut.transform.localScale = putSize;
        }
        else
        {
            putSize = new Vector3(two, GameConfig.goY, one);
            newPut.transform.localScale = putSize;
        }

        return putSize;
    }

    /// <summary>
    /// 计算可以放置的那个新cube的大小和位置
    /// </summary>
    /// <param name="movingCube"></param>
    /// <param name="notMove"></param>
    /// <param name="putCenterTemp"></param>
    /// <param name="one"></param>
    /// <param name="two"></param>
    private void GetCanPutCubeSizePos(Cube movingCube, Cube notMove, out Vector3 putCenterTemp, out float one, out float two)
    {
        Vector3 movingTemp = new Vector3(notMove.transform.localPosition.x, 0.0f, notMove.transform.localPosition.z);
        Vector3 notMoveTemp = new Vector3(movingCube.transform.localPosition.x, 0.0f, movingCube.transform.localPosition.z);
        //获取可以放在上一个cube上的新cube的位置
        putCenterTemp = (movingTemp + notMoveTemp) * 0.5f;
        //计算这个可以放置的cube的x,y,z的长度
        Vector3 notMoveTP = new Vector3(notMove.transform.localPosition.x, 0, notMove.transform.localPosition.z);
        float newDis = Vector3.Distance(notMoveTP, putCenterTemp);
        one = 0.0f;
        two = 0.0f;
        if (lastMoveDir == MoveEnumDirEnum.SubXDir || lastMoveDir == MoveEnumDirEnum.XDir)
        {
            one = ((lastSize.x / 2.0f) - newDis) * 2;
            two = lastSize.z;
        }
        else
        {
            one = ((lastSize.z / 2.0f) - newDis) * 2;
            two = lastSize.x;
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void StartGame()
    {
        ClearTowerGo();
        GameObject one = InstantiateBaseOne();
        one.GetComponent<Cube>().CanMove = false;
        mToweredGoList.Add(one);
        GameObject two = InstantiateBaseOne();
        Cube cube = two.GetComponent<Cube>();
        mToweredGoList.Add(two);
        InitMoveDir(cube);
    }

    /// <summary>
    /// 初始化移动
    /// </summary>
    /// <param name="cube"></param>
    private void InitMoveDir(Cube cube) {
        cube.CanMove = true;
        if (lastMoveDir == MoveEnumDirEnum.XDir)
        {
            lastMoveDir = MoveEnumDirEnum.SubZDir;
        }
        else {
            lastMoveDir = MoveEnumDirEnum.XDir;
        }

        switch (lastMoveDir) {
            //x正方向 右下
            case MoveEnumDirEnum.XDir:
                cube.SetInitData(lastCenter + new Vector3(-GameConfig.maxXCanMove, GameConfig.goY, 0), lastSize, lastMoveSpeed, MoveEnumDirEnum.XDir);
                break;
            //z负方向 左下
            case MoveEnumDirEnum.SubZDir:
                cube.SetInitData(lastCenter + new Vector3(0, GameConfig.goY, GameConfig.maxZCanMove), lastSize, lastMoveSpeed, MoveEnumDirEnum.SubZDir);
                break;
        }
    }

    /// <summary>
    /// 生成一个方块
    /// </summary>
    /// <param name="baseOne"></param>
    /// <param name="localPos"></param>
    /// <returns></returns>
    private GameObject InstantiateBaseOne()
    {
        GameObject baseOne = Resources.Load<GameObject>("Cube");
        GameObject one = Instantiate(baseOne);
        one.SetActive(true);
        one.transform.localPosition = Vector3.zero;
        one.transform.localRotation = Quaternion.identity;
        return one;
    }

    /// <summary>
    /// 清空已经存在于场景中的物体
    /// </summary>
    private void ClearTowerGo() {
        lastMoveDir = MoveEnumDirEnum.XDir;
        lastCenter = Vector3.zero;
        lastSize = new Vector3(GameConfig.goX, GameConfig.goY, GameConfig.goZ);
        for (int i=0;i< mToweredGoList.Count;i++) {
            DestroyImmediate(mToweredGoList[i]);
            mToweredGoList[i] = null;
        }
        mToweredGoList.Clear();
        for (int i = 0; i < mDisGoList.Count; i++)
        {
            if (mDisGoList[i]!=null) {
                DestroyImmediate(mDisGoList[i]);
                mDisGoList[i] = null;
            }
        }
        mDisGoList.Clear();
        EventManager.CallAction(Events.ClearTowerGo, null);
    }

    /// <summary>
    /// 获取最后一个正在移动的物体
    /// </summary>
    /// <returns></returns>
    private Cube GetLastMovingCube() {
        if (mToweredGoList != null)
        {
            return mToweredGoList[mToweredGoList.Count - 1].GetComponent<Cube>();
        }
        else {
            return null;
        }
    }

    /// <summary>
    /// 获取最后一个放置上去的cube
    /// </summary>
    /// <returns></returns>
    private Cube GetLastTowerCube() {
        if (mToweredGoList != null)
        {
            return mToweredGoList[mToweredGoList.Count - 2].GetComponent<Cube>();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 是否可以放下
    /// </summary>
    /// <returns></returns>
    private bool CanPutDown() {
        Cube moving = GetLastMovingCube();
        Cube notMove = GetLastTowerCube();
        Vector3 movingTemp = new Vector3(moving.transform.localPosition.x, 0, moving.transform.localPosition.z);
        Vector3 notMoveTemp = new Vector3(notMove.transform.localPosition.x, 0, notMove.transform.localPosition.z);
        float dis = Vector3.Distance(movingTemp, notMoveTemp);
        //在x轴上移动的时候
        if (lastMoveDir == MoveEnumDirEnum.SubXDir ||
            lastMoveDir == MoveEnumDirEnum.XDir) {
            if (dis >= lastSize.x)
            {
                return false;
            }
            else {
                return true;
            }
            
        }
        else {
            if (dis >= lastSize.z)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    

    private void OnDestroy()
    {
        EventManager.RemoveOneAction(Events.ReStartGame, this.OnReStartGame);
        EventManager.RemoveOneAction(Events.StartGame, this.OnStartGame);
        EventManager.RemoveOneAction(Events.AddTowerGoFinish, this.OnAddTowerGoFinish);
        EventManager.RemoveOneAction(Events.SceneTouched, this.OnSceneTouched);
        EventManager.RemoveOneAction(Events.BackMain, this.OnBackToMain);
    }
}
