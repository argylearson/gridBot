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

        //must return a list of length 
        public override List<float> CollectState()
        {
            var result = new List<float>();
            var colorDict = new Dictionary<ColorStruct, int>();
            if (player.isTurn && board != null)
            {
                result.Add(board.TryGetPlayerIndex(player.spriteColor));
                result.Add(board.playerPositions.Length);
                for (int i = 0; i < board.playerPositions.Length; i++)
                {
                    result.Add(board.playerPositions[i].x);
                    result.Add(board.playerPositions[i].y);
                    var color = board.score[i].x;
                    var key = new ColorStruct(color.r, color.g, color.b);
                    colorDict.Add(key, i);
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
                                result.Add(colorDict[lookup]);
                            else
                                result.Add(-1);
                            result.Add(board.horzEdges[i, j].traversals);
                        }
                        if (j != board.height)
                        {
                            var color = board.vertEdges[i, j].playerColor;
                            var lookup = new ColorStruct(color.r, color.g, color.b);
                            if (colorDict.ContainsKey(lookup))
                                result.Add(colorDict[lookup]);
                            else
                                result.Add(-1);
                            result.Add(board.vertEdges[i, j].traversals);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < brain.brainParameters.stateSize; i++)
                {
                    result.Add(.1f);
                }
            }
            return result;
        }

        public override void AgentReset()
        {
            direction = null;
            board = null;
        }

        public override void AgentStep(float[] action)
        {
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
                        var move = new Move()
                        {
                            direction = direction.Value,
                            playerColor = player.spriteColor,
                            x = board.playerPositions[myId].x,
                            y = board.playerPositions[myId].y
                        };
                        if (board.IsMoveLegal(move))
                        {
                            board.MakeMove(move);
                            reward = (float) board.score[myId].y /
                                     (board.height * (board.width + 1) + (board.height + 1) * board.width);
                        }
                        else
                        {
                            reward = -1f;
                        }
                    }
                    if ((int) action[0] > -1)
                    {
                        text.text = ((int) action[0]).ToString();
                        text.text += ": " + reward;
                    }
                }
            }
            else
            {
                done = true;
                if (gameOver == myId)
                    reward = 1f;
                else if (gameOver > -1)
                    reward = -1f;
                else
                    reward = 0;
                text.text = "Reward for game end: " + reward;
                gameOver = -2;
            }
        }
    }
}