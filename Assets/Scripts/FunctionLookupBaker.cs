using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;
using System.IO;

public class FunctionLookupBaker : MonoBehaviour
{
    [SerializeField] private Material screen;
    private enum MyEnum { MathFormula, CustomShader };
    [SerializeField] private int deneme;
    [SerializeField] private Shader myShader;
    [SerializeField] private bool toggleResult = false;
    [SerializeField] private int myTextureHeight = 65; //default value in compile time
    [SerializeField] private int testInt = 88; //default value in compile time
    [SerializeField] private int myTextureWidth = 256;
    [SerializeField] private MyEnum MethodSelection;

    
    private RenderTexture rt;

    //private void OnEnable()
    //{
    //    rt = new RenderTexture(1, 1024, 0);
    //    rt.name = "Baked Texture";
    //}
    public void getInfo()
    {

        Debug.Log(myTextureHeight+","+myTextureWidth);
    }
    
    public void bake(string test)
    {
        screen = GetComponentInChildren<Material>();

        //Graphics.Blit(myMaterial.mainTexture, rt, myMaterial);
        Debug.Log(screen.name);

    }



}
