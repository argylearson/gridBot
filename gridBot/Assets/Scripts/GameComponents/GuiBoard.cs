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
    // Use this for initialization
    void Awake()
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
        switch (move.direction)
        {
            case EdgeDirection.Up:
                vertEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = move.player.spriteColor;
                //TODO board.vertEdges[move.x, move.y].Traversals += 1;
                return;
            case EdgeDirection.Right:
                horzEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = move.player.spriteColor;
                return;
            case EdgeDirection.Down:
                vertEdges[move.x, move.y - 1].GetComponent<SpriteRenderer>().color = move.player.spriteColor;
                return;
            case EdgeDirection.Left:
                horzEdges[move.x - 1, move.y].GetComponent<SpriteRenderer>().color = move.player.spriteColor;
                return;
        }
    }
}
