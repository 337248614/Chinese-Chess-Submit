using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDG;
public class ChineseChessApi  {

    //进行棋盘的初始化0表示没有棋子，1-14依次是将，黑车，黑马，黑炮，
    //黑士，黑象，黑卒，红帅，红车，红马，红炮，红士，红相，红兵
    public void ChessInit()
    {
        ChessControl._instance.InitChessBoard();
    }
    //设置游戏模式
    public void SetGameModel(GameModel gamemodel) 
    {
        ChessControl._instance.SetGameModel(gamemodel);
    }
    //移动或吃棋子
    public bool MoveOrEatChess(ChessPosition TempPosFrom, ChessPosition TempPosTo) 
    { 
        return ChessControl._instance.MoveOrEatChess(TempPosFrom, TempPosTo);
    }
    //获取AI计算的棋子变换结果
    public ChessMove GetAiMove()
    {
        return ChessControl._instance.GetAiMove();
    }
    //判断是否结束游戏
    public bool isGameOver(int[,] position) 
    {
        return ChessControl._instance.isGameOver(position);
    }
    //判断是否是红方胜
    public bool IsRedWin(int[,] position) 
    {
        return ChessControl._instance.IsRedWin(position);
    }
    //判断是否是黑方胜
    public bool IsBlackWin(int[,] position)
    {
        return ChessControl._instance.IsBlackWin(position);
    }
    //设置AI游戏难度
    public void SetChessModel(DifficultyModel level) 
    {
        ChessControl._instance.SetChessModel(level) ;
    }
    //开始悔棋功能了并返回该走的棋子位置的变换
    public ChessMove BackStep()
    {
        return ChessControl._instance.BackStep();
    }
    //检测是否被将军，如果被将军返回将军的棋子的编号
    public int KingAttackCheck() 
    {
        return ChessControl._instance.KingAttackCheck();
    }
    //选择棋子之后得到棋子可以走的位置
    public List<ChessPosition> ChessCanMove(ChessPosition pos) 
    {
        return ChessControl._instance.ChessCanMove(pos);
    }
}
