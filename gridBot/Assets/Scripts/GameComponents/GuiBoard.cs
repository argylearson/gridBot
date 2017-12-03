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
                UpdateScore(vertEdges[move.x, move.y], color, move.direction);
                vertEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = color;
                board.vertEdges[move.x, move.y].traversals += 1;
                if (board.vertEdges[move.x, move.y].traversals == board.maxTraversals)
                    vertEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, + color.b, .5f);
                board.vertEdges[move.x, move.y].playerColor = color;
                break;
            case EdgeDirection.Right:
                UpdateScore(horzEdges[move.x, move.y], color, move.direction);
                horzEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = color;
                board.horzEdges[move.x, move.y].traversals += 1;
                if (board.horzEdges[move.x, move.y].traversals == board.maxTraversals)
                    horzEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, +color.b, .5f);
                board.horzEdges[move.x, move.y].playerColor = color;
                break;
            case EdgeDirection.Down:
                UpdateScore(vertEdges[move.x, move.y - 1], color, move.direction);
                vertEdges[move.x, move.y - 1].GetComponent<SpriteRenderer>().color = color;
                board.vertEdges[move.x, move.y - 1].traversals += 1;
                if (board.vertEdges[move.x, move.y - 1].traversals == board.maxTraversals)
                    vertEdges[move.x, move.y - 1].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, +color.b, .5f);
                board.vertEdges[move.x, move.y - 1].playerColor = color;
                break;
            case EdgeDirection.Left:
                UpdateScore(horzEdges[move.x - 1, move.y], color, move.direction);
                horzEdges[move.x - 1, move.y].GetComponent<SpriteRenderer>().color = color;
                board.horzEdges[move.x - 1, move.y].traversals += 1;
                if (board.horzEdges[move.x - 1, move.y].traversals == board.maxTraversals)
                    horzEdges[move.x - 1, move.y].GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, +color.b, .5f);
                board.horzEdges[move.x - 1, move.y].playerColor = color;
                break;
        }
    }

    private void UpdateScore(GuiEdge edge, Color color, EdgeDirection direction)
    {
        CheckSquares(edge, color, direction);
        for (int i = 0; i < scores.Length; i++)
        {
            if (ColorUtil.SameColor(board.score[i].x,color))
                board.score[i].y += 1;
            if (ColorUtil.SameColor(board.score[i].x, edge.edge.playerColor))
                board.score[i].y -= 1;
            scores[i] = board.score[i].y.ToString();
        }
    }

    private void CheckSquares(GuiEdge edge, Color color, EdgeDirection direction)
    {
        switch (direction)
        {
            case EdgeDirection.Down:
            case EdgeDirection.Up:
                if (edge.edge.x > 0)
                {
                    if (ColorUtil.SameColor(horzEdges[edge.edge.x - 1, edge.edge.y + 1].edge.playerColor, horzEdges[edge.edge.x - 1, edge.edge.y].edge.playerColor) &&
                        ColorUtil.SameColor(horzEdges[edge.edge.x - 1, edge.edge.y + 1].edge.playerColor, vertEdges[edge.edge.x - 1, edge.edge.y].edge.playerColor) &&
                        !ColorUtil.SameColor(vertEdges[edge.edge.x, edge.edge.y].edge.playerColor, color))
                    {
                        var index = board.TryGetPlayerIndex(horzEdges[edge.edge.x - 1, edge.edge.y].edge.playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(board.score[index].x, color))
                                board.score[index].y++;
                            else
                                board.score[index].y--;
                        }
                    }
                }
                if (edge.edge.x < board.width)
                {
                    if (ColorUtil.SameColor(horzEdges[edge.edge.x, edge.edge.y + 1].edge.playerColor, horzEdges[edge.edge.x, edge.edge.y].edge.playerColor) &&
                        ColorUtil.SameColor(horzEdges[edge.edge.x, edge.edge.y + 1].edge.playerColor, vertEdges[edge.edge.x + 1, edge.edge.y].edge.playerColor) &&
                        !ColorUtil.SameColor(vertEdges[edge.edge.x, edge.edge.y].edge.playerColor, color))
                    {
                        var index = board.TryGetPlayerIndex(horzEdges[edge.edge.x, edge.edge.y].edge.playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(board.score[index].x, color))
                                board.score[index].y++;
                            else
                                board.score[index].y--;
                        }
                    }
                }
                break;
            case EdgeDirection.Left:
            case EdgeDirection.Right:
                if (edge.edge.y > 0)
                {
                    if (ColorUtil.SameColor(vertEdges[edge.edge.x, edge.edge.y - 1].edge.playerColor, vertEdges[edge.edge.x + 1, edge.edge.y - 1].edge.playerColor) &&
                        ColorUtil.SameColor(vertEdges[edge.edge.x, edge.edge.y - 1].edge.playerColor, horzEdges[edge.edge.x, edge.edge.y - 1].edge.playerColor) &&
                        !ColorUtil.SameColor(horzEdges[edge.edge.x, edge.edge.y].edge.playerColor, color))
                    {
                        var index = board.TryGetPlayerIndex(vertEdges[edge.edge.x, edge.edge.y - 1].edge.playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(board.score[index].x, color))
                                board.score[index].y++;
                            else
                                board.score[index].y--;
                        }
                    }
                }
                if (edge.edge.y < board.height)
                {
                    if (ColorUtil.SameColor(vertEdges[edge.edge.x, edge.edge.y].edge.playerColor, vertEdges[edge.edge.x + 1, edge.edge.y].edge.playerColor) &&
                        ColorUtil.SameColor(vertEdges[edge.edge.x, edge.edge.y].edge.playerColor, horzEdges[edge.edge.x, edge.edge.y + 1].edge.playerColor) &&
                        !ColorUtil.SameColor(horzEdges[edge.edge.x, edge.edge.y].edge.playerColor, color))
                    {
                        var index = board.TryGetPlayerIndex(vertEdges[edge.edge.x, edge.edge.y].edge.playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(board.score[index].x, color))
                                board.score[index].y++;
                            else
                                board.score[index].y--;
                        }
                    }
                }
                break;
        }
    }
}
