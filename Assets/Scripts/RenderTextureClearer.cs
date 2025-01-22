using System;
using UnityEngine;
using UnityEngine.Video;

public class RenderTextureClearer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable() {
        var rt = GetComponent<VideoPlayer>().targetTexture;
        rt.Release();
        rt.Create();
    }
}
