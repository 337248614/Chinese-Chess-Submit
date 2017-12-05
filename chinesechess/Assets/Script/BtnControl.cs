using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BtnControl : MonoBehaviour
{

    public static int ChessPeople = 1;//判断当前是人人象棋 还是人机
    // Use this for initialization
    public GameObject BtnPosition,StartGame;
    public GameObject difficultselect;
    UILabel Lab;
    void Awake()
    {
        //把难度选择 和 new label  隐藏  人人对战的时候
        GameObject obj = GameObject.Find("NextTips");
        Lab = obj.GetComponent<UILabel>();
        if (ChessPeople == 2)
        {
            difficultselect.SetActive(false);
            Vector3 UIpos = BtnPosition.transform.localPosition;
            BtnPosition.transform.localPosition = new Vector3(UIpos.x, UIpos.y + 70, UIpos.z);
        }
    }

    public void ClickDropDown() {
        StartGame.GetComponent<UIButton>().enabled = false;
    }

    void Update()
    {
        Lab.text = ChessControl.NextPlayerTipStr;
    }
    //ChessChongzhi chess = new ChessChongzhi ();
    public void BackMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //选择游戏模式，加载游戏界面
    public void OnePeopleModel()
    {
        ChessPeople = 1;
        SceneManager.LoadScene("ChessToMe");
        board.start = true;
	}
    public void TwoPeopleModel()
    {
        ChessPeople = 2;
        SceneManager.LoadScene("ChessToMe");
        board.start = true;
    }
    public void SetUpButton() 
    {
        SceneManager.LoadScene("Setup");
    }

}
