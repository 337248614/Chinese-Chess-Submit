public interface IMoveSetting  {

    //判断一个棋子是不是黑色


    //产生给定棋盘上所有合法的走法
    //用来产生局面position中所有可能的走法
    //postion包含所有棋子位置信息的二维数组
    //nply指明当前搜索层数，每层将走法存在不同的位置
    //nside指明产生哪一方的走法 true red false black
    int CreatePossibleMove(int[,] position, int nPly, bool nSide);

    //点击棋子能判断得出来他的那些位置是能走的呢？

    //1、先得到他能走到的位置
    //2* 获取位置坐标
    //3.得到item对象
    //把预设体添加到场景
    //5.找到预设体的富容器变且添加， 
    //6.设置预设体的位置   tox， toy
    /*-----------------------------------------*/
    //判断点击到的是什么棋子
    void ClickChessMoveDraw(int fromx, int fromy);
}
