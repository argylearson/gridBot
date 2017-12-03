using UnityEngine;

public class RandomPlayer : Player
{
    public override Move MakeMove(Board board, float timeLimit = 10f)
    {
        System.Random random = new System.Random();
        int randvar = random.Next(1, 100);
        
        Move move = null;
        if (randvar >= 0 && randvar < 25)
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
        if (randvar >= 25 && randvar < 50)
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
        if (randvar >= 50 && randvar < 75)
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
        if (randvar >= 75 && randvar < 100)
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
