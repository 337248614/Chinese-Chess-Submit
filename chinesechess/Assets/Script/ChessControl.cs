using UnityEngine;
using System.Collections;
using System.Threading;
public class ChessControl : MonoBehaviour {
    public static ChessControl instance;

    public  int FromX = -1, FromY = -1, ToX = -1, ToY = -1;//存储位置移动的起点和终点信息
	
	public  string NextPlayerTipStr="";
	public  bool ChessMove =true;//true   redMove   false BlackMove
	public  bool TrueOrFalse=true;//判断这个时候输赢状态能否走棋  //重新开始记得该true
	public  string RedName=null,BlackName=null,ItemName;//blackchessname  and   redchessname
    public MoveSetting.CHESSMOVE chere;
	GameObject renji;//识别人机走到哪里
	public static bool posthread=true;//判断线程里面的内容是否执行完毕
    public Transform BlackSelectChess, RedSelectChess;
	
	//得到点击的象棋名字
	//判断点击到的是什么？
	//0是空   1 是黑色   2 是红色
    void Start()
    {
        instance = this;
    }

    //判断某个位置是黑方还是红方
    int IsBlackOrRed(int Posx, int Posy)
    {
        int Count = board.instance.chess[Posy, Posx];
		if (Count == 0)
			return 0;
		else if (Count > 0 && Count < 8)//是黑色
			return 1;
		else  //是红色 
			return 2;
	}

    //得到点击的棋子的名字
	void BlackNameOrRedName(GameObject ClickItem)
    {
        if (ClickItem.name.Substring(0, 1) == "r")
            RedName = ClickItem.name;//得到red名字
        else if (ClickItem.name.Substring(0, 1) == "b")
            BlackName = ClickItem.name;//得到black名字
		else
            ItemName = ClickItem.name;//得到item名字
	}
	//移动
    void MoveChess(GameObject FromPosObj, GameObject ToPosObj, int FromX, int FromY, int ToX, int ToY)
    {
        FromPosObj.transform.SetParent(ToPosObj.transform);
        FromPosObj.transform.localPosition = Vector3.zero;
        board.instance.chess[ToY, ToX] = board.instance.chess[FromY, FromX];
        board.instance.chess[FromY, FromX] = 0;
	}
	//吃子
    void EatChess(GameObject FromPosObj, GameObject ToPosObj, int FromX, int FromY, int ToX, int ToY)
    {

        GameObject ToPosObjfather = ToPosObj.gameObject.transform.parent.gameObject;//得到第二个的父亲
        FromPosObj.gameObject.transform.SetParent(ToPosObjfather.transform);
        FromPosObj.transform.localPosition = Vector3.zero;
        board.instance.chess[ToY, ToX] = board.instance.chess[FromY, FromX];
        board.instance.chess[FromY, FromX] = 0;
        GameObject a = GameObject.Find("xiaoshi");
        ToPosObj.transform.SetParent(a.transform);
        ToPosObj.transform.localPosition = new Vector3(5000, 5000, 0);
	}
	//用来悔棋功能
	//点击事件
	//播放音乐
    public void IsClickCheck(GameObject ClickItem)
    {
        renji = GameObject.Find("chessRobot");
		if (TrueOrFalse == false)
			return;
        BlackNameOrRedName(ClickItem);//是否点击到棋子  如果是  就得到棋子
        if (ClickItem.name.Substring(0, 1) != "i")
            ClickItem = ClickItem.gameObject.transform.parent.gameObject;//得到他的父容器
        int x = System.Convert.ToInt32((ClickItem.transform.localPosition.x) / 43);
        int y = System.Convert.ToInt32(Mathf.Abs((ClickItem.transform.localPosition.y) / 43));
		int Result = IsBlackOrRed (x, y);//判断点击到了什么
		switch (Result) {
		case 0://点击到了空  是否要走棋
			//如果点击到了空格  就把对象清空
			//p.MusicPlay();
			for(int i=1;i<=90;i++)
			{
				GameObject Clear = GameObject.Find("prefabs"+i.ToString());
				Destroy(Clear);
			}
			if(posthread ==false)
				return ;
			ToY = y;
			ToX = x;
			if(ChessMove){//红色走
				if(RedName == null)
					return;
                //string sssRed = RedName;//记录红色棋子的名字
                bool ba = rules.instance.IsValidMove(board.instance.chess, FromX, FromY, ToX, ToY);
			if(!ba)
					return;

            int a = board.instance.chess[FromY, FromX];
            int b = board.instance.chess[ToY, ToX];
                BackStepChess.instance.AddChess(BackStepChess.Count, FromX, FromY, ToX, ToY, true, a, b);
                GameObject RedPiece = GameObject.Find(RedName);
                MoveChess(RedPiece, ClickItem, FromX, FromY, ToX, ToY);//走了
                NextPlayerTipStr = "黑方走";
                rules.instance.JiangJunCheck();
				ChessMove = false;
				//getString();
                if (NextPlayerTipStr == "红色棋子胜利")
					return ;//因为没有携程关系  每次进入黑色走棋的时候都判断 棋局是否结束
                if (BtnControl.ChessPeople == 2)
				{//如果现在是双人对战模式
					BlackName = null;
					RedName = null;                    
					return;
				}
                if (ChessMove == false) {
                    NextPlayerTipStr = "黑方在思考中";
                    Invoke("threm", 0.2f);
                }
			//执行走棋

			}
			else{//黑色走
				if(BlackName==null)
					return;
                bool ba = rules.instance.IsValidMove(board.instance.chess, FromX, FromY, ToX, ToY);
				if(!ba)
					return;
                int a = board.instance.chess[FromY, FromX];
                int b = board.instance.chess[ToY, ToX];
                BackStepChess.instance.AddChess(BackStepChess.Count, FromX, FromY, ToX, ToY, true, a, b);
				//看看是否能播放音乐
                GameObject BlackPiece = GameObject.Find(BlackName);
                MoveChess(BlackPiece, ClickItem, FromX, FromY, ToX, ToY);
			
				//黑色走棋
				ChessMove = true;
                NextPlayerTipStr = "红方走";
                rules.instance.JiangJunCheck();
			}
			break;
		case 1://点击到了黑色  是否选中  还是  红色要吃子
			if(posthread ==false)
				return ;
			if(!ChessMove){
				FromX = x;
				FromY = y;
                if (BlackSelectChess != null)
                {
                    BlackSelectChess.GetChild(0).gameObject.SetActive(false);
                }
                BlackSelectChess = ClickItem.transform.GetChild(0);
                BlackSelectChess.GetChild(0).gameObject.SetActive(true);
				for(int i=1;i<=90;i++)
				{
					GameObject Clear = GameObject.Find("prefabs"+i.ToString());
					Destroy(Clear);
				}
                MoveSetting.instance.ClickChessMoveDraw(FromX, FromY);
			}
			else{
				for(int i=1;i<=90;i++)
				{
					GameObject Clear = GameObject.Find("prefabs"+i.ToString());
					Destroy(Clear);
				}
			if(RedName ==null)
					return;
				ToX = x;
				ToY = y;
                bool ba = rules.instance.IsValidMove(board.instance.chess, FromX, FromY, ToX, ToY);
				if(!ba)
					return;
                int a = board.instance.chess[FromY, FromX];
                int b = board.instance.chess[ToY, ToX];
                BackStepChess.instance.AddChess(BackStepChess.Count, FromX, FromY, ToX, ToY, true, a, b);
				//看看是否能播放音乐
                GameObject RedPiece = GameObject.Find(RedName);
                GameObject BlackPiece = GameObject.Find(BlackName);
                EatChess(RedPiece, BlackPiece, FromX, FromY, ToX, ToY);
				ChessMove = false;
				//红色吃子  变黑色走
                NextPlayerTipStr = "黑方走";
                rules.instance.JiangJunCheck();
                if (NextPlayerTipStr == "红色棋子胜利")
					return ;//因为没有携程关系  每次进入黑色走棋的时候都判断 棋局是否结束
                if (BtnControl.ChessPeople == 2)
                {
					RedName=null;
					BlackName=null;
					return;
				}
                if (ChessMove == false) {
                    NextPlayerTipStr = "黑方在思考中";
                    Invoke("threm", 0.2f);
                }
            }
			break;
		case 2://点击到了红色   是否选中  还是黑色要吃子                                            
			if(posthread ==false)
				return ;
            if (RedSelectChess != null)
            {
                RedSelectChess.GetChild(0).gameObject.SetActive(false);
            }
            RedSelectChess = ClickItem.transform.GetChild(0);
            RedSelectChess.GetChild(0).gameObject.SetActive(true);
			if(ChessMove){
				FromX=x;
				FromY = y;
				for(int i=1;i<=90;i++)
				{
					GameObject Clear = GameObject.Find("prefabs"+i.ToString());
					Destroy(Clear);
				}
                MoveSetting.instance.ClickChessMoveDraw(FromX, FromY);
                
			}
			else{
				for(int i=1;i<=90;i++)
				{
					GameObject Clear = GameObject.Find("prefabs"+i.ToString());
					Destroy(Clear);
				}
				if(BlackName==null)
					return;
					ToX = x;
					ToY = y;
                    bool ba = rules.instance.IsValidMove(board.instance.chess, FromX, FromY, ToX, ToY);
				if(!ba)
					return;
                int a = board.instance.chess[FromY, FromX];
                int b = board.instance.chess[ToY, ToX];
                BackStepChess.instance.AddChess(BackStepChess.Count, FromX, FromY, ToX, ToY, true, a, b);
				//看看是否能播放音乐
                GameObject RedPiece = GameObject.Find(RedName);
                GameObject BlackPiece = GameObject.Find(BlackName);
                EatChess(BlackPiece, RedPiece, FromX, FromY, ToX, ToY);                
				RedName = null;
				BlackName = null;
				ChessMove = true;
                NextPlayerTipStr = "红方走";
                rules.instance.JiangJunCheck();                
			}
			break;
	
		}
	
	}

    //调用AI进行下一步棋的计算并且进行界面的修改
	void threm(){
        if (ChessMove == false)
        {

            chere = SearchEngine.instance.SearchAGoodMove(board.instance.chess);
            string s1 = "";
            string s2 = "";
            string s3 = "";
            string s4 = "";
            s1 = PosGetChess(chere.From.x, chere.From.y).name;
            s2 = PosGetChess(chere.To.x, chere.To.y).name;
            GameObject one = GameObject.Find(s1);
            GameObject two = GameObject.Find(s2);
            foreach (Transform child in one.transform)
                s3 = child.name;//第一个象棋名字
            foreach (Transform child in two.transform)
                s4 = child.name;//吃到的子的象棋名字

            //将AI走的这一步加入棋走的每一步列表
            if (s4 == "")
            {
                int a = board.instance.chess[chere.From.y, chere.From.x];
                int b = board.instance.chess[chere.To.y, chere.To.x];
                BackStepChess.instance.AddChess(BackStepChess.Count, chere.From.x, chere.From.y, chere.To.x, chere.To.y, false, a, b);
                GameObject SelectPiece = GameObject.Find(s3);
                MoveChess(SelectPiece, two, chere.From.x, chere.From.y, chere.To.x, chere.To.y);//走了
                renji.transform.localPosition = one.transform.localPosition;

            }
            else
            {
                int a = board.instance.chess[chere.From.y, chere.From.x];
                int b = board.instance.chess[chere.To.y, chere.To.x];
                BackStepChess.instance.AddChess(BackStepChess.Count, chere.From.x, chere.From.y, chere.To.x, chere.To.y, false, a, b);
                GameObject BlackPiece = GameObject.Find(s3);
                GameObject RedPiece = GameObject.Find(s4);
                EatChess(BlackPiece, RedPiece, chere.From.x, chere.From.y, chere.To.x, chere.To.y);
                renji.transform.localPosition = one.transform.localPosition;

            }

            RedName = null;
            BlackName = null;
            if (BlackSelectChess!=null)
	        {
		        BlackSelectChess.transform.GetChild(0).gameObject.SetActive(false);
	        }
            BlackSelectChess = GameObject.Find(s3).transform;
            BlackSelectChess.GetChild(0).gameObject.SetActive(true);
            NextPlayerTipStr = "红方走";
            rules.instance.JiangJunCheck();
            ChessMove = true;
        }

	}
    
    public GameObject PosGetChess(int PosX, int PoxY)
    {//得到开始位置gameobject的对象名字
        GameObject obj;
        string s3 = "";
        for (int i = 1; i <= 90; i++)
        {
            obj = GameObject.Find("item" + i.ToString());
            int x = System.Convert.ToInt32((obj.transform.localPosition.x) / 43);
            int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y) / 43));
            if (x == PosX && y == PoxY)
                s3 = obj.name;
        }
        obj = GameObject.Find(s3);
        return obj;
    }
}
