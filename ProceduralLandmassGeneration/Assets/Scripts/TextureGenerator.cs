using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height) {
        Texture2D texture = new Texture2D(width, height);

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    } 

    public static Texture2D TextureFromNoiseMap(float[,] noiseMap) {
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                colorMap[y * mapWidth + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
    
        return TextureFromColorMap(colorMap, mapWidth, mapHeight);

    }
}
