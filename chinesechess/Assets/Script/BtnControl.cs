using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BtnControl : MonoBehaviour
{

	

    //选择游戏模式，加载游戏界面
    public void OnePeopleModel()
    {
        yemiantiaozhuang.ChessPeople = 1;
        SceneManager.LoadScene("ChessToMe");
        board.start = true;
	}
    public void TwoPeopleModel()
    {
        yemiantiaozhuang.ChessPeople = 2;
        SceneManager.LoadScene("ChessToMe");
        board.start = true;
    }
    public void SetUpButton() 
    {
        SceneManager.LoadScene("Setup");
    }

}
