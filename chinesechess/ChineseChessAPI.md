# Chinese Chess

		
## DifficultyModel枚举类型
枚举类型名称 |含义
---|---
easy|简单模式
middle|中等难度模式
difficult|困难模式

## GameModel枚举类型
枚举类型名称 |含义
---|---
PersonVSAi|人机模式
PersonVSPerson|人人模式

## ChessPosition属性类
属性名称 |类型|含义
---|---|---
x|int|数组中的位置下标
y|int|数组中的位置下标
## ChessMove属性类
属性名称 |类型|含义
---|---|---
From|ChessPosition|开始的位置
To|ChessPosition|走到的位置
FromChessNum|int|开始位置棋子的编号
ToChessNum|int|走到的位置棋子的编号|

## ChineseChessApi

函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
ChessInit   | 无| void | 进行棋盘的初始化
     //用二维数组表示棋盘。对棋子进行编号，0表示没有棋子，1-14依次是将，黑车，黑马，黑炮，黑士，黑象，黑卒，红帅，红车，红马，红炮，红士，红相，红兵
     {  
			{2 ,3 ,6 ,5 ,1 ,5 ,6 ,3 ,2 },
			{0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },
			{0 ,4 ,0 ,0 ,0 ,0 ,0 ,4 ,0 },
			{7 ,0 ,7 ,0 ,7 ,0 ,7 ,0 ,7 },
			{0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },
			{0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },
			{14,0 ,14,0 ,14,0 ,14,0 ,14},
			{0 ,11,0 ,0 ,0 ,0 ,0 ,11,0 },
			{0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },
			{9 ,10,13,12,8 ,12,13,10,9 }
		}
函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
SetGameModel   | GameModel| void | 设置游戏模式
SetChessModel   | DifficultyModel| void | 设置AI游戏难度
ChessCanMove   | List<ChessPosition>| ChessPosition | 选择棋子之后得到棋子可以走的位置
MoveOrEatChess   | ChessPosition，ChessPosition| bool | 移动或吃棋子可以移动返回true
GetAiMove   | 无| ChessMove | 获取AI计算的棋子变换结果
BackStep   | ChessMove| void | 悔棋功能并返回该走的棋子位置的变换
KingAttackCheck   | int| void | 检测是否被将军，如果被将军返回将军的棋子的编号
isGameOver   | 无| bool | 判断是否结束游戏
IsRedWin   | 无| bool | 判断是否是红方胜
IsBlackWin   | 无| bool | 判断是否是黑方胜


