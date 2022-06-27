using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[CustomEditor(typeof(FunctionLookupBaker))]
public class FunctionLookupBakerEditor : Editor
{
    private SerializedProperty myShader;
    private SerializedProperty MethodSelection;
    private SerializedProperty toggleResult;
    private SerializedProperty myTextureWidth;
    private SerializedProperty myTextureHeight;
    private SerializedProperty screen;

    private SerializedProperty testInt;


    private Material myMaterial;
    private GUIStyle myStyle;
    private FunctionLookupBaker flb;
    private Texture2D myTex;

    private Rect lastRect;
    private Rect controlRect;

    private string defaultShaderPath;
    private string shaderPath;
    private string inputText;
    private string[] shaderLines;

    private bool drawChart;

    public void OnEnable()
    {
        //!target
        flb = (FunctionLookupBaker)serializedObject.targetObject;
        myTex = new Texture2D(256,256);

        //alternative to string chk
        myTextureHeight = serializedObject.FindProperty(nameof(myTextureHeight));
        myTextureWidth = serializedObject.FindProperty(nameof(myTextureWidth));
        
        MethodSelection = serializedObject.FindProperty("MethodSelection");
        toggleResult = serializedObject.FindProperty("toggleResult");
        testInt = serializedObject.FindProperty("testInt");

        screen = serializedObject.FindProperty("screen");
        
        myMaterial = (Material)screen.objectReferenceValue;
        if (!myMaterial)
        {
            myMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Quad.mat");
        }


        myShader = serializedObject.FindProperty("myShader");
        defaultShaderPath = "Assets/Materials/Shaders/testShader.shader";

        shaderPath = AssetDatabase.GetAssetPath(myShader.objectReferenceValue);
        myMaterial.shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
        inputText = "cos((x - y)*4.0)";

        ReadShader();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SetLargeTitle("Function Lookup Baker");
        SetInfoText("This tool allows baking lookup tables from " +
                    "mathematical functions to use in shaders. Type the " +
                    "function in the text field and click the bake button. ");

        //EditorGUILayout.PropertyField(screen);

        SetLowerTitle("e.g. \"cos((x - y)*4.0)\"");

        SetMethodSelection();
        SetTexturePreview(myTex, myMaterial, myTextureWidth.intValue, myTextureHeight.intValue);
        SetResultTypeButton("Result as graph");

        SetTextureDataInputs();
     
        SetBakeButton("Bake");
        serializedObject.ApplyModifiedProperties();
        //Debug.Log(myShader.objectReferenceValue);
        //Debug.Log(myMaterial.shader.name);
        //Debug.Log(shaderPath);
        //flb.getInfo();
        Debug.Log(testInt.intValue);
    }

    private void SetTextureAreaGizmo(Rect rect)
    {
        Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.red);
    }

    private void SetMethodSelection()
    {
        
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(MethodSelection);
        if (MethodSelection.enumValueIndex == 0)
        {
            SetInputField(Color.white);
            SetRenderButton("Render Formula", true);
        }
        else
        {   
            // if GUI.changed
            
            EditorGUILayout.PropertyField(myShader);
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Shader changed");
                shaderPath = AssetDatabase.GetAssetPath(myShader.objectReferenceValue);
                myMaterial.shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
                ReadShader();
            }
            SetRenderButton("Render Shader", false);
        }
    }

    private void SetLargeTitle(string text)
    {
        myStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
        myStyle.alignment = TextAnchor.MiddleCenter; ;
        GUI.color = Color.white;
        EditorGUILayout.LabelField(text, myStyle);
        EditorGUILayout.Space();
    }

    private void SetLowerTitle(string text)
    {
        myStyle = new GUIStyle(EditorStyles.miniLabel);
        myStyle.alignment = TextAnchor.MiddleCenter;
        GUI.color = Color.white;
        EditorGUILayout.LabelField(text, myStyle);
        EditorGUILayout.Space();
    }

    private void SetInputField(Color color)
    {
        EditorGUILayout.Space();
        myStyle = new GUIStyle(EditorStyles.textField);
        GUI.color = color;
        inputText = EditorGUILayout.TextField(inputText, myStyle);

    }

    private void SetRenderButton(string text, bool writable)
    {
        EditorGUILayout.Space();
        myStyle = new GUIStyle(EditorStyles.miniButtonMid);
        myStyle.fixedHeight = 32;
       
        if (GUILayout.Button(text, myStyle) && writable)
        {
                shaderPath = defaultShaderPath;
                myMaterial.shader = AssetDatabase.LoadAssetAtPath<Shader>(defaultShaderPath);
                ReadShader();
                WriteShader();
        }
    }

    private void SetTexturePreview(Texture tex, Material mat, int width, int height)
    {
        myStyle.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.GetControlRect(false, 10);
        controlRect = EditorGUILayout.GetControlRect(false, 256, myStyle);
        lastRect = new Rect(EditorGUIUtility.currentViewWidth / 2 - 128, controlRect.y, width, height);
        EditorGUI.DrawPreviewTexture(lastRect, tex, mat, ScaleMode.ScaleAndCrop, 1.0f);
        //EditorGUI.DrawRect()
        //SetTextureAreaGizmo(lastRect);
        SetCartesianChart(drawChart);
        EditorGUILayout.Space();
   
    }

    private void SetCartesianChart(bool drawChart)
    {
        if (drawChart)
        {
            Handles.color = Color.green;
            Handles.DrawDottedLine(new Vector2(lastRect.xMin, lastRect.center.y), new Vector2(lastRect.xMax, lastRect.center.y), 0.5f);
            Handles.DrawDottedLine(new Vector2(lastRect.center.x, lastRect.yMin), new Vector2(lastRect.center.x, lastRect.yMax), 0.35f);
            myStyle = new GUIStyle(EditorStyles.miniLabel);
            myStyle.alignment = TextAnchor.MiddleCenter;
            GUI.color = Color.green;
            GUI.Label(new Rect(lastRect.xMin, lastRect.center.y+4, 30,16), "-1.0", myStyle);
            GUI.Label(new Rect(lastRect.xMax-30, lastRect.center.y-20, 30,16), "+1.0", myStyle);
            GUI.Label(new Rect(lastRect.center.x-30, lastRect.yMax-20, 30,16), "-1.0", myStyle);
            GUI.Label(new Rect(lastRect.center.x, lastRect.yMin, 30,16), "1.0", myStyle);
            GUI.color = Color.white;
        }

        
    }
    private void SetInfoText(string text)
    {
        myStyle = new GUIStyle(EditorStyles.label);
        myStyle.alignment = TextAnchor.MiddleLeft;
        myStyle.wordWrap = true;
        GUI.color = Color.white;
        EditorGUILayout.LabelField(text, myStyle);
        EditorGUILayout.Space();
    }

    private void SetTextureDataInputs()
    {
        myStyle = new GUIStyle(EditorStyles.textField);
        EditorGUILayout.Space();

        ///check this

        myTextureHeight.intValue = EditorGUILayout.IntField("tex height", Mathf.Min(myTextureHeight.intValue,256));
        myTextureWidth.intValue = EditorGUILayout.IntField("tex width", Mathf.Min(myTextureWidth.intValue,256));

        EditorGUILayout.Space();
    }

    private void SetResultTypeButton(string text)
    {
        EditorGUILayout.Space();
        myStyle = EditorStyles.miniButtonMid;

        toggleResult.boolValue = GUILayout.Toggle(toggleResult.boolValue, "Show Result as Graph", myStyle);

        if (toggleResult.boolValue)
        {
            myMaterial.EnableKeyword("RESULT_GRAPH");
            drawChart = true;
            
            
        }
        else
        {
            myMaterial.DisableKeyword("RESULT_GRAPH");
            drawChart = false;
        }
    }

    private void SetBakeButton(string text)
    {
        myStyle = new GUIStyle(EditorStyles.miniButtonMid);
        myStyle.fixedHeight = 32;
        EditorGUILayout.Space();
        if (GUILayout.Button(text, myStyle))
        {
            flb.bake("hooop bakeee");
        }
    }

    private void ReadShader()
    {
        shaderLines = File.ReadAllLines(shaderPath);
    }

    private void WriteShader()
    {
        Debug.Log(shaderLines[58]);
        shaderLines[58] = inputText;
        Debug.Log(shaderLines[58]);
        File.WriteAllLines(shaderPath, shaderLines);
        AssetDatabase.Refresh();
    }
}
