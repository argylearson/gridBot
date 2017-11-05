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
     *  (0,0) - (1,0) - (2,0)
     *    |       |       |
     *  (0,1) - (1,1) - (1,2)
     *    |       |       |
     *  (0,2) - (1,2) - (2,2)
    */


    private void Start()
    {
        vertEdges = new Edge[width, height];
        horzEdges = new Edge[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                vertEdges[i, j] = Instantiate(edge, new Vector3(i, j, 0), Quaternion.Euler(90f, 0f, 0f), this.transform);
                vertEdges[i, j].transform.name = "vert: " + i + ", " + j;
                horzEdges[i, j] = Instantiate(edge, new Vector3(i, j, 0), Quaternion.identity, this.transform);
                horzEdges[i, j].transform.name = "horz: " + i + ", " + j;
            }
        }
    }
}
