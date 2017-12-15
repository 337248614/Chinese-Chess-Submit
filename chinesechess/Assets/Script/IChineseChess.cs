using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDG;
public interface IChineseChess  {

    
     int[,] InitChessBoard();
     void BackStep();
     bool MoveChess();
     void KingAttackCheck();
     void GetChessState();
     void IsGameOver();
     void GetAiMove();
     void SetChessModel();
     DifficultyModel _difficultyModel { get; set; }
     GameModel _gameModel { get; set; }

    
}
