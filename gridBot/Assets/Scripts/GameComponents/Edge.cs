using UnityEngine;

public class Edge : MonoBehaviour {

    #region fields and properties
    public IPlayer owner;
    private SpriteRenderer sprite;
    public readonly int x;
    public readonly int y;
    private int _traversals;

    public int Traversals
    {
        get { return _traversals; }
    }
    #endregion

    #region constructors
    public Edge (int x, int y)
    {
        this.x = x;
        this.y = y;
        sprite = gameObject.GetComponent<SpriteRenderer>();

        sprite.color = Color.gray;
        sprite.size = new Vector2(1, 1);
        sprite.transform.parent = this.transform;
        transform.position = new Vector3Int(x,y,0);
    }
    #endregion

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
