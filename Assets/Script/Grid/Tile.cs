using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 tilePos;
    [SerializeField]
    private Color baseColor, offsetColor, selectColor, wallColor;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject highLight;
    public bool isOffset, isWall, isPath;

    // Start is called before the first frame update
    void Start()
    {
        tilePos = new Vector2(transform.position.x, transform.position.y);
        isWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(isWall) {
            spriteRenderer.color = wallColor;
        }
        else if(isPath){
            spriteRenderer.color = selectColor;
        }
        else {
            SetColor(isOffset);
        }
        */
    }

    public void SetColor(bool offset) {
        spriteRenderer.color = offset ? offsetColor : baseColor;
        isOffset = offset;
    }

    public Vector2 GetTilePos() {
        return tilePos;
    }

    public void SetTileToPath() {
        spriteRenderer.color = selectColor;
    }

    public void ResetTileFromPath() {
        SetColor(isOffset);
        isPath = false;
    }

    public void SetTileToWall() {
        isWall = true;
        spriteRenderer.color = wallColor;
        PathFindingManager.instance.SetNodeToWall(tilePos);
        GridManager.instance.AddTileToWalls(this);
    }

    private void OnMouseEnter() {
        if (!GridManager.instance.drawingWall && !isWall) {
            highLight.SetActive(true);
        }
        
        if (GridManager.instance.drawingWall && Input.GetMouseButton(0)) {
            Debug.Log("Mouse Enter, drawing Wall true");
            Debug.Log("mousedown0");
            SetTileToWall();
        }
        else {
            if (PathFindingManager.instance.startSet) {
                if (PathFindingManager.instance.GetStartPos().x == tilePos.x
                    && PathFindingManager.instance.GetStartPos().y == tilePos.y) {
                    PathFindingManager.instance.endSet = false;
                }
                else if(!isWall) {
                    CancelEndPos();
                    if (PathFindingManager.instance.shortestPath != null) {
                        GridManager.instance.ResetPathTile(PathFindingManager.instance.shortestPath);
                    }
                    SetEndPos();
                    
                }
            }
        }
    }

    private void OnMouseExit() {
        if (!isWall) {
            highLight.SetActive(false);
        }
    }

    private void OnMouseDown() {
        if (!isWall && !GridManager.instance.drawingWall) {
            if(PathFindingManager.instance.pathShown)
                GridManager.instance.ResetPathTile(PathFindingManager.instance.shortestPath);
            SetStartPos();
        }
        else if (GridManager.instance.drawingWall) {
            SetTileToWall();
        }
        
        /*
        if(spriteRenderer.color != selectColor) {
            if (!PathFindingManager.instance.startSet) {
                SetStartPos();
            }
            else if (!PathFindingManager.instance.endSet) {
                SetEndPos();
            }
            else {
                // select a new tile when start and end has marked, reset both start and end
                CancelStartPos();
                CancelEndPos();
                SetStartPos();
            }

        }
        else {
            if(PathFindingManager.instance.startSet && !PathFindingManager.instance.endSet) {
                // reset start pos
                CancelStartPos();
            }
            else {
                // reset end pos
                CancelStartPos();
                CancelEndPos();
                SetStartPos();
            }
        }
        */
    }
    private void SetStartPos() {
        PathFindingManager.instance.SetStartPos(new Vector2(transform.position.x, transform.position.y));
        isPath = true;
        SetTileToPath();
        //Debug.Log("Set start tile at location " + transform.position.x + " " + transform.position.y + "and its offset bool value: " + isOffset);
        PathFindingManager.instance.startSet = true;
    }

    private void SetEndPos() {
        PathFindingManager.instance.SetEndPos(new Vector2(transform.position.x, transform.position.y));
        isPath = true;
        SetTileToPath();
        //Debug.Log("Set end tile at location " + transform.position.x + " " + transform.position.y + "and its offset bool value: " + isOffset);
        PathFindingManager.instance.endSet = true;
    }

    private void CancelStartPos() {
        Tile startTile = GridManager.instance.GetTileFromPos(PathFindingManager.instance.GetStartPos());
        //Debug.Log("Restore start tile at location " + startTile.transform.position.x + " " + startTile.transform.position.y + "and its offset bool value: " + startTile.isOffset);
        startTile.spriteRenderer.color = startTile.isOffset ? startTile.offsetColor : startTile.baseColor;
        PathFindingManager.instance.startSet = false;
    }

    private void CancelEndPos() {
        Tile endTile = GridManager.instance.GetTileFromPos(PathFindingManager.instance.GetEndPos());
        //Debug.Log("Restore end tile at location " + endTile.transform.position.x + " " + endTile.transform.position.y + "and its offset bool value: " + endTile.isOffset);
        endTile.spriteRenderer.color = endTile.isOffset ? endTile.offsetColor : endTile.baseColor;
        PathFindingManager.instance.endSet = false;
    }
}
