using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReStartChess : MonoBehaviour {
	public GameObject spr;
    public Text sprite1;
	// Use this for initialization
	void Start () {
		spr = GameObject.Find ("chessRobot");
		GameObject obj = GameObject.Find("newnandu");
        sprite1 = obj.GetComponent<Text>();
	}
	
	
    BackStepChess chess = new BackStepChess();
	public void ChessPostion(){
		for(int i=1;i<=90;i++)
		{
			GameObject Clear = GameObject.Find("prefabs"+i.ToString());
			Destroy(Clear);
		}
		string s = sprite1.text;
		if (s == "简单")
			SearchEngine.m_nSearchDepth = 1;
		if (s == "一般")
			SearchEngine.m_nSearchDepth = 2;
		if (s == "困难")
			SearchEngine.m_nSearchDepth = 3;
		//ChessChongzhi chess = new ChessChongzhi ();
        if (BtnControl.ChessPeople == 1)
            for (int i = BackStepChess.Count; i > 1; i--)
				chess.IloveHUIQI ();
		else
            for (int i = BackStepChess.Count; i > 0; i--)
				chess.IloveHUIQI ();
		spr.transform.localPosition = new Vector3 (8888, 8888, 0);
        board.instance.ChessInit();
	}
}
