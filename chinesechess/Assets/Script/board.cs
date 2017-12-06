using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class board : MonoBehaviour {
	//public static ArrayList All;
    public static board instance;
    public Text DiffcultLevelText;
    public string DiffcultLevelStr = "简单";
	public int[,]chess = new int[10, 9]{
	
		{2,3,6,5,1,5,6,3,2},
		{0,0,0,0,0,0,0,0,0},
		{0,4,0,0,0,0,0,4,0},
		{7,0,7,0,7,0,7,0,7},
		{0,0,0,0,0,0,0,0,0},
		{0,0,0,0,0,0,0,0,0},
		{14,0,14,0,14,0,14,0,14},
		{0,11,0,0,0,0,0,11,0},
		{0,0,0,0,0,0,0,0,0},
		{9,10,13,12,8,12,13,10,9}

	};
	public static bool start=true;
	void Start () {
        instance = this;
        if (BtnControl.ChessPeople == 1) { 
		GameObject obj = GameObject.Find("newnandu");
        DiffcultLevelText = obj.GetComponent<Text>();
        }
	}
	
    //动态添加了90个棋盘的格子 并且给它们位置等信息
    public void SetPiecePos()
    {                                  

        //UIAtlas atlas;
		int xx=0, yy=0;
		GameObject a = GameObject.Find ("chess");
        //atlas = Resources.Load ("")as UIAtlas;   //让他不能找到图片集合
        for (int i = 1; i <= 90; i++)
        {
            GameObject ite = (GameObject)Instantiate(Resources.Load("item"));//找到预设体
            ite.transform.SetParent(a.transform);           //给预设体指定添加到什么地方
            GameObject b = GameObject.Find(ite.name);    //找到这个预设体的名字，给他做一些操作
            b.transform.localScale = new Vector3(1, 1, 1);
            b.name = "item" + i.ToString();                                           //suoyou所有的深度 都是5
            b.transform.localPosition = new Vector3(xx, yy, 0);
            xx += 43;
            if (xx >= 43 * 9)
            {
                yy -= 43;
                xx = 0;
            }
        }
	}
    public  void InitPiece(string sql, GameObject game, string name, int count)
    {    
		/// P/	/// </summary>引用prefab 生成象棋的棋子
		GameObject a = (GameObject)Instantiate(Resources.Load(sql));
		a.transform.SetParent(game.transform);
		GameObject b = GameObject.Find(a.name);
		b.name = name+count.ToString();
		b.transform.localPosition = new Vector3(0,0,0);
		b.transform.localScale = new Vector3(1,1,1);
	}

	public void ChessInit(){
		chess = new int[10, 9]{  //此注释要取消
			{2,3,6,5,1,5,6,3,2},
			{0,0,0,0,0,0,0,0,0},
			{0,4,0,0,0,0,0,4,0},
			{7,0,7,0,7,0,7,0,7},
			{0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0},
			{14,0,14,0,14,0,14,0,14},
			{0,11,0,0,0,0,0,11,0},
			{0,0,0,0,0,0,0,0,0},
			{9,10,13,12,8,12,13,10,9}
		};
		//初始化其他对象
        ChessControl.instance.ChessMove = true;
        ChessControl.instance.NextPlayerTipStr = "红方走";
        ChessControl.instance.TrueOrFalse = true;
		
		
        DiffcultLevelStr = DiffcultLevelText.text;
        if (DiffcultLevelStr == "简单")
			SearchEngine.m_nSearchDepth = 1;
        if (DiffcultLevelStr == "一般")
			SearchEngine.m_nSearchDepth = 2;
        if (DiffcultLevelStr == "困难")
			SearchEngine.m_nSearchDepth = 3;
        BackStepChess.Count = 0;//重置悔棋记录对象

		
		//初始化棋盘
        SetPiecePos();
		int count=1;
        for (int i = 1; i <= 90; i++)
        {
            GameObject obj = GameObject.Find("item" + i.ToString());
           
            int x = System.Convert.ToInt32((obj.transform.localPosition.x) / 43);
            int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y) / 43));
            switch (chess[y, x])
            {
                case 1:
                    count++;
                    InitPiece("black_jiang", obj, "b_jiang", count);
                    break;
                case 2:
                    count++;
                    InitPiece("black_ju", obj, "b_ju", count);
                    break;
                case 3:
                    count++;
                    InitPiece("black_ma", obj, "b_ma", count);
                    break;
                case 4:
                    count++;
                    InitPiece("black_pao", obj, "b_pao", count);
                    break;
                case 5:
                    count++;
                    InitPiece("black_shi", obj, "b_shi", count);
                    break;
                case 6:
                    count++;
                    InitPiece("black_xiang", obj, "b_xiang", count);
                    break;
                case 7:
                    count++;
                    InitPiece("black_bing", obj, "b_bing", count);
                    break;
                case 8:
                    count++;
                    InitPiece("red_shuai", obj, "r_shuai", count);
                    break;
                case 9:
                    count++;
                    InitPiece("red_ju", obj, "r_ju", count);
                    break;
                case 10:
                    count++;
                    InitPiece("red_ma", obj, "r_ma", count);
                    break;
                case 11:
                    count++;
                    InitPiece("red_pao", obj, "r_pao", count);
                    break;
                case 12:
                    count++;
                    InitPiece("red_shi", obj, "r_shi", count);
                    break;
                case 13:
                    count++;
                    InitPiece("red_xiang", obj, "r_xiang", count);
                    break;
                case 14:
                    count++;
                    InitPiece("red_bing", obj, "r_bing", count);
                    break;
            }
        }
	}
	
	
}
