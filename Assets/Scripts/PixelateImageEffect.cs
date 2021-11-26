using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=9bTFVaKGIIQ

[ExecuteInEditMode]
public class PixelateImageEffect : MonoBehaviour {

    public Material pixelateMat;

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, pixelateMat);
    }
    
}
