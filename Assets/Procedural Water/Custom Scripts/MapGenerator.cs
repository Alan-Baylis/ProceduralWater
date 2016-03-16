using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode
    {
        NoiseMap,ColorMap, MeshMap
    }
    public DrawMode drawMode;
    [Range (10,241)]
    public int mapSize = 30;
    public float scale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int offsetSeed;
    [Range(0,6)]
    public int levelofDetail;
    public float meshHeight = 20;
    public Vector2 offset;
    public bool _autoUpdate;
    public TerrainType[] regions;

    public AnimationCurve heightCurve;
    public void GenerateMap()
    {
        float[,] noiseMap= Noise.GenerateNoiseMap(mapSize,mapSize,scale,octaves,persistance,lacunarity,offsetSeed,offset);
        Color[] colorMap = new Color[mapSize * mapSize];
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i =0;i<regions.Length;i++)
                {
                    if (currentHeight<=regions[i].mapHeight)
                    {
                        colorMap[y*mapSize +x] = regions[i].mapColor;
                        break;
                    }
                }
            }
        }
                MapDisplay display = FindObjectOfType <MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if(drawMode == DrawMode.ColorMap){
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap,mapSize,mapSize));
        }
        else if (drawMode == DrawMode.MeshMap)
        {
            display.DrawMesh(MeshGenerator.GenerateterrainMesh(noiseMap, meshHeight,heightCurve,levelofDetail), TextureGenerator.TextureFromColorMap(colorMap, mapSize, mapSize));
        }
    }

 void OnValidate()
 {
        if (lacunarity<1)
        {
            lacunarity = 1;
        }
        if (octaves<0)
        {
            octaves = 0;
        }
 }
}
[System.Serializable]
public struct TerrainType
{
    public string mapName;
    public float mapHeight;
    public Color mapColor;
}
