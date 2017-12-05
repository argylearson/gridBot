using System;

public class RandomPlayer : Player
{
    private readonly Random random = new Random(Guid.NewGuid().GetHashCode());

    public override Move MakeMove(Board board, float timeLimit)
    {
        int randvar = random.Next(0, 4);
        var move = new Move()
        {
            direction = (EdgeDirection)Enum.ToObject(typeof(EdgeDirection), randvar),
            playerColor = spriteColor,
            x = x,
            y = y
        };
        switch (randvar)
        {
            case 0:
                y += 1;
                break;
            case 1:
                x -= 1;
                break;
            case 2:
                y -= 1;
                break;
            case 3:
                x += 1;
                break;
        }
        return move;
    }
}
