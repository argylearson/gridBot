public class SimpleMaxScore : Player
{
    IHeuristic heuristic = new MaxScoreHeuristic();

    public override Move MakeMove(Board board, float timeLimit)
    {
        return heuristic.BestMove(board, spriteColor);
    }
}