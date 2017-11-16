using UnityEngine;

public abstract class Player : MonoBehaviour {
    public int x;
    public int y;
    public Color spriteColor;
    public void UpdateColor(Color color)
    {
        this.GetComponent<SpriteRenderer>().color = color;
        this.spriteColor = color;
    }
    public abstract Move MakeMove(Board board, float timeLimit);
}
