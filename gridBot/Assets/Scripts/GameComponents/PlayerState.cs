using UnityEngine;

/// <summary>
/// used to abstract away the board state of a player,
/// allowing the gameRunner to own the the x/y value
/// of the player, and maintain their GUI state.
/// </summary>
public class PlayerState{

    public Player player;
    public int x;
    public int y;
    public float startTime;
    public Vector3 startPosition;
    public Vector3 endPosition;
}
