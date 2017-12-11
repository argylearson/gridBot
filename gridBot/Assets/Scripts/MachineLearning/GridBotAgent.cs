using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.MachineLearning
{
    public class GridBotAgent : Agent
    {
        public MLPlayer player;
        
        public Board board;
        public EdgeDirection? direction;

        public float targetNumber { get; set; }
        public float currentNumber { get; set; }
        public Text text;
        public int gameOver = -2;
        private int myId = -1;
        private int numberOfEdges = -1;
        private Dictionary<ColorStruct, int> colorDict = null;
        private IHeuristic heur = new MaxDiffHeuristic();

        //must return a list of length 
        public override List<float> CollectState()
        {
            var result = new List<float>();
            if (colorDict == null)
            {
                colorDict = new Dictionary<ColorStruct, int>();
                for (int i = 0; i < board.playerPositions.Length; i++)
                {
                    var color = board.score[i].x;
                    var key = new ColorStruct(color.r, color.g, color.b);
                    colorDict.Add(key, i);
                }
            }
            result.Add((float) board.TryGetPlayerIndex(player.spriteColor) / board.playerPositions.Length);
            for (int i = 0; i < board.playerPositions.Length; i++)
            {
                result.Add((float) board.playerPositions[i].x / board.width);
                result.Add((float) board.playerPositions[i].y / board.height);
            }
            for (int i = 0; i <= board.width; i++)
            {
                for (int j = 0; j <= board.height; j++)
                {
                    if (i != board.width)
                    {
                        var color = board.horzEdges[i, j].playerColor;
                        var lookup = new ColorStruct(color.r, color.g, color.b);
                        if (colorDict.ContainsKey(lookup))
                            result.Add((float) colorDict[lookup] / board.playerPositions.Length);
                        else
                            result.Add(-1);
                        result.Add((float) board.horzEdges[i, j].traversals / board.maxTraversals);
                    }
                    if (j != board.height)
                    {
                        var color = board.vertEdges[i, j].playerColor;
                        var lookup = new ColorStruct(color.r, color.g, color.b);
                        if (colorDict.ContainsKey(lookup))
                            result.Add((float) colorDict[lookup] / board.playerPositions.Length);
                        else
                            result.Add(-1);
                        result.Add((float) board.vertEdges[i, j].traversals / board.maxTraversals);
                    }
                }
            }
            return result;
        }

        public override void AgentReset()
        {
            direction = null;
        }

        public override void AgentStep(float[] action)
        {
            if (heur == null)
                heur = new MaxDiffHeuristic();
            if (gameOver < -1)
            {
                if (player.isTurn && !direction.HasValue)
                {
                    switch ((int) action[0])
                    {
                        case 0:
                            direction = EdgeDirection.Up;
                            break;
                        case 1:
                            direction = EdgeDirection.Right;
                            break;
                        case 2:
                            direction = EdgeDirection.Down;
                            break;
                        case 3:
                            direction = EdgeDirection.Left;
                            break;
                        default:
                            direction = null;
                            break;
                    }
                    if (board != null && direction.HasValue)
                    {
                        if (myId < 0)
                            myId = board.TryGetPlayerIndex(player.spriteColor);
                        if (numberOfEdges < 0)
                            numberOfEdges = board.height * (board.width + 1) + (board.height + 1) * board.width;
                        var move = new Move()
                        {
                            direction = direction.Value,
                            playerColor = player.spriteColor,
                            x = board.playerPositions[myId].x,
                            y = board.playerPositions[myId].y
                        };
                        reward = ((float)board.score[myId].y / numberOfEdges) * 2;
                        var heurValue = heur.Score(board, myId);
                        if (board.IsMoveLegal(move))
                        {
                            reward += .1f;
                            board.MakeMove(move);
                            heurValue = heur.Score(board, myId);
                        }
                        else
                        {
                            reward -= .1f;
                        }
                        if (heurValue > 0)
                            reward += heurValue * .1f;
                    }
                }
                reward = Math.Min(reward, 1f);
                reward = Math.Max(reward, -1f);
            }
            else
            {
                done = true;
                if (gameOver == myId)
                    reward = 1f;
                else
                    reward = 0;
                gameOver = -2;
            }
        }
    }
}