using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessClick : MonoBehaviour {

    public void clickItemOrChess() 
    {
        GameObject obj = this.GetComponent<UIButton>().gameObject;
        blackclick back = new blackclick();
        back.IsClickCheck(obj);
    }
}
