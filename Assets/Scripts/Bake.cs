using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;




public class Bake : MonoBehaviour
{
    [SerializeField] private Material screen;

    private RenderTexture rt;
    private Material QuadBaked;
    // Start is called before the first frame update
    void Start()
    {
        rt = new RenderTexture(1, 1024, 0);
        rt.name = "Baked Texture";
        
    }

    void bake()
    {
        var rend = GetComponent<Renderer>();
        var mat = rend.material;
        Graphics.Blit(mat.mainTexture, rt, mat);
        screen.mainTexture = rt;



    }

    // Update is called once per frame
    void Update()
    {

    }
}
