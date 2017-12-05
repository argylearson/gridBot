using UnityEngine;

interface IHeuristic
{
    int Score(Board board, int playerIndex);
}