using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReStartChess : MonoBehaviour {
	GameObject MisPosition;
    Text NextTips;
	// Use this for initialization
	void Start () {
        NextTips = GameObject.Find("NextTips").GetComponent<Text>();
        MisPosition = GameObject.Find("chessRobot");
	}
    void Update()
    {
        NextTips.text = ChessControl.instance.NextPlayerTipStr;
    }

	public void ChessPostion(){
        if (BtnControl.isFirstStart == true)
        {
            board.instance.ChessInit();
            BtnControl.isFirstStart = false;
        }
        else
        {
            if (ChessControl.instance.BlackSelectChess != null)
            {
                ChessControl.instance.BlackSelectChess.GetChild(0).gameObject.SetActive(false);
            }
            if (ChessControl.instance.RedSelectChess != null)
            {
                ChessControl.instance.RedSelectChess.GetChild(0).gameObject.SetActive(false);
            }
            board.instance.DiffcultLevelStr = board.instance.DiffcultLevelText.text;
            if (board.instance.DiffcultLevelStr == "简单")
                SearchEngine.m_nSearchDepth = 1;
            if (board.instance.DiffcultLevelStr == "一般")
                SearchEngine.m_nSearchDepth = 2;
            if (board.instance.DiffcultLevelStr == "困难")
                SearchEngine.m_nSearchDepth = 3;
            for (int i = 1; i <= 90; i++)
            {
                GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                Destroy(Clear);
            }

            if (BtnControl.ChessPeople == 1)
                for (int i = BackStepChess.Count; i > 1; i--)
                    BackStepChess.instance.IloveHUIQI();
            else
                for (int i = BackStepChess.Count; i > 0; i--)
                    BackStepChess.instance.IloveHUIQI();
            MisPosition.transform.localPosition = new Vector3(8888, 8888, 0);
        }
    }
}
