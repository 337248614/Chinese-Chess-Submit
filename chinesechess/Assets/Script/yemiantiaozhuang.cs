using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class yemiantiaozhuang : MonoBehaviour {
	public static int ChessPeople;//判断当前是人人象棋 还是人机
	// Use this for initialization
	public GameObject lab,sprite;
    public GameObject difficultselect;
    public UILabel Lab;
	void Awake () {
	//把难度选择 和 new label  隐藏  人人对战的时候
        GameObject obj = GameObject.Find("NextTips");
        Lab = obj.GetComponent<UILabel>();
        if (ChessPeople==2) difficultselect.SetActive(false);
	}

    void Update()
    {
        Lab.text = blackclick.str;
    }
    //ChessChongzhi chess = new ChessChongzhi ();
    public void BackMainScene() {
        SceneManager.LoadScene("MainMenu");
    }

    //public void OnCompleteSettingButtonClik(){//返回
    //    //返回界面  数组重新初始化
	
		
    //    for (int c=ChessChongzhi.Count; c>1; c--) {
    //        chess.IloveHUIQI();
    //    }
    //    for (int i=1; i<=90; i++) {
    //        GameObject game = GameObject.Find("item"+i.ToString());
    //        GameObject Clear = GameObject.Find("prefabs"+i.ToString());
    //        Destroy(game);
    //        Destroy(Clear);
    //    }
    //    //what = GameObject.Find ("Musiccup");
    //    //Destroy (what);
    //}

}
