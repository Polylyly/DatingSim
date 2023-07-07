using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathBehavior : MonoBehaviour
{
    // ref to the tilemap
    public Tilemap map;
    // the tile that will be used for the very end of the path
    public Tile curTile;
    // the tile used for the trailing parts of the path
    public Tile pathTile;
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

    public Action OnPathConfirmed;


    //used to prevent holding keys from being uncontrollable
    private float holdTime = 0.2f;
    private float holdTimer;

    void Start()
    {
        // set things up
        curPos = startPos;
        map.SetTile(startPos, curTile);
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
                // set the end of the path to a regular path tile to indicate it can no longer be moved
                map.SetTile(curPos, pathTile);
                confirmed = true;
                OnPathConfirmed();
            }
            else
            {
                ResetPath();
            }
        }
    }

    // this function moves the very end of the path
    // amount is a vector3 used to pass a direction to move in
    private void moveCurTile(Vector3Int amount)
    {
        // checks if we are moving backwards along the path
        // if so, we can erase the path
        if(tilePath.Count >= 1 && tilePath[tilePath.Count - 1] == curPos + amount)
        {
            // remove the last item from the list of tile pos
            tilePath.RemoveAt(tilePath.Count - 1);
            // remove the last item from the list of world pos
            worldPath.RemoveAt(worldPath.Count - 1);
            // change the tile we used to be at to a regular tile
            map.SetTile(curPos, normalTile);
            // move the very end of the path
            curPos += amount;
            // set the new very end of the path to the end of path tile
            map.SetTile(curPos, curTile);
            return;
        }
        // make sure you can't path over your own path
        if (map.GetTile(curPos + amount).Equals(pathTile)) return;
        // check that you cant path outside the bounds
        if ((curPos + amount).x < topLeftBound.x || (curPos + amount).x > bottomRightBound.x || (curPos + amount).y > topLeftBound.y || (curPos + amount).y < bottomRightBound.y) return;
        
        //now the code to just regularly move the tile

        //set the current end of the path to a trailing path tile
        map.SetTile(curPos, pathTile);
        // add the current end of the path to both lists
        tilePath.Add(curPos);
        worldPath.Add(map.GetCellCenterWorld(curPos)); //getcellcenterworld is used to get a world position
        // move the current position
        curPos += amount;
        // add in a new tile for the very end of the tile
        map.SetTile(curPos, curTile);
    }

    public void ResetPath()
    {
        map.FloodFill(startPos, normalTile);
        curPos = startPos;
        map.SetTile(startPos, curTile);
        confirmed = false;
    }
}
