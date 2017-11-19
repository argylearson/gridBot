using UnityEngine;

public class Edge {

    #region fields and properties
    public Color playerColor;
    public readonly int x;
    public readonly int y;
    public int traversals;
    #endregion

    #region constructors

    public Edge(int x, int y, Color color)
    {
        this.x = x;
        this.y = y;
        playerColor = color;
    }
    #endregion
}
