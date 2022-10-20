using UnityEditor;
using UnityEngine;

public class SpriteImportSettings : AssetPostprocessor
{
    private void OnPostprocessSprites(Texture2D texture, Sprite[] sprites) {
        //Log.DebugMessage(DebugGroup.AssetPipeline, "Importing sprites!");
        var importer = (TextureImporter)assetImporter;
        importer.filterMode = FilterMode.Point;
        importer.spritePixelsPerUnit = 16;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.maxTextureSize = 128;
        
    }
}