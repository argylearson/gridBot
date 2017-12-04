using UnityEngine;

class MaxScoreHeuristic : IHeuristic
{
    public Move BestMove(Board board, Color color)
    {
        var playerIndex = board.TryGetPlayerIndex(color);
        var moves = board.GetLegalMoves(color, board.playerPositions[playerIndex].x, board.playerPositions[playerIndex].y);
        Move bestMove = null;
        int bestScore = -1;
        foreach (var move in moves)
        {
            var clone = board.DeepCopy();
            clone.MakeMove(move);
            if (clone.score[playerIndex].y > bestScore)
            {
                bestScore = clone.score[playerIndex].y;
                bestMove = move;
            }
        }
        return bestMove;
    }
}