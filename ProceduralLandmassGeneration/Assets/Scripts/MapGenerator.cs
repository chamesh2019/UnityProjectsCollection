using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public float heightMultiplier;

    public bool autoUpdate;

    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;
    
    public TerrainType[] regions;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset);

        MapDisplay display = FindFirstObjectByType <MapDisplay>();

        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float noise = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (noise <= regions[i].height) {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        if (drawMode == DrawMode.NoiseMap) {
            Texture2D noiseTexture = TextureGenerator.TextureFromNoiseMap(noiseMap);
            display.DrawTexture(noiseTexture);
        } else if (drawMode == DrawMode.ColorMap) {
            Texture2D noiseTexture = TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight);
            display.DrawTexture(noiseTexture);
        } else if (drawMode == DrawMode.Mesh) {
            display.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, heightMultiplier), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
    }

    private void OnValidate() {
        if (mapWidth < 1) {
            mapWidth = 1;
        }
        if (mapHeight < 1) { 
            mapHeight = 1; 
        }
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