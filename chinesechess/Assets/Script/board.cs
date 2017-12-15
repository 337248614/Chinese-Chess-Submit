using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SDG;
public class board 
{
    public static board _instance=new board();
    public int[,] chess ;

    public void ChessInit()
    {
        chess = new int[10, 9]{  
			{2,3,6,5,1,5,6,3,2},
			{0,0,0,0,0,0,0,0,0},
			{0,4,0,0,0,0,0,4,0},
			{7,0,7,0,7,0,7,0,7},
			{0,0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0,0},
			{14,0,14,0,14,0,14,0,14},
			{0,11,0,0,0,0,0,11,0},
			{0,0,0,0,0,0,0,0,0},
			{9,10,13,12,8,12,13,10,9}
		};
        //初始化其他对象
        ChessControl._instance.ChessMoveList.Clear();//重置悔棋记录对象        
    }
}
