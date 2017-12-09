using System.Collections.Generic;
using UnityEngine;
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

        //must return a list of length 
        public override List<float> CollectState()
        {
            var result = new List<float>();
            var colorDict = new Dictionary<ColorStruct, int>();
            if (board != null)
            {
                for (int i = 0; i < board.playerPositions.Length; i++)
                {
                    result.Add(board.playerPositions[i].x);
                    result.Add(board.playerPositions[i].y);
                    var color = board.score[i].x;
                    var key = new ColorStruct(color.r, color.g, color.b);
                    colorDict.Add(key, board.score[i].y);
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
                            var color = board.vertEdges[i, j].playerColor; var lookup = new ColorStruct(color.r, color.g, color.b);
                            if (colorDict.ContainsKey(lookup))
                                result.Add(colorDict[lookup]);
                            else
                                result.Add(-1);
                        }
                    }
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
            switch ((int)action[0])
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
            }
            board = null;
        }
    }
}