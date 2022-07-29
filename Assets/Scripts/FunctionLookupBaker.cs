using UnityEngine;
using UnityEditor;

using System.IO;

public class FunctionLookupBaker : MonoBehaviour
{
    [SerializeField] private Material screen;
    private enum MyEnum { MathFormula, CustomShader };
    [SerializeField] private int deneme;
    [SerializeField] private Shader myShader;
    [SerializeField] private bool toggleResult = false;
    [SerializeField] private int myTextureHeight = 65; //default value in compile time
    [SerializeField] private int myTextureWidth = 256;
    [SerializeField] private MyEnum MethodSelection;


    private RenderTexture rt;

    public void getInfo()
    {

        Debug.Log(myTextureHeight + "," + myTextureWidth);
    }

    public void bake(string name, int w, int h, Material myMat)
    {

        if (!string.IsNullOrEmpty(name))
        {
            rt = new RenderTexture(w, h, 0);
            rt.name = name;
            Graphics.Blit(myMat.mainTexture, rt, myMat);

            RenderTexture.active = rt;

            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            RenderTexture.active = null;

            byte[] bytes;
            bytes = tex.EncodeToPNG();

            string texturePath = "Assets/Baked/" + name + ".png";
            File.WriteAllBytes(texturePath, bytes);
            AssetDatabase.ImportAsset(texturePath);
        }
        else
        {
            Debug.LogError("Enter a filename!");
        }

    }



}
