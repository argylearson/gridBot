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
                x = board.playerPositions[board.TryGetPlayerIndex(spriteColor)].x,
                y = board.playerPositions[board.TryGetPlayerIndex(spriteColor)].y
            };
            board.AdjustPosition(board.TryGetPlayerIndex(spriteColor), agent.direction.Value);
            agent.direction = null;
            isTurn = false;
        }
        return move;
    }
}
