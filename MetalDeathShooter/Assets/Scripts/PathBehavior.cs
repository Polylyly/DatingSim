using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathBehavior : MonoBehaviour
{
    // ref to the tilemap
    public Tilemap map;
    // the tiles that will be used for the very end of the path
    public Tile curTileUp;
    public Tile curTileDown;
    public Tile curTileLeft;
    public Tile curTileRight;
    // the tiles used for the trailing parts of the path
    public Tile pathTileHor;
    public Tile pathTileVer;
    public Tile cornerTileLeftDown;
    public Tile cornerTileLeftUp;
    public Tile cornerTileRightDown;
    public Tile cornerTileRightUp;

    // the tile we replace path tiles with when removing them
    public Tile normalTile;

    //where the path starts
    public Vector3Int startPos;
    //where the path must end
    public Vector3Int endPos;
    // the boundaries of the path
    public Vector2Int topLeftBound;
    public Vector2Int bottomRightBound;
    // variable that will store the tile position of the very end of the path
    private Vector3Int curPos;

    //the data structure that will store the path on a tile basis
    public List<Vector3Int> tilePath = new List<Vector3Int>();
    //the data structure that will store the path on a world basis
    public List<Vector3> worldPath = new List<Vector3>();

    //boolean variable that stores whether the path has been confirmed (uneditable)
    bool confirmed = false;

    //event that will be emitted when the path is confirmed

    public Action onPathConfirmed;


    //used to prevent holding keys from being uncontrollable
    private float holdTime = 0.2f;
    private float holdTimer;

    void Start()
    {
        // set things up
        ResetPath();
    }


    void Update()
    {
        holdTimer -= Time.deltaTime;
        if (Input.anyKeyDown) holdTimer = 0f; //no hold delay if we're tapping
        // yes this is a bad way to do this but i dont care that much
        if (!confirmed)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 && holdTimer <= 0f)
            {
                moveCurTile(new Vector3Int((int)Input.GetAxisRaw("Horizontal"), 0, 0));
                holdTimer = holdTime;
            }
            if (Input.GetAxisRaw("Vertical") != 0 && holdTimer <= 0f)
            {
                moveCurTile(new Vector3Int(0, (int) Input.GetAxisRaw("Vertical"), 0));
                holdTimer = holdTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!confirmed)
            {
                // if the path has not reached the end pos, do not confirm it
                if (curPos != endPos) return;
                ConfirmPath();
            }
            else
            {
                ResetPath();
            }
        }
    }

    public void ConfirmPath()
    {
        // set the end of the path to a regular path tile to indicate it can no longer be moved
        map.SetTile(curPos, FindTile(new Vector3Int(1, 0, 0)));

        //add the three ending tiles
        tilePath.Add(curPos);
        worldPath.Add(map.GetCellCenterWorld(curPos));
        Vector3Int nextPos = curPos + new Vector3Int(1, 0, 0);
        tilePath.Add(nextPos);
        worldPath.Add(map.GetCellCenterWorld(nextPos));
        nextPos += new Vector3Int(1, 0, 0);
        tilePath.Add(nextPos);
        worldPath.Add(map.GetCellCenterWorld(nextPos));

        confirmed = true;
        onPathConfirmed();
    }
    // this function moves the very end of the path
    // amount is a vector3 used to pass a direction to move in
    private void moveCurTile(Vector3Int amount)
    {
        // checks if we are moving backwards along the path
        // if so, we can erase the path
        if(tilePath.Count >= 1 && tilePath[tilePath.Count - 1] == curPos + amount)
        {
            Vector3Int prevTile;
            if (tilePath.Count < 2)
            {
                prevTile = startPos + new Vector3Int(-1, 0, 0);
            }
            else
            {
                prevTile = tilePath[tilePath.Count - 2];
            }
            Vector3Int amounty = tilePath[tilePath.Count - 1] - prevTile;
            // remove the last item from the list of tile pos
            tilePath.RemoveAt(tilePath.Count - 1);
            // remove the last item from the list of world pos
            worldPath.RemoveAt(worldPath.Count - 1);
            // change the tile we used to be at to a regular tile
            map.SetTile(curPos, null);
            // move the very end of the path
            curPos += amount;
            // set the new very end of the path to the end of path tile
            map.SetTile(curPos, FindCurTile(amounty));
            return;
        }
        // make sure you can't path over your own path
        if (map.GetTile(curPos + amount) != null) return;
        // check that you cant path outside the bounds
        if ((curPos + amount).x < topLeftBound.x || (curPos + amount).x > bottomRightBound.x || (curPos + amount).y > topLeftBound.y || (curPos + amount).y < bottomRightBound.y) return;
        
        //now the code to just regularly move the tile

        //set the current end of the path to a trailing path tile
        map.SetTile(curPos, FindTile(amount));
        // add the current end of the path to both lists
        tilePath.Add(curPos);
        worldPath.Add(map.GetCellCenterWorld(curPos)); //getcellcenterworld is used to get a world position
        // move the current position
        curPos += amount;
        // add in a new tile for the very end of the tile
        map.SetTile(curPos, FindCurTile(amount));
    }

    //returns the tile that, when placed at curPos, will properly adjoin the most recently placed tile to curpos+the given vector3Int
    public Tile FindTile(Vector3Int toNextTile)
    {
        Vector3Int nextTile = curPos + toNextTile;
        Vector3Int prevTile;
        if (tilePath.Count == 0)
        {
            prevTile = startPos + new Vector3Int(-1, 0, 0);
        }
        else
        {
            prevTile = tilePath[tilePath.Count - 1];
        }
        // check for straightaways
        if (prevTile.x == nextTile.x)
        {
            return pathTileVer;
        }
        if (prevTile.y == nextTile.y)
        {
            return pathTileHor;
        }
        // check for corners
        if(curPos.x > prevTile.x)
        {
            if(nextTile.y > curPos.y)
            {
                return cornerTileLeftUp;
            }
            else
            {
                return cornerTileLeftDown;
            }
        }else if(curPos.x < prevTile.x)
        {
            if (nextTile.y > curPos.y)
            {
                return cornerTileRightUp;
            }
            else
            {
                return cornerTileRightDown;
            }
        }
        else if(curPos.y > prevTile.y)
        {
            if (nextTile.x > curPos.x)
            {
                return cornerTileRightDown;
            }
            else
            {
                return cornerTileLeftDown;
            }
        }
        else
        {
            if (nextTile.x > curPos.x)
            {
                return cornerTileRightUp;
            }
            else
            {
                return cornerTileLeftUp;
            }
        }
    }

    // based on the amount moved, gives a current tile to use
    private Tile FindCurTile(Vector3Int amount)
    {
        if(amount.x == 1)
        {
            return curTileRight;
        }else if(amount.x == -1)
        {
            return curTileLeft;
        }else if(amount.y == 1)
        {
            return curTileUp;
        }
        else
        {
            return curTileDown;
        }
    }
    public void ResetPath()
    {
        //reset the tilemap visuals
        map.ClearAllTiles();
        map.SetTile(startPos, curTileRight);
        map.SetTile(startPos + new Vector3Int(-1, 0, 0), pathTileHor);
        map.SetTile(startPos + new Vector3Int(-2, 0, 0), pathTileHor);
        map.SetTile(endPos + new Vector3Int(1, 0, 0), pathTileHor);

        //add the starting tiles
        tilePath.Add(startPos + new Vector3Int(-2, 0, 0));
        worldPath.Add(map.GetCellCenterWorld(startPos + new Vector3Int(-2, 0, 0)));
        tilePath.Add(startPos + new Vector3Int(-1, 0, 0));
        worldPath.Add(map.GetCellCenterWorld(startPos + new Vector3Int(-1, 0, 0)));
        curPos = startPos;
        
        
        confirmed = false;
    }
}
