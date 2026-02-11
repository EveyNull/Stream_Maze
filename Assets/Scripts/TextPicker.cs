using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPicker : MonoBehaviour
{
    public Texture[] textures;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer Renderer = GetComponent<MeshRenderer>();
        if (Renderer)
        {
            Renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        }
    }
}
