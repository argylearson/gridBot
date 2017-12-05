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
            var clone = board.DeepCopy();
            clone.MakeMove(move);
            var score = heuristic.Score(clone, i);
            if (score > bestScore)
            {
                bestMove = move;
            }
        }
        return bestMove;
    }
}