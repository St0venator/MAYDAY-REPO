using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector3 gridWorldSize;
	public float nodeRadius;
	public TerrianType[] walkableRegions;
	public int obstacleProximityPenalty = 10;
	LayerMask walkableMask;
	Dictionary<int, int> walkableRegionDictionary = new Dictionary<int, int>();
	
	Node[,,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY, gridSizeZ;
	
	int penaltyMin = int.MaxValue;
	int penaltyMax = int.MinValue;

	void Awake() 
	{
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		gridSizeZ = Mathf.RoundToInt(gridWorldSize.z/nodeDiameter);

		foreach(TerrianType region in walkableRegions)
		{
			walkableMask.value |= region.terrainMask.value;
			walkableRegionDictionary.Add((int)Mathf.Log(region.terrainMask.value, 1), region.terrainPenatly);
		}

		CreateGrid();
	}

	public int MaxSize
	{
		get
		{
			return gridSizeX * gridSizeY * gridSizeZ;
		}
	}

	void CreateGrid() 
	{
		grid = new Node[gridSizeX,gridSizeY, gridSizeZ];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2 - Vector3.up * gridWorldSize.z/2;

		for (int x = 0; x < gridSizeX; x ++) 
		{
			for (int y = 0; y < gridSizeY; y ++) 
			{
				for (int z = 0; z < gridSizeZ; z ++)
				{
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius) + Vector3.up * (z * nodeDiameter + nodeRadius);
					bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));

					int movementPenalty = 0;

					
					Ray ray = new Ray(worldPoint + Vector3.left * 50, Vector3.right);
					RaycastHit hit;
					if(Physics.Raycast(ray, out hit, 100, walkableMask))
					{
						walkableRegionDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
					}

					if(!walkable)
					{
						movementPenalty += obstacleProximityPenalty;
					}
					

					grid[x,y,z] = new Node(walkable,worldPoint, x,y,z, movementPenalty);
				}
			}
		}
		//BlurPenaltyMap(3);
	}

	/*void BlurPenaltyMap(int blurSize)
	{
		int kernelSize = blurSize * 2 + 1;
		int kernelExtents = (kernelSize - 1) / 2;

		int[,] penaltiesHorizontalPass = new int[gridSizeX,gridSizeY];
		int[,] penaltiesVerticalPass = new int[gridSizeX,gridSizeY];

		for (int y = 0; y < gridSizeY; y++) 
		{
			for (int x = -kernelExtents; x <= kernelExtents; x++) 
			{
				int sampleX = Mathf.Clamp (x, 0, kernelExtents);
				penaltiesHorizontalPass [0, y] += grid [sampleX, y].movementPenalty;
			}

			for (int x = 1; x < gridSizeX; x++) 
			{
				int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
				int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX-1);

				penaltiesHorizontalPass [x, y] = penaltiesHorizontalPass [x - 1, y] - grid [removeIndex, y].movementPenalty + grid [addIndex, y].movementPenalty;
			}
		}
			
		for (int x = 0; x < gridSizeX; x++) 
		{
			for (int y = -kernelExtents; y <= kernelExtents; y++) 
			{
				int sampleY = Mathf.Clamp (y, 0, kernelExtents);
				penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleY];
			}

			int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, 0] / (kernelSize * kernelSize));
			grid [x, 0].movementPenalty = blurredPenalty;

			for (int y = 1; y < gridSizeY; y++) 
			{
				int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
				int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY-1);

				penaltiesVerticalPass [x, y] = penaltiesVerticalPass [x, y-1] - penaltiesHorizontalPass [x,removeIndex] + penaltiesHorizontalPass [x, addIndex];
				blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, y] / (kernelSize * kernelSize));
				grid [x, y].movementPenalty = blurredPenalty;

				if (blurredPenalty > penaltyMax) 
				{
					penaltyMax = blurredPenalty;
				}
				if (blurredPenalty < penaltyMin) 
				{
					penaltyMin = blurredPenalty;
				}
			}
		}

	}*/

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) 
		{
			for (int y = -1; y <= 1; y++) 
			{
				for(int z = -1; z <= 1; z++)
				{
					if (x == 0 && y == 0)
						continue;

					int checkX = node.gridX + x;
					int checkY = node.gridY + y;
					int checkZ = node.gridZ + z;

					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ) 
					{
						neighbours.Add(grid[checkX,checkY,checkZ]);
					}
				}
			}
		}
		return neighbours;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) 
	{
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		float percentZ = (worldPosition.z + gridWorldSize.z/2) / gridWorldSize.z;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		percentZ = Mathf.Clamp01(percentZ);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		int z = Mathf.RoundToInt((gridSizeZ-1) * percentZ);
		return grid[x,y,z];
	}

	public List<Node> path;
	void OnDrawGizmos() 
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x, gridWorldSize.z, gridWorldSize.y));
		if (grid != null && displayGridGizmos) 
		{
			foreach (Node n in grid) 
			{
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
}

[System.Serializable]
public class TerrianType
{
	public LayerMask terrainMask;
	public int terrainPenatly;
}

