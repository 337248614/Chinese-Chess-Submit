using UnityEngine;
using System.Collections;
using System.Threading;
public class ChessControl : MonoBehaviour {
    public static ChessControl instance;
    [System.NonSerialized]
    //存储位置移动的起点和终点信息
    public  int FromX = -1, FromY = -1, ToX = -1, ToY = -1;
    //判断是红方走还是黑方走
	public  bool RedMove =true;
    //判断这个时候输赢状态能否走棋  //重新开始记得该true
	public  bool IsCanMove=true;
    //判断线程里面的内容是否执行完毕
	public static bool posthread=true;
    //判断是否选择了棋子
    public bool IsSelectChess = false;

    void Start()
    {
        instance = this;
    }

    //移动或者吃子
    void MoveOrEatChess(int FromX, int FromY, int ToX, int ToY)
    {
        board.instance.chess[ToY, ToX] = board.instance.chess[FromY, FromX];
        board.instance.chess[FromY, FromX] = 0;
	}

    //走一步棋
    public void ChessGoStep(MoveSetting.CHESSMANPOS pos)
    {
        if (IsCanMove == false || posthread == false)
			return;
        if (IsSelectChess) 
        {
            int tox = pos.x;
            int toy = pos.y;
            bool ba = rules.instance.IsValidMove(board.instance.chess, FromX, FromY, tox, toy);            
            if (board.instance.chess[toy, tox] == 0)
            {
                if (!ba)
                {
                    IsSelectChess = false;
                    return;
                }
                if (IsSelectChess.Equals(false)) return;
                ToX = pos.x;
                ToY = pos.y;
                BackStepChess.instance.AddChessList(FromX, FromY, tox, toy); ;
                MoveOrEatChess(FromX,FromY, pos.x, pos.y);
                RedMove = !RedMove;
                IsSelectChess = false;
                if (!RedMove && BtnControl.ChessPeople == 1)
                Invoke("threm", 0.2f);
            }
            else
            {
                
                bool isSame = rules.instance.IsSameSide(board.instance.chess[toy, tox], board.instance.chess[FromY, FromX]);
                if (isSame)
                {
                    FromX = pos.x;
                    FromY = pos.y;                    
                }
                else
                {
                    if (!ba)
                    {
                        IsSelectChess = false;
                        return;
                    }
                    ToX = pos.x;
                    ToY = pos.y;
                    BackStepChess.instance.AddChessList(FromX, FromY, tox, toy);
                    MoveOrEatChess(FromX, FromY, ToX, ToY);
                    RedMove = !RedMove;
                    IsSelectChess = false;
                    if (!RedMove && BtnControl.ChessPeople == 1)
                        Invoke("threm", 0.2f);
                }
            }
        }
        else
        {
            if (board.instance.chess[pos.y, pos.x] == 0) return;
            if (!RedMove && board.instance.chess[pos.y, pos.x] > 8) return;
            if (RedMove && board.instance.chess[pos.y, pos.x] < 8) return;
            FromX = pos.x;
            FromY = pos.y;
            IsSelectChess = true;
        }
	}
    
    //调用AI进行下一步棋的计算并且进行界面的修改
	void threm(){
        if (!RedMove)
        {
            MoveSetting.CHESSMOVE chere = SearchEngine.instance.SearchAGoodMove(board.instance.chess);
            ViewManager.instance.AIGoStepView(chere);
            AIGoStep(chere);
            
        }
	}
    //AI进行移动
    void AIGoStep(MoveSetting.CHESSMOVE chere)
    {
        FromX = chere.From.x;
        FromY = chere.From.y;
        IsSelectChess = true;
        ChessGoStep(chere.To);
    }

 
}
