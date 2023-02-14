using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button drawButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GridManager.instance.drawingWall) {
            drawButton.image.color = new Color(0.7f,0.7f,0.7f);
        }
        else {
            drawButton.image.color = new Color(1, 1, 1);
        }
    }

    public void DrawingWalls() {
        GridManager.instance.drawingWall = !GridManager.instance.drawingWall;
    }

    public void CleanAllWalls() {
        foreach (Tile tile in GridManager.instance.GetWallsList()) {
            tile.ResetTileFromPath();
        }
        GridManager.instance.ResetPathTile(PathFindingManager.instance.shortestPath);
        PathFindingManager.instance.startSet = false;
    }

    public void ResetPathSelection() {
        if (PathFindingManager.instance.pathShown) {
            GridManager.instance.ResetPathTile(PathFindingManager.instance.shortestPath);
            PathFindingManager.instance.startSet = false;
            PathFindingManager.instance.endSet = false;
        }
            
    }

    public void QuitApplication() {
        Application.Quit();
    }
}
