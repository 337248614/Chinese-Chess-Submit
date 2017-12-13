using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class BackStepChess :MonoBehaviour{

    public static BackStepChess instance; 
    public List<BackChess> BackChessList =new List<BackChess>();//将对弈的过程存储起来

    void Start() 
    {        
        instance = this;
    }

	public struct HUIQIposition{
		public int x;
		public int y;
	}

    //存储每一步棋的位置信息，提示框的位置
    public struct BackChess
    {
        public HUIQIposition From;
        public HUIQIposition To;
        public int FromNum;
        public int ToNum;
    }
	//下面用来悔棋

    public void AddChessList(int fromx,int fromy,int tox,int toy) {
        BackChess TempBack = new BackChess();
        HUIQIposition TempPos = new HUIQIposition();
        TempPos.x = fromx;
        TempPos.y = fromy;
        TempBack.From = TempPos;
        TempPos.x = tox;
        TempPos.y = toy;
        TempBack.To = TempPos;
        TempBack.FromNum=board.instance.chess[fromy,fromx];
        TempBack.ToNum = board.instance.chess[toy, tox];
        BackChessList.Add(TempBack);
    }

	

	//开始悔棋功能了		
	public void IloveHUIQI()
    {
        int fromx, fromy, tox, toy, backStepNum;
        if (BtnControl.ChessPeople == 2 || ChessControl.instance.IsCanMove == false)
            backStepNum = 1;
        else
            backStepNum = 2;
        ChessControl.instance.IsCanMove = true;
        for (int i = 1; i < backStepNum + 1; i++)
        {
            int length=BackChessList.Count;
            fromx = BackChessList[length - 1].From.x;
            fromy = BackChessList[length - 1].From.y;
            tox = BackChessList[length -1].To.x;
            toy = BackChessList[length -1].To.y;
            board.instance.chess[fromy, fromx] = BackChessList[length -1].FromNum;
            board.instance.chess[toy, tox] = BackChessList[length - 1].ToNum;
            BackChessList.RemoveAt(length-1);
            ChessControl.instance.RedMove = !ChessControl.instance.RedMove;
        }
	}


}
