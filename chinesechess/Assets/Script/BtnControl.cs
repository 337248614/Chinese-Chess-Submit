using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SDG;
public class BtnControl : MonoBehaviour
{
    public static bool _isFirstStart = true;
    //判断当前是人人象棋 还是人机
    public static GameModel _gameModel;
    public GameObject _btnPosition;
    public GameObject _difficultSelect;
    void Start()
    {
        //把难度选择 和 new label  隐藏  人人对战的时候
        ChessControl._instance._difficultyModel = DifficultyModel.easy;
        if (_gameModel==GameModel.PersonVSPerson)
        {
            _difficultSelect.SetActive(false);
            Vector3 UIpos = _btnPosition.transform.localPosition;
            _btnPosition.transform.localPosition = new Vector3(UIpos.x, UIpos.y + 70, UIpos.z);
        }
    }
    //返回主菜单按钮
    public void BackMainScene()
    {
        _isFirstStart = true;
        _gameModel = GameModel.PersonVSAi;
        SceneManager.LoadScene("MainMenu");
    }

    //单人模式按钮
    public void OnePeopleModel()
    {
        _gameModel = GameModel.PersonVSAi;
        SceneManager.LoadScene("ChessToMe");
        _isFirstStart = true;
	}
    //双人模式按钮
    public void TwoPeopleModel()
    {
        _gameModel = GameModel.PersonVSPerson;
        SceneManager.LoadScene("ChessToMe");
        _isFirstStart = true;
    }
    //开始游戏按钮
    public void StartGameBtn()
    {
        if (BtnControl._isFirstStart)
        {
            ViewManager._instance.InitChessView();
            BtnControl._isFirstStart = false;
        }
        else
        {
            while (ChessControl._instance.ChessMoveList.Count > 0)
            {
                ViewManager._instance.BackStepView();
            }
            ViewManager._instance.StartGameViewClear();
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
        if (ChessControl._instance.ChessMoveList.Count == 0) return;
        ViewManager._instance.BackStepView();
    }
    //难度选择按钮
    public void OnDifficultSelect() {
        int DropDwonVlaue = _difficultSelect.GetComponent<Dropdown>().value;
        switch (DropDwonVlaue)
        {
            case 0: 
                {
                    ChessControl._instance._difficultyModel = DifficultyModel.easy;
                }
                break;
            case 1:
                {
                    ChessControl._instance._difficultyModel = DifficultyModel.middle;
                }
                break;
            case 2:
                {
                    ChessControl._instance._difficultyModel = DifficultyModel.difficult;
                }
                break;
            default:
                break;
        }
        ChessControl._instance.SetChessModel(ChessControl._instance._difficultyModel);
    }






}
