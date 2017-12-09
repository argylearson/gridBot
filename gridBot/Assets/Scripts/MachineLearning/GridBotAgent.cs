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

        [SerializeField]
        private int solved;

        //must return a list of length 
        public override List<float> CollectState()
        {
            List<float> state = new List<float>();
            state.Add(currentNumber);
            state.Add(targetNumber);
            return state;
        }

        public override void AgentReset()
        {
            direction = null;
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
            if (/*we lost*/true)
            {
                reward = -1f;
                done = true;
            }
            
            else if (/*we won*/true)
            {
                solved++;
                reward = 1f;
                done = true;
            }

        }
    }
}