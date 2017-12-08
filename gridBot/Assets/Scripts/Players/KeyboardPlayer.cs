using UnityEngine;

public class KeyboardPlayer : Player
{
    public override Move MakeMove(Board board, float timeLimit = 10f)
    {
        Move move = null;
        if (Input.GetKeyDown(KeyCode.W))
        {
            move = new Move()
            {
                direction = EdgeDirection.Up,
                playerColor = spriteColor,
                x = x,
                y = y
            };
            y += 1;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            move = new Move()
            {
                direction = EdgeDirection.Left,
                playerColor = spriteColor,
                x = x,
                y = y
            };
            x -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            move = new Move()
            {
                direction = EdgeDirection.Down,
                playerColor = spriteColor,
                x = x,
                y = y
            };
            y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            move = new Move()
            {
                direction = EdgeDirection.Right,
                playerColor = spriteColor,
                x = x,
                y = y
            };
            x += 1;
        }
        return move;
    }
}
