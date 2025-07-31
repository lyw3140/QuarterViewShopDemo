using UnityEditor;
using UnityEngine;

public class ApplyTextureToAllMaterials
{
    [MenuItem("Tools/Apply Texture To All Materials")]
    public static void ApplyTexture()
    {
        string texturePath = "Assets/NecroPoly Free/Texture/T_colorPalette2048_CGT.png";
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

        if (texture == null)
        {
            Debug.LogError("❌ 텍스처를 찾을 수 없습니다: " + texturePath);
            return;
        }

        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        int count = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && mat.HasProperty("_BaseMap"))
            {
                Undo.RecordObject(mat, "Apply Texture");
                mat.SetTexture("_BaseMap", texture);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ {count}개의 머티리얼에 텍스처 자동 적용 완료!");
    }
}
