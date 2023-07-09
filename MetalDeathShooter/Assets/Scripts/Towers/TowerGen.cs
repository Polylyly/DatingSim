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
    public float arrowDistance;
    public float mineDistance, wallDistance;

    [Space]
    public GameObject arrowTower;
    public GameObject laserTower, mineTower, wallTower, sniperTower;

    private void Start()
    {
        instance = this;
        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Generate();
        }
    }

    //Called by another script (nonexistent) whenever scrap needs to be generated
    public void Generate()
    {
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

        // repeats to generate each entity
        for (int i = 0; i < maxScrap; i++)
        {
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

            Debug.Log(distance);
            // towerLocations.Add(tileWorldLocations[itemIndex] + new Vector3(0.5f, 0.5f, 0));
            tilemap.SetTile(tilemap.WorldToCell(tileWorldLocations[itemIndex]), null);

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
        }
    }
}
