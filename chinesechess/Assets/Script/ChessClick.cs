using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDG;

public class ChessClick : MonoBehaviour {

    public void clickItemOrChess() 
    {
        GameObject obj = this.GetComponent<Button>().gameObject;
        ChessPosition pos= View._instance.GetClickItemPos(obj);
        ViewManager._instance.ChessGoStepView(pos);
    }
}
