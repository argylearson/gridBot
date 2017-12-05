class MaxScoreHeuristic : IHeuristic
{
    public int Score(Board board, int playerIndex)
    {
        return board.score[playerIndex].y;
    }
}