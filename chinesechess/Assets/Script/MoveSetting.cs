using UnityEngine;
using System.Collections;

public class MoveSetting : MonoBehaviour
{
    //单例模式
    public static MoveSetting instance;
    //存放合法走法的队列
    public CHESSMOVE[,] m_MoveList = new CHESSMOVE[8, 80];
    //记录一个棋子相关位置的数组
    public CHESSMANPOS[] RelatePos = new CHESSMANPOS[30];



    //定义一个棋子位置的结构
	public struct CHESSMANPOS{
	    public int x;//记录x值
		public int y;//记录y值
	}
	//定义一个走法结构
	public struct CHESSMOVE{
		public int ChessID;//标明是什么棋子
		public CHESSMANPOS From;//开始的位置
		public CHESSMANPOS To;//走到的位置
		public int Score;//值
	}
    void Start()
    {
        instance = this;
    }

	//产生给定棋盘上所有合法的走法
	public int CreatePossibleMove(int[,] position,int nPly,bool nSide){
		int  nChessID;//x, y,
        //bool flag;
		int i, j;
        int m_nMoveCount = 0;
		for (j=0; j<9; j++)
			for (i=0; i<10; i++) {
			if(position[i,j]!=0){
				nChessID=position[i,j];
                if (!nSide && rules.instance.IsRed(nChessID))
					continue;//如果产生黑棋走法，跳过红棋
                if (nSide && rules.instance.IsBlack(nChessID))
					continue;//如果产生黑棋走法，跳过红棋
				switch(nChessID){
				case 1://黑将
                        Gen_KingMoveAddSearch(m_nMoveCount,position, i, j, nPly);
					break;
				case 2://黑车
                    Gen_CarMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 3://黑马
                    Gen_HorseMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 4://hei pao
                    Gen_CanonMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 5://shi
                    Gen_BBishopMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 6://xiang
                    Gen_ElephantMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 7://bing
                    Gen_BPawnMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 8://shuai
                    Gen_KingMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 9://che
                    Gen_CarMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 10://ma
                    Gen_HorseMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 11://pao
                    Gen_CanonMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 12://shi
                    Gen_RBishopMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 13://xiang
                    Gen_ElephantMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				case 14://bing
                    Gen_RPawnMoveAddSearch(m_nMoveCount, position, i, j, nPly);
					break;
				}
			}
		
		}
		return m_nMoveCount;//返回总的走法数
	}

	//在m_MoveList中插入一个走法
	//nfromx开始横坐标
	//nfromy开始纵坐标
	//ntox目标横坐标
	//ntoy目标纵坐标
	//nply走法所在层次
    int AddMove(int m_nMoveCount, int[,] position, int nFromx, int nFromy, int nTox, int nToy, int nPly)
    {
		if (m_nMoveCount >= 80)
		if (nPly >= 8)
            if (!rules.instance.KingBye(position, nFromx, nFromy, nTox, nToy))//判断是否老将见面 如果是见面了   就让他不加
			return m_nMoveCount;
		m_MoveList [nPly, m_nMoveCount].From.x = nFromx;
		m_MoveList [nPly, m_nMoveCount].From.y = nFromy;
		m_MoveList [nPly, m_nMoveCount].To.x = nTox;
		m_MoveList [nPly, m_nMoveCount].To.y = nToy;
		m_nMoveCount++;
		//string str="";
		//print ("棋子类型：" + position [nFromy, nFromx] + "可走位置" + nFromx + "," + nFromy + "--" + nTox + "," + nToy);
		return m_nMoveCount;
	}
	//将帅的走法
	//i，j标明棋子位置
	//nply标明插入到list第几层
    void Gen_KingMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		for (y=0; y<3; y++)
			for (x=3; x<6; x++)
                if (rules.instance.IsValidMove(position, j, i, x, y)) //走法是否合法
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		for(y=7;y<10;y++)
			for(x=3;x<6;x++)
                if (rules.instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		
	}
	//红士的走法
    void Gen_RBishopMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		//i j棋子位置   x y目标位置
		int x, y;
		for(y=7;y<10;y++)
			for(x=3;x<6;x++)
                if (rules.instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
	}
	//黑士走法
    void Gen_BBishopMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		for(y=0;y<3;y++)
			for(x=3;x<6;x++)
                if (rules.instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
	}
	//相象走法
    void Gen_ElephantMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		//向右下方走步
		x = j + 2;
		y = i + 2;
        if (x < 9 && y < 10 && rules.instance.IsValidMove(position, j, i, x, y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//向右上方走步
		x = j + 2;
		y = i - 2;
        if (x < 9 && y >= 0 && rules.instance.IsValidMove(position, j, i, x, y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//向左下方走步
		x = j - 2;
		y = i + 2;
        if (x >= 0 && y < 10 && rules.instance.IsValidMove(position, j, i, x, y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//向左上方走步
		x = j - 2;
		y = i - 2;
        if (x >= 0 && y >= 0 && rules.instance.IsValidMove(position, j, i, x, y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);

	}
	//马的走法
    void Gen_HorseMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		//插入右下方的有效走法
		x = j + 2;
		y = i + 1;
        if ((x < 9 && y < 10) && rules.instance.IsValidMove(position, j, i, x, y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//插入右上方的有效走法
		x = j + 2;
		y = i - 1;
		if((x<9&&y>=0)&&rules.instance.IsValidMove(position,j,i,x,y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//左下
		x = j - 2;
		y = i + 1;
		if((x>=0&&y<10)&&rules.instance.IsValidMove(position,j,i,x,y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//左上
		x = j - 2;
		y = i - 1;
		if((x>=0&&y>=0)&&rules.instance.IsValidMove(position,j,i,x,y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//右下
		x = j + 1;
		y = i + 2;
		if((x<9&&y<10)&&rules.instance.IsValidMove(position,j,i,x,y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//left down
		x = j - 1;
		y = i + 2;
		if((x>=0&&y<10)&&rules.instance.IsValidMove(position,j,i,x,y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//right down
		x = j + 1;
		y = i - 2;
		if((x<9&&y>=0)&&rules.instance.IsValidMove(position,j,i,x,y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		//left top
		x = j - 1;
		y = i - 2;
		if((x>=0&&y>=0)&&rules.instance.IsValidMove(position,j,i,x,y))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);

	}
	//车的走法
    void Gen_CarMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		int nChessID;
		nChessID = position [i, j];
		//右边
		x = j + 1;
		y = i;
		while (x<9) {
			if(position[y,x]==0)
                AddMove(m_nMoveCount, position, j, i, x, y, nPly);
			else{
                if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
				break;
			}
			x++;
		}
		//left
		x = j - 1;
		y = i;
		while (x>=0) {

		if(position[y,x]==0)
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
			else{
                if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
				break;
			}
			x--;
		}
		x = j;
		y = i + 1;
		//down
		while (y<10) {
		
				if (position [y, x] == 0)
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
				else {
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(m_nMoveCount, position, j, i, x, y, nPly);
					break;
				}
				y++;
		}
		//top
		x = j;
		y = i - 1;
		while (y>=0) {
				if (position [y, x] == 0)
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
				else {
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(m_nMoveCount, position, j, i, x, y, nPly);
					break;
				}
				y--;
		}
	}
	//红卒的走法
    void Gen_RPawnMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		int nChessID;
		nChessID = position [i, j];
		y = i - 1;
		x = j;
        if (y > 0 && !rules.instance.IsSameSide(nChessID, position[y, x]))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		if (i < 5) {
			y = i;
			x = j + 1;//right
            if (x < 9 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                AddMove(m_nMoveCount, position, j, i, x, y, nPly);
				x = j - 1;//right
                if (x >= 0 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		}
	}
	//黑兵走法
    void Gen_BPawnMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		int nChessID;
		nChessID = position [i, j];
		y = i + 1;//前
		x = j;
        if (y < 10 && !rules.instance.IsSameSide(nChessID, position[y, x]))
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		if (i > 4) {
			y=i;
			x=j+1;
            if (x < 9 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                AddMove(m_nMoveCount, position, j, i, x, y, nPly);
			x=j-1;
            if (x >= 0 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                AddMove(m_nMoveCount, position, j, i, x, y, nPly);
		}

	}
	//炮走法
    void Gen_CanonMoveAddSearch(int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
		int x, y;
		bool flag;
		int nChessID;
		nChessID = position [i, j];
		//right
		x = j + 1;
		y = i;
		flag = false;
		while (x<9) {
			if(position[y,x]==0){
				if(!flag)
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);//没有格棋子插入可以走位置
			}
			else{
				if(!flag)
					flag = true;
				else{
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(m_nMoveCount, position, j, i, x, y, nPly);
					break;
				}
			}
			x++;
		}
		x = j - 1;
		flag = false;
		while (x>=0) {
			if (position [y, x] == 0) {
		if(!flag)
            AddMove(m_nMoveCount, position, j, i, x, y, nPly);
			}
			else{
				if(!flag)
					flag=true;
				else{
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(m_nMoveCount, position, j, i, x, y, nPly);
					break;
				}
			}
			x--;
		}
		x = j;
		y = i + 1;
		flag = false;
		while (y<10) {
			if(position[y,x]==0){
				if(!flag)
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
			}
			else{
				if(!flag)
					flag = true;
				else{
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(m_nMoveCount, position, j, i, x, y, nPly);
					break;
				}
			}
			y++;
		}
		y = i - 1;
		flag = false;
		while (y>=0) {
			if(position[y,x]==0){
				if(!flag)
                    AddMove(m_nMoveCount, position, j, i, x, y, nPly);
			}
			else{
				if(!flag)
					flag = true;
				else{
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(m_nMoveCount, position, j, i, x, y, nPly);
					break;
				}
			}
			y--;
		}
	}




}
