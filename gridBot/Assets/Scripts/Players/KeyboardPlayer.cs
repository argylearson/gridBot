using UnityEngine;

public class KeyboardPlayer : IPlayer
{
    private readonly Color _color = Color.red;

    public override Color color
    {
        get { return _color; }
    }

    [SerializeField]
    private int x;
    [SerializeField]
    private int y;

    void Start ()
    {
        this.GetComponent<SpriteRenderer>().color = _color;
    }

    void Update()
    {
        transform.position = new Vector3(x - .1f, y);
    }

    public override Move MakeMove(Board board, float timeLimit = 10f)
    {
        float time = 0f;
        while (time < timeLimit)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                var move = new Move()
                {
                    direction = EdgeDirection.Up,
                    player = this,
                    x = x,
                    y = y
                };
                y += 1;
                return move;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                var move = new Move()
                {
                    direction = EdgeDirection.Left,
                    player = this,
                    x = x,
                    y = y
                };
                x -= 1;
                return move;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                var move = new Move()
                {
                    direction = EdgeDirection.Down,
                    player = this,
                    x = x,
                    y = y
                };
                y -= 1;
                return move;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                var move = new Move()
                {
                    direction = EdgeDirection.Right,
                    player = this,
                    x = x,
                    y = y
                };
                x += 1;
                return move;
            }

            time += Time.deltaTime;
        }
        return null;
    }
}
