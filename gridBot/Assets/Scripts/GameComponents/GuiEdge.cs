using UnityEngine;

public class GuiEdge : MonoBehaviour
{
    public Edge edge;
    private SpriteRenderer sprite;
    public readonly int x;
    public readonly int y;

    private void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        sprite.color = Color.gray;
        sprite.size = new Vector2(1, 1);
        sprite.transform.parent = this.transform;
        transform.position = new Vector3(x, y, 0);
    }
}
