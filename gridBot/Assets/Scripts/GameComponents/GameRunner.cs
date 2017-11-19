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
    [SerializeField]
    private int numberOfTurns;

    private void Awake()
    {
        players = new List<PlayerState>();
        board.board.playerPositions = new Pair<int, int>[playerTypes.Length];
        board.board.score = new Pair<Color, int>[playerTypes.Length];
        for (int i = 0; i < playerTypes.Length; i++)
        {
            var state = new PlayerState();
            switch (playerTypes[i])
            {
                case (PlayerType.Keyboard):
                    CreatePlayer(state, typeof(KeyboardPlayer), i);
                    board.board.playerPositions[i] = new Pair<int, int>(players[i].x, players[i].y);
                    board.board.score[i] = new Pair<Color, int>(playerColors[i], 0);
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
        state.player.transform.position = state.startPosition;
        state.endPosition = state.startPosition;
        players.Add(state);
        player.AddComponent<SpriteRenderer>().sprite = playerSprite;
        var color = new Color(playerColors[playerNumber].r, playerColors[playerNumber].g, playerColors[playerNumber].b);
        state.player.UpdateColor(color);
        player.transform.parent = this.transform;
    }

    private void NextTurn()
    {
        activePlayerNumber = (activePlayerNumber + 1) % playerTypes.Length;
        playerBoard = board.board.DeepCopy();
        currentPlayersTime = 0;
        numberOfTurns -= 1;
    }

    private void Update()
    {
        if (numberOfTurns > 0)
        {
            for (int i = 0; i < playerTypes.Length; i++)
            {
                players[i].player.transform.position = Vector3.Lerp(players[i].startPosition, players[i].endPosition,
                    (Time.time - players[i].startTime) * 2);
            }
            if (currentPlayersTime > timeLimit)
            {
                NextTurn();
            }
            var move = players[activePlayerNumber].player.MakeMove(playerBoard, timeLimit);
            if (move != null)
            {
                move.x = players[activePlayerNumber].x;
                move.y = players[activePlayerNumber].y;
                move.playerColor = playerColors[activePlayerNumber];
                if (board.board.IsMoveLegal(move))
                {
                    players[activePlayerNumber].startPosition = players[activePlayerNumber].endPosition;
                    players[activePlayerNumber].startTime = Time.time;
                    board.board.playerPositions[activePlayerNumber] = new Pair<int, int>(players[activePlayerNumber].x,
                        players[activePlayerNumber].y);
                    switch (move.direction)
                    {
                        case EdgeDirection.Up:
                            players[activePlayerNumber].endPosition = new Vector3(move.x - .1f, move.y + 1);
                            players[activePlayerNumber].y += 1;
                            board.board.playerPositions[activePlayerNumber].y += 1;
                            break;
                        case EdgeDirection.Down:
                            players[activePlayerNumber].endPosition = new Vector3(move.x - .1f, move.y - 1);
                            players[activePlayerNumber].y -= 1;
                            board.board.playerPositions[activePlayerNumber].y -= 1;
                            break;
                        case EdgeDirection.Left:
                            players[activePlayerNumber].endPosition = new Vector3(move.x - 1.1f, move.y);
                            players[activePlayerNumber].x -= 1;
                            board.board.playerPositions[activePlayerNumber].x -= 1;
                            break;
                        case EdgeDirection.Right:
                            players[activePlayerNumber].endPosition = new Vector3(move.x + .9f, move.y);
                            players[activePlayerNumber].x += 1;
                            board.board.playerPositions[activePlayerNumber].x += 1;
                            break;
                    }
                    board.RecolorEdge(move);
                }
                NextTurn();
            }
            currentPlayersTime += Time.deltaTime;
        }
    }
}
