using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour {
    public static ViewManager instance;
    [System.NonSerialized]
    public GameObject BlackSelectPic, RedSelectPic, AIMoveTipPic;
    int Jiang_x, Jiang_y, Shuai_x, Shuai_y;
    GameObject MissChess;
    Text NextTips;//提示下一步谁走的信息
	void Start () {
        instance = this;
        MissChess = GameObject.Find("xiaoshi");
        NextTips = GameObject.Find("NextTips").GetComponent<Text>();
        AIMoveTipPic = GameObject.Find("chessRobot");
	}

    //开始游戏清除界面
    public void StartGameViewClear()
    {
        ClearItem();
        if (BlackSelectPic!=null) BlackSelectPic.SetActive(false);
        if (RedSelectPic != null) RedSelectPic.SetActive(false);
    }
   
    //初始化棋盘界面
    public void InitChessView(int[,] chess) 
    {
        int count = 1;
        SetPiecePos();
        for (int i = 1; i <= 90; i++)
        {
            GameObject obj = GameObject.Find("item" + i.ToString());
            int x = GetClickItemPos(obj).x;
            int y = GetClickItemPos(obj).y;
            switch (chess[y, x])
            {
                case 1:
                    count++;
                    InitPiece("black_jiang", obj, "b_jiang", count);
                    break;
                case 2:
                    count++;
                    InitPiece("black_ju", obj, "b_ju", count);
                    break;
                case 3:
                    count++;
                    InitPiece("black_ma", obj, "b_ma", count);
                    break;
                case 4:
                    count++;
                    InitPiece("black_pao", obj, "b_pao", count);
                    break;
                case 5:
                    count++;
                    InitPiece("black_shi", obj, "b_shi", count);
                    break;
                case 6:
                    count++;
                    InitPiece("black_xiang", obj, "b_xiang", count);
                    break;
                case 7:
                    count++;
                    InitPiece("black_bing", obj, "b_bing", count);
                    break;
                case 8:
                    count++;
                    InitPiece("red_shuai", obj, "r_shuai", count);
                    break;
                case 9:
                    count++;
                    InitPiece("red_ju", obj, "r_ju", count);
                    break;
                case 10:
                    count++;
                    InitPiece("red_ma", obj, "r_ma", count);
                    break;
                case 11:
                    count++;
                    InitPiece("red_pao", obj, "r_pao", count);
                    break;
                case 12:
                    count++;
                    InitPiece("red_shi", obj, "r_shi", count);
                    break;
                case 13:
                    count++;
                    InitPiece("red_xiang", obj, "r_xiang", count);
                    break;
                case 14:
                    count++;
                    InitPiece("red_bing", obj, "r_bing", count);
                    break;
            }
            SetTipsText("红方走");
        }
    }
    //生成棋盘格子
    public void SetPiecePos()
    {
        int xx = 0, yy = 0;
        GameObject a = GameObject.Find("chess");
        for (int i = 1; i <= 90; i++)
        {
            GameObject ite = (GameObject)Instantiate(Resources.Load("item"));//找到预设体
            ite.transform.SetParent(a.transform);           //给预设体指定添加到什么地方
            GameObject b = GameObject.Find(ite.name);    //找到这个预设体的名字，给他做一些操作
            b.transform.localScale = new Vector3(1, 1, 1);
            b.name = "item" + i.ToString();                                           //suoyou所有的深度 都是5
            b.transform.localPosition = new Vector3(xx, yy, 0);
            xx += 50;
            if (xx >= 50 * 9)
            {
                yy -= 50;
                xx = 0;
            }
        }
    }
    //引用prefab 生成象棋的棋子
    public void InitPiece(string sql, GameObject game, string name, int count)
    {
        GameObject a = (GameObject)Instantiate(Resources.Load(sql));
        a.transform.SetParent(game.transform);
        GameObject b = GameObject.Find(a.name);
        b.name = name + count.ToString();
        b.transform.localPosition = new Vector3(0, 0, 0);
        b.transform.localScale = new Vector3(1, 1, 1);
    }


    //走一步棋的界面UI控制
    public void ChessGoStepView(MoveSetting.CHESSMANPOS pos)
    {
        ClearItem();
        if (ChessControl.instance.IsCanMove == false || ChessControl.posthread == false) return;
        if (ChessControl.instance.IsSelectChess) 
        {
            int fromx = ChessControl.instance.FromX;
            int fromy = ChessControl.instance.FromY;
            int tox   = pos.x;
            int toy   = pos.y;
            GameObject FromObj = PosGetChess(ChessControl.instance.FromX, ChessControl.instance.FromY);
            GameObject ToObj   = PosGetChess(tox, toy);
            bool ba = rules.instance.IsValidMove(board.instance.chess, fromx, fromy, tox, toy);
            if (board.instance.chess[toy, tox] == 0)
            {
                if (!ba)
                    return;
                MoveChessView(FromObj, ToObj);
                if (board.instance.chess[fromy, fromx] > 8) 
                    SetTipsText("黑方走！");
                else
                    SetTipsText("红方走！");
                if(!ChessControl.instance.RedMove&&BtnControl.ChessPeople==1)
                {
                    AIMoveTipPic.transform.SetParent(FromObj.transform);
                    AIMoveTipPic.transform.localPosition = Vector3.zero;
                    AIMoveTipPic.transform.SetParent(NextTips.transform);
                }
            }
            else
            {
                bool isSame = rules.instance.IsSameSide(board.instance.chess[toy, tox], board.instance.chess[fromy, fromx]);
                if (isSame)
                {
                    if (RedSelectPic != null) RedSelectPic.SetActive(false);
                    RedSelectPic = PosGetChess(pos.x, pos.y).transform.GetChild(0).GetChild(0).gameObject;
                    RedSelectPic.SetActive(true);
                    ClickChessMoveDraw(tox, toy);
                    return;
                }
                else
                {                    
                    if (!ba)
                        return;
                    EatChessView(FromObj, ToObj);
                    if (!ChessControl.instance.RedMove && BtnControl.ChessPeople == 1)
                    {
                        AIMoveTipPic.transform.SetParent(FromObj.transform);
                        AIMoveTipPic.transform.localPosition = Vector3.zero;
                        AIMoveTipPic.transform.SetParent(NextTips.transform);
                    }
                }
            }
            if (board.instance.chess[fromy, fromx] > 8)
            {
                if (BtnControl.ChessPeople == 1) 
                    SetTipsText("电脑正在思考！");
                else 
                    SetTipsText("黑方走！");
            }
            else
                SetTipsText("红方走！");
            JiangJunCheck();
            if (BlackSelectPic != null) BlackSelectPic.SetActive(false);
            BlackSelectPic = PosGetChess(pos.x, pos.y).transform.GetChild(0).GetChild(0).gameObject;
            BlackSelectPic.SetActive(true);            
        }
        else
        {
            if (board.instance.chess[pos.y, pos.x] != 0)
            {
                if (!ChessControl.instance.RedMove && board.instance.chess[pos.y, pos.x] > 8) return;
                if (ChessControl.instance.RedMove && board.instance.chess[pos.y, pos.x] < 8) return;
                if (RedSelectPic!=null) RedSelectPic.SetActive(false);
                RedSelectPic = PosGetChess(pos.x, pos.y).transform.GetChild(0).GetChild(0).gameObject;
                RedSelectPic.SetActive(true);
                ClickChessMoveDraw(pos.x, pos.y);
                return;
            }
        }
    }
    //悔棋界面控制
    public void HUIQI_View()
    {
        AIMoveTipPic.transform.localPosition = new Vector3(5000, 5000, 0);
        int fromx, fromy, tox, toy, backStepNum;
        if (BtnControl.ChessPeople == 2 || ChessControl.instance.IsCanMove == false)
            backStepNum = 1;
        else
            backStepNum = 2;        
        for (int i = 1; i < backStepNum + 1; i++)
        {
            int length = BackStepChess.instance.BackChessList.Count;
            fromx = BackStepChess.instance.BackChessList[length - i].From.x;
            fromy = BackStepChess.instance.BackChessList[length - i].From.y;
            tox = BackStepChess.instance.BackChessList[length - i].To.x;
            toy = BackStepChess.instance.BackChessList[length - i].To.y;
            GameObject FromObj = PosGetChess(fromx, fromy);
            GameObject ToObj = PosGetChess(tox, toy);
            GameObject FromChess = ToObj.transform.GetChild(0).gameObject;
            GameObject ToChess;
            if (BackStepChess.instance.BackChessList[length - i].ToNum != 0)
            {
                int ChildCount = MissChess.transform.childCount;
                ToChess = MissChess.transform.GetChild(ChildCount - 1).gameObject;
                ToChess.transform.SetParent(ToObj.transform);
                ToChess.transform.localPosition = Vector3.zero;
            }
            FromChess.transform.SetParent(FromObj.transform);
            FromChess.transform.localPosition = Vector3.zero;
        }
        if (BtnControl.ChessPeople != 1 && ChessControl.instance.RedMove)
            SetTipsText("黑方走！");
        else
            SetTipsText("红方走！");
    }
    //移动的UI控制
    void MoveChessView(GameObject FromPosObj, GameObject ToPosObj)
    {
        FromPosObj = FromPosObj.transform.GetChild(0).gameObject;
        FromPosObj.transform.SetParent(ToPosObj.transform);
        FromPosObj.transform.localPosition = Vector3.zero;
    }
    //吃子的UI控制
    void EatChessView(GameObject FromPosObj, GameObject ToPosObj)
    {
        
        ToPosObj.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(5000, 5000, 0);
        ToPosObj.transform.GetChild(0).gameObject.transform.SetParent(MissChess.transform);
        FromPosObj = FromPosObj.transform.GetChild(0).gameObject;
        FromPosObj.gameObject.transform.SetParent(ToPosObj.transform);
        FromPosObj.transform.localPosition = Vector3.zero;
    }
    //AI走一步棋的界面UI控制
    public void AIGoStepView(MoveSetting.CHESSMOVE chere)
    {
        ChessControl.instance.FromX = chere.From.x;
        ChessControl.instance.FromY = chere.From.y;
        ChessControl.instance.IsSelectChess = true;
        ChessGoStepView(chere.To);
        SetTipsText("红方走");
        JiangJunCheck();
    }



    //将选择的棋子可以走的位置绘制出来
    public void ClickChessMoveDraw(int fromx, int fromy)
    {
        int ChessID = board.instance.chess[fromy, fromx];
        switch (ChessID)
        {
            case 1:
            case 8:
                Gen_KingMoveDraw(board.instance.chess, fromx, fromy);
                break;
            case 2:
            case 9:
                Gen_CarMoveDraw(board.instance.chess, fromx, fromy);
                break;
            case 3:
            case 10:
                Gen_HorseMoveDraw(board.instance.chess, fromx, fromy);
                break;
            case 4:
            case 11:
                Gen_CanonMoveDraw(board.instance.chess, fromx, fromy);
                break;
            case 5://黑士
                Gen_BBishopMoveDraw(board.instance.chess, fromx, fromy);
                break;
            case 6://黑象
            case 13://红相
                Gen_ElephantMoveDraw(board.instance.chess, fromx, fromy);
                break;
            case 7://黑兵
                Gen_BPawnMoveDraw(board.instance.chess, fromx, fromy);
                break;
            case 12://红shi
                Gen_RBishopMoveDraw(board.instance.chess, fromx, fromy);
                break;

            case 14://红兵
                Gen_RPawnMoveDraw(board.instance.chess, fromx, fromy);
                break;
        }
    }
    //得到将的走法
    void Gen_KingMoveDraw(int[,] position, int j, int i)
    {//两个参数 fromx  和fromy
        int x, y;
        for (y = 0; y < 3; y++)
            for (x = 3; x < 6; x++)
                if (rules.instance.IsValidMove(position, j, i, x, y)) //走法是否合法
                    GetPrefabs(position, j, i, x, y);
        for (y = 7; y < 10; y++)
            for (x = 3; x < 6; x++)
                if (rules.instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    GetPrefabs(position, j, i, x, y);

    }
    //红士的走法
    void Gen_RBishopMoveDraw(int[,] position, int j, int i)
    {
        //i j棋子位置   x y目标位置
        int x, y;
        for (y = 7; y < 10; y++)
            for (x = 3; x < 6; x++)
                if (rules.instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    GetPrefabs(position, j, i, x, y);
    }
    //黑士走法
    void Gen_BBishopMoveDraw(int[,] position, int j, int i)
    {
        int x, y;
        for (y = 0; y < 3; y++)
            for (x = 3; x < 6; x++)
                if (rules.instance.IsValidMove(position, j, i, x, y))//走法是否合法
                    GetPrefabs(position, j, i, x, y);
    }
    //相象走法
    void Gen_ElephantMoveDraw(int[,] position, int j, int i)
    {
        int x, y;
        //向右下方走步
        x = j + 2;
        y = i + 2;
        if (x < 9 && y < 10 && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //向右上方走步
        x = j + 2;
        y = i - 2;
        if (x < 9 && y >= 0 && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //向左下方走步
        x = j - 2;
        y = i + 2;
        if (x >= 0 && y < 10 && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //向左上方走步
        x = j - 2;
        y = i - 2;
        if (x >= 0 && y >= 0 && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
    }
    //马的走法
    void Gen_HorseMoveDraw(int[,] position, int j, int i)
    {
        int x, y;
        //插入右下方的有效走法
        x = j + 2;
        y = i + 1;
        if ((x < 9 && y < 10) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //插入右上方的有效走法
        x = j + 2;
        y = i - 1;
        if ((x < 9 && y >= 0) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //左下
        x = j - 2;
        y = i + 1;
        if ((x >= 0 && y < 10) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //左上
        x = j - 2;
        y = i - 1;
        if ((x >= 0 && y >= 0) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //右下
        x = j + 1;
        y = i + 2;
        if ((x < 9 && y < 10) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //left down
        x = j - 1;
        y = i + 2;
        if ((x >= 0 && y < 10) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //right down
        x = j + 1;
        y = i - 2;
        if ((x < 9 && y >= 0) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);
        //left top
        x = j - 1;
        y = i - 2;
        if ((x >= 0 && y >= 0) && rules.instance.IsValidMove(position, j, i, x, y))
            GetPrefabs(position, j, i, x, y);

    }
    //车的走法
    void Gen_CarMoveDraw(int[,] position, int j, int i)
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
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
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
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
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
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
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
                GetPrefabs(position, j, i, x, y);
            else
            {
                if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                    GetPrefabs(position, j, i, x, y);
                break;
            }
            y--;
        }
    }
    //红卒的走法
    void Gen_RPawnMoveDraw(int[,] position, int j, int i)
    {
        int x, y;
        int nChessID;
        nChessID = position[i, j];
        y = i - 1;
        x = j;
        if (y > 0 && !rules.instance.IsSameSide(nChessID, position[y, x]))
            GetPrefabs(position, j, i, x, y);
        if (i < 5)
        {
            y = i;
            x = j + 1;//right
            if (x < 9 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
            x = j - 1;//right
            if (x >= 0 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
        }
    }
    //黑兵走法
    void Gen_BPawnMoveDraw(int[,] position, int j, int i)
    {
        int x, y;
        int nChessID;
        nChessID = position[i, j];
        y = i + 1;//前
        x = j;
        if (y < 10 && !rules.instance.IsSameSide(nChessID, position[y, x]))
            GetPrefabs(position, j, i, x, y);
        if (i > 4)
        {
            y = i;
            x = j + 1;
            if (x < 9 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
            x = j - 1;
            if (x >= 0 && !rules.instance.IsSameSide(nChessID, position[y, x]))
                GetPrefabs(position, j, i, x, y);
        }

    }
    //炮走法
    void Gen_CanonMoveDraw(int[,] position, int j, int i)
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
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        GetPrefabs(position, j, i, x, y);
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
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        GetPrefabs(position, j, i, x, y);
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
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                        GetPrefabs(position, j, i, x, y);
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
                    GetPrefabs(position, j, i, x, y);
            }
            else
            {
                if (!flag)
                    flag = true;
                else
                {
                    if (!rules.instance.IsSameSide(nChessID, position[y, x]))
                    {
                        GetPrefabs(position, j, i, x, y);
                    }
                    break;
                }
            }
            y--;
        }
    }
    //把传入进来的可走位置全部画出来
    void GetPrefabs(int[,] position, int c, int d, int x, int y)
    {
        if (!rules.instance.KingBye(position, c, d, x, y))
            return;
        int wid = x * 50;
        int heit = y * (-50);
        for (int i = 1; i <= 90; i++)
        {
            GameObject game = GameObject.Find("item" + i.ToString());
            if (game.transform.localPosition.x == wid && game.transform.localPosition.y == heit)
            {
                GameObject obj = GameObject.Find("chess");
                GameObject ite;
                if (board.instance.chess[y, x] == 0)
                    ite = (GameObject)GameObject.Instantiate(Resources.Load("canmove"));
                else
                {
                    ite = (GameObject)GameObject.Instantiate(Resources.Load("nengchi"));
                    heit = heit + 4;
                }
                ite.transform.SetParent(obj.transform);
                ite.name = "prefabs" + i.ToString();
                ite.transform.localPosition = new Vector3(wid, heit, 0);
                ite.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }


    //通过Gameobject获取位置信息
    public MoveSetting.CHESSMANPOS GetClickItemPos(GameObject ClickItem)
    {
        MoveSetting.CHESSMANPOS pos = new MoveSetting.CHESSMANPOS();
        if (!ClickItem.name.Contains("item"))
            ClickItem = ClickItem.transform.parent.gameObject;
        pos.x = System.Convert.ToInt32((ClickItem.transform.localPosition.x) / 50);
        pos.y = System.Convert.ToInt32(Mathf.Abs((ClickItem.transform.localPosition.y) / 50));
        return pos;
    }
    //通过位置信息获取Gameobject
    public GameObject PosGetChess(int PosX, int PoxY)
    {
        //得到开始位置gameobject的对象名字
        GameObject obj;
        string s3 = "";
        for (int i = 1; i <= 90; i++)
        {
            obj = GameObject.Find("item" + i.ToString());
            int x = System.Convert.ToInt32((obj.transform.localPosition.x) / 50);
            int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y) / 50));
            if (x == PosX && y == PoxY)
                s3 = obj.name;
        }
        obj = GameObject.Find(s3);
        return obj;
    }

    //判断将和帅是否被将军了
    public void JiangJunCheck()
    {
        IsPosition();
        if (board.instance.chess[Jiang_y, Jiang_x] != 1)
        {
            SetTipsText("红色棋子胜利");
            ChessControl.instance.IsCanMove = false;
            return;
        }
        else if (board.instance.chess[Shuai_y, Shuai_x] != 8)
        {
            SetTipsText("黑色棋子胜利");
            ChessControl.instance.IsCanMove = false;
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
                            SetTipsText("帅被車将军了");

                        break;
                    case 3:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Shuai_x, Shuai_y);
                        if (BOL)
                            SetTipsText("帅被马将军了");
                        break;
                    case 4:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Shuai_x, Shuai_y);
                        if (BOL)
                            SetTipsText("帅被炮将军了");
                        break;

                    case 7:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Shuai_x, Shuai_y);
                        if (BOL)
                            SetTipsText("帅被兵将军了");
                        break;
                    case 9:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            SetTipsText("将被車将军了");
                        break;
                    case 10:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            SetTipsText("将被马将军了");
                        break;
                    case 11:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            SetTipsText("将被炮将军了");
                        break;
                    case 14:
                        BOL = rules.instance.IsValidMove(board.instance.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            SetTipsText("将被兵将军了");
                        break;
                }
            }
        }

    }
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
    
    //设置文字提示
    public void SetTipsText(string NextPlayerTipStr)
    {
        NextTips.text = NextPlayerTipStr;
    }


    //清除棋盘上可走路线的提示
    public void ClearItem()
    {
        for (int i = 1; i <= 90; i++)
        {
            GameObject Clear = GameObject.Find("prefabs" + i.ToString());
            Destroy(Clear);
        }
    }
}
