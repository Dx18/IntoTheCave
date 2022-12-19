using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Leopotam.EcsLite;

using Random = UnityEngine.Random;

namespace Game {
    public class MapGenerator {
	private delegate int SpawnFunc(
	    EcsWorld world, Vector2 position, GameSharedData data
	);

	private struct Spawner : IComparable<Spawner> {
	    public float potential;
	    public SpawnFunc spawn;

	    public Spawner(float potential, SpawnFunc spawn) {
		this.potential = potential;
		this.spawn = spawn;
	    }

	    public int CompareTo(Spawner other) {
		return potential.CompareTo(other.potential);
	    }
	}

	private struct Room {
	    public int x;
	    public int y;
	    public int width;
	    public int height;
	    public int? topExit;
	    public int? rightExit;
	    public int? bottomExit;
	    public int? leftExit;
	}

	private EcsWorld _world;
	private GameAgentSpawner _gameAgentSpawner;
	private GameSharedData _data;

	private int _level;
	private Tilemap _tilemapFloor;
	private Tilemap _tilemapCollision;

	private Spawner[] _spawners;

	public MapGenerator(
	    EcsWorld world, GameAgentSpawner gameAgentSpawner,
	    GameSharedData data
	) {
	    _world = world;
	    _data = data;

	    _level = _data.rules.CalculateLevel(
		_data.playerData.experience
	    );

	    _gameAgentSpawner = gameAgentSpawner;

	    _spawners = new Spawner[] {
		new Spawner(
		    _data.rules.ratSpawnPotential,
		    (world, position, data) =>
		    _gameAgentSpawner.SpawnRat(
			world, position, data
		    )
		),
		new Spawner(
		    _data.rules.stonethrowerSpawnPotential,
		    (world, position, data) =>
		    _gameAgentSpawner.SpawnStonethrower(
			world, position, data
		    )
		),
		new Spawner(
		    _data.rules.alchemistSpawnPotential,
		    (world, position, data) =>
		    _gameAgentSpawner.SpawnAlchemist(
			world, position, data
		    )
		),
		new Spawner(
		    _data.rules.slimeSpawnPotential,
		    (world, position, data) =>
		    _gameAgentSpawner.SpawnSlime(
			world, position, data
		    )
		),
		new Spawner(
		    _data.rules.slimeNestSpawnPotential,
		    (world, position, data) =>
		    _gameAgentSpawner.SpawnSlimeNest(
			world, position, _gameAgentSpawner, data
		    )
		)
	    };

	    Array.Sort(_spawners);
	}

	public Vector2 SpawnMap() {
	    int roomCount = _data.rules.CalculateRoomCount(_level);
	    int roomWidth = _data.rules.roomWidth;

	    GameObject mapInstance = GameObject.Instantiate(
		_data.prefabs.mapPrefab, Vector3.zero,
		Quaternion.identity
	    );

	    Transform mapTransform = mapInstance.transform;

	    _tilemapFloor = mapTransform.Find("Floor")
		.GetComponent<Tilemap>();
	    _tilemapCollision = mapTransform.Find("Collision")
		.GetComponent<Tilemap>();

	    int currY = 0;
	    int currX = 0;
	    Room[] rooms = new Room[roomCount];
	    for (int room = 0; room < roomCount; ++room) {
		int roomHeight = Random.Range(
		    _data.rules.roomHeightMin,
		    _data.rules.roomHeightMax + 1
		);

		int? topExit = room < roomCount - 1
		    ? Random.Range(0, roomWidth) : null;
		int? bottomExit = room > 0
		    ? rooms[room - 1].topExit : null;

		rooms[room] = new Room {
		    x = currX, y = currY,
		    width = roomWidth, height = roomHeight,
		    topExit = topExit, rightExit = null,
		    bottomExit = bottomExit, leftExit = null
		};

		currY += roomHeight + 1;
	    }

	    int index = 0;
	    foreach (Room room in rooms) {
		float spawnPotential = index == 0
		    ? 0
		    : _data.rules.CalculateRoomSpawnPotential(_level);
		bool createExit = index == rooms.Length - 1;

		SpawnRoom(room, spawnPotential, createExit);

		++index;
	    }

	    return new Vector2(
		rooms[0].x + rooms[0].width / 2f,
		rooms[0].y + rooms[0].height / 2f
	    );
	}

	private void SpawnRoom(
	    Room room, float spawnPotential, bool createExit
	) {
	    FillRoom(room);
	    SpawnRoomMonsters(room, spawnPotential);

	    if (createExit) {
		int x = room.x + 1 + Random.Range(0, room.width - 2);
		int y = room.y + 1 + Random.Range(0, room.height - 2);

		_tilemapFloor.SetTile(
		    new Vector3Int(x, y, 0), _data.tiles.exit
		);

		EntityBuilder.BeginEmpty(_world)
		    .With(new ExitZone(new Rect(x, y, 1, 1)));
	    }
	}

	private void FillRoom(Room room) {
	    for (int x = -1; x < room.width + 1; ++x) {
		for (int y = -1; y < room.height + 1; ++y) {
		    bool isWall =
			x == -1 || x == room.width ||
			y == -1 || y == room.height;

		    bool isTopExit = y == room.height &&
			room.topExit != null &&
			x == (int) room.topExit;
		    bool isRightExit = x == room.width &&
			room.rightExit != null &&
			y == (int) room.rightExit;
		    bool isBottomExit = y == -1 &&
			room.bottomExit != null &&
			x == (int) room.bottomExit;
		    bool isLeftExit = x == 0 &&
			room.leftExit != null &&
			y == (int) room.leftExit;

		    if (isWall && !isTopExit && !isRightExit &&
			!isBottomExit && !isLeftExit) {
			_tilemapCollision.SetTile(
			    new Vector3Int(room.x + x, room.y + y, 0),
			    _data.tiles.solid
			);
		    } else {
			_tilemapFloor.SetTile(
			    new Vector3Int(room.x + x, room.y + y, 0),
			    _data.tiles.floor
			);
		    }
		}
	    }
	}

	private void SpawnRoomMonsters(
	    Room room, float spawnPotential
	) {
	    // Calculating usage counts
	    int[] usageCounts = CalculateSpawnerUsageCounts(
		spawnPotential, room.width * room.height
	    );

	    // Creating array of possible positions for monsters
	    int[] positions = new int[room.width * room.height];
	    for (int i = 0; i < room.width * room.height; ++i) {
		positions[i] = i;
	    }

	    // Shuffling it (Fisher--Yates algorithm)
	    for (int i = 0; i < room.width * room.height; ++i) {
		int j = Random.Range(i, room.width * room.height);

		int temp = positions[i];
		positions[i] = positions[j];
		positions[j] = temp;
	    }

	    // Spawning monsters

	    int currPosition = 0;
	    for (int i = 0; i < usageCounts.Length; ++i) {
		for (int j = 0; j < usageCounts[i]; ++j) {
		    int x = positions[currPosition] % room.width;
		    int y = positions[currPosition] / room.width;
		    ++currPosition;

		    Vector2 position = new Vector2(
			room.x + x + 0.5f, room.y + y + 0.5f
		    );

		    _spawners[i].spawn(_world, position, _data);
		}
	    }
	}

	private int[] CalculateSpawnerUsageCounts(
	    float spawnPotential, int maxCount
	) {
	    int[] result = new int[_spawners.Length];
	    int currCount = 0;

	    // Number of spawners can currently be used
	    int choice = _spawners.Length;

	    while (currCount < maxCount) {
		while (choice > 0 && _spawners[choice - 1].potential >
		       spawnPotential) {
		    --choice;
		}

		if (choice == 0) {
		    break;
		}

		int index = Random.Range(0, choice);
		++result[index];

		spawnPotential -= _spawners[index].potential;
		++currCount;
	    }

	    return result;
	}
    }
}
