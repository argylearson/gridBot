using System;
using System.Collections.Generic;
using UnityEngine;

public class Board{

    [Range(1,10)]
    public int width;
    [Range(1,10)]
    public int height;
    public Edge edge;
    public readonly int maxTraversals = 3;
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

    public int TryGetPlayerIndex(Color color)
    {
        var index = -1;
        var i = 0;
        while (index < 0 && i < score.Length)
        {
            if (ColorUtil.SameColor(color, score[i].x))
                index = i;
            i++;
        }
        return index;
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

    public void MakeMove(Move move)
    {
        var color = new Color(move.playerColor.r, move.playerColor.g, move.playerColor.b);
        switch (move.direction)
        {
            case EdgeDirection.Up:
                UpdateScore(vertEdges[move.x, move.y], color, move.direction);
                vertEdges[move.x, move.y].traversals += 1;
                vertEdges[move.x, move.y].playerColor = color;
                break;
            case EdgeDirection.Right:
                UpdateScore(horzEdges[move.x, move.y], color, move.direction);
                horzEdges[move.x, move.y].traversals += 1;
                horzEdges[move.x, move.y].playerColor = color;
                break;
            case EdgeDirection.Down:
                UpdateScore(vertEdges[move.x, move.y - 1], color, move.direction);
                vertEdges[move.x, move.y - 1].traversals += 1;
                vertEdges[move.x, move.y - 1].playerColor = color;
                break;
            case EdgeDirection.Left:
                UpdateScore(horzEdges[move.x - 1, move.y], color, move.direction);
                horzEdges[move.x - 1, move.y].traversals += 1;
                horzEdges[move.x - 1, move.y].playerColor = color;
                break;
        }
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

    private void UpdateScore(Edge edge, Color color, EdgeDirection direction)
    {
        CheckSquares(edge, color, direction);
        for (int i = 0; i < score.Length; i++)
        {
            if (ColorUtil.SameColor(score[i].x, color))
                score[i].y += 1;
            if (ColorUtil.SameColor(score[i].x, edge.playerColor))
                score[i].y -= 1;
        }
    }

    private void CheckSquares(Edge edge, Color color, EdgeDirection direction)
    {
        switch (direction)
        {
            case EdgeDirection.Down:
            case EdgeDirection.Up:
                if (edge.x > 0)
                {
                    if (ColorUtil.SameColor(horzEdges[edge.x - 1, edge.y + 1].playerColor, horzEdges[edge.x - 1, edge.y].playerColor) &&
                        ColorUtil.SameColor(horzEdges[edge.x - 1, edge.y + 1].playerColor, vertEdges[edge.x - 1, edge.y].playerColor) &&
                        !ColorUtil.SameColor(vertEdges[edge.x, edge.y].playerColor, color))
                    {
                        var index = TryGetPlayerIndex(horzEdges[edge.x - 1, edge.y].playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(score[index].x, color))
                                score[index].y++;
                            else
                                score[index].y--;
                        }
                    }
                }
                if (edge.x < width)
                {
                    if (ColorUtil.SameColor(horzEdges[edge.x, edge.y + 1].playerColor, horzEdges[edge.x, edge.y].playerColor) &&
                        ColorUtil.SameColor(horzEdges[edge.x, edge.y + 1].playerColor, vertEdges[edge.x + 1, edge.y].playerColor) &&
                        !ColorUtil.SameColor(vertEdges[edge.x, edge.y].playerColor, color))
                    {
                        var index = TryGetPlayerIndex(horzEdges[edge.x, edge.y].playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(score[index].x, color))
                                score[index].y++;
                            else
                                score[index].y--;
                        }
                    }
                }
                break;
            case EdgeDirection.Left:
            case EdgeDirection.Right:
                if (edge.y > 0)
                {
                    if (ColorUtil.SameColor(vertEdges[edge.x, edge.y - 1].playerColor, vertEdges[edge.x + 1, edge.y - 1].playerColor) &&
                        ColorUtil.SameColor(vertEdges[edge.x, edge.y - 1].playerColor, horzEdges[edge.x, edge.y - 1].playerColor) &&
                        !ColorUtil.SameColor(horzEdges[edge.x, edge.y].playerColor, color))
                    {
                        var index = TryGetPlayerIndex(vertEdges[edge.x, edge.y - 1].playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(score[index].x, color))
                                score[index].y++;
                            else
                                score[index].y--;
                        }
                    }
                }
                if (edge.y < height)
                {
                    if (ColorUtil.SameColor(vertEdges[edge.x, edge.y].playerColor, vertEdges[edge.x + 1, edge.y].playerColor) &&
                        ColorUtil.SameColor(vertEdges[edge.x, edge.y].playerColor, horzEdges[edge.x, edge.y + 1].playerColor) &&
                        !ColorUtil.SameColor(horzEdges[edge.x, edge.y].playerColor, color))
                    {
                        var index = TryGetPlayerIndex(vertEdges[edge.x, edge.y].playerColor);
                        if (index >= 0)
                        {
                            if (ColorUtil.SameColor(score[index].x, color))
                                score[index].y++;
                            else
                                score[index].y--;
                        }
                    }
                }
                break;
        }
    }
}
