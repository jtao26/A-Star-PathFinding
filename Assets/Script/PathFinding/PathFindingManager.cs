using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingManager : MonoBehaviour
{
    public static PathFindingManager instance;
    
    public Pathfinding pathFinder;

    private Node[,] nodeMap;

    private Vector2 startPos, endPos;
    private int width, height;
    public bool startSet, endSet, pathShown;

    public List<Node> shortestPath;
    
    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(pathShown) {
                GridManager.instance.ResetTileGrid();
                startSet = false;
                endSet = false;
                pathShown = false;
            }
            else if(!pathShown && startSet && endSet){
                shortestPath = pathFinder.FindPath(nodeMap, nodeMap[(int)startPos.x, (int)startPos.y], nodeMap[(int)endPos.x, (int)endPos.y]);
                if (shortestPath != null) {
                    for (int i = 1; i < shortestPath.Count - 1; i++) {
                        // Debug.Log(node.ToString());
                        Debug.DrawLine(new Vector3(shortestPath[i].GetX(), shortestPath[i].GetY(),-1),
                            new Vector3(shortestPath[i + 1].GetX(), shortestPath[i + 1].GetY(),-1), new Color(0, 1, 0));
                        GridManager.instance.SetTileToPath(GridManager.instance.GetTileFromPos(new Vector2(shortestPath[i].GetX(), shortestPath[i].GetY())));
                    }
                    pathShown = true;
                }
            }
        }
        */
        if (startSet && endSet) {
            shortestPath = pathFinder.FindPath(nodeMap, nodeMap[(int)startPos.x, (int)startPos.y], nodeMap[(int)endPos.x, (int)endPos.y]);
            if (shortestPath != null) {
                Debug.Log("Path find length" + shortestPath.Count);
                for (int i = 0; i < shortestPath.Count; i++) {
                    // Debug.Log(node.ToString());
                    //Debug.DrawLine(new Vector3(shortestPath[i].GetX(), shortestPath[i].GetY(), -1),
                    //    new Vector3(shortestPath[i + 1].GetX(), shortestPath[i + 1].GetY(), -1), new Color(0, 1, 0));
                    GridManager.instance.SetTileToPath(GridManager.instance.GetTileFromPos(new Vector2(shortestPath[i].GetX(), shortestPath[i].GetY())));
                }
                pathShown = true;
            }
            else {
                pathShown = false;
            }
        }

    }

    public void SetStartPos(Vector2 startPos) {
        this.startPos = startPos;
        startSet = true;
    }

    public void SetEndPos(Vector2 endPos) {
        this.endPos = endPos;
        endSet = true;
    }

    public Vector2 GetStartPos() {
        return startPos;
    }

    public Vector2 GetEndPos() { 
        return endPos;
    }

    public void SetNodeMap(Dictionary<Vector2, Tile> map) {
        width = GridManager.instance.GetWidth();
        height = GridManager.instance.GetHeight();
        nodeMap = new Node[width, height];

        for(int i=0; i<width; i++) {
            for(int j=0; j<height; j++) {
                nodeMap[i,j] = new Node(i,j);
                if (GridManager.instance.GetTileFromPos(new Vector2(i, j)).isWall) {
                    nodeMap[i, j].accessable = false;
                }
                else {
                    nodeMap[i, j].accessable = true;
                }
            }
        }
    }

    public void SetNodeToWall(Vector2 pos) {
        nodeMap[(int)pos.x, (int)pos.y].accessable = false;
    }
}
