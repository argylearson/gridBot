using UnityEngine;

public abstract class Player : MonoBehaviour {
    public int x;
    public int y;
    public abstract Color color { get; }
    public abstract Move MakeMove(Board board, float timeLimit);
}
