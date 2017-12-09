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

        //must return a list of length 
        public override List<float> CollectState()
        {
            var result = new List<float>();
            var colorDict = new Dictionary<ColorStruct, int>();
            if (player.isTurn)
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
                        }
                        if (j != board.height)
                        {
                            var color = board.vertEdges[i, j].playerColor;
                            var lookup = new ColorStruct(color.r, color.g, color.b);
                            if (colorDict.ContainsKey(lookup))
                                result.Add(colorDict[lookup]);
                            else
                                result.Add(-1);
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
                text.text = ((int) action[0]).ToString();
                if (board != null)
                {
                    reward = (float) board.score[board.TryGetPlayerIndex(player.spriteColor)].y /
                             (board.height * (board.width + 1) + (board.height + 1) * board.width);
                }
            }
        }
    }
}