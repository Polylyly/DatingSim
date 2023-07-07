using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
// almost all of this code is ripped from an old pathfinding script i have
// it follows a path defined by a list of vector3s
/// </summary>
public class PathFollow : MonoBehaviour
{
    // this is a reference to the pathBehavior script that we will be getting path data from
    public PathBehavior path;
    // dumb little boolean flag so i can start the agent by pressing space
    bool started = false;
    // boolean flag so we stop pathfinding at the end of the path
    bool pathCompleted = false;
    public Rigidbody2D rb;
    // speed defines how fast the agent follows the path
    public float speed;
    // when the agent is within this distance of the next point on the path, it stops pathfinding to that point and starts going to the next one
    public float nextWaypointDistance;
    // used to track which position in the list we are pathfinding to
    private int currentWaypoint;
    // will store path data
    List<Vector3> p;

    void Start()
    {
        // load path data from the pathmanager script
         p = path.worldPath;
    }

    // Update is called once per frame
    void Update()
    {
        // dumb little boolean flag so i can start the agent with space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
        }
        if (started)
        {
            //check if the path is completed
            //if so, don't run the pathing code beneath this
            if (currentWaypoint == p.Count - 1)
            {
                pathCompleted = true;
                return;
            }
            else
            {
                pathCompleted = false;
            }
            //find the direction to move in using some gross vector math
            Vector2 direction = ((Vector2)p[currentWaypoint + 1] - rb.position).normalized;
            //apply a force in that direction
            rb.AddForce(speed * direction * Time.deltaTime);

            //check if we've reached the next waypoint
            if (Vector2.Distance(rb.position, (Vector2)p[currentWaypoint + 1]) < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }
}