using System;
using System.Collections.Generic;
using UnityEngine;

public class Board{

    [Range(1,10)]
    public int width;
    [Range(1,10)]
    public int height;
    public Edge edge;
    public int maxTraversals = 3;
    public Pair<int, int>[] playerPositions;
    public Pair<Color, int>[] score;

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


    public bool IsMoveLegal(Move move)
    {
        var result = true;
        Pair<int, int> targetSpace = null;
        switch (move.direction)
        {
            case EdgeDirection.Down:
                targetSpace = new Pair<int, int>(move.x, move.y - 1);
                break;
            case EdgeDirection.Left:
                targetSpace = new Pair<int, int>(move.x - 1, move.y);
                break;
            case EdgeDirection.Right:
                targetSpace = new Pair<int, int>(move.x + 1, move.y);
                break;
            case EdgeDirection.Up:
                targetSpace = new Pair<int, int>(move.x, move.y + 1);
                break;
        }
        result &= targetSpace != null && 
            !SpaceOccupied(targetSpace) &&
            (targetSpace.x >= 0 && targetSpace.y >= 0 && targetSpace.x <= width && targetSpace.y <= height) &&
            !MaxTraversals(move);
        return result;
    }

    public IEnumerable<Move> GetLegalMoves(Color player, int x, int y)
    {
        var result = new List<Move>();
        foreach (EdgeDirection direction in Enum.GetValues(typeof(EdgeDirection)))
        {
            var move = new Move
            {
                playerColor = player,
                direction = direction,
                x = x,
                y = y
            };
            if (IsMoveLegal(move))
                result.Add(move);
        }
        return result;
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
        newB.playerPositions = new Pair<int, int>[playerPositions.Length] ;
        for (int i = 0; i < playerPositions.Length; i++)
        {
            var newPosition = new Pair<int, int>(playerPositions[i].x, playerPositions[i].y);
            newB.playerPositions[i] = newPosition;
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

    private bool SpaceOccupied(Pair<int, int> targetSpace)
    {
        var result = false;
        for (int i = 0; i < playerPositions.Length; i++)
        {
            result |= (targetSpace.x == playerPositions[i].x &&
                       targetSpace.y == playerPositions[i].y);
        }
        return result;
    }

    private bool MaxTraversals(Move move)
    {
        Pair<int, int> targetSpace = null;
        Edge[,] edges = null;
        switch (move.direction)
        {
            case EdgeDirection.Down:
                targetSpace = new Pair<int, int>(move.x, move.y - 1);
                edges = vertEdges;
                break;
            case EdgeDirection.Left:
                targetSpace = new Pair<int, int>(move.x - 1, move.y);
                edges = horzEdges;
                break;
            case EdgeDirection.Right:
                targetSpace = new Pair<int, int>(move.x, move.y);
                edges = horzEdges;
                break;
            case EdgeDirection.Up:
                targetSpace = new Pair<int, int>(move.x, move.y);
                edges = vertEdges;
                break;
        }
        return edges == null ||
            edges[targetSpace.x, targetSpace.y].traversals >= maxTraversals;
    }
}
