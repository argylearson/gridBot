class MaxScoreHeuristic : IHeuristic
{
    public int MoveScore(Board board, Move move)
    {
        board.MakeMove(move);
        var index = board.TryGetPlayerIndex(move.playerColor);
        return board.score[index].y;
    }
}