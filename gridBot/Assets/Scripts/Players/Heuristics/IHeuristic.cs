using UnityEngine;

interface IHeuristic
{
    Move BestMove(Board board, Color color);
}