using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour{

    [Range(1,10)]
    public int width;
    [Range(1,10)]
    public int height;
    public Edge edge;

    /*represents all of the vertical edges in the board
    uses [x,y] notation to indicate the top vertex of
    the line. For example, vertEdges[0,0] is the edge
    descending from the top left node. vertEdges[1,0] 
    is the next edge to the right.*/
    public Edge[,] vertEdges;
    /*represents the horizontal edges in the board.
    horzEdges[0,0] is the top left horizontal line,
    and horzEdges[1,0] is the next line to the right.
    */
    public Edge[,] horzEdges;
    /* Here's how the board indices work
     *  (0,2) - (1,2) - (2,2)
     *    |       |       |
     *  (0,1) - (1,1) - (1,2)
     *    |       |       |
     *  (0,0) - (1,0) - (2,0)
    */


    public bool IsMoveLegal(Player player, Move move)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Move> GetLegalMoves(Player player)
    {
        var result = new List<Move>();
        throw new NotImplementedException();
    }

    private void Awake()
    {
        vertEdges = new Edge[width + 1, height];
        horzEdges = new Edge[width, height + 1];

        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                if (i != width) horzEdges[i, j] = CreateEdge(i, j, EdgeDirection.Right);
                if (j != height) vertEdges[i, j] = CreateEdge(i, j, EdgeDirection.Down);
            }
        }
    }

    private Edge CreateEdge(int i, int j, EdgeDirection direction)
    {
        var position = direction == EdgeDirection.Right ? new Vector3(i + .4f, j, 0) : new Vector3(i - .1f, j + .5f, 0);
        var result = Instantiate(edge, position, Quaternion.identity, this.transform);
        result.transform.name = direction == EdgeDirection.Right ? "horz: " : "vert: ";
        result.transform.name += i + ", " + j;
        if (direction == EdgeDirection.Down) result.transform.Rotate(Vector3.forward * -90);
        return result;
    }
}
