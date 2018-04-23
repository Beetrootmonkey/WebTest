using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelEditor))]
public class MapEditor : Editor
{
    Level level;
    LevelEditor editor;

    void OnSceneGUI()
    {
        if(!level)
        {
            level = FindObjectOfType<Level>();
        }
        if(!editor)
        {
            editor = FindObjectOfType<LevelEditor>();
        }
        if (!editor)
        {
            Debug.Log("Fatal: Couldn't find the Level!");
            return;
        }
        if(!level)
        {
            Debug.Log("Fatal: Couldn't find the Level Editor!");
            return;
        }
        Tile.TileType type = editor.type;
        //if 'E' pressed, spawn the selected prefab
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == editor.key)
        {
            Vector2 spawnPosition = MousePos();
            Tile tile = Tile.FindTile(spawnPosition);
            if (!tile)
            {
                tile = Instantiate<Tile>(editor.prefab, spawnPosition, Quaternion.identity);
                tile.gameObject.transform.SetParent(level.transform);
                tile.Settle();
            }
            tile.SetType(type);
            //Debug.Log("Set tile type at " + tile.transform.position + " to " + type);
        }
    }

    private Vector2 MousePos()
    {
        return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
    }
}
