using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GameTiles")]
public class GameTiles : ScriptableObject {
    public Tile solid;
    public Tile floor;
    public Tile exit;
}
