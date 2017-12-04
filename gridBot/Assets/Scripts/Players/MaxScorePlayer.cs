using System;
using System.Linq;
using UnityEngine;

public class MaxScorePlayer : Player
{
    private IHeuristic heuristic = new MaxScoreHeuristic();
    private int maxDepth = 3;
    private int myIndex;
    private int[] scores;

    public override Move MakeMove(Board board, float timeLimit)
    {
        myIndex = board.TryGetPlayerIndex(spriteColor);
        scores = new int[4];
        var moves = board.GetLegalMoves(spriteColor, board.playerPositions[myIndex].x,
            board.playerPositions[myIndex].y);
        var moveArray = moves as Move[] ?? moves.ToArray();
        for (int i = 0; i < moveArray.Count(); i++)
        {
            scores[i] = -1;
            CalculateScore(board.DeepCopy(), i, 0);
        }
        var bestIndex = Array.IndexOf(scores, scores.Max());
        return moveArray[bestIndex];
    }

    private void CalculateScore(Board board, int orig, int depth)
    {
        if (depth == maxDepth)
        {
            var clone = board.DeepCopy();
            for (int i = 0; i < board.score.Length; i++)
            {
                var playerIndex = (myIndex + i) % clone.score.Length;
                MakeBestMove(clone, clone.score[playerIndex].x);
            }
            if (clone.score[myIndex].y > scores[orig])
                scores[orig] = clone.score[myIndex].y;
        }
        else
        {
            var clone = board.DeepCopy();
            //dig down to find my move

            //all other players make simple move
            for (int i = 1; i < board.score.Length; i++)
            {
                var playerIndex = (myIndex + i) % board.score.Length;
                MakeBestMove(clone, clone.score[playerIndex].x);
            }
            CalculateScore(clone, orig, depth + 1);
        }
    }

    private void MakeBestMove(Board board, Color color)
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
        if (bestMove != null)
            board.MakeMove(bestMove);
    }
}