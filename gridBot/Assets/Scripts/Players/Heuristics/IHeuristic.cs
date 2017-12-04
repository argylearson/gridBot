using UnityEngine;

interface IHeuristic
{
    int MoveScore(Board board, Move move);
}