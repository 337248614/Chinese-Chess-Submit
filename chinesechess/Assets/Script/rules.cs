using UnityEngine;
using System.Collections;
public class rules  {
    public static rules instance=new rules();

    int Jiang_x, Jiang_y, Shuai_x, Shuai_y;

    //得到将和帅的坐标
    void IsPosition()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 3; j < 6; j++)
                if (board.instance.chess[i, j] == 1)
                {
                    Jiang_x = j;
                    Jiang_y = i;
                }
        for (int i = 3; i < 6; i++)
            for (int j = 7; j < 10; j++)
                if (board.instance.chess[j, i] == 8)
                {
                    Shuai_x = i;
                    Shuai_y = j;
                }
    }

    //判断将和帅是否被将军了
    public void JiangJunCheck()
    {
        IsPosition();
        if (board.instance.chess[Jiang_y, Jiang_x] != 1)
        {
            //			blackclick.CanMove = false;
            ChessControl.instance.NextPlayerTipStr = "红色棋子胜利";
            ChessControl.instance.TrueOrFalse = false;
            return;
        }
        else if (board.instance.chess[Shuai_y, Shuai_x] != 8)
        {
            //		blackclick.CanMove = false;
            ChessControl.instance.NextPlayerTipStr = "黑色棋子胜利";
            ChessControl.instance.TrueOrFalse = false;
            return;
        }
        bool BOL;//bool 值
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                switch (board.instance.chess[j, i])
                {
                    case 2:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Shuai_x, Shuai_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "帅被車将军了";

                        break;
                    case 3:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Shuai_x, Shuai_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "帅被马将军了";
                        break;
                    case 4:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Shuai_x, Shuai_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "帅被炮将军了";
                        break;

                    case 7:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Shuai_x, Shuai_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "帅被兵将军了";
                        break;
                    case 9:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "将被車将军了";
                        break;
                    case 10:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "将被马将军了";
                        break;
                    case 11:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "将被炮将军了";
                        break;
                    case 14:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            ChessControl.instance.NextPlayerTipStr = "将被兵将军了";
                        break;
                }
            }
        }

    }

	//让王不能会面
    public bool IsBlack(int PieceNum)
    {
        if (PieceNum > 0 && PieceNum < 8)
			return true;
		else
			return false;
	}
	
	//判断一个棋子是不是红色
    public bool IsRed(int PieceNum)
    {
        if (PieceNum >= 8 && PieceNum < 15)
			return true;
		else
			return false;
	}
	
	//判断两个棋子是不是同颜色
    public bool IsSameSide(int PieceNumX, int PieceNumY)
    {
        if (IsBlack(PieceNumX) && IsBlack(PieceNumY) || IsRed(PieceNumX) && IsRed(PieceNumY))
			return true;
		else
			return false;
	}
	//public int [,] CHESS1 = new int[10, 9];//定义一个数组
	public  bool KingBye(int [,]CHESS,int FromX,int FromY,int ToX,int ToY){
		//假设法
		int Count=0, jiangx=0, jiangy=0, shuaix=0, shuaiy=0;
		//引用类型循环赋值
		int [,] CHESS1 = new int[10, 9];
		for (int i=0; i<CHESS.GetLength(0); i++)
			for(int j=0;j<CHESS.GetLength(1);j++)
				CHESS1[i,j]=CHESS[i,j];
		CHESS1 [ToY, ToX] = CHESS1 [FromY, FromX];
		CHESS1 [FromY, FromX] = 0;//清空原来位置
		for (int i=0; i<3; i++) {
			for (int j=3; j<6; j++)
				if (CHESS1 [i, j] == 1) {
					jiangx = j;
					jiangy = i;
				}
		}
			for(int i=7;i<10;i++){
			for(int j=3;j<6;j++)
				if(CHESS1[i,j]==8){
					shuaix = j;
					shuaiy = i;
				}
			}
		//上面找到了将 和帅的坐标
		if (jiangx == shuaix) {
			//将和帅的x  相等
			for (int i=jiangy+1; i<shuaiy; i++) {
				if (CHESS1 [i, jiangx] != 0)
					Count++;
			}
		} else
			Count = -1;
		//print (shuaix + "," + shuaiy + "." + jiangx + "," + jiangy);
		if (Count == 0)
			return false;
		return true;
	
	}
	//所有棋子的走棋规则
	//type =false 黑色走棋,true 红色走
	public  bool IsValidMove(int [,]position, int FromX,int FromY,int ToX,int ToY){	
		int i=0, j=0;

		int nMoveChessID, nTargetID;
		nMoveChessID = position [FromY, FromX];//得到移动前的棋
		nTargetID = position [ToY, ToX];//得到移动后的棋子
		if (FromY == ToY && FromX == ToX) //目的与原相同
			return false;
        //int ChessOne = board.chess[FromY,FromX];
        //int ChessTwo = board.chess [ToY, ToX];
		if (IsSameSide (nMoveChessID, nTargetID)) 
			return false;
		if (!KingBye (position,FromX, FromY, ToX, ToY))
			return false;
		switch (nMoveChessID) {
		case 1://如果现在是黑将走棋
				if(ToY>2||ToX>5||ToX<3)//出了九宫格
					return false;
				if(Mathf.Abs(FromY-ToY)+Mathf.Abs(ToX-FromX)>1)
					return false;//将只能走一步
			break;
		case 8://如果现在是红将走棋
		
				if(ToY<7||ToX>5||ToX<3)
					return false;//目标出了九宫格
				if(Mathf.Abs(FromY-ToY)+Mathf.Abs(ToX-FromX)>1)
					return false;
			break;
		case 12://红士
			if(ToY<7||ToX>5||ToX<3)
				return false;//出了九宫格
			if(Mathf.Abs(FromY-ToY)!=1||Mathf.Abs(ToX-FromX)!=1)
				return false;//士走斜线
			break;
		case 5:
			if(ToY>2||ToX>5||ToX<3)
				return false;//士出九宫格
			if(Mathf.Abs(FromY-ToY)!=1||Mathf.Abs(ToX-FromX)!=1)
				return false;//士走斜线
			break;
		case 13://红相走棋
			if(ToY<5)
				return false;//相不能过河
			if(Mathf.Abs(FromX-ToX)!=2||Mathf.Abs(FromY-ToY)!=2)
				return false;//相走田字
			if(position[(FromY+ToY)/2,(FromX+ToX)/2]!=0)
				return false;//相眼被独
			break;
		case 6://黑相
			if(ToY>4)
				return false;//象不能过河
			if(Mathf.Abs(FromX-ToX)!=2||Mathf.Abs(FromY-ToY)!=2)
				return false;//象走田字
			if(position[(FromY+ToY)/2,(FromX+ToX)/2]!=0)
				return false;//象眼被堵
			break;
		case 7://黑兵
			if(ToY<FromY)
				return false;//兵不能回头
			if(FromY<5&&FromY==ToY)
				return false;//兵过河前智能走直线
			if(ToY-FromY+Mathf.Abs(ToX-FromX)>1)
				return false;//兵只能走一步
			break;
		case 14:
			if(ToY>FromY)
				return false;//卒不回头
			if(FromY>4&&FromY==ToY)
				return false;//兵过河前只能走直线
			if(FromY-ToY+Mathf.Abs(ToX-FromX)>1)
				return false;
			break;
		case 2:
		case 9:
			if(FromY!=ToY&&FromX!=ToX)
				return false;//车走直线
			if(FromY==ToY){
				if(FromX<ToX){//right
					for(i=FromX+1;i<ToX;i++)
						if(position[FromY,i]!=0)
							return false;//中间有棋子
				}
				else{
					for(i=ToX+1;i<FromX;i++)
						if(position[FromY,i]!=0)
							return false;
				}
			}
			else{
				if(FromY<ToY){
					for(j=FromY+1;j<ToY;j++)
						if(position[j,FromX]!=0)
							return false;
				}
				else{
					for(j=ToY+1;j<FromY;j++)
						if(position[j,FromX]!=0)
							return false;
				}
			}
			break;
		case 3://黑马
		case 10:
			if(!((Mathf.Abs(ToX-FromX)==1&&Mathf.Abs(ToY-FromY)==2)||(Mathf.Abs(ToX-FromX)==2&&Mathf.Abs(ToY-FromY)==1)))
				return false;//马走日字
			if(ToX-FromX==2){
				i=FromX+1;
				j=FromY;
			}
			else if(FromX-ToX==2){
				i=FromX-1;
				j=FromY;
			}
			else if(ToY-FromY==2){
				i=FromX;
				j=FromY+1;
			}
			else if(FromY-ToY==2){
				i=FromX;
				j=FromY-1;
			}
			if(position[j,i]!=0)
				return false;//斑马脚
			break;
		case 4://黑炮
		case 11:
			if(FromY!=ToY&&FromX!=ToX)
				return false;//炮走直线
			//炮吃子时经过的路线中不能有棋子
			if(position[ToY,ToX]==0){
				if(FromY==ToY){
					if(FromX<ToX){
						for(i=FromX+1;i<ToX;i++)
							if(position[FromY,i]!=0)
								return false;

					}
					else{
						for(i=ToX+1;i<FromX;i++)
							if(position[FromY,i]!=0)
								return false;
					}
				}
				else{
					if(FromY<ToY){
						for(j=FromY+1;j<ToY;j++)
							if(position[j,FromX]!=0)
								return false;
					}
					else{
						for(j=ToY+1;j<FromY;j++)
							if(position[j,FromX]!=0)
								return false;
					}
				}
			}
			else{//炮吃子时
				int count = 0;
				if(FromY==ToY){
					if(FromX<ToX){
						for(i=FromX+1;i<ToX;i++)
							if(position[FromY,i]!=0)
								count++;
						if(count!=1)
							return false;
					}
					else{
						for(i=ToX+1;i<FromX;i++)
							if(position[FromY,i]!=0)
								count++;
						if(count!=1)
							return false;
					}
				}
				else{
					if(FromY<ToY){
						for(j=FromY+1;j<ToY;j++)
							if(position[j,FromX]!=0)
								count++;
						if(count!=1)
							return false;
					}
					else{
						for(j=ToY+1;j<FromY;j++)
							if(position[j,FromX]!=0)
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
		//print ("棋子类型："+position[FromY,FromX]+"    ："+FromX +","+ FromY + "--" + ToX + "," + ToY);
		return true;

	}

}

