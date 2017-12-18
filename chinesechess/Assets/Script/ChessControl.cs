using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using SDG;
namespace SDG
{
    public class ChessMove
    {
        public ChessPosition From;//开始的位置
        public ChessPosition To;//走到的位置
        public int FromChessNum;
        public int ToChessNum;

        public  ChessMove(ChessPosition _from, ChessPosition _to) {
            From = _from;
            To = _to;
            FromChessNum = board._instance.chess[_from.y, _from.x];
            ToChessNum = board._instance.chess[_to.y, _to.x];
        }
    }
    public class ChessPosition
    {
        public int x;//记录x值
        public int y;//记录y值

        public  ChessPosition(int _x,int _y)
        {
            x = _x;
            y = _y;
        }
    }
    public class ChessControl 
    {
        public static ChessControl _instance=new ChessControl();
        
        public DifficultyModel _difficultyModel;
        public GameModel _gameModel;
        public List<ChessMove> ChessMoveList = new List<ChessMove>();//将对弈的过程存储起来


        //进行棋盘的初始化
        public void InitChessBoard()
        {
            board._instance.ChessInit();
            
        }
        //设置游戏模式
        public void SetGameModel(GameModel gamemodel) 
        {
            _gameModel = gamemodel;
        }
        //移动或吃棋子
        public bool MoveOrEatChess(ChessPosition TempPosFrom, ChessPosition TempPosTo)
        {
            int _fromX = TempPosFrom.x;
            int _fromY = TempPosFrom.y;
            int _toX = TempPosTo.x;
            int _toY = TempPosTo.y;
            bool ba = rules._instance.IsValidMove(board._instance.chess, _fromX, _fromY, _toX, _toY);
            if (!ba) return false;
            else
            {
                AddChessList(TempPosFrom, TempPosTo);
                board._instance.chess[_toY, _toX] = board._instance.chess[_fromY, _fromX];
                board._instance.chess[_fromY, _fromX] = 0;
                return true;
            }
        }
        //根据选择的棋子返回可走的位置
        public List<ChessPosition> ChessCanMove(ChessPosition pos)
        {
            int fromx = pos.x;
            int fromy = pos.y;
            List<ChessPosition> chessPositionList = new List<ChessPosition>();
            int ChessID = board._instance.chess[fromy, fromx];
            switch (ChessID)
            {
                case 1:
                case 8:
                    Gen_KingMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
                case 2:
                case 9:
                    Gen_CarMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
                case 3:
                case 10:
                    Gen_HorseMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
                case 4:
                case 11:
                    Gen_CanonMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
                case 5://黑士
                    Gen_BBishopMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
                case 6://黑象
                case 13://红相
                    Gen_ElephantMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
                case 7://黑兵
                    Gen_BPawnMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
                case 12://红shi
                    Gen_RBishopMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;

                case 14://红兵
                    Gen_RPawnMove(chessPositionList, board._instance.chess, fromx, fromy);
                    break;
            }
            return chessPositionList;
        }
        //获取AI搜索的结果
        public ChessMove GetAiMove()
        {
            SearchEngine.Move move =SearchEngine._instance.SearchAGoodMove(board._instance.chess);
            ChessPosition posFrom = new ChessPosition(move.From.x, move.From.y);
            ChessPosition posTo = new ChessPosition(move.To.x, move.To.y);
            ChessMove chessmove = new ChessMove(posFrom, posTo);
            return chessmove; 
        }
        //判断是否结束游戏
        public bool isGameOver(int[,] position)
        {
            bool RedLive = false, BlackLive = false;
            for (int i = 3; i < 6; i++) { 
                for (int j = 0; j < 3; j++)
                {
                    if (position[j, i] == 1)
                        BlackLive = true;
                }
            }
            for (int i = 3; i < 6; i++){
                for (int j = 7; j < 10; j++)
                {
                    if (position[j, i] == 8)
                        RedLive = true;
                }
            }
            return RedLive && BlackLive;
        }
        //判断是否是红方胜
        public bool IsRedWin(int[,] position)
        {
            bool BlackLive = false;
            for (int i = 3; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (position[j, i] == 1)
                        BlackLive = true;
                }
            }
            return !BlackLive;
        }
        //判断是否是黑方胜
        public bool IsBlackWin(int[,] position)
        {
            bool RedLive = false;
            for (int i = 3; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (position[j, i] == 1)
                        RedLive = true;
                }
            }
            return !RedLive;
        }
        //设置AI游戏难度
        public void SetChessModel(DifficultyModel level)
        {
            switch (level)
	        {
                case DifficultyModel.easy     : SearchEngine._searchDepth = 1; break;
                case DifficultyModel.middle   : SearchEngine._searchDepth = 2; break;
                case DifficultyModel.difficult: SearchEngine._searchDepth = 3; break;
                default: break;
	        }
        }
        //检测是否被将军
        public int KingAttackCheck()
        {
            ChessPosition blackKing = GetBlackKingPosition();
            ChessPosition redKing = GetRedKingPosition();
            int _jiang_X = blackKing.x;
            int _jiang_Y = blackKing.y;
            int _shuai_X = redKing.x;
            int _shuai_Y = redKing.y;
            if (board._instance.chess[_jiang_Y, _jiang_X] != 1)
            {
                return -1;
            }
            else if (board._instance.chess[_shuai_Y, _shuai_X] != 8)
            {
                return -1;
            }
            bool BOL;//bool 值
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (board._instance.chess[j, i])
                    {
                        case 2:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _shuai_X, _shuai_Y);
                            if (BOL)
                                return 2;

                            break;
                        case 3:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _shuai_X, _shuai_Y);
                            if (BOL)
                                return 3;
                            break;
                        case 4:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _shuai_X, _shuai_Y);
                            if (BOL)
                                return 4;
                            break;

                        case 7:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _shuai_X, _shuai_Y);
                            if (BOL)
                                return 7;
                            break;
                        case 9:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _jiang_X, _jiang_Y);
                            if (BOL)
                                return 9;
                            break;
                        case 10:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _jiang_X, _jiang_Y);
                            if (BOL)
                                return 10;
                            break;
                        case 11:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _jiang_X, _jiang_Y);
                            if (BOL)
                                return 11;
                            break;
                        case 14:
                            BOL = rules._instance.IsValidMove(board._instance.chess, i, j, _jiang_X, _jiang_Y);
                            if (BOL)
                                return 14;
                            break;
                    }
                }
            }
            return 0;

        }
        //开始悔棋功能了并返回该走的棋子位置的变换
        public ChessMove BackStep()
        {
            int fromx, fromy, tox, toy;            
            int length = ChessMoveList.Count;

            fromx = ChessMoveList[length - 1].From.x;
            fromy = ChessMoveList[length - 1].From.y;
            tox = ChessMoveList[length - 1].To.x;
            toy = ChessMoveList[length - 1].To.y;

            ChessPosition TempPosFrom = ChessMoveList[length - 1].To;
            ChessPosition TempPosTo = ChessMoveList[length - 1].From;
            ChessMove move = new ChessMove(TempPosFrom, TempPosTo);
            board._instance.chess[fromy, fromx] = ChessMoveList[length - 1].FromChessNum;
            board._instance.chess[toy, tox] = ChessMoveList[length - 1].ToChessNum;
            ChessMoveList.RemoveAt(length - 1);
            return move;
        }
        void AddChessList(ChessPosition TempPosFrom, ChessPosition TempPosTo)
        {
            ChessMove TempBack = new ChessMove(TempPosFrom,TempPosTo);
            ChessMoveList.Add(TempBack);
        }
        //得到将和帅的坐标
        ChessPosition GetBlackKingPosition()
        {
            int x=0, y=0;
            for (int j = 0; j < 3; j++)
                for (int i = 3; i < 6; i++)
                    if (board._instance.chess[j, i] == 1)
                    {
                        x = i; y = j;
                    }
            ChessPosition pos = new ChessPosition(x,y);
            return pos;
        }
        ChessPosition GetRedKingPosition()
        {
            int x=0, y=0;
            for (int j = 7; j < 10; j++)
             for (int i = 3; i < 6; i++)
                    if (board._instance.chess[j, i] == 8)
                    {
                         x=i;y=j;
                    }
            ChessPosition pos = new ChessPosition(x,y);
            return pos;
        }
        //得到将的走法
        void Gen_KingMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
        {//两个参数 fromx  和fromy
            int x, y;
            for (y = 0; y < 3; y++)
                for (x = 3; x < 6; x++)
                    if (rules._instance.IsValidMove(position, j, i, x, y))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    } //走法是否合法
            for (y = 7; y < 10; y++)
                for (x = 3; x < 6; x++)
                    if (rules._instance.IsValidMove(position, j, i, x, y))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }//走法是否合法

        }
        //红士的走法
        void Gen_RBishopMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
        {
            //i j棋子位置   x y目标位置
            int x, y;
            for (y = 7; y < 10; y++)
                for (x = 3; x < 6; x++)
                    if (rules._instance.IsValidMove(position, j, i, x, y))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }//走法是否合法
        }
        //黑士走法
        void Gen_BBishopMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
        {
            int x, y;
            for (y = 0; y < 3; y++)
                for (x = 3; x < 6; x++)
                    if (rules._instance.IsValidMove(position, j, i, x, y))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }//走法是否合法
        }
        //相象走法
        void Gen_ElephantMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
        {
            int x, y;
            //向右下方走步
            x = j + 2;
            y = i + 2;
            if (x < 9 && y < 10 && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //向右上方走步
            x = j + 2;
            y = i - 2;
            if (x < 9 && y >= 0 && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //向左下方走步
            x = j - 2;
            y = i + 2;
            if (x >= 0 && y < 10 && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //向左上方走步
            x = j - 2;
            y = i - 2;
            if (x >= 0 && y >= 0 && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
        }
        //马的走法
        void Gen_HorseMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
        {
            int x, y;
            //插入右下方的有效走法
            x = j + 2;
            y = i + 1;
            if ((x < 9 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //插入右上方的有效走法
            x = j + 2;
            y = i - 1;
            if ((x < 9 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //左下
            x = j - 2;
            y = i + 1;
            if ((x >= 0 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //左上
            x = j - 2;
            y = i - 1;
            if ((x >= 0 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //右下
            x = j + 1;
            y = i + 2;
            if ((x < 9 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //left down
            x = j - 1;
            y = i + 2;
            if ((x >= 0 && y < 10) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //right down
            x = j + 1;
            y = i - 2;
            if ((x < 9 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            //left top
            x = j - 1;
            y = i - 2;
            if ((x >= 0 && y >= 0) && rules._instance.IsValidMove(position, j, i, x, y))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }

        }
        //车的走法
        void Gen_CarMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
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
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
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
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
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
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
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
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
                else
                {
                    if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
                    break;
                }
                y--;
            }
        }
        //红卒的走法
        void Gen_RPawnMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
        {
            int x, y;
            int nChessID;
            nChessID = position[i, j];
            y = i - 1;
            x = j;
            if (y > 0 && !rules._instance.IsSameSide(nChessID, position[y, x]))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            if (i < 5)
            {
                y = i;
                x = j + 1;//right
                if (x < 9 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
                x = j - 1;//right
                if (x >= 0 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
            }
        }
        //黑兵走法
        void Gen_BPawnMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
        {
            int x, y;
            int nChessID;
            nChessID = position[i, j];
            y = i + 1;//前
            x = j;
            if (y < 10 && !rules._instance.IsSameSide(nChessID, position[y, x]))
            {
                MoveCheck(chessPositionList, position, j, i, x, y);
            }
            if (i > 4)
            {
                y = i;
                x = j + 1;
                if (x < 9 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
                x = j - 1;
                if (x >= 0 && !rules._instance.IsSameSide(nChessID, position[y, x]))
                {
                    MoveCheck(chessPositionList, position, j, i, x, y);
                }
            }

        }
        //炮走法
        void Gen_CanonMove(List<ChessPosition> chessPositionList, int[,] position, int j, int i)
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
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
                }
                else
                {
                    if (!flag)
                        flag = true;
                    else
                    {
                        if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                            MoveCheck(chessPositionList, position, j, i, x, y);
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
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
                }
                else
                {
                    if (!flag)
                        flag = true;
                    else
                    {
                        if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                            MoveCheck(chessPositionList, position, j, i, x, y);
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
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
                }
                else
                {
                    if (!flag)
                        flag = true;
                    else
                    {
                        if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                            MoveCheck(chessPositionList, position, j, i, x, y);
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
                    {
                        MoveCheck(chessPositionList, position, j, i, x, y);
                    }
                }
                else
                {
                    if (!flag)
                        flag = true;
                    else
                    {
                        if (!rules._instance.IsSameSide(nChessID, position[y, x]))
                        {
                            MoveCheck(chessPositionList, position, j, i, x, y);
                        }
                        break;
                    }
                }
                y--;
            }
        }
        //把传入进来的可走位置全部画出来
        void MoveCheck(List<ChessPosition> chessPositionList, int[,] position, int j, int i, int x, int y)
        {
            if (rules._instance.KingBye(position, j, i, x, y))
            {
                ChessPosition pos = new ChessPosition(x,y);
                chessPositionList.Add(pos);
            }
        }
    }
}