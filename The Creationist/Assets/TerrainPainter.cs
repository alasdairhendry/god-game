using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPainter : MonoBehaviour {

    [SerializeField] private Shader terrainPainterShader;
    private Material terrainMaterial;

    private Material terrainPainterMaterialGrasslands;
    private RenderTexture _splatMapGrasslands;

    private Material terrainPainterMaterialForest;
    private RenderTexture _splatMapForest;

    private Material terrainPainterMaterialTundra;
    private RenderTexture _splatMapTundra;

    private void Start()
    {
        terrainPainterMaterialGrasslands = new Material(terrainPainterShader);
        terrainPainterMaterialGrasslands.SetVector("Color", Color.red);

        terrainPainterMaterialForest = new Material(terrainPainterShader);
        terrainPainterMaterialForest.SetVector("Color", Color.red);

        terrainPainterMaterialTundra = new Material(terrainPainterShader);
        terrainPainterMaterialTundra.SetVector("Color", Color.red);

        _splatMapGrasslands = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _splatMapForest = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _splatMapTundra = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);


        terrainMaterial = GetComponent<MeshRenderer>().material;
        terrainMaterial.SetTexture("_SplatGrasslands", _splatMapGrasslands);
        terrainMaterial.SetTexture("_SplatForest", _splatMapForest);
        terrainMaterial.SetTexture("_SplatTundra", _splatMapTundra);
    }

    private void Update()
    {
        DrawTexturesGrasslands();
        DrawTexturesForest();
        DrawTexturesTundra();
    }

    private void DrawTexturesGrasslands()
    {
        RenderTexture _tempGrasslands = RenderTexture.GetTemporary(_splatMapGrasslands.width, _splatMapGrasslands.height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(_splatMapGrasslands, _tempGrasslands);
        Graphics.Blit(_tempGrasslands, _splatMapGrasslands, terrainPainterMaterialGrasslands);
        RenderTexture.ReleaseTemporary(_tempGrasslands);
    }

    private void DrawTexturesForest()
    {
        RenderTexture _tempForest = RenderTexture.GetTemporary(_splatMapForest.width, _splatMapForest.height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(_splatMapForest, _tempForest);
        Graphics.Blit(_tempForest, _splatMapForest, terrainPainterMaterialForest);
        RenderTexture.ReleaseTemporary(_tempForest);
    }

    private void DrawTexturesTundra()
    {
        RenderTexture _tempTundra = RenderTexture.GetTemporary(_splatMapTundra.width, _splatMapTundra.height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(_splatMapTundra, _tempTundra);
        Graphics.Blit(_tempTundra, _splatMapTundra, terrainPainterMaterialTundra);
        RenderTexture.ReleaseTemporary(_tempTundra);
    }

    public void DrawGrasslands(Vector4 textureCoordinate)
    {
        terrainPainterMaterialGrasslands.SetVector("_Coordinate", new Vector4(textureCoordinate.x, textureCoordinate.y, 0, 0));        
    }

    public void DrawForest(Vector4 textureCoordinate)
    {
        terrainPainterMaterialForest.SetVector("_Coordinate", new Vector4(textureCoordinate.x, textureCoordinate.y, 0, 0));        
    }

    public void DrawTundra(Vector4 textureCoordinate)
    {
        terrainPainterMaterialTundra.SetVector("_Coordinate", new Vector4(textureCoordinate.x, textureCoordinate.y, 0, 0));        
    }

}
