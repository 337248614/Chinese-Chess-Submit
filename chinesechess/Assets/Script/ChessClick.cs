using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessClick : MonoBehaviour {

    public void clickItemOrChess() 
    {
        GameObject obj = this.GetComponent<Button>().gameObject;
        ChessControl back = GameObject.Find("Main Camera").GetComponent<ChessControl>();
        back.IsClickCheck(obj);
    }
}
