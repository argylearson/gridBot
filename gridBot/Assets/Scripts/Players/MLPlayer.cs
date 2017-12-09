using System.Collections;
using Assets.Scripts.MachineLearning;
using UnityEngine;

public class MLPlayer : Player
{
    public GridBotAgent agent;

    public override Move MakeMove(Board board, float timeLimit)
    {
        Move move = null;
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
            agent.board = null;
        }
        else if (agent.board == null)
        {
            agent.board = board;
            agent.direction = null;
        }
        return move;
    }
}
