using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawTexture(Texture2D texture) {
        int mapWidth = texture.width;
        int mapHeight = texture.height;

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(mapWidth, 1, mapHeight);
    }
}
