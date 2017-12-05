using UnityEngine;
using System.Collections;

public class MoveSetting : MonoBehaviour {
    public CHESSMOVE[,] m_MoveList = new CHESSMOVE[8, 80]; //存放合法走法的队列
	public int m_nMoveCount;
    public int NOCHESS = 0;//没有棋子时候

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

	//判断一个棋子是不是黑色
	public bool IsBlack(int x){
		if (x > 0 && x < 8)
			return true;
		else
			return false;
	}

	//判断一个棋子是不是红色
	public bool IsRed(int x){
		if (x >= 8 && x < 15)
			return true;
		else
			return false;
	}

	//判断两个棋子是不是同颜色
	public bool IsSameSide(int x,int y){
		if (IsBlack (x) && IsBlack (y) || IsRed (x) && IsRed (y))
			return true;
		else
			return false;
	}

	//产生给定棋盘上所有合法的走法
	//用来产生局面position中所有可能的走法
	//postion包含所有棋子位置信息的二维数组
	//nply指明当前搜索层数，每层将走法存在不同的位置
	//nside指明产生哪一方的走法 true red false black
	public int CreatePossibleMove(int[,] position,int nPly,bool nSide){
		int  nChessID;//x, y,
        //bool flag;
		int i, j;
		m_nMoveCount = 0;
		for (j=0; j<9; j++)
			for (i=0; i<10; i++) {
			if(position[i,j]!=NOCHESS){
				nChessID=position[i,j];
				if(!nSide&&IsRed(nChessID))
					continue;//如果产生黑棋走法，跳过红棋
				if(nSide&&IsBlack(nChessID))
					continue;//如果产生黑棋走法，跳过红棋
				switch(nChessID){
				case 1://黑将
					Gen_KingMove(position,i,j,nPly);
					break;
				case 2://黑车
					Gen_CarMove(position,i,j,nPly);
					break;
				case 3://黑马
					Gen_HorseMove(position,i,j,nPly);
					break;
				case 4://hei pao
					Gen_CanonMove(position,i,j,nPly);
					break;
				case 5://shi
					Gen_BBishopMove(position,i,j,nPly);
					break;
				case 6://xiang
					Gen_ElephantMove(position,i,j,nPly);
					break;
				case 7://bing
					Gen_BPawnMove(position,i,j,nPly);
					break;
				case 8://shuai
					Gen_KingMove(position,i,j,nPly);
					break;
				case 9://che
					Gen_CarMove(position,i,j,nPly);
					break;
				case 10://ma
					Gen_HorseMove(position,i,j,nPly);
					break;
				case 11://pao
					Gen_CanonMove(position,i,j,nPly);
					break;
				case 12://shi
					Gen_RBishopMove(position,i,j,nPly);
					break;
				case 13://xiang
					Gen_ElephantMove(position,i,j,nPly);
					break;
				case 14://bing
					Gen_RPawnMove(position,i,j,nPly);
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
	int AddMove(int [,]position,int nFromx,int nFromy,int nTox,int nToy,int nPly){
		if (m_nMoveCount >= 80)
			print (m_nMoveCount);
		if (nPly >= 8)
			print (nPly);
		if (!rules.KingBye (position, nFromx, nFromy, nTox, nToy))//判断是否老将见面 如果是见面了   就让他不加
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
	void Gen_KingMove(int [,]position,int i,int j,int nPly){
		int x, y;
		for (y=0; y<3; y++)
			for (x=3; x<6; x++)
				if (rules.IsValidMove (position, j, i, x, y)) //走法是否合法
					AddMove(position,j,i,x,y,nPly);
		for(y=7;y<10;y++)
			for(x=3;x<6;x++)
				if (rules.IsValidMove (position, j, i, x, y))//走法是否合法
					AddMove(position,j,i,x,y,nPly);
		
	}
	//红士的走法
	void Gen_RBishopMove(int [,]position,int i,int j,int nPly){
		//i j棋子位置   x y目标位置
		int x, y;
		for(y=7;y<10;y++)
			for(x=3;x<6;x++)
				if (rules.IsValidMove (position, j, i, x, y))//走法是否合法
					AddMove(position,j,i,x,y,nPly);
	}
	//黑士走法
	void Gen_BBishopMove(int [,]position,int i,int j,int nPly){
		int x, y;
		for(y=0;y<3;y++)
			for(x=3;x<6;x++)
				if (rules.IsValidMove (position, j, i, x, y))//走法是否合法
					AddMove(position,j,i,x,y,nPly);
	}
	//相象走法
	void Gen_ElephantMove(int [,]position,int i,int j,int nPly){
		int x, y;
		//向右下方走步
		x = j + 2;
		y = i + 2;
		if (x <9 && y < 10 && rules.IsValidMove (position, j, i, x, y))
			AddMove(position,j,i,x,y,nPly);
		//向右上方走步
		x = j + 2;
		y = i - 2;
		if (x <9&&y>=0&&rules.IsValidMove (position, j, i, x, y))
			AddMove(position,j,i,x,y,nPly);
		//向左下方走步
		x = j - 2;
		y = i + 2;
		if (x >=0&&y<10&&rules.IsValidMove (position, j, i, x, y))
			AddMove(position,j,i,x,y,nPly);
		//向左上方走步
		x = j - 2;
		y = i - 2;
		if (x>=0&&y >=0&&rules.IsValidMove (position, j, i, x, y))
			AddMove(position,j,i,x,y,nPly);

	}
	//马的走法
	void Gen_HorseMove(int [,]position,int i,int j,int nPly){
		int x, y;
		//插入右下方的有效走法
		x = j + 2;
		y = i + 1;
		if ((x < 9 && y < 10) && rules.IsValidMove (position, j, i, x, y))
			AddMove(position,j,i,x,y,nPly);
		//插入右上方的有效走法
		x = j + 2;
		y = i - 1;
		if((x<9&&y>=0)&&rules.IsValidMove(position,j,i,x,y))
			AddMove(position,j,i,x,y,nPly);
		//左下
		x = j - 2;
		y = i + 1;
		if((x>=0&&y<10)&&rules.IsValidMove(position,j,i,x,y))
			AddMove(position,j,i,x,y,nPly);
		//左上
		x = j - 2;
		y = i - 1;
		if((x>=0&&y>=0)&&rules.IsValidMove(position,j,i,x,y))
			AddMove(position,j,i,x,y,nPly);
		//右下
		x = j + 1;
		y = i + 2;
		if((x<9&&y<10)&&rules.IsValidMove(position,j,i,x,y))
			AddMove(position,j,i,x,y,nPly);
		//left down
		x = j - 1;
		y = i + 2;
		if((x>=0&&y<10)&&rules.IsValidMove(position,j,i,x,y))
			AddMove(position,j,i,x,y,nPly);
		//right down
		x = j + 1;
		y = i - 2;
		if((x<9&&y>=0)&&rules.IsValidMove(position,j,i,x,y))
			AddMove(position,j,i,x,y,nPly);
		//left top
		x = j - 1;
		y = i - 2;
		if((x>=0&&y>=0)&&rules.IsValidMove(position,j,i,x,y))
			AddMove(position,j,i,x,y,nPly);

	}
	//车的走法
	void Gen_CarMove(int [,]position,int i,int j,int nPly){
		int x, y;
		int nChessID;
		nChessID = position [i, j];
		//右边
		x = j + 1;
		y = i;
		while (x<9) {
			if(position[y,x]==NOCHESS)
				AddMove(position,j,i,x,y,nPly);
			else{
				if(!IsSameSide(nChessID,position[y,x]))
					AddMove(position,j,i,x,y,nPly);
				break;
			}
			x++;
		}
		//left
		x = j - 1;
		y = i;
		while (x>=0) {

		if(position[y,x]==NOCHESS)
				AddMove(position,j,i,x,y,nPly);
			else{
				if(!IsSameSide(nChessID,position[y,x]))
					AddMove(position,j,i,x,y,nPly);
				break;
			}
			x--;
		}
		x = j;
		y = i + 1;
		//down
		while (y<10) {
		
				if (position [y, x] == NOCHESS)
					AddMove(position,j,i,x,y,nPly);
				else {
					if (!IsSameSide (nChessID, position [y, x]))
						AddMove(position,j,i,x,y,nPly);
					break;
				}
				y++;
		}
		//top
		x = j;
		y = i - 1;
		while (y>=0) {
				if (position [y, x] == NOCHESS)
					AddMove(position,j,i,x,y,nPly);
				else {
					if (!IsSameSide (nChessID, position [y, x]))
						AddMove(position,j,i,x,y,nPly);
					break;
				}
				y--;
		}
	}
	//红卒的走法
	void Gen_RPawnMove(int [,]position,int i,int j,int nPly){
		int x, y;
		int nChessID;
		nChessID = position [i, j];
		y = i - 1;
		x = j;
		if(y>0&&!IsSameSide(nChessID,position[y,x]))
		AddMove(position,j,i,x,y,nPly);
		if (i < 5) {
			y = i;
			x = j + 1;//right
				if (x < 9 && !IsSameSide (nChessID, position [y, x]))
					AddMove(position,j,i,x,y,nPly);
				x = j - 1;//right
				if (x >= 0 && !IsSameSide (nChessID, position [y, x]))
					AddMove(position,j,i,x,y,nPly);
		}
	}
	//黑兵走法
	void Gen_BPawnMove(int [,]position,int i,int j,int nPly){
		int x, y;
		int nChessID;
		nChessID = position [i, j];
		y = i + 1;//前
		x = j;
			if (y < 10 && !IsSameSide (nChessID, position [y, x]))
				AddMove(position,j,i,x,y,nPly);
		if (i > 4) {
			y=i;
			x=j+1;
			if(x<9&&!IsSameSide(nChessID,position[y,x]))
				AddMove(position,j,i,x,y,nPly);
			x=j-1;
			if(x>=0&&!IsSameSide(nChessID,position[y,x]))
				AddMove(position,j,i,x,y,nPly);
		}

	}
	//炮走法
	void Gen_CanonMove(int [,]position,int i,int j,int nPly){
		int x, y;
		bool flag;
		int nChessID;
		nChessID = position [i, j];
		//right
		x = j + 1;
		y = i;
		flag = false;
		while (x<9) {
			if(position[y,x]==NOCHESS){
				if(!flag)
					AddMove(position,j,i,x,y,nPly);//没有格棋子插入可以走位置
			}
			else{
				if(!flag)
					flag = true;
				else{
					if(!IsSameSide(nChessID,position[y,x]))
						AddMove(position,j,i,x,y,nPly);
					break;
				}
			}
			x++;
		}
		x = j - 1;
		flag = false;
		while (x>=0) {
			if (position [y, x] == NOCHESS) {
		if(!flag)
					AddMove(position,j,i,x,y,nPly);
			}
			else{
				if(!flag)
					flag=true;
				else{
					if(!IsSameSide(nChessID,position[y,x]))
						AddMove(position,j,i,x,y,nPly);
					break;
				}
			}
			x--;
		}
		x = j;
		y = i + 1;
		flag = false;
		while (y<10) {
			if(position[y,x]==NOCHESS){
				if(!flag)
					AddMove(position,j,i,x,y,nPly);
			}
			else{
				if(!flag)
					flag = true;
				else{
					if(!IsSameSide(nChessID,position[y,x]))
						AddMove(position,j,i,x,y,nPly);
					break;
				}
			}
			y++;
		}
		y = i - 1;
		flag = false;
		while (y>=0) {
			if(position[y,x]==NOCHESS){
				if(!flag)
					AddMove(position,j,i,x,y,nPly);
			}
			else{
				if(!flag)
					flag = true;
				else{
					if(!IsSameSide(nChessID,position[y,x]))
						AddMove(position,j,i,x,y,nPly);
					break;
				}
			}
			y--;
		}
	}



    //点击棋子能判断得出来他的那些位置是能走的呢？

    //1、先得到他能走到的位置
    //2* 获取位置坐标
    //3.得到item对象
    //把预设体添加到场景
    //5.找到预设体的富容器变且添加， 
    //6.设置预设体的位置   tox， toy
    /*-----------------------------------------*/
    //判断点击到的是什么棋子
    public void ClickChess(int fromx, int fromy)
    {
        //print (board.chess[fromy,fromx]+" "+fromx + " " + fromy);
        int ChessID = board.chess[fromy, fromx];
        switch (ChessID)
        {
            case 1:
            case 8:
                Gen_KingMove(board.chess, fromx, fromy);
                break;
            case 2:
            case 9:
                Gen_CarMove(board.chess, fromx, fromy);
                break;
            case 3:
            case 10:
                Gen_HorseMove(board.chess, fromx, fromy);
                break;
            case 4:
            case 11:
                Gen_CanonMove(board.chess, fromx, fromy);
                break;
            case 5://黑士
                Gen_BBishopMove(board.chess, fromx, fromy);
                break;
            case 6://黑象
            case 13://红相
                Gen_ElephantMove(board.chess, fromx, fromy);
                break;
            case 7://黑兵
                Gen_BPawnMove(board.chess, fromx, fromy);
                break;
            case 12://红shi
                Gen_RBishopMove(board.chess, fromx, fromy);
                break;

            case 14://红兵
                Gen_RPawnMove(board.chess, fromx, fromy);
                break;
        }
    }
    //把传入进来的可走位置全部画出来
    public void GetPrefabs(int[,] position, int c, int d, int x, int y)
    {//得到相关位置的item坐标  tox  toy
        //先进行与社体清空
        //	for (int i=1; i<=90; i++) {
        //	GameObject obj = GameObject.Find("prefabs"+i.ToString());
        //	Destroy(obj);
        //}
        /*string str = "";
        for(int t=0;t<board.chess.GetLength(0);t++){
            for(int f=0;f<board.chess.GetLength(1);f++)
                str+=" "+board.chess[t,f];
            str+="\n";
        }
        print (str);*/
        if (!rules.KingBye(position, c, d, x, y))
            //print(!rules.KingBye(position,c,d,x,y));
            //			print (c + "," + d + "-" + x + "," + y);
            return;
        int wid = x * 112;
        int heit = y * (-112);

        for (int i = 1; i <= 90; i++)
        {
            GameObject game = GameObject.Find("item" + i.ToString());
            if (game.transform.localPosition.x == wid && game.transform.localPosition.y == heit)
            {
                //得到了game  对象    了解game对象坐标 
                GameObject obj = GameObject.Find("chess");//找到预设体参照物
                GameObject ite;
                if (board.chess[y, x] == 0)
                    ite = (GameObject)Instantiate(Resources.Load("canmove"));//找到预设体
                else
                    ite = (GameObject)Instantiate(Resources.Load("nengchi"));//找到预设体
                ite.transform.parent = obj.transform;
                GameObject b = GameObject.Find(ite.name);    //找到这个预设体的名字，给他做一些操作
                b.name = "prefabs" + i.ToString();
                b.transform.localPosition = new Vector3(wid, heit, 0);
                b.transform.localScale = new Vector3(1, 1, 1);
                //GameObject objecta = GameObject.Find(b.name);
            }
        }
    }
    //得到将的走法
    void Gen_KingMove(int[,] position, int j, int i)
    {//两个参数 fromx  和fromy
        int x, y;
        for (y = 0; y < 3; y++)
            for (x = 3; x < 6; x++)
                if (rules.IsValidMove(position, j, i, x, y)) //走法是否合法
                    GetPrefabs(position, j, i, x, y);
        for (y = 7; y < 10; y++)
            for (x = 3; x < 6; x++)
                if (rules.IsValidMove(position, j, i, x, y))//走法是否合法
                    GetPrefabs(position, j, i, x, y);

    }
    //红士的走法
    void Gen_RBishopMove(int[,] position, int j, int i)
    {
        //i j棋子位置   x y目标位置
        int x, y;
        for (y = 7; y < 10; y++)
            for (x = 3; x < 6; x++)
                if (rules.IsValidMove(position, j, i, x, y))//走法是否合法
                    GetPrefabs(position, j, i, x, y);
    }
    //黑士走法
    void Gen_BBishopMove(int[,] position, int j, int i)
    {
        int x, y;
        for (y = 0; y < 3; y++)
            for (x = 3; x < 6; x++)
                if (rules.IsValidMove(position, j, i, x, y))//走法是否合法
                    GetPrefabs(position, j, i, x, y);
    }
    //相象走法
    void Gen_ElephantMove(int[,] position, int j, int i)
    {
        int x, y;
        //向右下方走步
        x = j + 2;
        y = i + 2;
        if (x < 9 && y < 10 && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //向右上方走步
        x = j + 2;
        y = i - 2;
        if (x < 9 && y >= 0 && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //向左下方走步
        x = j - 2;
        y = i + 2;
        if (x >= 0 && y < 10 && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //向左上方走步
        x = j - 2;
        y = i - 2;
        if (x >= 0 && y >= 0 && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
    }
    //马的走法
    void Gen_HorseMove(int[,] position, int j, int i)
    {
        int x, y;
        //插入右下方的有效走法
        x = j + 2;
        y = i + 1;
        if ((x < 9 && y < 10) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //插入右上方的有效走法
        x = j + 2;
        y = i - 1;
        if ((x < 9 && y >= 0) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //左下
        x = j - 2;
        y = i + 1;
        if ((x >= 0 && y < 10) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //左上
        x = j - 2;
        y = i - 1;
        if ((x >= 0 && y >= 0) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //右下
        x = j + 1;
        y = i + 2;
        if ((x < 9 && y < 10) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //left down
        x = j - 1;
        y = i + 2;
        if ((x >= 0 && y < 10) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //right down
        x = j + 1;
        y = i - 2;
        if ((x < 9 && y >= 0) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //left top
        x = j - 1;
        y = i - 2;
        if ((x >= 0 && y >= 0) && rules.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);

    }
    //车的走法
    void Gen_CarMove(int[,] position, int j, int i)
    {
        int x, y;
        int nChessID;
        nChessID = position[i, j];
        //右边
        x = j + 1;
        y = i;
        while (x < 9)
        {
            if (position[y, x] == 0)
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
                break;
            }
            x++;
        }
        //left
        x = j - 1;
        y = i;
        while (x >= 0)
        {
            if (position[y, x] == 0)
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
                break;
            }
            x--;
        }
        x = j;
        y = i + 1;
        //down
        while (y < 10)
        {
            if (position[y, x] == 0)
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
                break;
            }
            y++;
        }
        //top
        x = j;
        y = i - 1;
        while (y >= 0)
        {
            if (position[y, x] == 0)
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
                break;
            }
            y--;
        }
    }
    //红卒的走法
    void Gen_RPawnMove(int[,] position, int j, int i)
    {
        int x, y;
        int nChessID;
        nChessID = position[i, j];
        y = i - 1;
        x = j;
        if (y > 0 && !IsSameSide(nChessID, position[y, x]))
            GetPrefabs(position, j, i, x, y);
        if (i < 5)
        {
            y = i;
            x = j + 1;//right
            if (x < 9 && !IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
            x = j - 1;//right
            if (x >= 0 && !IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
        }
    }
    //黑兵走法
    void Gen_BPawnMove(int[,] position, int j, int i)
    {
        int x, y;
        int nChessID;
        nChessID = position[i, j];
        y = i + 1;//前
        x = j;
        if (y < 10 && !IsSameSide(nChessID, position[y, x]))
            GetPrefabs(position, j, i, x, y);
        if (i > 4)
        {
            y = i;
            x = j + 1;
            if (x < 9 && !IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
            x = j - 1;
            if (x >= 0 && !IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
        }

    }
    //炮走法
    void Gen_CanonMove(int[,] position, int j, int i)
    {
        int x, y;
        bool flag;
        int nChessID;
        nChessID = position[i, j];
        //right
        x = j + 1;
        y = i;
        flag = false;
        while (x < 9)
        {
            if (position[y, x] == 0)
            {
                if (!flag)
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!IsSameSide(nChessID, position[y, x]))
                        GetPrefabs(position, j, i, x, y);
                    break;
                }
            }
            x++;
        }
        x = j - 1;
        flag = false;
        while (x >= 0)
        {
            if (position[y, x] == 0)
            {
                if (!flag)
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!IsSameSide(nChessID, position[y, x]))
                        GetPrefabs(position, j, i, x, y);
                    break;
                }
            }
            x--;
        }
        x = j;
        y = i + 1;
        flag = false;
        while (y < 10)
        {
            if (position[y, x] == 0)
            {
                if (!flag)
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!IsSameSide(nChessID, position[y, x]))
                        GetPrefabs(position, j, i, x, y);
                    break;
                }
            }
            y++;
        }
        y = i - 1;
        flag = false;
        while (y >= 0)
        {
            if (position[y, x] == 0)
            {
                if (!flag)
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    Debug.Log("wait");
                    Debug.Log(!IsSameSide(nChessID, position[y, x]));
                    if (!IsSameSide(nChessID, position[y, x]))
                    {
                        Debug.Log("wait");
                        GetPrefabs(position, j, i, x, y);
                    }
                    break;
                }
            }
            y--;
        }
    }
}
