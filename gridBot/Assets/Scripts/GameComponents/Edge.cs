using UnityEngine;

public class Edge {

    #region fields and properties
    public Color playerColor;
    private SpriteRenderer sprite;
    public readonly int x;
    public readonly int y;
    private int _traversals;

    public int Traversals
    {
        get { return _traversals; }
    }
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
