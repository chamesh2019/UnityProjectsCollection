using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
     public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale,
         int seed, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        if (scale <= 0) {
            scale = 0.0001f;
        }

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        System.Random rand = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++) {
            octaveOffsets[i] = new Vector2(
                rand.Next(-100000, 100000) + offset.x,
                rand.Next(-100000, 100000) + offset.y
            );
        }

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {

                float frequency = 1f;
                float amplitude = 1f;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxHeight) {
                    maxHeight = noiseHeight;
                } else if (noiseHeight < minHeight) {
                    minHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
            }
        }

                return noiseMap;
    }
}
