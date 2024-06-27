using UnityEngine;
using UnityEngine.Tilemaps;

public class Resource
{
    static public GameObject s_prefabLevelEmpty = null;
    static public GameObject s_prefabLevelEntity = null;
    
    static public GameObject s_prefabDoors = null;
    
    static public GameObject s_prefabDanger = null;
    static public GameObject s_prefabSpade = null;

    static public GameObject s_prefabLever = null;
    
    static public GameObject s_prefabExit = null;
    static public GameObject s_prefabSpawn = null;

    static public Tile[] s_tilesBlock = new Tile[66];

    static public void Init()
    {
        // Levels.

        s_prefabLevelEmpty      =   Resources.Load("Levels/Level Empty") as GameObject;
        s_prefabLevelEntity     =   Resources.Load("Levels/Level Entity") as GameObject;

        // Objects.
        
        s_prefabDoors           =   Resources.Load("Objects/Blocks/Doors") as GameObject;

        s_prefabDanger          =   Resources.Load("Objects/Dangers/Danger") as GameObject;
        s_prefabSpade           =   Resources.Load("Objects/Dangers/Spade") as GameObject;

        s_prefabLever           =   Resources.Load("Objects/Items/Lever") as GameObject;

        s_prefabExit            =   Resources.Load("Objects/Exit") as GameObject;
        s_prefabSpawn           =   Resources.Load("Objects/Spawn") as GameObject;

        // Tiles.

        for (int k = 0; k < 66; k++)
        {
            s_tilesBlock[k] = Resources.Load("Tiles/Blocks/Blocks_" + k.ToString()) as Tile;

            Debug.Assert(s_tilesBlock[k] != null);
        }

        // Debug.
        
        Debug.Assert(s_prefabLevelEmpty);
        Debug.Assert(s_prefabLevelEntity);

        Debug.Assert(s_prefabDoors);

        Debug.Assert(s_prefabDanger);
        Debug.Assert(s_prefabSpade);

        Debug.Assert(s_prefabLever);
        
        Debug.Assert(s_prefabExit);
        Debug.Assert(s_prefabSpawn);
    }
}
