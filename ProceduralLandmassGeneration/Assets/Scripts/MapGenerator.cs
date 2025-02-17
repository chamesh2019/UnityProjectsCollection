using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;

    public const int mapChunckSize = 241;

    [Range(0, 6)]
    public int levelOfDetail = 1;

    //public int mapChunckSize;
    //public int mapChunckSize;
    public float noiseScale;
    public float heightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;
    
    public TerrainType[] regions;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunckSize, mapChunckSize, noiseScale, seed, octaves, persistance, lacunarity, offset);

        MapDisplay display = FindFirstObjectByType <MapDisplay>();

        Color[] colorMap = new Color[mapChunckSize * mapChunckSize];

        for (int y = 0; y < mapChunckSize; y++) {
            for (int x = 0; x < mapChunckSize; x++) {
                float noise = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (noise <= regions[i].height) {
                        colorMap[y * mapChunckSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        if (drawMode == DrawMode.NoiseMap) {
            Texture2D noiseTexture = TextureGenerator.TextureFromNoiseMap(noiseMap);
            display.DrawTexture(noiseTexture);
        } else if (drawMode == DrawMode.ColorMap) {
            Texture2D noiseTexture = TextureGenerator.TextureFromColorMap(colorMap, mapChunckSize, mapChunckSize);
            display.DrawTexture(noiseTexture);
        } else if (drawMode == DrawMode.Mesh) {
            display.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, heightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunckSize, mapChunckSize));
        }
    }

    private void OnValidate() {
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}