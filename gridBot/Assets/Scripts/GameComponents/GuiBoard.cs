using UnityEngine;
using System.Collections;

public class GuiBoard : MonoBehaviour
{
    public Board board;
    public int width;
    public int height;
    public GuiEdge edge;
    public GuiEdge[,] vertEdges;
    public GuiEdge[,] horzEdges;
    public string[] scores;
    // Use this for initialization
    private void Awake()
    {
        board = new Board(width, height);
        vertEdges = new GuiEdge[width + 1, height];
        horzEdges = new GuiEdge[width, height + 1];
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                if (i != width)
                {
                    horzEdges[i, j] = CreateEdge(i, j, EdgeDirection.Right);
                    board.horzEdges[i, j] = horzEdges[i, j].edge;
                }
                if (j != height)
                {
                    vertEdges[i, j] = CreateEdge(i, j, EdgeDirection.Down);
                    board.vertEdges[i, j] = vertEdges[i, j].edge;
                }

            }
        }
    }

    private GuiEdge CreateEdge(int i, int j, EdgeDirection direction)
    {
        var position = direction == EdgeDirection.Right ? new Vector3(i + .4f, j, 0) : new Vector3(i - .1f, j + .5f, 0);
        var result = Instantiate(edge, position, Quaternion.identity, this.transform);
        result.edge = new Edge(i, j, Color.gray);
        result.transform.name = direction == EdgeDirection.Right ? "horz: " : "vert: ";
        result.transform.name += i + ", " + j;
        if (direction == EdgeDirection.Down) result.transform.Rotate(Vector3.forward * -90);
        return result;
    }

    public void RecolorEdge(Move move)
    {
        var color = new Color(move.playerColor.r, move.playerColor.g, move.playerColor.b);
        switch (move.direction)
        {
            case EdgeDirection.Up:
                UpdateScore(vertEdges[move.x, move.y], color);
                vertEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = color;
                board.vertEdges[move.x, move.y].traversals += 1;
                if (board.vertEdges[move.x, move.y].traversals == board.maxTraversals)
                    vertEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, + color.b, .5f);
                board.vertEdges[move.x, move.y].playerColor = color;
                break;
            case EdgeDirection.Right:
                UpdateScore(horzEdges[move.x, move.y], color);
                horzEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = color;
                board.horzEdges[move.x, move.y].traversals += 1;
                if (board.horzEdges[move.x, move.y].traversals == board.maxTraversals)
                    horzEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, +color.b, .5f);
                board.horzEdges[move.x, move.y].playerColor = color;
                break;
            case EdgeDirection.Down:
                UpdateScore(vertEdges[move.x, move.y - 1], color);
                vertEdges[move.x, move.y - 1].GetComponent<SpriteRenderer>().color = color;
                board.vertEdges[move.x, move.y - 1].traversals += 1;
                if (board.vertEdges[move.x, move.y - 1].traversals == board.maxTraversals)
                    vertEdges[move.x, move.y - 1].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, +color.b, .5f);
                board.vertEdges[move.x, move.y - 1].playerColor = color;
                break;
            case EdgeDirection.Left:
                UpdateScore(horzEdges[move.x - 1, move.y], color);
                horzEdges[move.x - 1, move.y].GetComponent<SpriteRenderer>().color = color;
                board.horzEdges[move.x - 1, move.y].traversals += 1;
                if (board.horzEdges[move.x - 1, move.y].traversals == board.maxTraversals)
                    horzEdges[move.x - 1, move.y].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, +color.b, .5f);
                board.horzEdges[move.x - 1, move.y].playerColor = color;
                break;
        }
    }

    private void UpdateScore(GuiEdge edge, Color color)
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (ColorUtil.SameColor(board.score[i].x,color))
                board.score[i].y += 1;
            if (ColorUtil.SameColor(board.score[i].x, edge.edge.playerColor))
                board.score[i].y -= 1;

            scores[i] = board.score[i].y.ToString();
        }
    }
}
