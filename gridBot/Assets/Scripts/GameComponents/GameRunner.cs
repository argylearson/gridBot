using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

public class GameRunner : MonoBehaviour {
    
    [SerializeField]
    private Board board;
    private List<Player> players;
    private float[] startTimes;
    private Vector3[] startPositions;
    private Vector3[] endPositions;
    [SerializeField]
    private PlayerType[] playerTypes;
    [SerializeField]
    private Sprite playerSprite;

    private void Start()
    {
        players = new List<Player>();
        startTimes = new float[playerTypes.Length];
        startPositions = new Vector3[playerTypes.Length];
        endPositions = new Vector3[playerTypes.Length];
        for (int i = 0; i < playerTypes.Length; i++)
        {
            var player = new GameObject("player " + i);
            switch (playerTypes[i])
            {
                case (PlayerType.Keyboard):
                    var keyboardPlayer = player.AddComponent<KeyboardPlayer>();
                    keyboardPlayer.x = 0;
                    keyboardPlayer.y = 0;
                    startPositions[i] = new Vector3(0,0);
                    endPositions[i] = new Vector3(0,0);
                    players.Add(keyboardPlayer);
                    player.AddComponent<SpriteRenderer>().sprite = playerSprite;
                    Instantiate(player, new Vector3(-.1f, 0), Quaternion.identity, this.transform);
                    break;
            }
        }

        StartGame();
    }

    private void StartGame()
    {
        startPositions[0] = endPositions[0];
        Move move = null;
        while (move == null)
        {
            move = players[0].MakeMove(board, 50f);
        }
        RecolorEdge(move);
        endPositions[0] = new Vector3(move.x - .1f, move.y);
        startTimes[0] = Time.time;
        endPositions[0] = new Vector3();
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
            players[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], Time.time - startTimes[i]);
        }
    }
}
