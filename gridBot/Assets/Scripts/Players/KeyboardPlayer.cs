using UnityEngine;

public class KeyboardPlayer : Player
{
    private readonly Color _color = Color.red;

    public override Color color
    {
        get { return _color; }
    }

    void Start ()
    {
        this.GetComponent<SpriteRenderer>().color = _color;
    }

    public override Move MakeMove(Board board, float timeLimit = 10f)
    {
        Move move = null;
        if (Input.GetKeyDown(KeyCode.W))
        {
            move = new Move()
            {
                direction = EdgeDirection.Up,
                player = this,
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
                player = this,
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
                player = this,
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
                player = this,
                x = x,
                y = y
            };
            x += 1;
        }
        return move;
    }
}
