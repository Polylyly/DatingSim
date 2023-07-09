using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerGen : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Vector3> tileWorldLocations, towerLocations;
    public int maxScrap;
    public GameObject scrapMound;
    public static TowerGen instance;

    [Space]

    public float scanRadius;
    Vector3 closestEnemy;
    public PathBehavior path;

    [Space]
    public int wallThreshold; // the number of times wall towers can be placed before prompting a reroll
    private int wallPlacements = 0;
    public int mineThreshold; // the number of times mines can be placed before prompting a reroll
    private int minePlacements = 0;
    public int arrowThreshold; // the number of times arrow towers can be placed before prompting a reroll
    private int arrowPlacements = 0;
    public int laserThreshold; // the number of times laser towers can be placed before prompting a reroll
    private int laserPlacements = 0;
    public int sniperThreshold; // the number of times arrow towers can be placed before prompting a reroll
    private int sniperPlacements = 0;

    public int rerollChance; // the percentage chance that a placement past its threshold will be rerolled

    [Space]
    public GameObject arrowTower;
    public GameObject laserTower, mineTower, wallTower, sniperTower;

    private void Start()
    {
        Generate();
        instance = this;

        //links the generation to the path being confirmed
        path.onPathConfirmed += () => Generate();
    }

    private void Update()
    {
        // for debug purposes
        if (Input.GetKeyDown("q"))
        {
            Generate();
        }
    }

    //Called by another script (nonexistent) whenever scrap needs to be generated
    public void Generate()
    {
        wallPlacements = 0;
        minePlacements = 0;
        arrowPlacements = 0;
        laserPlacements = 0;
        sniperPlacements = 0;
        // repeats to generate each entity
        int loopCounter = 0;
        for (int i = 0; i < maxScrap; i++)
        {
            loopCounter++;
            //break out of the loop if it becomes infinite
            if(loopCounter > 150)
            {
                break;
            }
            // a list that stores all possible tiles that entities can spawn at, by world position
            tileWorldLocations = new List<Vector3>();

            // initialize tileWorldLocations based on whether the tilemap has a tile at that position
            // spawnable positions are marked by placing invisible tiles
            // every invisible tile is one spawnable location in tileWorldLocations
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    tileWorldLocations.Add(place);
                }
            }

            // ensures that the amount of generations we perform is not more than the maximum available locations and not negative
            maxScrap = Mathf.Clamp(maxScrap, 0, tileWorldLocations.Count);

            // selects a random spawn positoin from the list of potential spawn positions
            int itemIndex = Random.Range(0, tileWorldLocations.Count);

            // finds the distance from the entity to the path
            // by finding the minimum distance between the spawn location and the center of a path square
            
            float distance = -1;

            foreach (Vector3 pos in path.worldPath)
            {
                float newDis = Vector2.Distance(tileWorldLocations[itemIndex], ((Vector2) pos + new Vector2(-0.5f, -0.5f))); //the addition here is a minor adjustment to correct for offset between the center of a tile and of a tower
                if (distance == -1 || newDis < distance)
                {
                    distance = newDis;
                    closestEnemy = pos;
                }
            }

            if (distance == -1) //there is no path found, so make tower placement fully random.
            {
                int chance = Random.Range(0, 5);
                if (chance == 0) { if (PlaceWall(itemIndex)) { i--; continue; } }
                if (chance == 1) { if (PlaceMine(itemIndex)) { i--; continue; } }
                if (chance == 2) { if (PlaceLaser(itemIndex)) { i--; continue; } }
                if (chance == 3) { if (PlaceArrow(itemIndex)) { i--; continue; } }
                if (chance == 4) { if (PlaceSniper(itemIndex)) { i--; continue; } }
            }
            else if (distance <= 0.5) //we are directly on the path
            {
                int chance = Random.Range(0, 100);
                if (chance < 50) { if (PlaceWall(itemIndex)) { i--; continue; } }
                else if (chance < 80) { if (PlaceMine(itemIndex)) { i--; continue; } }
                else { if (PlaceLaser(itemIndex)) { i--; continue; } }
            }
            else if (distance <= 1.5) // we are adjacent to the path
            {
                int chance = Random.Range(0, 100);
                if (chance < 50) { if (PlaceMine(itemIndex)) { i--; continue; } }
                else if (chance < 80) { if (PlaceLaser(itemIndex)) { i--; continue; } }
                else { if (PlaceArrow(itemIndex)) { i--; continue; } }
            }
            else if (distance <= 2.7) // we are roughly two tiles away from the path
            {
                int chance = Random.Range(0, 100);
                if (chance < 60) { if (PlaceArrow(itemIndex)) { i--; continue; } }
                else if (chance < 85) { if (PlaceLaser(itemIndex)) { i--; continue; } }
                else { if (PlaceSniper(itemIndex)) { i--; continue; } }
            }
            else if (distance > 2.7) //more than roughly two tiles away from the path
            {
                int chance = Random.Range(0, 100);
                if (chance < 70) { if (PlaceSniper(itemIndex)) { i--; continue; } }
                else if (chance < 90) { if (PlaceArrow(itemIndex)) { i--; continue; } }
                else { if (PlaceLaser(itemIndex)) { i--; continue; } }
            }

            /** the old AI solution
            if(distance <= wallDistance)
            {
                Instantiate(wallTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            }

            if (distance > wallDistance && distance <= mineDistance)
            {
                int chance = Random.Range(1, 2);
                if(chance == 1) Instantiate(mineTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
                if (chance == 2) Instantiate(laserTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            }

            if (distance > mineDistance && distance <= arrowDistance)
            {
                Instantiate(arrowTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            }

            if (distance > arrowDistance)
            {
                Instantiate(sniperTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            }
            */
        }
    }

    // returns true if a reroll should occur
    // takes in a tileWorldLocations index to place at
    private bool PlaceWall(int itemIndex)
    {
        if (wallPlacements >= wallThreshold && Random.Range(0, 100) < rerollChance)
        {
            return true;
        }
        else
        {
            Instantiate(wallTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            wallPlacements++;
            // removes the placed tile so that it is no longer placeable
            tilemap.SetTile(tilemap.WorldToCell(tileWorldLocations[itemIndex]), null);
            return false;
        }

    }

    private bool PlaceMine(int itemIndex)
    {
        if (minePlacements >= mineThreshold && Random.Range(0, 100) < rerollChance)
        {
            return true;
        }
        else
        {
            Instantiate(mineTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            minePlacements++;
            // removes the placed tile so that it is no longer placeable
            tilemap.SetTile(tilemap.WorldToCell(tileWorldLocations[itemIndex]), null);
            return false;
        }

    }

    private bool PlaceArrow(int itemIndex)
    {
        if (arrowPlacements >= arrowThreshold && Random.Range(0, 100) < rerollChance)
        {
            return true;
        }
        else
        {
            Instantiate(arrowTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            arrowPlacements++;
            // removes the placed tile so that it is no longer placeable
            tilemap.SetTile(tilemap.WorldToCell(tileWorldLocations[itemIndex]), null);
            return false;
        }

    }
    private bool PlaceLaser(int itemIndex)
    {
        if (laserPlacements >= laserThreshold && Random.Range(0, 100) < rerollChance)
        {
            return true;
        }
        else
        {
            Instantiate(laserTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            laserPlacements++;
            // removes the placed tile so that it is no longer placeable
            tilemap.SetTile(tilemap.WorldToCell(tileWorldLocations[itemIndex]), null);
            return false;
        }

    }
    private bool PlaceSniper(int itemIndex)
    {
        if (sniperPlacements >= sniperThreshold && Random.Range(0, 100) < rerollChance)
        {
            return true;
        }
        else
        {
            Instantiate(sniperTower, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);
            sniperPlacements++;
            // removes the placed tile so that it is no longer placeable
            tilemap.SetTile(tilemap.WorldToCell(tileWorldLocations[itemIndex]), null);
            return false;
        }

    }

    
}
