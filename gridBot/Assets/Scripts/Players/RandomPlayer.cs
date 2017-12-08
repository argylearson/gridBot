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
        board.AdjustPosition(board.TryGetPlayerIndex(spriteColor),move.direction);
        return move;
    }
}
