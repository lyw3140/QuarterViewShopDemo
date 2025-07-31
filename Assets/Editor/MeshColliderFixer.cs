using UnityEditor;
using UnityEngine;

public class MeshColliderFixer : EditorWindow
{
    [MenuItem("Tools/Fix Mesh Colliders")]
    static void FixMeshColliders()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        int fixedCount = 0;

        foreach (GameObject obj in selectedObjects)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

            if (meshFilter != null && meshCollider != null)
            {
                if (meshCollider.sharedMesh == null && meshFilter.sharedMesh != null)
                {
                    meshCollider.sharedMesh = meshFilter.sharedMesh;
                    fixedCount++;
                }
            }

            // 자식까지 재귀 탐색
            foreach (MeshFilter childFilter in obj.GetComponentsInChildren<MeshFilter>())
            {
                MeshCollider childCollider = childFilter.GetComponent<MeshCollider>();
                if (childCollider != null && childCollider.sharedMesh == null && childFilter.sharedMesh != null)
                {
                    childCollider.sharedMesh = childFilter.sharedMesh;
                    fixedCount++;
                }
            }
        }

        Debug.Log($"🔧 수정된 MeshCollider 수: {fixedCount}");
    }
}
