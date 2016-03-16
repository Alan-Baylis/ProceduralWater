using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        MapGenerator generator = (MapGenerator)target;

        if (DrawDefaultInspector()) {
            if (generator._autoUpdate)
            {
                generator.GenerateMap();
            }
        };

        if (GUILayout.Button("Generate"))
        {
            generator.GenerateMap();
        }
    }
}
