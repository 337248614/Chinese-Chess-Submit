using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDG;
public class ViewManager : MonoBehaviour {
    public static ViewManager _instance;
    [System.NonSerialized]
    public GameObject _blackSelectPic, _redSelectPic, _aiMoveTipPic;
    //int  _jiang_X, _jiang_Y, _shuai_X, _shuai_Y;
    GameObject _missChess;
    Text _nextTips;//提示下一步谁走的信息
    public  const int LatticeLength = 50;
    public const int LatticeTotalNum = 90;
    //判断是否选择了棋子
    bool _isSelectChess = false;
    //判断AI搜索内容是否执行完毕
    public static bool _posThread = true;
    //判断是红方走还是黑方走
    public bool _redMove = true;
    //判断这个时候输赢状态能否走棋
    public ChessPosition _selectPosition;
    public bool _isCanMove = true;
	void Start () {
        _instance = this;
        _missChess = GameObject.Find("xiaoshi");
        _nextTips = GameObject.Find("NextTips").GetComponent<Text>();
        _aiMoveTipPic = GameObject.Find("chessRobot");
	}

    //开始游戏清除界面
    public void StartGameViewClear()
    {
        ClearItem();
        if (_blackSelectPic!=null) _blackSelectPic.SetActive(false);
        if (_redSelectPic != null) _redSelectPic.SetActive(false);
    }
   
    //初始化棋盘界面
    public void InitChessView() 
    {
        _redMove = true;
        _isCanMove = true;
        board._instance.ChessInit();
        int count = 1;
        SetPiecePos();
        for (int i = 1; i <= LatticeTotalNum; i++)
        {
            GameObject obj = GameObject.Find("item" + i.ToString());
            int x = GetClickItemPos(obj).x;
            int y = GetClickItemPos(obj).y;
            switch (board._instance.chess[y, x])
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
        for (int i = 1; i <= LatticeTotalNum; i++)
        {
            GameObject ite = (GameObject)Instantiate(Resources.Load("item"));//找到预设体
            ite.transform.SetParent(a.transform);           //给预设体指定添加到什么地方
            GameObject b = GameObject.Find(ite.name);    //找到这个预设体的名字，给他做一些操作
            b.transform.localScale = Vector3.one;
            b.name = "item" + i.ToString();                                           //suoyou所有的深度 都是5
            b.transform.localPosition = new Vector3(xx, yy, 0);
            xx += LatticeLength;
            if (xx >= LatticeLength * board._instance.chess.GetLength(1))
            {
                yy -= LatticeLength;
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
        b.transform.localPosition = Vector3.zero;
        b.transform.localScale = Vector3.one;
    }

    
    //走一步棋的界面UI控制
    public void ChessGoStepView(ChessPosition pos)
    {
        ClearItem();
        if (_isCanMove == false || _posThread == false) return;
        if (_isSelectChess) 
        {
            int tox   = pos.x;
            int toy   = pos.y;
            ChessPosition posFrom = new ChessPosition(_selectPosition.x, _selectPosition.y);
            ChessPosition posTo = new ChessPosition(pos.x, pos.y);            
            if (board._instance.chess[toy, tox] == 0)
            {
                if (ChessControl._instance.MoveOrEatChess(posFrom, posTo)) {
                    MoveChessView(posFrom, posTo);
                    SetNextText();
                }
                else
                {
                    _isSelectChess = false;
                    return;
                }
            }
            else
            {
                bool isSame = rules._instance.IsSameSide(board._instance.chess[toy, tox], board._instance.chess[posFrom.y, posFrom.x]);
                if (isSame)
                {
                    ChessCanMoveView(pos);
                    return;
                }
                else
                {
                    if (ChessControl._instance.MoveOrEatChess(posFrom, posTo))
                    {
                        EatChessView(posFrom, posTo);
                        SetNextText();
                    }
                    else
                    {
                        _isSelectChess = false;
                        return;
                    }
                }
            }
            if (ChessControl._instance._gameModel.Equals(GameModel.PersonVSAi))
                SetSelectPic(pos);
        }
        else
        {
            if (board._instance.chess[pos.y, pos.x] != 0)
            {
                ChessCanMoveView(pos);
            }
        }
    }

    void SetSelectPic(ChessPosition pos)
    {
        if (_redMove) 
        { 
            if (_redSelectPic != null) _redSelectPic.SetActive(false);
            _redSelectPic = PosGetChess(pos.x, pos.y).transform.GetChild(0).GetChild(0).gameObject;
            _redSelectPic.SetActive(true);
        }
        else
        {
            if (_blackSelectPic != null) _blackSelectPic.SetActive(false);
            _blackSelectPic = PosGetChess(pos.x, pos.y).transform.GetChild(0).GetChild(0).gameObject;
            _blackSelectPic.SetActive(true);
        }
    }
    void ChessCanMoveView(ChessPosition pos)
    {
        if (!_redMove && board._instance.chess[pos.y, pos.x] > 8) return;
        if (_redMove && board._instance.chess[pos.y, pos.x] < 8) return;
        //if (_redSelectPic != null) _redSelectPic.SetActive(false);
        _selectPosition= pos;
        _isSelectChess = true;
        List<ChessPosition> listpos = ChessControl._instance.ChessCanMove(pos);
        for (int i = 0; i < listpos.Count; i++)
        {
            GetPrefabs(board._instance.chess, listpos[i].x, listpos[i].y);
        }
        SetSelectPic(pos);
    }
    void SetNextText() {
        if (_isSelectChess == true) return;
        if (!_redMove)
        {
            if (BtnControl._gameModel == GameModel.PersonVSAi)
                SetTipsText("电脑正在思考！");
            else
                SetTipsText("黑方走！");
        }
        else
            SetTipsText("红方走！");
        KingAttackCheckSetTextView(ChessControl._instance.KingAttackCheck());
    }
    //悔棋界面控制
    public void BackStepView()
    {
        _aiMoveTipPic.transform.localPosition = new Vector3(5000, 5000, 0);
        int fromx, fromy, tox, toy, backStepNum;
        if (BtnControl._gameModel == GameModel.PersonVSPerson || _isCanMove == false)
            backStepNum = 1;
        else
            backStepNum = 2;        
        for (int i = 1; i < backStepNum + 1; i++)
        {
            int length = ChessControl._instance.ChessMoveList.Count;
            fromx = ChessControl._instance.ChessMoveList[length - 1].From.x;
            fromy = ChessControl._instance.ChessMoveList[length - 1].From.y;
            tox = ChessControl._instance.ChessMoveList[length - 1].To.x;
            toy = ChessControl._instance.ChessMoveList[length - 1].To.y;
            GameObject FromObj = PosGetChess(fromx, fromy);
            GameObject ToObj = PosGetChess(tox, toy);
            GameObject FromChess = ToObj.transform.GetChild(0).gameObject;
            GameObject ToChess;
            if (ChessControl._instance.ChessMoveList[length - 1].ToChessNum != 0)
            {
                int ChildCount = _missChess.transform.childCount;
                ToChess = _missChess.transform.GetChild(ChildCount - 1).gameObject;
                ToChess.transform.SetParent(ToObj.transform);
                ToChess.transform.localPosition = Vector3.zero;
            }
            FromChess.transform.SetParent(FromObj.transform);
            FromChess.transform.localPosition = Vector3.zero;
            ChessControl._instance.BackStep();
            _redMove = !_redMove;
        }
        if (BtnControl._gameModel != GameModel.PersonVSAi && !_redMove)
            SetTipsText("黑方走！");
        else
            SetTipsText("红方走！");
    }
    //移动的UI控制
    void MoveChessView(ChessPosition posFrom, ChessPosition posTo)
    {
        GameObject FromObj = PosGetChess(posFrom.x, posFrom.y);
        GameObject ToObj = PosGetChess(posTo.x, posTo.y);
        FromObj = FromObj.transform.GetChild(0).gameObject;
        FromObj.transform.SetParent(ToObj.transform);
        FromObj.transform.localPosition = Vector3.zero;
        _redMove = !_redMove;
        if (_redMove && BtnControl._gameModel == GameModel.PersonVSAi)
        {
            GameObject _aiMove = PosGetChess(_selectPosition.x, _selectPosition.y);
            _aiMoveTipPic.transform.SetParent(_aiMove.transform);
            _aiMoveTipPic.transform.localPosition = Vector3.zero;
            _aiMoveTipPic.transform.SetParent(_nextTips.transform);
        }
        _isSelectChess = false;
        if (!_redMove)
            SetTipsText("黑方走！");
        else
            SetTipsText("红方走！");
        if (!_redMove && BtnControl._gameModel == GameModel.PersonVSAi)
            Invoke("AIGoStepView", 0.2f);
    }
    //吃子的UI控制
    void EatChessView(ChessPosition posFrom, ChessPosition posTo)
    {
        GameObject FromObj = PosGetChess(posFrom.x, posFrom.y);
        GameObject ToObj = PosGetChess(posTo.x, posTo.y);
        ToObj.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(5000, 5000, 0);
        ToObj.transform.GetChild(0).gameObject.transform.SetParent(_missChess.transform);
        FromObj = FromObj.transform.GetChild(0).gameObject;
        FromObj.gameObject.transform.SetParent(ToObj.transform);
        FromObj.transform.localPosition = Vector3.zero;
        _redMove = !_redMove;
        if (_redMove && BtnControl._gameModel == GameModel.PersonVSAi)
        {
            GameObject _aiMove = PosGetChess(_selectPosition.x, _selectPosition.y);
            _aiMoveTipPic.transform.SetParent(_aiMove.transform);
            _aiMoveTipPic.transform.localPosition = Vector3.zero;
            _aiMoveTipPic.transform.SetParent(_nextTips.transform);
        }
        _isSelectChess = false;
        if (!_redMove && BtnControl._gameModel == GameModel.PersonVSAi)
            Invoke("AIGoStepView", 0.2f);
    }
    //AI走一步棋的界面UI控制
    public void AIGoStepView()
    {
        if (!_redMove)
        {
            _posThread = false;
            ChessMove chere = ChessControl._instance.GetAiMove();
            _posThread = true;
            _selectPosition = chere.From;
            _isSelectChess = true;
            ChessGoStepView(chere.To);
            SetTipsText("红方走");
            KingAttackCheckSetTextView(ChessControl._instance.KingAttackCheck());
        }
       
    }
    
    //把传入进来的可走位置全部画出来
    void GetPrefabs(int[,] position, int x, int y)
    {
        int wid = x * LatticeLength;
        int heit = y * (-LatticeLength);
        for (int i = 1; i <= LatticeTotalNum; i++)
        {
            GameObject game = GameObject.Find("item" + i.ToString());
            if (game.transform.localPosition.x == wid && game.transform.localPosition.y == heit)
            {
                GameObject obj = GameObject.Find("chess");
                GameObject ite;
                if (board._instance.chess[y, x] == 0)
                    ite = (GameObject)GameObject.Instantiate(Resources.Load("canmove"));
                else
                {
                    ite = (GameObject)GameObject.Instantiate(Resources.Load("nengchi"));
                    heit = heit + 4;
                }
                ite.transform.SetParent(obj.transform);
                ite.name = "prefabs" + i.ToString();
                ite.transform.localPosition = new Vector3(wid, heit, 0);
                ite.transform.localScale = Vector3.one;
            }
        }
    }


    //通过Gameobject获取位置信息
    public ChessPosition GetClickItemPos(GameObject clickItem)
    {
        if (!clickItem.name.Contains("item"))
            clickItem = clickItem.transform.parent.gameObject;
        int x = System.Convert.ToInt32((clickItem.transform.localPosition.x) / LatticeLength);
        int y = System.Convert.ToInt32(Mathf.Abs((clickItem.transform.localPosition.y) / LatticeLength));
        ChessPosition pos = new ChessPosition(x,y);
        return pos;
    }
    //通过位置信息获取Gameobject
    GameObject PosGetChess(int PosX, int PoxY)
    {
        //得到开始位置gameobject的对象名字
        GameObject obj;
        string s3 = "";
        for (int i = 1; i <= LatticeTotalNum; i++)
        {
            obj = GameObject.Find("item" + i.ToString());
            int x = System.Convert.ToInt32((obj.transform.localPosition.x) / LatticeLength);
            int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y) / LatticeLength));
            if (x == PosX && y == PoxY)
                s3 = obj.name;
        }
        obj = GameObject.Find(s3);
        return obj;
    }

    //判断将和帅是否被将军了
    void KingAttackCheckSetTextView(int num)
    {
        switch (num)
                {
                    case -1:
                        {
                            _isCanMove = false;
                            if (ChessControl._instance.IsBlackWin(board._instance.chess)) 
                                SetTipsText("黑方胜利");
                            if (ChessControl._instance.IsRedWin(board._instance.chess))
                                SetTipsText("红方胜利");
                        } 
                        break;
                    case 2:
                            SetTipsText("帅被車将军了");

                        break;
                    case 3:
                            SetTipsText("帅被马将军了");
                        break;
                    case 4:
                            SetTipsText("帅被炮将军了");
                        break;

                    case 7:
                            SetTipsText("帅被兵将军了");
                        break;
                    case 9:
                            SetTipsText("将被車将军了");
                        break;
                    case 10:
                            SetTipsText("将被马将军了");
                        break;
                    case 11:
                            SetTipsText("将被炮将军了");
                        break;
                    case 14:
                            SetTipsText("将被兵将军了");
                        break;
                }

    }

    //设置文字提示
    void SetTipsText(string NextPlayerTipStr)
    {
        _nextTips.text = NextPlayerTipStr;
    }

    //清除棋盘上可走路线的提示
    void ClearItem()
    {
        for (int i = 1; i <= LatticeTotalNum; i++)
        {
            GameObject Clear = GameObject.Find("prefabs" + i.ToString());
            Destroy(Clear);
        }
    }
}
