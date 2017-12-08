using System;
using System.Linq;

public class QuickHeuristicPlayer : Player
{
    public IHeuristic heuristic = new MaxScoreHeuristic();
    private int maxDepth = 8;
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
        maxScore = Int32.MinValue;
        var moves = board.GetLegalMoves(board.score[playerIndex].x, board.playerPositions[playerIndex].x,
            board.playerPositions[playerIndex].y).ToArray();
        if (depth >= maxDepth)
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
                    int score;
                    clone.MakeMove(move);
                    clone.AdjustPosition(playerIndex, move.direction);
                    for (int i = 1; i < clone.score.Length; i++)
                    {
                        var otherMove = BestMove(clone, (myIndex + i) % clone.score.Length, maxDepth, out score);
                        if (otherMove != null)
                        {
                            clone.MakeMove(otherMove);
                            clone.AdjustPosition((myIndex + i) % clone.score.Length, otherMove.direction);
                        }
                    }
                    BestMove(clone, playerIndex, depth + 1, out score);
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