using UnityEngine;

public abstract class IPlayer : MonoBehaviour{
    public abstract Color color { get; }
    public abstract Move MakeMove(Board board, float timeLimit);
}
