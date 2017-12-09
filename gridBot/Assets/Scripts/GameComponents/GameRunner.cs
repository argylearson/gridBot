using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using Assets.Scripts.MachineLearning;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private GridBotAgent agent;
    [SerializeField]
    private List<GridBotAgent> agents;
    private int agentIndex;
    [SerializeField]
    private Brain brain;

    private void OnGUI()
    {
        for (int i = 0; i < board.scores.Length; i++)
        {
            var style = new GUIStyle();
            style.normal.textColor = playerColors[i];
            GUI.Label(new Rect(10, 20 * (i + 2), 30, 30), board.scores[i], style);
        }
        GUI.Label(new Rect(Screen.width / 2, 10, 200, 50), "Turns Remaining: " + numberOfTurns.ToString());
        GUI.Label(new Rect(Screen.width / 2, 40, 50, 50), winnerString, winnerStyle);

        GUI.Label(new Rect(10, 20, 200, 50), "Current Player: " + (activePlayerNumber + 1));
        GUI.Label(new Rect(10, 0, 200, 50), "Time Remaining: " + (timeLimit - currentPlayersTime));
    }

    private void Awake()
    {
        winnerString = "";
        players = new List<PlayerState>();
        board.scores = new string[playerTypes.Length];
        agentIndex = 0;
        //only create agents once;
        /*if (agents == null)
        {
            agent.brain = brain;
            agents = new List<GridBotAgent>();
            foreach (var type in playerTypes)
            {
                if (type == PlayerType.MLPlayer)
                    agents.Add(Instantiate(agent));
            }
        }*/
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
                    CreatePlayer(state, typeof(HeuristicPlayer), i);
                    break;
                case (PlayerType.MaxDiff):
                    CreatePlayer(state, typeof(HeuristicPlayer), i);
                    ((HeuristicPlayer) state.player).heuristic = new MaxDiffHeuristic();
                    break;
                case (PlayerType.QuickMaxScore):
                    CreatePlayer(state, typeof(QuickHeuristicPlayer), i);
                    break;
                case (PlayerType.QuickMaxDiff):
                    CreatePlayer(state, typeof(QuickHeuristicPlayer), i);
                    ((QuickHeuristicPlayer)state.player).heuristic = new MaxDiffHeuristic();
                    break;
                case (PlayerType.MLPlayer):
                    CreatePlayer(state, typeof(MLPlayer), i);
                    ((MLPlayer) state.player).agent = agents[agentIndex];
                    agents[agentIndex].player = (MLPlayer) state.player;
                    agentIndex++;
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
        else if (type == typeof(HeuristicPlayer))
            state.player = player.AddComponent<HeuristicPlayer>();
        else if (type == typeof(QuickHeuristicPlayer))
            state.player = player.AddComponent<QuickHeuristicPlayer>();
        else if (type == typeof(MLPlayer))
            state.player = player.AddComponent<MLPlayer>();
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
            foreach (var agent in agents)
            {
                agent.gameOver = winner;
            }
            if (winner < 0)
                winnerString = "TIE GAME";
            else
            {
                winnerStyle.normal.textColor = board.board.score[winner].x;
                winnerStyle.fontSize = 32;
                winnerString = "Player " + (winner + 1) + " wins!";
            }

            winnerChosen = true;
        }
        else if (AgentsReset())
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        foreach (var player in players)
        {
            Destroy(player.player.gameObject);
        }
        board.Reset();
        Awake();
        numberOfTurns = 60;
        currentPlayersTime = 0;
        activePlayerNumber = 0;
        winnerChosen = false;
    }

    private bool AgentsReset()
    {
        var result = true;
        foreach (var agent in agents)
        {
            result &= agent.gameOver == -2;
        }
        return result;
    }

    private void SaveCsv(int winner, int score)
    {
        string path = @"/Saved_data.csv";
        string data;
        if (winner < 0)
            data = "-1, tie, 0";
        else
            data = winner + ", " + playerTypes[winner] + ", " + score;

        if (!File.Exists(path))
        {
            using (File.Create(path));
        }
        using (var writer = File.AppendText(path))
        {
            writer.WriteLine(data);
        }
    }
}
