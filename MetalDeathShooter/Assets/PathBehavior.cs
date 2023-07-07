using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathBehavior : MonoBehaviour
{
    public Tilemap map;
    public Tile curTile;
    public Tile pathTile;
    public Tile normalTile;
    public Vector3Int startPos;
    public Vector2Int topLeftBound;
    public Vector2Int bottomRightBound;
    private Vector3Int curPos;

    //the data structure that will store the path on a tile basis
    public List<Vector3Int> tilePath = new List<Vector3Int>();
    //the data structure that will store the path on a world basis
    public List<Vector3> worldPath = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        curPos = startPos;
        map.SetTile(startPos, curTile);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            moveCurTile(new Vector3Int(0, 1, 0));
        }
        if (Input.GetKeyDown("a"))
        {
            moveCurTile(new Vector3Int(-1, 0, 0));
        }
        if (Input.GetKeyDown("s"))
        {
            moveCurTile(new Vector3Int(0, -1, 0));
        }
        if (Input.GetKeyDown("d"))
        {
            moveCurTile(new Vector3Int(1, 0, 0));
        }
    }

    private void moveCurTile(Vector3Int amount)
    {
        if(tilePath.Count >= 1 && tilePath[tilePath.Count - 1] == curPos + amount)
        {
            tilePath.RemoveAt(tilePath.Count - 1);
            worldPath.RemoveAt(worldPath.Count - 1);
            map.SetTile(curPos, normalTile);
            curPos += amount;
            map.SetTile(curPos, curTile);
            return;
        }
        if (map.GetTile(curPos + amount).Equals(pathTile)) return;
        if ((curPos + amount).x < topLeftBound.x || (curPos + amount).x > bottomRightBound.x || (curPos + amount).y > topLeftBound.y || (curPos + amount).y < bottomRightBound.y) return;
            map.SetTile(curPos, pathTile);
        tilePath.Add(curPos);
        worldPath.Add(map.GetCellCenterWorld(curPos));
        curPos += amount;
        map.SetTile(curPos, curTile);
    }
}
