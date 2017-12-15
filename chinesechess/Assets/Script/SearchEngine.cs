using UnityEngine;
using System.Collections;
using SDG;
using System.Collections.Generic;
public class SearchEngine  {

    public static SearchEngine _instance=new SearchEngine(); 
	int [,]_curPosition = new int[10, 9];
    //AI算法最大搜索深度
    int _maxDepth;
    //AI算法搜索深度
	public static int _searchDepth=1;
    //存放最佳走法的变量
    Move _saveBestMove;
    //棋子基本价值
    int[] _baseValue = new int[15] { 0, 10000, 500, 350, 350, 250, 250, 100, 10000, 500, 350, 350, 250, 250, 100 };
    //棋子灵活性分数数组
		//che 6 ma 12 xiang 1 shi 1 pao 6 jiang 0 bing 15
    int[] _flexValue = new int[15]{0,0,6,12,6,1,1,15,0,6,12,6,1,1,15};
    //每个位置威胁信息
    int[,] _attackPos = new int[10, 9];
    //存放每个位置被保护的信息
    int[,] _guardPos = new int[10, 9];
    //存放每个位置上的棋子灵活性
    int[,] _flexibilityPos = new int[10, 9];
    //存放每个位置的棋子总价值
    int[,] _chessValue = new int[10, 9];
    //记录棋子的相关位置个数
    int _posCount;
    //实例化

    //红兵的附加值数组
    int[,] _redPawnValue = new int[10, 9]
	{
		{0,0,0,0,0,0,0,0,0} ,
		{90,90,110,120,120,120,110,90,90} ,
		{90,90,110,120,120,120,110,90,90} ,
		{70,90,110,110,110,110,110,90,70} ,
		{70,70,70,70,70,70,70,70,70} ,
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
	} ;
    //黑兵附加值数组
    int[,]  _blackPawnValue = new int [10,9]
	{
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
		{0,0,0,0,0,0,0,0,0} ,
		{70,70,70,70,70,70,70,70,70} ,
		{70,90,110,110,110,110,110,90,70} ,
		{90,90,110,120,120,120,110,90,90} ,
		{90,90,110,120,120,120,110,90,90} ,
		{0,0,0,0,0,0,0,0,0} ,
	} ;
    //存放合法走法的队列
    public Move[,] _moveList = new Move[8, 80];
    //记录一个棋子相关位置的数组
    public Position[] _relatePos = new Position[30];
    public struct Move
    {
        public Position From;//开始的位置
        public Position To;//走到的位置
        public int FromChessNum;
        public int ToChessNum;
    }
    public struct Position
    {

        public int x;
        public int y;
    }
    
	//根据传入的走法改变棋盘
    int MakeMove(Move move)
    {
		int nChessID;
		nChessID = _curPosition [move.To.y, move.To.x];
		_curPosition [move.To.y, move.To.x] =_curPosition [move.From.y, move.From.x];
		_curPosition [move.From.y, move.From.x] = 0;
		return nChessID;
	}
	//根据走法恢复棋盘
    void UnMakeMove(Move move, int nChessID)
    {
		_curPosition [move.From.y, move.From.x] = _curPosition [move.To.y, move.To.x];
		_curPosition [move.To.y, move.To.x] = nChessID;
	}
	//检查游戏是否结束
	public int IsGameOver(int [,]position,int nDepth){
		int i, j;
		bool RedLive = false, BlackLive = false;
		for (i=3; i<6; i++)
			for (j=0; j<3; j++) {
			if(position[j,i]==1)
				BlackLive = true;
		}
		for (i=3; i<6; i++)
			for (j=7; j<10; j++) {
			if(position[j,i]==8)
				RedLive = true;
		}
		i = (_maxDepth - nDepth+1) % 2;
		if (!RedLive)
		if (i != 0)
			return 19990 + nDepth;
		else
			return -19990 - nDepth;
		if (!BlackLive)
		if (i != 0)
			return -19990 - nDepth;
		else
			return 19990 + nDepth;
		return 0;

	}

    //Alpha-beta 剪枝算法
	int PrincipalVariation(int depth,int alpha,int beta)
	{
		int score;
		int Count, i;
		int type;
		int best;
		i = IsGameOver (_curPosition, depth);
		if (i != 0)
			return i;
		if (depth <= 0)
			return Eveluate (_curPosition, (_maxDepth - depth) % 2 != 0);
        Count = CreatePossibleMove(_curPosition, depth, (_maxDepth - depth) % 2 != 0);
        type = MakeMove(_moveList[depth,0]);
		best = -PrincipalVariation (depth - 1, -beta, -alpha);
        UnMakeMove(_moveList[depth,0], type);
		if (depth == _maxDepth)
            _saveBestMove = _moveList[depth, i];
		for (i=1; i<Count; i++) {
			if(best<beta){
				if(best<beta)
					if(best>alpha)
						alpha=best;
                type = MakeMove(_moveList[depth,i]);
					score = -PrincipalVariation(depth-1,-alpha-1,-alpha);
					if(score>alpha&&score<beta){
						best = -PrincipalVariation(depth-1,-beta,-score);
						if(depth==_maxDepth)
                            _saveBestMove = _moveList[depth,i];
					}
					else if(score>best){
						best=score;
						if(depth==_maxDepth)
                            _saveBestMove = _moveList[depth,i];
					}
                    UnMakeMove(_moveList[depth,i], type);
			}
		}
		return best;
	}
    public Move SearchAGoodMove(int[,] position)
    {
		_maxDepth = _searchDepth;
		for (int i=0; i<position.GetLength(0); i++)
			for (int j=0; j<position.GetLength(1); j++)
				_curPosition [i, j] = position [i, j];
		PrincipalVariation (_maxDepth, -20000, 20000);
		MakeMove (_saveBestMove);
		  return _saveBestMove ;
	}

	
	//为每一个兵返回附加值

	int GetBingValue(int x,int y,int [,]CurSituation){
		if (CurSituation [y, x] == 14) 
			return _redPawnValue[y,x];
		if (CurSituation [y, x] == 7)
			return _blackPawnValue[y,x];
		return 0;
	}

	//position是要估值的棋盘
	int Eveluate(int [,]position,bool bIsRedTum)
	{
		int i, j, k;
		int nChessType, nTargetType;
		_chessValue = new int[10,9];
		_attackPos = new int[10, 9];
		_guardPos = new int[10, 9];
		_flexibilityPos = new int[10, 9];
		for (i = 0; i < 10; i++)
			for (j = 0; j < 9; j++)
		{
			if (position [i,j]!=0 )
			{
				nChessType = position [i,j];
				GetRelatePiece (position ,j,i);//找出该棋子的所有相关位置
				for (k =0;k<_posCount;k++)//对每一目标位置
				{
					//取目标位置棋子类型
                    nTargetType = position[_relatePos[k].y, _relatePos[k].x];
					if (nTargetType == 0 )//如果是空白
						_flexibilityPos [i,j]++;//灵活性增加
					else
					{
						//是棋子
                        if (rules._instance.IsSameSide(nChessType, nTargetType))
						{
							//如果是己方棋子，目标受到保护
                            _guardPos[_relatePos[k].y, _relatePos[k].x]++;
						}
						else 
						{
							//如果是敌方棋子，目标受到威胁
                            _attackPos[_relatePos[k].y,_relatePos[k].x]++;
							_flexibilityPos [i,j]++;//灵活性增加
							//string sss = "";
							switch (nTargetType)
							{
							case 8://如果是红将
								if (!bIsRedTum)
									return 18888;//返回失败极值
								break;
							case 1: //如果是黑将
								if (bIsRedTum)//如果是黑将
									return 18888;//返回失败极值
								break;

							default ://不是将的其他棋子
								//根据威胁的棋子加上威胁分值
							{
                                _attackPos[_relatePos[k].y, _relatePos[k].x] += (30 + (_baseValue[nTargetType] - _baseValue[nChessType]) / 10) / 10;							
								break ;
							}
							
							}
						}
					}
				}
				
			}
		}
		//以上扫描棋盘部分
		//一下循环统计扫描到的数据
		for (i = 0; i<10; i++)
			for (j =0; j<9; j++) 
		{
			if (position [i,j]!=0 )
			{
				nChessType = position [i,j];//棋子类型
				_chessValue [i,j]++;			//如果存在其价值不为0
				//把每个棋子的灵活性价值加进棋子价值
				_chessValue [i,j]+=_flexValue [nChessType ]*_flexibilityPos  [i,j];
				//加上兵的位置附加值
				_chessValue [i,j]+=GetBingValue (j,i,position );
				
			}
		}
		//下面循环继续统计扫描到的数据
		int nHalfvalue;
		for (i = 0; i<10; i++)
			for (j =0; j<9; j++)
		{
			if (position [i,j]!=0 )//如果不是空白
			{
				nChessType = position [i,j];//取棋子类型
				//棋子基本价值的1/16作为威胁/保护增量
				nHalfvalue = _baseValue [nChessType]/16;
				//把每个棋子的基本价值介入其总价值
				_chessValue [i,j]+=_baseValue [nChessType ];
                if (rules._instance.IsRed(nChessType))//如果是红旗
				{
					if (_attackPos [i,j]!=0)//当前红棋如果被威胁
					{
						if (bIsRedTum)//如果轮到红旗走
						{
							if (nChessType == 8)//如果是红将
							{
								_chessValue [i,j]-=20;//价值降低20
							}
							else
							{
								//价值减去2倍nHalfvalue
								_chessValue [i,j]-=nHalfvalue *2;
								if (_guardPos [i,j]!=0)//是否被己方棋子保护
								 _chessValue [i,j]+=nHalfvalue ;/*被保护再加上nHalfvalue*/
							}
						}
						else  //红棋被威胁，轮到黑棋走
						{
							if (nChessType ==8)//是否是红将
								return 18888;//返回失败的极值
							_chessValue [i,j]-=nHalfvalue *10;//减去10倍的nHalfvalue，表示威胁程度高
							if(_guardPos [i,j]!=0)//如果被保护
							{
								_chessValue [i,j]+=nHalfvalue *9;//被保护再加上9倍nHalfvalue
							}
						}
						//被威胁的棋子加上威胁差，防止一个并威胁
						//一个被保护的車，而估值函数没有反映这类问题
						_chessValue [i,j]-=_attackPos [i,j];
					}
					else //没受到威胁
					{
						if(_guardPos [i,j]!=0)
							_chessValue [i,j]+=5;//受保护，加一分
					}
				}
				else  //如果是黑棋
				{
					if(_attackPos [i,j]!=0)//黑棋受威胁
					{
						if(!bIsRedTum )//轮到黑棋走
						{
							if (nChessType ==1)//如果是黑将
								_chessValue [i,j]-=20;//棋子价值降低20
							else
							{
								_chessValue [i,j]-=nHalfvalue *2;
								if (_guardPos [i,j]!=0)//如果受保护
									_chessValue [i,j]+=nHalfvalue ;
							}
						}
						else //轮到红旗走
						{
							if(nChessType == 1)
								return 18888;
							_chessValue [i,j]-=nHalfvalue *10;
							if (_guardPos [i,j]!=0)
								_chessValue [i,j]+=nHalfvalue *9;
						}
						_chessValue [i,j]-=_attackPos [i,j];
					}
					else //黑棋没有被威胁
					{
						if(_guardPos [i,j]!=0)
							_chessValue [i,j]+=5;
					}
				}
			}
		}
		
		//以上生成每个棋子的总价值
		//以下统计红黑方的总分
		int nRedValue = 0;
		int nBlackValue = 0;
		for (i=0; i<10; i++)
			for (j =0; j<9; j++) 
		{
			nChessType = position [i,j];
			if (nChessType !=0 )//如果不是空白
			{
                if (rules._instance.IsRed(nChessType))  //如果是红旗
					nRedValue +=_chessValue [i,j];//将这格棋子的价值加入到红旗价值当中
				else
					nBlackValue += _chessValue [i,j];
			}
		}
		if (bIsRedTum) {
			return  nRedValue - nBlackValue;//如果轮到红旗走返回估值
		} else {
			return nBlackValue - nRedValue;//轮到黑棋走返回估值的相反数
		}

	}
	
	
	//列举与指定位置的棋子相关的棋子
	//这个函数枚举了给定位上棋子的所有相关位置
	void GetRelatePiece(int [,]position,int j,int i ){
		_posCount = 0;
		int nChessID;
		//_relatePos = new Blackmove.ChessPosition[30];
		bool flag = false;
		int x, y;
		nChessID = position [i, j];
		switch (nChessID) {
		case 1:
			for (y=0; y<3; y++)
				for (x=3; x<6; x++)
					if (CanTouch (position, j, i, x, y)) {
						AddPoint (x, y);
					}
			break;
		case 8:
			for (y=7; y<10; y++)
				for (x=3; x<6; x++)
					if (CanTouch (position, j, i, x, y))
						AddPoint (x, y);
			break;
		case 12:
			for (y=7; y<10; y++)
				for (x=3; x<6; x++)
					if (CanTouch (position, j, i, x, y))
						AddPoint (x, y);
			break;
		case 5:
			for (y=0; y<3; y++)
				for (x=3; x<6; x++)
					if (CanTouch (position, j, i, x, y))
						AddPoint (x, y);
			break;
		case 6:
		case 13:
			x = j + 2;
			y = i + 2;
			if (x < 9 && y < 10 && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//右下
			x = j + 2;
			y = i - 2;
			if (x < 9 && y >= 0 && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//左下
			x = j - 2;
			y = i + 2;
			if (x >= 0 && y < 10 && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//左下
			x = j - 2;
			y = i - 2;
			if (x >= 0 && y >= 0 && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			break;
		case 3:
		case 10:
			x = j + 2;
			y = i + 1;
			if ((x < 9 && y < 10) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//检查right top
			x = j + 2;
			y = i - 1;
			if ((x < 9 && y >= 0) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//left down
			x = j - 2;
			y = i + 1;
			if ((x >= 0 && y < 10) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//left top
			x = j - 2;
			y = i - 1;
			if ((x >= 0 && y >= 0) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//right down
			x = j + 1;
			y = i + 2;
			if ((x < 9 && y < 10) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//left down
			x = j - 1;
			y = i + 2;
			if ((x >= 0 && y < 10) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//right top
			x = j + 1;
			y = i - 2;
			if ((x < 9 && y >= 0) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			//left top
			x = j - 1;
			y = i - 2;
			if ((x >= 0 && y >= 0) && CanTouch (position, j, i, x, y))
				AddPoint (x, y);
			break;
		case 2:
		case 9:
			x = j + 1;
			y = i;
			while (x<9) {
				if (position [y, x] == 0)
					AddPoint (x, y);
				else {
					AddPoint (x, y);
					break;
				}
				x++;
			}
			x = j - 1;
			y = i;
			while (x>=0) {
				if (position [y, x] == 0)
					AddPoint (x, y);
				else {
					AddPoint (x, y);
					break;
				}
				x--;
			}
			x = j;
			y = i + 1;
			while (y<10) {
				if (position [y, x] == 0)
					AddPoint (x, y);
				else {
					AddPoint (x, y);
					break;
				}
				y++;
			}
			x = j;
			y = i - 1;
			while (y>=0) {
				if (position [y, x] == 0)
					AddPoint (x, y);
				else {
					AddPoint (x, y);
					break;
				}
				y--;
			}
			break;
		case 14:
			y = i - 1;
			x = j;
			if (y >= 0)
				AddPoint (x, y);
			if (i < 5) {
				y = i;
				x = j + 1;
				if (x < 9)
					AddPoint (x, y);
				x = j - 1;
				if (x >= 0)
					AddPoint (x, y);
			}

			break;
		case 7:
			y = i + 1;
			x = j;
			if (y < 10)
				AddPoint (x, y);
			if (i > 4) {
				y = i;
				x = j + 1;
				if (x < 9)
					AddPoint (x, y);
				x = j - 1;
				if (x >= 0)
					AddPoint (x, y);
			}
			break;
		case 4:
		case 11:
			x = j + 1;
			y = i;
			flag = false;
			while (x<9) {
				if (position [y, x] == 0) {
					if (!flag)
						AddPoint (x, y);
				} else {
					if (!flag)
						flag = true;
					else {
						AddPoint (x, y);
						break;
					}
				}
				x++;
			}
			x = j - 1;
			flag = false;
			while (x>=0) {
				if (position [y, x] == 0) {
					if (!flag)
						AddPoint (x, y);
				} else {
					if (!flag)
						flag = true;
					else {
						AddPoint (x, y);
						break;
					}
				}
				x--;
			}
			x = j;
			y = i + 1;
			flag = false;
			while (y<10) {
				if (position [y, x] == 0) {
					if (!flag)
						AddPoint (x, y);
				} else {
					if (!flag)
						flag = true;
					else {
						AddPoint (x, y);
						break;
					}
				}
				y++;
			}
			y = i - 1;
			flag = false;
			while (y>=0) {
				if (position [y, x] == 0) {
					if (!flag)
						AddPoint (x, y);
				} else {
					if (!flag)
						flag = true;
					else {
						AddPoint (x, y);
						break;
					}
				}
				y--;
			}
			break;
		default:
			break;
		}
	}
	bool CanTouch(int [,]position,int nFromX,int nFromY,int nToX,int nToY){
		
		int i=0, j=0;
		int nMoveChessID;//, nTargetID;
		if (nFromX == nToX && nFromY == nToY)
			return false;//目的与原相同
        if (!rules._instance.KingBye(position, nFromX, nFromY, nToX, nToY))
			return false;
		nMoveChessID = position [nFromY, nFromX];
        //nTargetID = position [nToY, nToX];
		switch (nMoveChessID) {
		case 1://将
				if(nToY>2||nToX>5||nToX<3)
					return false;
				if(Mathf.Abs(nFromY-nToY)+Mathf.Abs(nToX-nFromX)>1)
					return false;//将智能走一步
			break;
		case 5://黑士
			if(nToY>2||nToX>5||nToX<3)
				return false;//士出九宫格
			if(Mathf.Abs(nFromY-nToY)!=1||Mathf.Abs(nToX-nFromX)!=1)
				return false;//走斜线
			break;
		case 6://黑相
			if(nToY>4)
				return false;
			if(Mathf.Abs(nFromX-nToX)!=2||Mathf.Abs(nFromY-nToY)!=2)
				return false;
			if(position[(nFromY+nToY)/2,(nFromX+nToX)/2]!=0)
				return false;
			break;
		case 7:
			if(nToY<nFromY)
				return false;
			if(nFromY<5&&nFromY==nToY)
				return false;
			if(nToY-nFromY+Mathf.Abs(nToX-nFromX)>1)
				return false;
			break;
		case 8:
				if(nToY<7||nToX>5||nToX<3)
					return false;
				if(Mathf.Abs(nFromY-nToY)+Mathf.Abs(nToX-nFromX)>1)
					return false;
			break;
		case 12://红士
			if(nToY<7||nToX>5||nToX<3)
				return false;
			if(Mathf.Abs(nFromY-nToY)!=1||Mathf.Abs(nToX-nFromX)!=1)
				return false;
			break;
		case 13://红相
			if(nToY<5)
				return false;//相不能过河
			if(Mathf.Abs(nFromX-nToX)!=2||Mathf.Abs(nFromY-nToY)!=2)
				return false;
			if(position[(nFromY+nToY)/2,(nFromX+nToX)/2]!=0)
				return false;
			break;
		case 14:
			if(nToY>nFromY)
				return false;
			if(nFromY>4&&nFromY==nToY)
				return false;
			if(nFromY-nToY+Mathf.Abs(nToX-nFromX)>1)
				return false;
			break;
		case 2:
		case 9://黑车和红车
			if(nFromY!=nToY&&nFromX!=nToX)
				return false;
			if(nFromY==nToY){
				if(nFromX<nToX){
					//right
					for(i=nFromX+1;i<nToX;i++)
						if(position[nFromY,i]!=0)
							return false;
				}
				else{
					for(i=nToX+1;i<nFromX;i++)
						if(position[nFromY,i]!=0)
							return false;
				}
			}
			else{
				//纵向
				if(nFromY<nToY){
					//down
					for(j=nFromY+1;j<nToY;j++)
						if(position[j,nFromX]!=0)
							return false;
				}
				else{
					//top
					for(j=nToY+1;j<nFromY;j++)
						if(position[j,nFromX]!=0)
							return false;
				}
			}
			break;
		case 3:
		case 10://两个马的走法
			if(!((Mathf.Abs(nToX-nFromX)==1&&Mathf.Abs(nToY-nFromY)==2)||(Mathf.Abs(nToX-nFromX)==2&&Mathf.Abs(nToY-nFromY)==1)))
				return false;
			if(nToX-nFromX==2){
				i=nFromX+1;
				j=nFromY;
			}
			else if(nFromX-nToX==2){
				i=nFromX-1;
				j=nFromY;
			}
			else if(nToY-nFromY==2){
				i=nFromX;
				j=nFromY+1;
			}
			else if(nFromY-nToY==2){
				i=nFromX;
				j=nFromY-1;

			}
			if(position[j,i]!=0)
				return false;
			break;
		case 4:
		case 11:
			if(nFromY!=nToY&&nFromX!=nToX)
				return false;
			//炮不吃子结果的路线中不能有棋子
			if(position[nToY,nToX]==0)
			{
				if(nFromY==nToY){
					//横向
					if(nFromX<nToX){
						//right
						for(i=nFromX+1;i<nToX;i++)
							if(position[nFromY,i]!=0)
								return false;
					}
					else{
						//left
						for(i=nToX+1;i<nFromX;i++)
							if(position[nFromY,i]!=0)
								return false;
					}
				}
				else{
					if(nFromY<nToY){
						//down
						for(j=nFromY+1;j<nToY;j++)
							if(position[j,nFromX]!=0)
								return false;
					}
					else{
						for(j=nToY+1;j<nFromY;j++)
							if(position[j,nFromX]!=0)
								return false;
					}
				}
			}
			//炮吃子时候
			else{
				int count=0;
				if(nFromY==nToY){
					if(nFromX<nToX){
						for(i=nFromX+1;i<nToX;i++)
							if(position[nFromY,i]!=0)
								count++;
						if(count!=1)
							return false;
					}
					else{
						//left
						for(i=nToX+1;i<nFromX;i++)
							if(position[nFromY,i]!=0)
								count++;
						if(count!=1)
							return false;
					}
				}
				else{
					//纵相
					if(nFromY<nToY){
						for(j=nFromY+1;j<nToY;j++)
							if(position[j,nFromX]!=0)
								count++;
						if(count!=1)
							return false;
					}
					else{
						//top
						for(j=nToY+1;j<nFromY;j++)
							if(position[j,nFromX]!=0)
								count++;
						if(count!=1)
							return false;
					}
				}
			}
			break;
		default:
			return false;
		}
		return true;
	
	}
	//将一个位置加入相关队列
	void AddPoint(int x,int y){
		//这个函数将一个位置加入数组relatepas中
        _relatePos[_posCount].x = x;
        _relatePos[_posCount].y = y;
		_posCount++;
	}

    

    //产生给定棋盘上所有合法的走法
    public int CreatePossibleMove(int[,] position, int nPly, bool nSide)
    {
        int nChessID;//x, y,
        //bool flag;
        int i, j;
        int m_nMoveCount = 0;
        for (j = 0; j < 9; j++)
            for (i = 0; i < 10; i++)
            {
                if (position[i, j] != 0)
                {
                    nChessID = position[i, j];
                    if (!nSide && rules._instance.IsRed(nChessID))
                        continue;//如果产生黑棋走法，跳过红棋
                    if (nSide && rules._instance.IsBlack(nChessID))
                        continue;//如果产生黑棋走法，跳过红棋
                    switch (nChessID)
                    {
                        case 1://黑将
                            Gen_KingMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 2://黑车
                            Gen_CarMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 3://黑马
                            Gen_HorseMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 4://hei pao
                            Gen_CanonMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 5://shi
                            Gen_BBishopMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 6://xiang
                            Gen_ElephantMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 7://bing
                            Gen_BPawnMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 8://shuai
                            Gen_KingMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 9://che
                            Gen_CarMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 10://ma
                            Gen_HorseMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 11://pao
                            Gen_CanonMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 12://shi
                            Gen_RBishopMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 13://xiang
                            Gen_ElephantMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                        case 14://bing
                            Gen_RPawnMoveAddSearch(ref m_nMoveCount, position, i, j, nPly);
                            break;
                    }
                }

            }
        return m_nMoveCount;//返回总的走法数
    }

    //在_moveList中插入一个走法
    //nfromx开始横坐标
    //nfromy开始纵坐标
    //ntox目标横坐标
    //ntoy目标纵坐标
    //nply走法所在层次
    int AddMove(ref int m_nMoveCount, int[,] position, int nFromx, int nFromy, int nTox, int nToy, int nPly)
    {
        if (m_nMoveCount >= 80)
            if (nPly >= 8)
                if (!rules._instance.KingBye(position, nFromx, nFromy, nTox, nToy))//判断是否老将见面 如果是见面了   就让他不加
                    return m_nMoveCount;
        _moveList[nPly, m_nMoveCount].From.x = nFromx;
        _moveList[nPly, m_nMoveCount].From.y = nFromy;
        _moveList[nPly, m_nMoveCount].To.x = nTox;
        _moveList[nPly, m_nMoveCount].To.y = nToy;
        m_nMoveCount++;
        return m_nMoveCount;
    }
    //将帅的走法
    //i，j标明棋子位置
    //nply标明插入到list第几层
    void Gen_KingMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
        int x, y;
        for (y = 0; y < 3; y++)
            for (x = 3; x < 6; x++)
                if (rules._instance.IsValidMove(position, j, i, x, y)) //走法是否合法
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        for (y = 7; y < 10; y++)
            for (x = 3; x < 6; x++)
                if (rules._instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);

    }
    //红士的走法
    void Gen_RBishopMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
        //i j棋子位置   x y目标位置
        int x, y;
        for (y = 7; y < 10; y++)
            for (x = 3; x < 6; x++)
                if (rules._instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
    }
    //黑士走法
    void Gen_BBishopMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
        int x, y;
        for (y = 0; y < 3; y++)
            for (x = 3; x < 6; x++)
                if (rules._instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
    }
    //相象走法
    void Gen_ElephantMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
        int x, y;
        //向右下方走步
        x = j + 2;
        y = i + 2;
        if (x < 9 && y < 10 && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //向右上方走步
        x = j + 2;
        y = i - 2;
        if (x < 9 && y >= 0 && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //向左下方走步
        x = j - 2;
        y = i + 2;
        if (x >= 0 && y < 10 && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //向左上方走步
        x = j - 2;
        y = i - 2;
        if (x >= 0 && y >= 0 && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);

    }
    //马的走法
    void Gen_HorseMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
        int x, y;
        //插入右下方的有效走法
        x = j + 2;
        y = i + 1;
        if ((x < 9 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //插入右上方的有效走法
        x = j + 2;
        y = i - 1;
        if ((x < 9 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //左下
        x = j - 2;
        y = i + 1;
        if ((x >= 0 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //左上
        x = j - 2;
        y = i - 1;
        if ((x >= 0 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //右下
        x = j + 1;
        y = i + 2;
        if ((x < 9 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //left down
        x = j - 1;
        y = i + 2;
        if ((x >= 0 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //right down
        x = j + 1;
        y = i - 2;
        if ((x < 9 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        //left top
        x = j - 1;
        y = i - 2;
        if ((x >= 0 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);

    }
    //车的走法
    void Gen_CarMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
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
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            else
            {
                if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
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
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            else
            {
                if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
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
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            else
            {
                if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
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
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            else
            {
                if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
                break;
            }
            y--;
        }
    }
    //红卒的走法
    void Gen_RPawnMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
        int x, y;
        int nChessID;
        nChessID = position[i, j];
        y = i - 1;
        x = j;
        if (y > 0 && !rules._instance.IsSameSide(nChessID, position[y, x]))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        if (i < 5)
        {
            y = i;
            x = j + 1;//right
            if (x < 9 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            x = j - 1;//right
            if (x >= 0 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        }
    }
    //黑兵走法
    void Gen_BPawnMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
    {
        int x, y;
        int nChessID;
        nChessID = position[i, j];
        y = i + 1;//前
        x = j;
        if (y < 10 && !rules._instance.IsSameSide(nChessID, position[y, x]))
            AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        if (i > 4)
        {
            y = i;
            x = j + 1;
            if (x < 9 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            x = j - 1;
            if (x >= 0 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
        }

    }
    //炮走法
    void Gen_CanonMoveAddSearch(ref int m_nMoveCount, int[,] position, int i, int j, int nPly)
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
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);//没有格棋子插入可以走位置
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
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
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
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
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
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
                    AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                        AddMove(ref m_nMoveCount, position, j, i, x, y, nPly);
                    break;
                }
            }
            y--;
        }
    }
}
