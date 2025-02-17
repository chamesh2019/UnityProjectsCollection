using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endlessterrain : MonoBehaviour
{
    public const float maxViewDistance = 450f;
    public Transform viewer;

    public static Vector2 viewerPos;
    int chunckSize;
    int chuncksVisible;

    Dictionary<Vector2, TerrainChunk> terrainChuncks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

     void Start() {
        chunckSize = MapGenerator.mapChunckSize - 1;
        chuncksVisible = Mathf.RoundToInt(maxViewDistance / chunckSize);
     }

    private void Update() {
        viewerPos = new Vector2(viewer.position.x, viewer.position.z);
        UppdateVisibleChucks();
    }
    
    void UppdateVisibleChucks() {

        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }

        terrainChunksVisibleLastUpdate.Clear();

        int currchunckCoordX = Mathf.RoundToInt(viewerPos.x / chunckSize);
        int currchunckCoordY = Mathf.RoundToInt(viewerPos.y / chunckSize);

        for (int yOffset = -chuncksVisible; yOffset <= chuncksVisible; yOffset++) {
            for (int xOffset = -chuncksVisible; xOffset <= chuncksVisible; xOffset++) {
                Vector2 ViewedChunkCoord = new Vector2(currchunckCoordX + xOffset, currchunckCoordY + yOffset);

                if (terrainChuncks.ContainsKey(ViewedChunkCoord)) {
                    terrainChuncks[ViewedChunkCoord].UpdateTerrainChunk();
                    if (terrainChuncks[ViewedChunkCoord].IsVisible()) {
                        terrainChunksVisibleLastUpdate.Add(terrainChuncks[ViewedChunkCoord]);
                    }

                } else {
                    terrainChuncks.Add(ViewedChunkCoord, new TerrainChunk(ViewedChunkCoord, chunckSize, transform));
                }
            }
        }
    }

    public class TerrainChunk {

        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        public TerrainChunk(Vector2 coord, int size, Transform parent) {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0 , position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;

            SetVisible(false);
        }

        public void UpdateTerrainChunk() {
            float viewerDstFromEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPos));
            bool visible = viewerDstFromEdge <= maxViewDistance;
            SetVisible(visible);
        }

        public void SetVisible(bool visible) {
            meshObject.SetActive(visible);  
        }

        public bool IsVisible() {
            return meshObject.activeSelf;
        }
    }

}
