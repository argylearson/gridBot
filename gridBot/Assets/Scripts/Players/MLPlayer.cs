using Assets.Scripts.MachineLearning;

public class MLPlayer : Player
{
    public GridBotAgent agent;
    public bool isTurn;

    public override Move MakeMove(Board board, float timeLimit)
    {
        Move move = null;
        isTurn = true;
        agent.board = board;
        if (agent.direction.HasValue)
        {
            move = new Move()
            {
                direction = agent.direction.Value,
                playerColor = spriteColor,
                x = x,
                y = y
            };
            agent.direction = null;
            isTurn = false;
        }
        return move;
    }
}
