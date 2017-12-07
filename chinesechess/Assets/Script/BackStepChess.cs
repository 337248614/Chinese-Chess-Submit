using UnityEngine;
using System.Collections;
using System;
public class BackStepChess :MonoBehaviour{

    public static BackStepChess instance; 

    bool IsOnePersonWin= false;//如果现在是用户胜利，点击悔棋
	public  QIZI[]pos = new QIZI[400];//将对弈的过程存储起来
	public static int Count=0;//统计的步数初始化为0

    void Start() 
    {        
        instance = this;
    }

	public struct HUIQIposition{
		public int x;
		public int y;
	}

    //存储每一步棋的位置信息，提示框的位置
	public struct QIZI{
		public HUIQIposition From;
		public HUIQIposition To;
		public GameObject obj1;//第一个框
		public GameObject obj2;//第二个框
		public GameObject objfrist;//第一个名字
		public GameObject objsconde;//第二个棋子名字
		public bool BlackOrRed;//当前是红旗吃黑棋  还是黑棋吃红旗//true 红吃黑   false 黑吃红
		public HUIQIposition ChessID;//吃子时候的两个ID
	}
	//下面用来悔棋
	//开始位置的坐标和终点位置的坐标   
	public void AddChess(int count,int fromx,int fromy,int tox,int toy,bool IsTrueOrfalse,int ID1,int ID2)
    {//每次走棋都把棋子位置添加进去
        GameObject item1 = ChessControl.instance.PosGetChess(fromx, fromy);//得到第一个框名字
        GameObject item2 = ChessControl.instance.PosGetChess(tox, toy);//得到第二个框名字
		//如果是吃子
        GameObject firstChess = item1.transform.GetChild(0).gameObject;//得到第一个旗子名字
        GameObject scondeChess = null;//得到第二个棋子名字
        if (item2.transform.childCount != 0) scondeChess = item2.transform.GetChild(0).gameObject;
		pos [count].From.x = fromx;
		pos [count].From.y = fromy;
		pos [count].To.x = tox;
		pos [count].To.y = toy;
		pos [count].obj1 = item1;
		pos [count].obj2 = item2;
		pos [count].objfrist = firstChess;
        if (scondeChess!=null)
		pos [count].objsconde = scondeChess;
		pos [count].BlackOrRed = IsTrueOrfalse;//判断当前是红吃黑还是黑吃红
		pos [count].ChessID.x = ID1;
		pos [count].ChessID.y = ID2;
		Count++;
	}


	

	//开始悔棋功能了		
	public void IloveHUIQI()
    {
        if (Count <= 0) {
            Count = 0;
            IsOnePersonWin = false;
            Array.Clear(pos,0,pos.Length);
            return; 
        }
        GameObject obj = GameObject.Find("chessRobot");
		obj.transform.localPosition = new Vector3 (8888, 8888, 0);
		for(int i=1;i<=90;i++)
		{
			GameObject Clear = GameObject.Find("prefabs"+i.ToString());
            GameObject.Destroy(Clear);
		}
        if (ChessControl.instance.ChessMove == false && BtnControl.ChessPeople == 1) 
        {
            if (ChessControl.instance.NextPlayerTipStr == "红色棋子胜利")
            {
				//人机状态时候红色棋子胜利悔棋
                BtnControl.ChessPeople = 2;//先把他等于2
                IsOnePersonWin=true;
			}
			return;
		}
        if (BtnControl.ChessPeople == 1) 
        {//如果是人机 就让他悔棋两部
			if (Count <= 1)
				return;
			int f = Count - 1;
			int s = Count - 2;
			int oneID = pos [f].ChessID.x;//黑色旗子原来位置ID
			int twoID = pos [f].ChessID.y;//黑色旗子移动到的位置的ID
			int threeID = pos [s].ChessID.x;//红色旗子原来位置的ID
			int forID = pos [s].ChessID.y;//红色旗子移动到的位置ID 

			GameObject obj1, obj2, obj3, obj4;

			obj1 = pos [f].obj1;//第一个款
			obj2 = pos [f].obj2;//第二个框
			obj3 = pos [f].objfrist;//第一个旗子
			obj4 = pos [f].objsconde;//第二个旗子

			GameObject o1, o2, o3, o4;

			o1 = pos [s].obj1;//第一个款
			o2 = pos [s].obj2;//第二个框
			o3 = pos [s].objfrist;//第一个旗子
			o4 = pos [s].objsconde;//第二个旗子
            //IstrueOrfalse = pos [s].BlackOrRed;

			if (obj4 != null) {//黑色等于吃子
				obj3.transform.SetParent(obj1.transform);
				obj4.transform.SetParent(obj2.transform);
				obj3.transform.localPosition = Vector3.zero;
				obj4.transform.localPosition = Vector3.zero;
				board.instance.chess [pos [f].From.y, pos [f].From.x] = oneID;
                board.instance.chess[pos[f].To.y, pos[f].To.x] = twoID;
				if (o4 != null) {//红色吃子状态
                    o3.transform.SetParent(o1.transform);
                    o4.transform.SetParent(o2.transform);
					o3.transform.localPosition = Vector3.zero;
					o4.transform.localPosition = Vector3.zero;
                    board.instance.chess[pos[s].From.y, pos[s].From.x] = threeID;
                    board.instance.chess[pos[s].To.y, pos[s].To.x] = forID;
				} else {
					o3.transform.SetParent(o1.transform);
					o3.transform.localPosition = Vector3.zero;
                    board.instance.chess[pos[s].From.y, pos[s].From.x] = threeID;
                    board.instance.chess[pos[s].To.y, pos[s].To.x] = 0;
				}
			} else {//黑色等于走棋
                obj3.transform.SetParent(obj1.transform);
				obj3.transform.localPosition = Vector3.zero;
                board.instance.chess[pos[f].From.y, pos[f].From.x] = oneID;
                board.instance.chess[pos[f].To.y, pos[f].To.x] = 0;
				if (o4 != null) {
					o3.transform.SetParent(o1.transform);
					o4.transform.SetParent(o2.transform);
					o3.transform.localPosition = Vector3.zero;
					o4.transform.localPosition = Vector3.zero;
                    board.instance.chess[pos[s].From.y, pos[s].From.x] = threeID;
                    board.instance.chess[pos[s].To.y, pos[s].To.x] = forID;
				} else {
					o3.transform.SetParent(o1.transform);
					o3.transform.localPosition = Vector3.zero;
                    board.instance.chess[pos[s].From.y, pos[s].From.x] = threeID;
                    board.instance.chess[pos[s].To.y, pos[s].To.x] = 0;
				}
		
			}
            ChessControl.instance.ChessMove = true;
			Count -= 2;
            ChessControl.instance.TrueOrFalse = true;//在将帅被吃了的情况下 给他能走棋
            ChessControl.instance.NextPlayerTipStr = "红方走";
            rules.instance.JiangJunCheck();


        }
        else if (BtnControl.ChessPeople == 2)
        {
            if (IsOnePersonWin)
            {
                BtnControl.ChessPeople = 1;
                IsOnePersonWin=false;
			}
			if(Count<=0)
				return;
			int f = Count - 1;
			//int s = Count - 2;
			int oneID = pos [f].ChessID.x;//黑色旗子原来位置ID
			int twoID = pos [f].ChessID.y;//黑色旗子移动到的位置的ID
			GameObject obj1, obj2, obj3, obj4;
            //bool IsfalseOrtrue;
			obj1 = pos [f].obj1;//第一个款
			obj2 = pos [f].obj2;//第二个框
			obj3 = pos [f].objfrist;//第一个旗子
			obj4 = pos [f].objsconde;//第二个旗子
            //IsfalseOrtrue = pos [f].BlackOrRed;//现在红色还是黑色
            //GameObject o1, o2, o3, o4;
            //bool IstrueOrfalse;
			if (obj4 != null) {//最后一步等于吃子
                obj3.transform.SetParent(obj1.transform);
                obj4.transform.SetParent(obj2.transform);
				obj3.transform.localPosition = Vector3.zero;
				obj4.transform.localPosition = Vector3.zero;
                board.instance.chess[pos[f].From.y, pos[f].From.x] = oneID;
                board.instance.chess[pos[f].To.y, pos[f].To.x] = twoID;

			}
			else{
                obj3.transform.SetParent(obj1.transform);
				obj3.transform.localPosition = Vector3.zero;
                board.instance.chess[pos[f].From.y, pos[f].From.x] = oneID;
                board.instance.chess[pos[f].To.y, pos[f].To.x] = 0;
			}
            if (ChessControl.instance.ChessMove == false)
            {
                ChessControl.instance.NextPlayerTipStr = "红方走";
                ChessControl.instance.ChessMove = true;
			}
			else{
                ChessControl.instance.ChessMove = false;
                ChessControl.instance.NextPlayerTipStr = "黑方走";
			}Count -= 1;
            ChessControl.instance.TrueOrFalse = true;//在将帅被吃了的情况下 给他能走棋
            rules.instance.JiangJunCheck();
		} 
		else {
			return;
		}
	}



}
