using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessClick : MonoBehaviour {

    public void clickItemOrChess() 
    {
        GameObject obj = this.GetComponent<Button>().gameObject;
        MoveSetting.CHESSMANPOS pos= ViewManager.instance.GetClickItemPos(obj);
        ViewManager.instance.ChessGoStepView(pos);
        ChessControl.instance.ChessGoStep(pos);
        ViewManager.instance.JiangJunCheck();
    }
}
