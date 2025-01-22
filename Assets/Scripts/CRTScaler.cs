using System.Collections.Generic;
using UnityEngine;

public class CRTScaler : MonoBehaviour {
    [SerializeField] Camera camera;
    [SerializeField] List<RenderTexture> renderTextures;
    [SerializeField] float aspectRatio = (float)4 / 3;
    [SerializeField] float scale = 0.75f;

    public float AspectRatio => aspectRatio;
    public float Scale => scale;

    int width;
    int height;
    
    void Update() {
        var heightNow = camera.scaledPixelHeight;
        var widthNow = camera.scaledPixelWidth;

        if (widthNow != width || heightNow != height) {
            foreach (var renderTexture in renderTextures) {
                renderTexture.Release();
                renderTexture.width = (int) (heightNow * aspectRatio);
                renderTexture.height = heightNow;
                renderTexture.Create();
                height = heightNow;
            }
        }

        transform.localScale = new Vector3(
            aspectRatio * camera.orthographicSize * scale * 2, 
            camera.orthographicSize * scale * 2, 
            1
        );
    }
}
