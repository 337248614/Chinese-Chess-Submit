using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessClick : MonoBehaviour {

    public void clickItemOrChess() 
    {
        GameObject obj = this.GetComponent<UIButton>().gameObject;
        ChessControl back = GameObject.Find("Main Camera").GetComponent<ChessControl>();
        back.IsClickCheck(obj);
    }
}
