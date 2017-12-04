public class SimpleMaxScorePlayer : Player
{
    private IHeuristic heuristic = new MaxScoreHeuristic();

    public override Move MakeMove(Board board, float timeLimit)
    {
        Move bestMove = null;
        int bestScore = -1;
        int i = board.TryGetPlayerIndex(spriteColor);
        foreach (var move in board.GetLegalMoves(spriteColor, board.playerPositions[i].x, board.playerPositions[i].y))
        {
            var score = heuristic.MoveScore(board.DeepCopy(), move);
            if (score > bestScore)
            {
                bestMove = move;
            }
        }
        return bestMove;
    }
}