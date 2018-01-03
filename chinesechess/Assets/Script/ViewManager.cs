using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDG;
public class ViewManager : MonoBehaviour {
    public static ViewManager _instance;

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
	}

    //开始游戏清除界面
    public void StartGameClearView()
    {
        View._instance.ClearItem();
        View._instance.SelectPicClear();
    }
   
    //初始化棋盘界面
    public void InitChessView() 
    {
        _redMove = true;
        _isCanMove = true;
        board._instance.ChessInit();
        View._instance.SetPiecePos();
        View._instance.InitView();
        View._instance.SetTipsText("红方走");
    }

    
    //走一步棋的界面UI控制
    public void ChessGoStepView(ChessPosition pos)
    {
        View._instance.ClearItem();
        if (_isCanMove == false || _posThread == false) return;
        if (_isSelectChess) 
        {
            int tox   = pos.x;
            int toy   = pos.y;
            ChessPosition posFrom = new ChessPosition(_selectPosition.x, _selectPosition.y);
            ChessPosition posTo = new ChessPosition(pos.x, pos.y);            
            if (board.chess[toy, tox] == 0)
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
                bool isSame = rules._instance.IsSameSide(board.chess[toy, tox], board.chess[posFrom.y, posFrom.x]);
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
                SetSelectPicView(pos);
        }
        else
        {
            if (board.chess[pos.y, pos.x] != 0)
            {
                ChessCanMoveView(pos);
            }
        }
    }

    void SetSelectPicView(ChessPosition pos)
    {
        if (_redMove) 
        {
            View._instance.PlayerSelectPicSet(pos);
        }
        else
        {
            View._instance.AiSelectPicSet(pos);
        }
    }
    void ChessCanMoveView(ChessPosition pos)
    {
        if (!_redMove && board.chess[pos.y, pos.x] > 8) return;
        if (_redMove && board.chess[pos.y, pos.x] < 8) return;
        _selectPosition= pos;
        _isSelectChess = true;
        List<ChessPosition> listpos = ChessControl._instance.ChessCanMove(pos);
        for (int i = 0; i < listpos.Count; i++)
        {
            View._instance.GetPrefabs(board.chess, listpos[i].x, listpos[i].y);
        }
        SetSelectPicView(pos);
    }
    void SetNextText() {
        if (_isSelectChess == true) return;
        if (!_redMove)
        {
            if (BtnControl._gameModel == GameModel.PersonVSAi)
                View._instance.SetTipsText("电脑正在思考！");
            else
                View._instance.SetTipsText("黑方走！");
        }
        else
            View._instance.SetTipsText("红方走！");
        KingAttackCheckSetTextView(ChessControl._instance.KingAttackCheck());
    }
    //悔棋界面控制
    public void BackStepView()
    {        
        int backStepNum;
        if (BtnControl._gameModel == GameModel.PersonVSPerson || _isCanMove == false)
            backStepNum = 1;
        else
            backStepNum = 2;        
        for (int i = 1; i < backStepNum + 1; i++)
        {
            View._instance.BackStepView();
            ChessControl._instance.BackStep();
            _redMove = !_redMove;
        }
        if (BtnControl._gameModel != GameModel.PersonVSAi && !_redMove)
            View._instance.SetTipsText("黑方走！");
        else
            View._instance.SetTipsText("红方走！");
    }


    //移动的UI控制
    void MoveChessView(ChessPosition posFrom, ChessPosition posTo)
    {
        View._instance.moveChess(posFrom,posTo);
        _redMove = !_redMove;
        if (_redMove && BtnControl._gameModel == GameModel.PersonVSAi)
        {
            View._instance.AiLastMovePicSet(_selectPosition);
        }
        _isSelectChess = false;
        if (!_redMove)
            View._instance.SetTipsText("黑方走！");
        else
            View._instance.SetTipsText("红方走！");
        if (!_redMove && BtnControl._gameModel == GameModel.PersonVSAi)
            Invoke("AIGoStepView", 0.2f);
    }
    //吃子的UI控制
    void EatChessView(ChessPosition posFrom, ChessPosition posTo)
    {
        View._instance.EatChess(posFrom, posTo);
        _redMove = !_redMove;
        if (_redMove && BtnControl._gameModel == GameModel.PersonVSAi)
        {
            View._instance.AiLastMovePicSet(_selectPosition);
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
            View._instance.SetTipsText("红方走");
            KingAttackCheckSetTextView(ChessControl._instance.KingAttackCheck());
        }
       
    }
    

    //判断将和帅是否被将军了
    void KingAttackCheckSetTextView(int num)
    {
        switch (num)
                {
                    case -1:
                        {
                            _isCanMove = false;
                            if (ChessControl._instance.IsBlackWin())
                                View._instance.SetTipsText("黑方胜利");
                            if (ChessControl._instance.IsRedWin())
                                View._instance.SetTipsText("红方胜利");
                        } 
                        break;
                    case 2:
                        View._instance.SetTipsText("帅被車将军了");

                        break;
                    case 3:
                        View._instance.SetTipsText("帅被马将军了");
                        break;
                    case 4:
                        View._instance.SetTipsText("帅被炮将军了");
                        break;

                    case 7:
                        View._instance.SetTipsText("帅被兵将军了");
                        break;
                    case 9:
                        View._instance.SetTipsText("将被車将军了");
                        break;
                    case 10:
                        View._instance.SetTipsText("将被马将军了");
                        break;
                    case 11:
                        View._instance.SetTipsText("将被炮将军了");
                        break;
                    case 14:
                        View._instance.SetTipsText("将被兵将军了");
                        break;
                }

    }


}
