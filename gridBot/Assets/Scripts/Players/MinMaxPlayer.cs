using System;
using System.Linq;

public class MinMaxPlayer : Player
{
    public IHeuristic heuristic = new MaxScoreHeuristic();
    private int maxDepth = 2;
    private int myIndex;
    private int currentMin = Int32.MaxValue;
    private int currentMax = Int32.MinValue;

    public override Move MakeMove(Board board, float timeLimit)
    {
        myIndex = board.TryGetPlayerIndex(spriteColor);
        int score;
        return BestMove(board, myIndex, 0, out score);
    }


    private Move BestMove(Board board, int playerIndex, int depth, out int outScore)
    {
        Move bestMove = null;
        int localBest;
        outScore = 0;
        var moves = board.GetLegalMoves(board.score[playerIndex].x, board.playerPositions[playerIndex].x, board.playerPositions[playerIndex].y).ToArray();
        if (depth >= maxDepth * board.score.Length)
        {
            foreach (var move in moves)
            {
                var clone = board.DeepCopy();
                clone.MakeMove(move);
                var score = heuristic.Score(clone, playerIndex);
                if (playerIndex == myIndex)
                {
                    localBest = Int32.MinValue;
                    if (score >= localBest)
                    {
                        outScore = score;
                        bestMove = move;
                    }
                    else if (score <= currentMin)
                    {
                        outScore = Int32.MinValue;
                        break;
                    }
                }
                else
                {
                    currentMin = score;
                    outScore = score;
                    bestMove = move;
                }
            }
        }
        else
        {
            if (moves.Length == 0)
            {
                BestMove(board, (playerIndex + 1) % board.score.Length, depth + 1, out outScore);
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
                    if (playerIndex == myIndex)
                    {
                        if (score <= currentMin)
                        {
                            outScore = Int32.MinValue;
                            break;
                        }
                        else if (score >= currentMax)
                        {
                            outScore = score;
                            bestMove = move;
                        }
                    }
                    else
                    {
                        if (score >= currentMax)
                        {
                            outScore = Int32.MaxValue;
                            break;
                        }
                        else if (score <= currentMin)
                        {
                            outScore = score;
                            bestMove = move;
                        }
                    }
                }
            }
        }
        return bestMove;
    }
}
