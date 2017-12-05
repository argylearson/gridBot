using System.Linq;

public class MaxScorePlayer : Player
{
    private IHeuristic heuristic = new MaxScoreHeuristic();
    private int maxDepth = 4;
    private int myIndex;
    private int[] scores;

    public override Move MakeMove(Board board, float timeLimit)
    {
        myIndex = board.TryGetPlayerIndex(spriteColor);
        int score;
        return BestMove(board, myIndex, 0, out score);
    }

    private Move BestMove(Board board, int playerIndex, int depth, out int maxScore)
    {
        Move bestMove = null;
        maxScore = -1;
        var moves = board.GetLegalMoves(board.score[playerIndex].x, board.playerPositions[playerIndex].x, board.playerPositions[playerIndex].y).ToArray();
        if (depth >= maxDepth * board.score.Length)
        {
            maxScore = heuristic.Score(board, playerIndex);
            foreach (var move in moves)
            {
                var clone = board.DeepCopy();
                clone.MakeMove(move);
                var score = heuristic.Score(clone, playerIndex);
                if (score >= maxScore)
                {
                    maxScore = score;
                    bestMove = move;
                }
            }
        }
        else
        {
            if (moves.Length == 0)
            {
                BestMove(board, (playerIndex + 1) % board.score.Length, depth + 1, out maxScore);
            }
            else
            {
                foreach (Move move in moves)
                {
                    var clone = board.DeepCopy();
                    clone.MakeMove(move);
                    clone.AdjustPosition(playerIndex, move.direction);
                    int score;
                    BestMove(clone, (playerIndex + 1) % board.score.Length, depth + 1, out score);
                    if (score > maxScore)
                    {
                        maxScore = score;
                        bestMove = move;
                    }
                }
            }
        }
        return bestMove;
    }
}