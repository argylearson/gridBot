using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;
using System;

public class GameRunner : MonoBehaviour {
    
    [SerializeField]
    private GuiBoard board;
    private Board playerBoard;
    private List<PlayerState> players;
    [SerializeField]
    private PlayerType[] playerTypes;
    [SerializeField]
    private Color[] playerColors;
    [SerializeField]
    private Sprite playerSprite;
    [SerializeField]
    private int activePlayerNumber;
    [SerializeField]
    private float timeLimit;
    [SerializeField]
    private float currentPlayersTime;

    private void Awake()
    {
        players = new List<PlayerState>();
        board.board.playerPositions = new Pair<int>[playerTypes.Length];
        for (int i = 0; i < playerTypes.Length; i++)
        {
            var state = new PlayerState();
            switch (playerTypes[i])
            {
                case (PlayerType.Keyboard):
                    CreatePlayer(state, typeof(KeyboardPlayer), i);
                    board.board.playerPositions[i] = new Pair<int>(players[i].x, players[i].y);
                    break;
            }
        }
        playerBoard = board.board.DeepCopy();
    }

    private void CreatePlayer(PlayerState state, Type type, int playerNumber)
    {
        var player = new GameObject("player " + playerNumber);
        state.player = player.AddComponent<KeyboardPlayer>();
        state.x = 0;
        state.y = 0;
        state.startPosition = new Vector3(-.1f, 0);
        state.endPosition = state.startPosition;
        players.Add(state);
        player.AddComponent<SpriteRenderer>().sprite = playerSprite;
        var color = new Color(playerColors[playerNumber].r, playerColors[playerNumber].g, playerColors[playerNumber].b);
        state.player.UpdateColor(color);
        player.transform.parent = this.transform;
    }

    private void Update()
    {
        for (int i = 0; i < playerTypes.Length; i++)
        {
            players[i].player.transform.position = Vector3.Lerp(players[i].startPosition, players[i].endPosition, (Time.time - players[i].startTime)*2);
        }
        var move = players[activePlayerNumber].player.MakeMove(playerBoard, timeLimit);
        if (currentPlayersTime > timeLimit)
        {
            activePlayerNumber = (activePlayerNumber + 1) % playerTypes.Length;
            currentPlayersTime = 0;
        }
        else if (move != null)
        {
            players[activePlayerNumber].startPosition = players[activePlayerNumber].endPosition;
            players[activePlayerNumber].startTime = Time.time;
            switch(move.direction)
            {
                case EdgeDirection.Up:
                    players[activePlayerNumber].endPosition = new Vector3(move.x - .1f, move.y + 1);
                    break;
                case EdgeDirection.Down:
                    players[activePlayerNumber].endPosition = new Vector3(move.x - .1f, move.y - 1);
                    break;
                case EdgeDirection.Left:
                    players[activePlayerNumber].endPosition = new Vector3(move.x - 1.1f, move.y);
                    break;
                case EdgeDirection.Right:
                    players[activePlayerNumber].endPosition = new Vector3(move.x + .9f, move.y);
                    break;
            }
            board.RecolorEdge(move);
            activePlayerNumber = (activePlayerNumber + 1) % playerTypes.Length;
            playerBoard = board.board.DeepCopy();
            currentPlayersTime = 0;
        }
        currentPlayersTime += Time.deltaTime;
    }
}
