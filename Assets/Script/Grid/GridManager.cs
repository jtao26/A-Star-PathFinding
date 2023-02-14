using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int width, height;
    [SerializeField]
    private Tile tilePrefab;
    [SerializeField]
    private Camera cam;

    private Dictionary<Vector2, Tile> grid;
    private List<Tile> walls;

    public bool drawingWall;

    public static GridManager instance;

    void Awake() {
        instance = this;
        walls = new List<Tile>();
        drawingWall = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateTilesMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTilesMap() {
        grid = new Dictionary<Vector2, Tile>();
        for(int i=0; i<width; i++) {
            for(int j=0; j<height; j++) {
                var generatedTile = Instantiate(tilePrefab, new Vector3(i,j), Quaternion.identity);
                generatedTile.name = "Tile(" + i + "," + j + ")";

                bool isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                tilePrefab.SetColor(isOffset);

                grid[new Vector2(i,j)] = generatedTile;

            }
        }
        PathFindingManager.instance.SetNodeMap(grid);
        cam.transform.position = new Vector3(cam.transform.position.x + (float)width / 2 -0.5f, cam.transform.position.y + (float)height / 2 - 0.5f, cam.transform.position.z);
    }

    public Tile GetTileFromPos(Vector2 pos) {
        return grid[pos];
    }

    public void SetTileToPath(Tile t) {
        t.SetTileToPath();
    }

    public void ResetPathTile(List<Node> path) {
        for (int i = 0; i < path.Count; i++) {
            grid[new Vector2(path[i].GetX(), path[i].GetY())].ResetTileFromPath();
        }
    }

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public List<Tile> GetWallsList() { return walls; }

    public void AddTileToWalls(Tile tile) {
        walls.Add(tile);
    }
}
