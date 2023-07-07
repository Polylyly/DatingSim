using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public PathBehavior path;
    bool started = false;
    bool pathCompleted = false;
    public Rigidbody2D rb;
    public float speed;
    public float nextWaypointDistance;
    private int currentWaypoint;
    List<Vector3> p;
    // Start is called before the first frame update
    void Start()
    {
         p = path.worldPath;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
        }
        if (started)
        {
            //check if the path is completed
            if (currentWaypoint == p.Count - 1)
            {
                pathCompleted = true;
                return;
            }
            else
            {
                pathCompleted = false;
            }
            //find the direction to move in
            Vector2 direction = ((Vector2)p[currentWaypoint + 1] - rb.position).normalized;
            //apply a force in that direction
            rb.AddForce(speed * direction);

            //check if we've reached the next waypoint
            if (Vector2.Distance(rb.position, (Vector2)p[currentWaypoint + 1]) < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }
}
