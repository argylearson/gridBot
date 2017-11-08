using UnityEngine;
using System.Collections;

public class RandomPlayer : IPlayer
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

    void Start()
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
           System.Random random = new System.Random();
           int randvar = random.Next(0, 100);
            
            if (randvar >= 0 && randvar < 25)
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
            if (randvar >= 25 && randvar < 50)
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
            if (randvar >= 50 && randvar < 75)
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
            if (randvar >= 75 && randvar < 100)
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
