//This script will generate the world map and everything on it. Only called once at start of game

using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {

	//Determine size of area
	public int _sizeX;
	public int _sizeZ;
	//Determine number of areas the map is split into (atm only water/normal)
	public int _variance;
	//Bool to trigger when max number of towns has been reached
	public bool _filled;
	//List of all points in the map, reduces as it fills with objects
	public List <Vector3> _points;
	//List of all the biome area seeds
	public List <Vector3> _seedCache;
	//List of available terrains to spawn objects onto
	public List <Vector3> _spawnable;
	//As above, but with town s
	public List <Vector3> _townSpawns;
	//List of actua locations of final towns
	public List <Vector3> _towns;
	//List to store the areas which are already populated with objects
	public List <Vector3> _used;
	//Various prefabs called on createdeation
	public List <GameObject> _terrainPrefab = new List<GameObject>();
	public GameObject _water;
	public GameObject _townPrefab;
	public GameObject _mill, _quarry, _ironMine, _coalMine;
	public GameObject _resource;
	public GameObject _trees;
	public GameObject _blocker;
	//All the gameobjects in the heirarchy for assigning parents and keeping inspector organised
	public Transform _townParent;
	public Transform _factoryParent;
	public Transform _resourceParent;
	public Transform _terrainParent;
	public Transform _waterParent;
	//Access Player Controls for setting spawn location
	public PlayerControls_WM _playerControl;

	void Start () {
		//Allocate transforms for parenting
		_townParent = GameObject.Find("Towns").GetComponent<Transform>();
		_factoryParent = GameObject.Find("Factories").GetComponent<Transform>();
		_resourceParent = GameObject.Find("Resources").GetComponent<Transform>();
		_terrainParent = GameObject.Find("Terrain").GetComponent<Transform>();
		_waterParent = GameObject.Find("Water").GetComponent<Transform>();
		//Access Player Contro Scripts
		_playerControl = GameObject.Find("Player").GetComponent<PlayerControls_WM>();
		//Call each function to instantiate the map
		CachePoints();
		GenerateWater();
		GenerateTerrain();
		CalculateTowns();
		GenerateTowns();	
		ObjectSpawns(_mill, 2.0f);	
		ObjectSpawns(_quarry, 3.0f);
		ObjectSpawns(_ironMine, 4.0f);
		ObjectSpawns(_coalMine, 4.0f);
		GenerateResources();
		PositionPlayer();
		FinishTerrain();
		ScaleSet();					
	}
	//Function to spawn a number of points alongthe predetermined height and width
	void CachePoints(){
		for (int x = 0; x < _sizeX; x++){
			for (int y = 0; y < _sizeZ; y++){
				_points.Add(new Vector3(x, 0, y));
			}
		}
	}

	void GenerateWater(){
		for (int i = 0; i < _points.Count; i++){
			var sea = (GameObject) Instantiate(_water, _points[i], Quaternion.identity);
			sea.transform.SetParent(_waterParent);
		}
		//Move up above the terrain
		_waterParent.transform.position = new Vector3(0, -6, 0);

	}
	//Function to split world into regular terrain and water
	void GenerateTerrain(){
		//Create random spawns throughout the map to the number declared under _cariance
		for (int i = 0; i < _variance; i++){
			int _seed = Random.Range(0, _points.Count-1);
			_seedCache.Add(_points[_seed]);
		}
		//Add a seed in each corner and halfway points
		_seedCache.Add(new Vector3(0,0,0));
		_seedCache.Add(new Vector3(0,0,_sizeZ));
		_seedCache.Add(new Vector3(_sizeX,0,0));
		_seedCache.Add(new Vector3(_sizeX,0,_sizeZ));
		_seedCache.Add(new Vector3(_sizeX/2,0,0));
		_seedCache.Add(new Vector3(0,0,_sizeZ/2));
		_seedCache.Add(new Vector3(_sizeX/2,0, _sizeZ));
		_seedCache.Add(new Vector3(_sizeX,0,_sizeZ/2));
		//Loop through all available points, and determine closest seed by comparing distances
		for (int i = 0; i < _points.Count; i++){
			float dist;
			float prevDist = Mathf.Infinity;
			int index = 0;
			for (int j = 0; j < _seedCache.Count; j++){
				dist = Vector3.Distance(_points[i], _seedCache[j]);
				if (dist < prevDist){
					prevDist = dist;
					index = j;
				}				
			}
			//Leave point blank (sea) along edges, add point to _used list
			if (_points[i].x == 0 || _points[i].x == (_sizeX -1) || _points[i].z == 0 || _points[i].z == (_sizeZ -1)){
				var blocker = (GameObject) Instantiate(_blocker, _points[i], Quaternion.identity);
				blocker.transform.SetParent(_terrainParent);
				_used.Add(_points[i]);
			}
			//If the closest seed is far enough inland, ignore it
			else if (_seedCache[index].x > 7.0f && _seedCache[index].x < (_sizeX - 7.0f) && _seedCache[index].z > 7.0f && _seedCache[index].z < (_sizeZ - 7.0f)){
				//Leave point
			}
			//Otherwise leave area blank and add to _used list		
			else{
				var blockerB = (GameObject) Instantiate(_blocker, _points[i], Quaternion.identity);
				blockerB.transform.SetParent(_terrainParent);
				_used.Add(_points[i]);
			}			
		}
		//Call the update points function
		UpdatePoints();
	}
	//This function loops through all the available Vector3's and removes any that are now in the _used list
	void UpdatePoints(){
		for (int x = _points.Count-1; x > -1; x--){
			if (_used.Contains(_points[x])){
				_points.RemoveAt(x);
			}
		}
	}
	void PositionPlayer(){
		//Position the player on an unused tile
		int x = Random.Range(0, _points.Count -1);
		_playerControl._startPos = _points[x];
		var terrain = (GameObject) Instantiate(_terrainPrefab[0], _points[x], Quaternion.identity);
		terrain.transform.SetParent(_terrainParent);
		_used.Add(_points[x]);UpdatePoints();
	}
	//Function to calculate available areas for towns
	void CalculateTowns(){
		//loops through all available poitns and ensure they are at least 10.0f from the edges, and add to _spawnable list if so
		for (int i = _points.Count-1; i > -1; i--){
			if (_points[i].x > 8 && _points[i].x < (_sizeX-8)){
				if (_points[i].z > 8 && _points[i].z < (_sizeZ-8)){
					_spawnable.Add(_points[i]);
				}
			}
		}
		//pick one random value from the spawable list and create a town there
		int x = Random.Range(0, _spawnable.Count-1);
		_towns.Add(_spawnable[x]);
		//Until the all slots are allocated, call the GenerateTowns function
		while(!_filled){
			GenerateTowns();
		}
		//For each town in the list created via the above functions, instantiate a town and add it to the _used List 
		for (int i = 0; i < _towns.Count; i++){
			var town = (GameObject) Instantiate (_townPrefab, _towns[i], Quaternion.identity);
			town.transform.SetParent(_townParent);
			_used.Add(_towns[i]);
		}
		UpdatePoints();
	}

	//Funciton to generate town positions
	void GenerateTowns(){
		//Begin by clearing the list of all potential areas
		_townSpawns.Clear();
		//Loop through the spawnable areas and incrementally increase counter to ensure all currently positioned towns are checked
		for (int k = 0; k < _spawnable.Count; k++){
			int totalChecked = 0;
			for (int l = 0; l < _towns.Count; l++){
				//If the point is far enough  from a town, increase the totalChecked value
				if (Mathf.Abs(_spawnable[k].x - _towns[l].x) > 14.0f || Mathf.Abs(_spawnable[k].z - _towns[l].z) > 14.0f){
					totalChecked++;
					//if it checks out against every currenttown, add the point as a potential spawn point
					if (totalChecked == _towns.Count){
						_townSpawns.Add(_spawnable[k]);
					}
				}
			}			
		}
		//if this loop returns no value, then the map is filled
		if (_townSpawns.Count < 1){
			_filled = true;
		}
		//Otherwise, select one of the potential spawn points at random, and add it to the list of towns to spawn
		else{
			int x = Random.Range(0, _townSpawns.Count-1);
			_towns.Add(_townSpawns[x]);
		}
	}
	//Funciton to determine spawn points for other buildings near each town
	void ObjectSpawns(GameObject prefab, float range){
		//Clear the spawnable list and loop through each town, adding all areas within the requested range
		_spawnable.Clear();
		for (int i = 0; i < _towns.Count; i++){
			for (int j = 0; j < _points.Count; j++){
				float xdist = Mathf.Abs(_points[j].x - _towns[i].x);
				float zdist = Mathf.Abs(_points[j].z - _towns[i].z);
				if (xdist <= range && zdist <= range){	
				//Ensure the object spawns at a set distance from the town				
					if (Mathf.Approximately(xdist, range) || Mathf.Approximately(zdist, range)){
						_spawnable.Add(_points[j]);
					}
				}
			}
			//Call the function to instantiate the building
			SpawnObject(prefab);
		}
	}
	//This function creates objects on the World Map
	void SpawnObject(GameObject prefab){
		//Get a random location from the list of spawnable points
		int x = Random.Range(0, _spawnable.Count-1);
		//Spawn the object in the location 
		var obj = (GameObject) Instantiate(prefab, _spawnable[x], Quaternion.identity);
		//Check the type and allocate the parent accordingly
		if (prefab != _resource){
			_used.Add(_spawnable[x]);
			obj.transform.SetParent(_factoryParent);
		}
		else{
			_used.Add(_spawnable[x]);
			obj.transform.SetParent(_resourceParent);
		}
		//Clear the list
		_spawnable.Clear();
		UpdatePoints();
	}
	//Function to spawn random resources and factories across the remainder of the map
	void GenerateResources(){
		//First, spawn a set numbe rof resources close to each town
		for (int i = 0; i < 5; i++){
			float j = (float) Random.Range(2, 4);
			ObjectSpawns(_resource, j);
		}
		//Loop through each remaining point on the map and randomly allocate buildings and resources
		for (int x = 0; x < _points.Count; x++){
			//Create a value that can be incrementally increased as each town is checked to ensure 
			//extra resources do no spawn near them
			int available = 0;
			for (int y = 0; y < _towns.Count; y++){
				if (Vector3.Distance(_points[x], _towns[y]) > 6.0f){
					available++;
					//If not near any town, possibly spawn a random resource or factory
					if (available == _towns.Count){
						_spawnable.Add(_points[x]);
						int chance = Random.Range(0, 201);
						if (chance == 200){
							SpawnObject(_coalMine);
						}
						else if (chance == 199){
							SpawnObject(_ironMine);
						}
						else if (chance > 196){
							SpawnObject(_quarry);
						}
						else if (chance > 193){
							SpawnObject(_mill);
						}
						else if (chance >= 185){
							SpawnObject(_resource);
						}
					}
				}
			}			
		}
	}
	//Populate the remainder of the map with trees and other terrain additions
	void FinishTerrain(){
		//Loop through each remaining point and add random terrain element
		for (int i = 0; i < _points.Count; i++){
			var scenery = Random.Range(0, 10);
			//Set random rotation
			var rotVal = (float)Random.Range(0, 4)*90;
			var rot = Quaternion.Euler(new Vector3(0, rotVal, 0));
			if (scenery > 5){
				//Insantiate and set parent
				var forest = (GameObject) Instantiate(_trees, _points[i], rot);
				forest.transform.SetParent(_terrainParent);
				_used.Add(_points[i]);
			}
			else{
				//Spawn random terrain tile into remainder of map
				int x = Random.Range(0, _terrainPrefab.Count);
				var terrain = (GameObject) Instantiate(_terrainPrefab[x], _points[i], rot);
				terrain.transform.SetParent(_terrainParent);
			}			
		}		
	}
	//Increase the terrain scale to 10:1, equalling the player
	void ScaleSet(){
		_townParent.localScale = new Vector3(10,10,10);
		_factoryParent.localScale = new Vector3(10,10,10);
		_terrainParent.localScale = new Vector3(10,10,10);
		_resourceParent.localScale = new Vector3(10,10,10);
		_waterParent.localScale = new Vector3(10,10,10);
	}	
}
