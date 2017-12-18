using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDG;
using UnityEngine.UI;
public class View : MonoBehaviour {

    public static View _instance;
    [System.NonSerialized]
    public GameObject _blackSelectPic, _redSelectPic, _aiMoveTipPic;
    GameObject _missChess;
    Text _nextTips;//提示下一步谁走的信息

    public  const int LatticeLength = 50;
    public  const int LatticeTotalNum = 90;

    void Start()
    {
        _instance = this;
        _missChess = GameObject.Find("xiaoshi");
        _nextTips = GameObject.Find("NextTips").GetComponent<Text>();
        _aiMoveTipPic = GameObject.Find("chessRobot");
    }
    public void moveChess(ChessPosition posFrom, ChessPosition posTo)
    {
        GameObject FromObj = PosGetChess(posFrom.x, posFrom.y);
        GameObject ToObj = PosGetChess(posTo.x, posTo.y);
        FromObj = FromObj.transform.GetChild(0).gameObject;
        FromObj.transform.SetParent(ToObj.transform);
        FromObj.transform.localPosition = Vector3.zero;
    
    }
    public void EatChess(ChessPosition posFrom, ChessPosition posTo) 
    {
        GameObject FromObj = PosGetChess(posFrom.x, posFrom.y);
        GameObject ToObj = PosGetChess(posTo.x, posTo.y);
        ToObj.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(5000, 5000, 0);
        ToObj.transform.GetChild(0).gameObject.transform.SetParent(_missChess.transform);
        FromObj = FromObj.transform.GetChild(0).gameObject;
        FromObj.gameObject.transform.SetParent(ToObj.transform);
        FromObj.transform.localPosition = Vector3.zero;
    }
    public void SelectPicClear()
    {
        if (_blackSelectPic != null) _blackSelectPic.SetActive(false);
        if (_redSelectPic != null) _redSelectPic.SetActive(false);
    }
    public void ClearItem()
    {
        for (int i = 1; i <= LatticeTotalNum; i++)
        {
            GameObject Clear = GameObject.Find("prefabs" + i.ToString());
            Destroy(Clear);
        }
    }
    public void AiSelectPicSet(ChessPosition pos) 
    {
        if (_blackSelectPic != null) _blackSelectPic.SetActive(false);
        _blackSelectPic = PosGetChess(pos.x, pos.y).transform.GetChild(0).GetChild(0).gameObject;
        _blackSelectPic.SetActive(true);
    }
    public void PlayerSelectPicSet(ChessPosition pos)
    {
        if (_redSelectPic != null) _redSelectPic.SetActive(false);
        _redSelectPic = PosGetChess(pos.x, pos.y).transform.GetChild(0).GetChild(0).gameObject;
        _redSelectPic.SetActive(true);
    }
    public void AiLastMovePicSet(ChessPosition pos)
    {
        GameObject _aiMove = PosGetChess(pos.x, pos.y);
        _aiMoveTipPic.transform.SetParent(_aiMove.transform);
        _aiMoveTipPic.transform.localPosition = Vector3.zero;
        _aiMoveTipPic.transform.SetParent(_nextTips.transform);
    }
    public void SetTipsText(string str) 
    {
        _nextTips.text = str;
    }


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
            if (xx >= LatticeLength * board.chess.GetLength(1))
            {
                yy -= LatticeLength;
                xx = 0;
            }
        }
    }


    public void InitView()
    {
        int count = 1;
        for (int i = 1; i <= LatticeTotalNum; i++)
        {
            GameObject obj = GameObject.Find("item" + i.ToString());
            int x = GetClickItemPos(obj).x;
            int y = GetClickItemPos(obj).y;
            switch (board.chess[y, x])
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
        }
    }

    public void InitPiece(string sql, GameObject game, string name, int count)
    {
        GameObject a = (GameObject)Instantiate(Resources.Load(sql));
        a.transform.SetParent(game.transform);
        GameObject b = GameObject.Find(a.name);
        b.name = name + count.ToString();
        b.transform.localPosition = Vector3.zero;
        b.transform.localScale = Vector3.one;
    }

    public ChessPosition GetClickItemPos(GameObject clickItem)
    {
        if (!clickItem.name.Contains("item"))
            clickItem = clickItem.transform.parent.gameObject;
        int x = System.Convert.ToInt32((clickItem.transform.localPosition.x) / LatticeLength);
        int y = System.Convert.ToInt32(Mathf.Abs((clickItem.transform.localPosition.y) / LatticeLength));
        ChessPosition pos = new ChessPosition(x, y);
        return pos;
    }

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

    public void GetPrefabs(int[,] position, int x, int y)
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
                if (board.chess[y, x] == 0)
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

    public void BackStepView()
    {
        int fromx, fromy, tox, toy;
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
        _aiMoveTipPic.transform.localPosition = new Vector3(5000, 5000, 0);
    }
}
