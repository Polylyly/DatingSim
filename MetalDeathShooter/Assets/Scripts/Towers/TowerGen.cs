using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerGen : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Vector3> tileWorldLocations;
    public int maxScrap;
    public GameObject scrapMound;
    public static TowerGen instance;

    [Space]
    private float distance = -1;
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

    //Called by another script (nonexistent) whenever scrap needs to be generated
    public void Generate()
    {
        tileWorldLocations = new List<Vector3>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }

        maxScrap = Mathf.Clamp(maxScrap, 0, tileWorldLocations.Count);
        Random.seed = System.DateTime.Now.Millisecond;
        for (int i = 0; i < maxScrap; i++)
        {
            int itemIndex = Random.Range(0, tileWorldLocations.Count);
            Instantiate(scrapMound, new Vector3(tileWorldLocations[itemIndex].x + 0.5f, tileWorldLocations[itemIndex].y + 0.5f, tileWorldLocations[itemIndex].z), Quaternion.identity);

            float newDis = -2;
            foreach (Vector3 pos in path.worldPath)
            {
                newDis = Vector2.Distance(tileWorldLocations[i], pos);
                if (distance == -1 || newDis < distance)
                {
                    distance = newDis;
                    closestEnemy = pos;
                }
            }

            if(newDis <= wallDistance)
            {
                Instantiate(wallTower, new Vector3(tileWorldLocations[i].x + 0.5f, tileWorldLocations[i].y + 0.5f, tileWorldLocations[i].z), Quaternion.identity);
            }

            if (newDis > wallDistance && newDis <= mineDistance)
            {
                int chance = Random.Range(1, 2);
                if(chance == 1) Instantiate(mineTower, new Vector3(tileWorldLocations[i].x + 0.5f, tileWorldLocations[i].y + 0.5f, tileWorldLocations[i].z), Quaternion.identity);
                if (chance == 2) Instantiate(laserTower, new Vector3(tileWorldLocations[i].x + 0.5f, tileWorldLocations[i].y + 0.5f, tileWorldLocations[i].z), Quaternion.identity);
            }

            if (newDis > mineDistance && newDis <= arrowDistance)
            {
                Instantiate(arrowTower, new Vector3(tileWorldLocations[i].x + 0.5f, tileWorldLocations[i].y + 0.5f, tileWorldLocations[i].z), Quaternion.identity);
            }

            if (newDis > arrowDistance)
            {
                Instantiate(sniperTower, new Vector3(tileWorldLocations[i].x + 0.5f, tileWorldLocations[i].y + 0.5f, tileWorldLocations[i].z), Quaternion.identity);
            }
        }
    }
}
