using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class yemiantiaozhuang : MonoBehaviour {
	public static int ChessPeople;//判断当前是人人象棋 还是人机
	// Use this for initialization
	public GameObject BtnPosition;
    public GameObject difficultselect;
    UILabel Lab;
	void Awake () {
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

    void Update()
    {
        Lab.text = blackclick.str;
    }
    //ChessChongzhi chess = new ChessChongzhi ();
    public void BackMainScene() {
        SceneManager.LoadScene("MainMenu");
    }

    

}
