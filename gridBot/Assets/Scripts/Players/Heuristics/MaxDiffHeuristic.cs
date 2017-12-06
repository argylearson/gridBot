public class MaxDiffHeuristic : IHeuristic
{
    public int Score(Board board, int playerIndex)
    {
        var myScore = board.score[playerIndex].y;
        var maxScore = 0;
        for (int i = 1; i < board.score.Length; i++)
        {
            var score = board.score[(playerIndex + i) % board.score.Length].y;
            if (maxScore < score)
                maxScore = score;
        }
        return myScore - maxScore;
    }
}