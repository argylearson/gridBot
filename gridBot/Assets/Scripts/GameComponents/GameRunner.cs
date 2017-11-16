using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

public class GameRunner : MonoBehaviour {
    
    [SerializeField]
    private Board board;
    private List<PlayerState> players;
    [SerializeField]
    private PlayerType[] playerTypes;
    [SerializeField]
    private Color[] playerColors;
    [SerializeField]
    private Sprite playerSprite;

    private void Awake()
    {
        players = new List<PlayerState>();
        for (int i = 0; i < playerTypes.Length; i++)
        {
            var state = new PlayerState();
            var player = new GameObject("player " + i);
            switch (playerTypes[i])
            {
                case (PlayerType.Keyboard):
                    state.player = player.AddComponent<KeyboardPlayer>();
                    state.x = 0;
                    state.y = 0;
                    state.startPosition = new Vector3(-.1f,0);
                    state.endPosition = state.startPosition;
                    players.Add(state);
                    player.AddComponent<SpriteRenderer>().sprite = playerSprite;
                    //Instantiate(player, new Vector3(-.1f, 0), Quaternion.identity, this.transform);
                    break;
            }
        }
    }

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

    private void Update()
    {
        for (int i = 0; i < playerTypes.Length; i++)
        {
            players[i].player.transform.position = Vector3.Lerp(players[i].startPosition, players[i].endPosition, (Time.time - players[i].startTime)*2);
        }
        var move = players[0].player.MakeMove(board, 0f);
        if (move != null)
        {
            players[0].startPosition = players[0].endPosition;
            players[0].startTime = Time.time;
            switch(move.direction)
            {
                case EdgeDirection.Up:
                    players[0].endPosition = new Vector3(move.x - .1f, move.y + 1);
                    break;
                case EdgeDirection.Down:
                    players[0].endPosition = new Vector3(move.x - .1f, move.y - 1);
                    break;
                case EdgeDirection.Left:
                    players[0].endPosition = new Vector3(move.x - 1.1f, move.y);
                    break;
                case EdgeDirection.Right:
                    players[0].endPosition = new Vector3(move.x + .9f, move.y);
                    break;
            }
            RecolorEdge(move);
        }
    }
}
