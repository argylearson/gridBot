﻿using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor.Experimental.Build.Player;
using UnityEditorInternal;

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
    private Pair<int, int>[] startPairs;
    private string winnerString;
    private bool winnerChosen = false;
    private GUIStyle winnerStyle = new GUIStyle();

    private void OnGUI()
    {
        for (int i = 0; i < board.scores.Length; i++)
        {
            var style = new GUIStyle();
            style.normal.textColor = playerColors[i];
            GUI.Label(new Rect(10, 20 * (i + 1), 30, 30), board.scores[i], style);
        }
        GUI.Label(new Rect(Screen.width/2, 10, 50, 50), numberOfTurns.ToString());
        GUI.Label(new Rect(Screen.width / 2, 40, 50, 50), winnerString, winnerStyle);
    }

    private void Awake()
    {
        winnerString = "";
        players = new List<PlayerState>();
        board.scores = new string[playerTypes.Length];
        startPairs = new Pair<int, int>[]
        {
            new Pair<int, int>(0, 0),
            new Pair<int, int>(board.width, board.height),
            new Pair<int, int>(0, board.height),
            new Pair<int, int>(board.width, 0) 
        };
        board.board.playerPositions = new Pair<int, int>[playerTypes.Length];
        board.board.score = new Pair<Color, int>[playerTypes.Length];
        for (int i = 0; i < playerTypes.Length; i++)
        {
            board.scores[i] = "0";
            var state = new PlayerState();
            switch (playerTypes[i])
            {
                case (PlayerType.Keyboard):
                    CreatePlayer(state, typeof(KeyboardPlayer), i);
                    break;
                case (PlayerType.Random):
                    CreatePlayer(state, typeof(RandomPlayer), i);
                    break;
                case (PlayerType.SimpleMaxScore):
                    CreatePlayer(state, typeof(SimpleMaxScorePlayer), i);
                    break;
                case (PlayerType.MaxScore):
                    CreatePlayer(state, typeof(MaxScorePlayer), i);
                    break;
            }
            board.board.playerPositions[i] = new Pair<int, int>(players[i].x, players[i].y);
            board.board.score[i] = new Pair<Color, int>(playerColors[i], 0);
        }
        playerBoard = board.board.DeepCopy();
    }

    private void CreatePlayer(PlayerState state, Type type, int playerNumber)
    {
        var player = new GameObject("player " + playerNumber);
        if (type == typeof(KeyboardPlayer))
            state.player = player.AddComponent<KeyboardPlayer>();
        else if (type == typeof(RandomPlayer))
            state.player = player.AddComponent<RandomPlayer>();
        else if (type == typeof(SimpleMaxScorePlayer))
            state.player = player.AddComponent<SimpleMaxScorePlayer>();
        else if (type == typeof(MaxScorePlayer))
            state.player = player.AddComponent<MaxScorePlayer>();
        state.x = startPairs[playerNumber].x;
        state.y = startPairs[playerNumber].y;
        state.startPosition = new Vector3(state.x -.1f, state.y);
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
        for (int i = 0; i < playerTypes.Length; i++)
        {
            players[i].player.transform.position = Vector3.Lerp(players[i].startPosition, players[i].endPosition,
                (Time.time - players[i].startTime) * 2);
        }
        if (numberOfTurns > 0)
        {
            var moves = playerBoard.GetLegalMoves(playerColors[activePlayerNumber], players[activePlayerNumber].x, players[activePlayerNumber].y);
            Move move = null;
            if (moves.Any())
                move = players[activePlayerNumber].player.MakeMove(playerBoard, timeLimit);
            if (currentPlayersTime > timeLimit || !moves.Any())
            {
                NextTurn();
            }
            else if (move != null) {
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
        else if (!winnerChosen)
        {
            int winner = -1;
            int currentMax = -1;
            for (int i = 0; i < players.Count; i++)
            {
                if (board.board.score[i].y > currentMax)
                {
                    winner = i;
                    currentMax = board.board.score[i].y;
                }
                else if (board.board.score[i].y == currentMax)
                    winner = -1;
            }
            if (winner < 0)
                winnerString = "TIE GAME";
            else
            {
                winnerStyle.normal.textColor = board.board.score[winner].x;
                winnerStyle.fontSize = 32;
                winnerString = "Player " + winner + " wins!";
            }
            winnerChosen = true;
        }
    }
}
