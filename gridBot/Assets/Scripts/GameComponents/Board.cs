using System;
using System.Collections.Generic;
using UnityEngine;

public class Board{

    [Range(1,10)]
    public int width;
    [Range(1,10)]
    public int height;
    public Edge edge;
    public Pair<int>[] playerPositions;

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

    public Board(int width, int height)
    {
        this.width = width;
        this.height = height;
        vertEdges = new Edge[width + 1, height];
        horzEdges = new Edge[width, height + 1];
    }

    public Board DeepCopy()
    {
        var newB = new Board(width, height);
        newB.playerPositions = new Pair<int>[playerPositions.Length];
        for(int i = 0; i < playerPositions.Length; i++)
        {
            newB.playerPositions[i] = new Pair<int>(playerPositions[i].x, playerPositions[i].y);
        }
        newB.vertEdges = new Edge[width + 1, height];
        newB.horzEdges = new Edge[width, height + 1];
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                if (i != width)
                    newB.horzEdges[i, j] = new Edge(i, j, horzEdges[i, j].playerColor);
                if (j != height)
                    newB.vertEdges[i, j] = new Edge(i, j, vertEdges[i, j].playerColor);
            }
        }

        return newB;
    }
}
