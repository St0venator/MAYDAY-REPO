using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStarGrid : MonoBehaviour
{
    public LayerMask unWalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    aStarNode[,] grid;
    //aStarNode[,,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    //int gridSizeZ;

    void Start()
    {
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        //gridSizeZ = Mathf.RoundToInt(gridWorldSize.z/nodeDiameter);
        CreatGrid();
    }

    void CreatGrid()
    {
        grid = new aStarNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask));
                    grid[x,y] = new aStarNode(walkable, worldPoint);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));

        if (grid != null)
        {
            foreach (aStarNode n in grid)
            {
                Gizmos.color = (n.walkable)?Color.white: Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-0.1f));
            }
        }
    }
}
