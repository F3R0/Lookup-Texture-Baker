using UnityEngine;
using UnityEditor;
using System.IO;

public class LookupBaker : MonoBehaviour
{
    [SerializeField] private Material screen;
    private enum MyEnum { MathFormula, CustomShader };

    [SerializeField] private Shader sourceShader;
    [SerializeField] private int targetTextureHeight;
    [SerializeField] private int targetTextureWidth;
    [SerializeField] private MyEnum MethodSelection;

    private RenderTexture rt;
    private Texture2D texture;

    public void Bake(string name, int width, int height, Material mat)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Enter a filename!");
            return;
        }
        rt = new RenderTexture(width, height, 0);
        Graphics.Blit(mat.mainTexture, rt, mat); //add -1 as a parameter to bake multiple passes

        texture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        byte[] bytes = texture.EncodeToPNG();

        string texturePath = $"Assets/Baked/{name}.png";
        File.WriteAllBytes(texturePath, bytes);
        AssetDatabase.ImportAsset(texturePath);

        // Dispose all allocated objects
        RenderTexture.active = null;
        DestroyImmediate(texture);
        DestroyImmediate(rt);
    }




}
