using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private int x;
    private int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public Node cameFromNode;

    public bool accessable;

    public Node(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void SetNodeToFloor() {
        accessable = true;
    }

    public void SetNodeToWall() {
        accessable = false;
    }

    public Node TileToNode(Tile tile) {
        return new Node((int)tile.transform.position.x, (int)tile.transform.position.y);
    }

    public override string ToString() {
        return x + "," + y;
    }
    
    public int GetX() { return x; }
    public int GetY() { return y; }
}
