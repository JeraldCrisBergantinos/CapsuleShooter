using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class MapGenerator : MonoBehaviour {
	
	public Map[] maps;
	public int mapIndex;

	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Transform mapFloor;
	//public Transform navMeshFloor;
	//public Transform navMeshMaskPrefab;
//	public Vector2 mapSize;
	//public Vector2 maxMapSize;
	
	[Range(0,1)]
	public float outlinePercent;
	
//	[Range(0,1)]
//	public float obstaclePercent;
	
//	public int seed = 10;
	public float tileSize = 1;
	
	List<Coord> allTileCoords;
	Queue<Coord> shuffledTileCoords;
	Queue<Coord> shuffledOpenTileCoords;
//	Coord mapCenter;
	
	Map currentMap;
	Transform[,] tileMap;
	
	void Start() {
		//GenerateMap();
		Spawner spawner = FindObjectOfType( typeof(Spawner) ) as Spawner;
		spawner.OnNewWave += OnNewWave;
	}
	
	public void OnNewWave( int waveNumber ) {
		mapIndex = waveNumber - 1;
		//print( "mapIndex " + mapIndex );
		GenerateMap();
	}
	
	public void GenerateMap() {
		currentMap = maps[mapIndex];
		tileMap = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];
		System.Random pnrg = new System.Random( currentMap.seed );
		//GetComponent<BoxCollider>().size = new Vector3( currentMap.mapSize.x * tileSize, 0.05f, currentMap.mapSize.y * tileSize );
		
		//generating coords
		allTileCoords = new List<Coord>();
		for ( int x = 0; x < currentMap.mapSize.x; ++x ) {
			for ( int y = 0; y < currentMap.mapSize.y; ++y ) {
				allTileCoords.Add( new Coord(x,y) );
			}
		}
		shuffledTileCoords = new Queue<Coord>( Utility.ShuffleArray(allTileCoords.ToArray(),currentMap.seed) );
		
		//generating map objects
		string holderName = "Generated Map";
		if ( transform.FindChild(holderName) ) {
			DestroyImmediate( transform.FindChild(holderName).gameObject );
		}
		
		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;
		
		//spawning tiles
		for ( int x = 0; x < currentMap.mapSize.x; ++x ) {
			for ( int y = 0; y < currentMap.mapSize.y; ++y ) {
				Vector3 tilePosition = CoordToPosition( x, y );
				Transform newTile = Instantiate( tilePrefab, tilePosition, Quaternion.identity ) as Transform;
				newTile.localScale = Vector3.one * ( 1 - outlinePercent ) * tileSize;
				newTile.parent = mapHolder;
				tileMap[x,y] = newTile;
			}
		}
		
		//spawning obstacles
		bool[,] obstacleMap = new bool[currentMap.mapSize.x, currentMap.mapSize.y];
		
		int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
		int currentObstacleCount = 0;
		List<Coord> allOpenCoords = new List<Coord>(allTileCoords);
		
		for ( int i = 0; i < obstacleCount; ++i ) {
			Coord randomCoord = GetRandomCoord();
			obstacleMap[randomCoord.x, randomCoord.y] = true;
			++currentObstacleCount;
			
			if ( randomCoord != currentMap.mapCenter && MapIsFullyAccessible( obstacleMap, currentObstacleCount ) ) {
				float obstacleHeight = Mathf.Lerp( currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float)pnrg.NextDouble() );
				Vector3 obstaclePosition = CoordToPosition( randomCoord.x, randomCoord.y );
				Transform newObstacle = Instantiate( obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight/2, Quaternion.identity ) as Transform;
				newObstacle.localScale = new Vector3( ( 1 - outlinePercent ) * tileSize, obstacleHeight, ( 1 - outlinePercent ) * tileSize );
				newObstacle.parent = mapHolder;
				
				//newObstacle.gameObject.layer = LayerMask.NameToLayer( "Obstacles" );
				//The layer of prefab is already set to "Obstacles".
				
				Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
				Material obstacleMaterial = new Material( obstacleRenderer.sharedMaterial );
				float colorPercent = randomCoord.y/(float)currentMap.mapSize.y;
				obstacleMaterial.color = Color.Lerp( currentMap.foregroundColor, currentMap.backgroundColor, colorPercent );
				obstacleRenderer.sharedMaterial = obstacleMaterial;
				
				allOpenCoords.Remove( randomCoord );
			}
			else {
				obstacleMap[randomCoord.x, randomCoord.y] = false;
				--currentObstacleCount;
			}
		}
		
		shuffledOpenTileCoords = new Queue<Coord>( Utility.ShuffleArray(allOpenCoords.ToArray(),currentMap.seed) );
		
		//creating nav mesh mask
		//we do not need nav mesh mask
		/*Transform maskLeft = Instantiate( navMeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + maxMapSize.x)/4f * tileSize, Quaternion.identity ) as Transform;
		maskLeft.parent = mapHolder;
		maskLeft.localScale = new Vector3( (maxMapSize.x - currentMap.mapSize.x)/2f, 1, currentMap.mapSize.y ) * tileSize;
		
		Transform maskRight = Instantiate( navMeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + maxMapSize.x)/4f * tileSize, Quaternion.identity ) as Transform;
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3( (maxMapSize.x - currentMap.mapSize.x)/2f, 1, currentMap.mapSize.y ) * tileSize;
		
		Transform maskTop = Instantiate( navMeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y)/4f * tileSize, Quaternion.identity ) as Transform;
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3( maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y)/2f ) * tileSize;
		
		Transform maskBottom = Instantiate( navMeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + maxMapSize.y)/4f * tileSize, Quaternion.identity ) as Transform;
		maskBottom.parent = mapHolder;
		maskBottom.localScale = new Vector3( maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y)/2f ) * tileSize;*/
		
		//navMeshFloor.localScale = new Vector3( maxMapSize.x, 0, maxMapSize.y ) * tileSize;
		mapFloor.localScale = new Vector3( currentMap.mapSize.x * tileSize, 0, currentMap.mapSize.y * tileSize );
		
		//AI
		InitAI();
	}
	
	bool MapIsFullyAccessible( bool[,] obstacleMap, int currentObstacleCount ) {
		bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue( currentMap.mapCenter );
		mapFlags[currentMap.mapCenter.x, currentMap.mapCenter.y] = true;
		int accessibleTileCount = 1;
		
		while ( queue.Count > 0 ) {
			Coord tile = queue.Dequeue();
			
			for ( int x = -1; x <= 1; ++x ) {
				for ( int y = -1; y <= 1; ++y ) {
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;
					if ( x == 0 || y == 0 ) {
						if ( neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) &&
							neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1) ) {
							if ( !mapFlags[neighbourX, neighbourY] &&
								!obstacleMap[neighbourX, neighbourY] ) {
								mapFlags[neighbourX, neighbourY] = true;
								queue.Enqueue( new Coord(neighbourX,neighbourY) );
								++accessibleTileCount;
							}
						}
					}
				}
			}
		}
		
		int targetAccessibleTileCount = currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount;
		return targetAccessibleTileCount == accessibleTileCount;
	}
	
	Vector3 CoordToPosition( int x, int y ) {
		return new Vector3( -currentMap.mapSize.x/2f + 0.5f + x, 0, -currentMap.mapSize.y/2f + 0.5f + y ) * tileSize;
	}
	
	public Transform GetTileFromPosition( Vector3 position ) {
		int x = Mathf.RoundToInt( position.x / tileSize + (currentMap.mapSize.x - 1) / 2f );
		int y = Mathf.RoundToInt( position.z / tileSize + (currentMap.mapSize.y - 1) / 2f );
		x = Mathf.Clamp( x, 0, tileMap.GetLength(0) - 1 );
		y = Mathf.Clamp( y, 0, tileMap.GetLength(1) - 1 );
		return tileMap[x,y];
	}
	
	public Coord GetRandomCoord() {
		Coord randomCoord = shuffledTileCoords.Dequeue();
		shuffledTileCoords.Enqueue( randomCoord );
		return randomCoord;
	}
	
	public Transform GetRandomOpenTile() {
		Coord randomCoord = shuffledOpenTileCoords.Dequeue();
		shuffledOpenTileCoords.Enqueue( randomCoord );
		return tileMap[randomCoord.x,randomCoord.y];
	}
	
	void InitAI() {
		string aStarPath = "A*";
		GameObject aStar = GameObject.Find( aStarPath );
		if ( aStar != null )
			DestroyImmediate( aStar );
		
		aStar = Instantiate( currentMap.aStar ) as GameObject;
		aStar.name = aStarPath;
		AstarPath.active = aStar.GetComponent<AstarPath>();
		AstarPath.active.Scan();
	}
	
	[System.Serializable]
	public class Coord {
		public int x;
		public int y;
		
		public Coord( int _x, int _y ) {
			x = _x;
			y = _y;
		}
		
		public static bool operator ==( Coord c1, Coord c2 ) {
			return c1.x == c2.x && c1.y == c2.y;
		}
		
		public static bool operator !=( Coord c1, Coord c2 ) {
			return !(c1 == c2);
		}
	}
	
	[System.Serializable]
	public class Map {
		
		public Coord mapSize;
		[Range(0,1)]
		public float obstaclePercent;
		public int seed;
		public float minObstacleHeight;
		public float maxObstacleHeight;
		public Color foregroundColor;
		public Color backgroundColor;
		public GameObject aStar;
		
		public Coord mapCenter {
			get {
				return new Coord( mapSize.x/2, mapSize.y/2 );
			}
		}
		
	}
}
