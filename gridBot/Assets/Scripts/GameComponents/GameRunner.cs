using UnityEngine;

public class GameRunner : MonoBehaviour {

    [SerializeField]
    private IPlayer player1;
    [SerializeField]
    private Board board;

    private void RecolorEdge(Move move)
    {
        switch (move.direction)
        {
            case EdgeDirection.Up:
                board.vertEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = move.player.color;
                return;
            case EdgeDirection.Right:
                board.horzEdges[move.x, move.y].GetComponent<SpriteRenderer>().color = move.player.color;
                return;
            case EdgeDirection.Down:
                board.vertEdges[move.x, move.y - 1].GetComponent<SpriteRenderer>().color = move.player.color;
                return;
            case EdgeDirection.Left:
                board.horzEdges[move.x - 1, move.y].GetComponent<SpriteRenderer>().color = move.player.color;
                return;
        }
    }

	void Update ()
	{
	    var move = player1.MakeMove(board, 1f);
	    if (move != null) RecolorEdge(move);
	}
}
