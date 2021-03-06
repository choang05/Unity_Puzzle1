﻿using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    // VARIABLES
	public Transform[] tilePrefabs;
	public Vector2 mapSize;
    public float Yoffset;
    public static Tile[] tiles;

	[Range(0,1)]
	public float outlinePercent;

    // Visuals
    private bool isFirstMat = true;
    public Material firstMat;
    public Material secondMat;

	void Awake()
    {
		GenerateMap();
	}

    public void GenerateMap()
    {

        /*string holderName = "Generated Map";
		if (transform.FindChild (holderName))
			DestroyImmediate(transform.FindChild(holderName).gameObject); 

		Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;
        */

        tiles = new Tile[(int)(mapSize.x * mapSize.y)];
        int totalTiles = 0;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                //Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
                Vector3 tilePosition = new Vector3(x, Yoffset, y);
                Transform newTile = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                //newTile.parent = mapHolder;
                newTile.parent = transform;

                tiles[totalTiles] = newTile.GetComponent<Tile>();
                totalTiles++;

                // Visuals - Assign material to create the checkerboard effect
                Renderer rend = newTile.GetComponent<Renderer>();
                if(isFirstMat)
                {
                    rend.sharedMaterial = firstMat;
                    isFirstMat = false;
                }
                else
                {
                    rend.sharedMaterial = secondMat;
                    isFirstMat = true;
                }
            }
            isFirstMat = !isFirstMat;   // Alternate tile material at the start of each X interval
        }
    }

    void Start()
    {
       AssignAdjacentTiles();
    }

    public void AssignAdjacentTiles()
    {
        foreach (Tile tile in tiles)
        {
            Tile _tile = tile.GetComponent<Tile>();
            Vector2 top     = new Vector2(_tile.TileCoord.x, _tile.TileCoord.y + 1);
            Vector2 right   = new Vector2(_tile.TileCoord.x + 1, _tile.TileCoord.y);
            Vector2 bottom  = new Vector2(_tile.TileCoord.x, _tile.TileCoord.y - 1);
            Vector2 left    = new Vector2(_tile.TileCoord.x - 1, _tile.TileCoord.y);

            //print( _tile.name + top + " " + right + " " + bottom + " " +left);
            foreach (Tile localTile in tiles)
            {
                //print(localTile.name);
                Tile _localTile = localTile.GetComponent<Tile>();
                
                if (_localTile.TileCoord == top)
                    _tile.TopTile = localTile;
                else if (_localTile.TileCoord == right)
                    _tile.RightTile = localTile;
                else if (_localTile.TileCoord == bottom)
                    _tile.BottomTile = localTile;
                else if (_localTile.TileCoord == left)
                    _tile.LeftTile = localTile;
            }
        }
    }

    // Accessors and Mutators
    public Tile[] Tiles
    {
        get { return tiles; }
        set { tiles = value; }
    }
}
