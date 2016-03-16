using UnityEngine;
using System.Collections;

//http://freespace.virgin.net/hugo.elias/models/m_perlin.htm
//http://www.cs.cmu.edu/afs/cs/academic/class/15462-s09/www/lec/09/lec09.pdf

public static class Noise {

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity, int offsetSeed,Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth,mapHeight];

        System.Random pseudoRand = new System.Random(offsetSeed);
        Vector2[] octaveOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float xOffset = pseudoRand.Next(-100000, 100000);
            float yOffset = pseudoRand.Next(-100000, 100000);
            octaveOffset[i] = new Vector2(xOffset,yOffset) + offset;
        }
        int midHeight = mapHeight/2;
        int midWidth = mapWidth / 2;
        if (scale == (int)scale && scale>0)
        {
            scale+= 0.1f;                                         //adding decimal fraction for more noise
        }
        if (scale <= 0)
        {
            scale = 0.1f;
        }
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int i =0; i<octaves;i++) {

                    float sampleX = ((x - midWidth) / Mathf.Abs(scale)) * frequency + octaveOffset[i].x;
                    float sampleY = ((y - midHeight) / Mathf.Abs(scale)) * frequency + octaveOffset[i].y;

                    float perlinNoise = Mathf.PerlinNoise(sampleX, sampleY) *2 -1;  //to get negative perlin noise
                    noiseHeight += perlinNoise * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (maxNoiseHeight<noiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if(minNoiseHeight>noiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight,maxNoiseHeight, noiseMap[x, y]);                
            }
        }
                return noiseMap;
    }
}
