using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessClick : MonoBehaviour {

    public void clickItemOrChess() 
    {
        GameObject obj = this.GetComponent<UIButton>().gameObject;
        blackclick back = GameObject.Find("Main Camera").GetComponent<blackclick>();
        back.IsClickCheck(obj);
    }
}
