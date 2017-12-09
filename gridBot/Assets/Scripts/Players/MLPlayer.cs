using Assets.Scripts.MachineLearning;
using UnityEngine;

public class MLPlayer : Player
{
    public GridBotAgent agent;

    public override Move MakeMove(Board board, float timeLimit)
    {
        Move move = null;
        if (agent.board == null)
            agent.board = board;
        if (agent.direction != null)
        {
            move = new Move()
            {
                direction = (EdgeDirection) agent.direction,
                playerColor = spriteColor,
                x = x,
                y = y
            };
            agent.direction = null;
        }
        return move;
    }
}
