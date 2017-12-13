using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BtnControl : MonoBehaviour
{
    //判断当前是人人象棋 还是人机
    public static bool isFirstStart = true;
    //判断当前是人人象棋 还是人机
    public static int ChessPeople = 1;
    public GameObject BtnPosition;
    public GameObject difficultselect;
    
    void Awake()
    {
        //把难度选择 和 new label  隐藏  人人对战的时候
        SearchEngine.m_nSearchDepth = 1;
        if (ChessPeople == 2)
        {
            difficultselect.SetActive(false);
            Vector3 UIpos = BtnPosition.transform.localPosition;
            BtnPosition.transform.localPosition = new Vector3(UIpos.x, UIpos.y + 70, UIpos.z);
        }
    }
    //返回主菜单按钮
    public void BackMainScene()
    {
        isFirstStart = true;
        ChessPeople = 1;
        SceneManager.LoadScene("MainMenu");
    }

    //单人模式按钮
    public void OnePeopleModel()
    {
        ChessPeople = 1;
        SceneManager.LoadScene("ChessToMe");
        board.start = true;
        isFirstStart = true;
	}
    //双人模式按钮
    public void TwoPeopleModel()
    {
        ChessPeople = 2;
        SceneManager.LoadScene("ChessToMe");
        board.start = true;
        isFirstStart = true;
    }
    //开始游戏按钮
    public void StartGameBtn()
    {
        if (BtnControl.isFirstStart)
        {
            board.instance.ChessInit();
            ViewManager.instance.InitChessView(board.instance.chess);
            BtnControl.isFirstStart = false;
        }
        else
        {
            while (BackStepChess.instance.BackChessList.Count >0)
            {
                ViewManager.instance.HUIQI_View();
                BackStepChess.instance.IloveHUIQI();
            }
            ViewManager.instance.StartGameViewClear();
        }
    }
    //设置按钮
    public void SetUpBtn() 
    {
        SceneManager.LoadScene("Setup");
    }
    //退出游戏按钮
    public void GameOverBtn()
    {
        Application.Quit();
    }
    //悔棋按钮
    public void BackStepBtn() 
    {
        if (BackStepChess.instance.BackChessList.Count == 0) return;
        
    }
    //难度选择按钮
    public void OnDifficultSelect() {
        SearchEngine.m_nSearchDepth = difficultselect.GetComponent<Dropdown>().value+1;
    }
}
