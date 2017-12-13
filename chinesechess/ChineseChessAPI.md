# Chinese Chess
## BtnControl类


函数名 |参数| 返回值类型 | 功能
---|---|---|---
BackMainScene     | 无| void | 返回主菜单按钮
OnePeopleModel    | 无| void | 单人模式按钮
TwoPeopleModel    | 无| void | 双人模式按钮
StartGameBtn      | 无| void | 开始游戏按钮
SetUpBtn          | 无| void | 设置按钮
GameOverBtn       | 无| void | 退出游戏按钮
BackStepBtn       | 无| void | 悔棋按钮
OnDifficultSelect | 无| void | 难度选择下拉菜单
## MoveSetting类

属性名 | 类型 | 功能
---|---|---|---|---
CHESSMANPOS        | void | 定义一个棋子位置的结构体
CHESSMOVE          | void | 单人模式按钮


函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
CreatePossibleMove | 无| void | 选定的棋子所有合法的走法

## Board类
属性名 | 类型 | 功能
---|---|---
chess   |int[,] | 定义一个棋子位置的结构体


函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
ChessInit   | 无| void | 初始化棋盘

## ChessClickL类


函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
clickItemOrChess  | 无| void | 点击棋子或者棋盘事件

## ViewManager类


函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
StartGameViewClear        | 无| void | 开始游戏清除界面
InitChessView        | int[,] | void | 初始化棋盘界面
SetPiecePos        | 无| void | 生成棋盘格子
InitPiece        | string,GameObject,string,int| void | 生成象棋的棋子
ChessGoStepView        | MoveSetting.CHESSMANPOS| void | 走一步棋的界面UI控制
HUIQI_View        | 无| void | 悔棋界面控制
AIGoStepView        | 无| void |AI走一步棋的界面UI控制 
ClickChessMoveDraw        |int, int| void | 将选择的棋子可以走的位置绘制出来
GetClickItemPos        | GameObject|MoveSetting.CHESSMANPOS | 通过Gameobject获取位置信息
PosGetChess        | int, int| GameObject |通过位置信息获取Gameobject AI走一步棋的界面UI控制
JiangJunCheck        | 无| void | 判断将和帅是否被将军了
SetTipsText        | string| void | 设置文字提示
JiangJunCheck        | 无| void | 清除棋盘上可走路线的提示

## BackStepChess类
属性名 | 类型 | 功能
---|---|---
HUIQIposition   |struct | 定义一个棋子位置的结构体
BackChess   |struct | 存储每一步棋的位置信息


函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
AddChessList   | 无| void | 将每一步下的棋存储起来
IloveHUIQI   | 无| void | 悔棋功能

## rules类

函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
IsBlack   | int| bool | 判断一个棋子是不是黑色
IsRed   | int| bool | 判断一个棋子是不是红色
IsSameSide   | int, int| bool | 判断两个棋子是不是同颜色
KingBye   | int [,],int,int,int,int| bool | 让王不能会面
IsValidMove   | int [,], int,int,int,int| bool |判断该步棋符不符合规则

## ChessControl类
属性名 | 类型 | 功能
---|---|---
RedMove   |bool | 判断是红方走还是黑方走
IsCanMove   |bool | 判断这个时候输赢状态能否走棋
posthread   |bool | 判断线程里面的内容是否执行完毕
IsSelectChess   |bool | 判断是否选择了棋子


函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
ChessGoStep   | MoveSetting.CHESSMANPOS| void | 走一步棋
threm   | 无| void | 调用AI进行下一步棋的计算并且进行界面的修改

## SearchEngine类
属性名 | 类型 | 功能
---|---|---
m_nSearchDepth   |int | AI算法最大搜索深度


函数名 | 参数 | 返回值类型 | 功能
---|---|---|---
<<<<<<< HEAD
PrincipalVariation   | MoveSetting.CHESSMANPOS| void | Alpha-beta 剪枝算法
SearchAGoodMove   | int[,]| MoveSetting.CHESSMOVE | 调用AI进行下一步棋的计算并且进行界面的修改
=======
<<<<<<< HEAD
PrincipalVariation   | MoveSetting.CHESSMANPOS| void | Alpha-beta 剪枝算法
SearchAGoodMove   | int[,]| MoveSetting.CHESSMOVE | 调用AI进行下一步棋的计算并且进行界面的修改
=======
PrincipalVariation   | MoveSetting.CHESSMANPOS pos| void | Alpha-beta 剪枝算法
SearchAGoodMove   | int[,] position| MoveSetting.CHESSMOVE | 调用AI进行下一步棋的计算并且进行界面的修改
>>>>>>> 231cc984d5596aafcfdb9d94ba2eba0fd0ef4561
>>>>>>> eff8265fea8da8be29c94be51bb37c00edf8a885
