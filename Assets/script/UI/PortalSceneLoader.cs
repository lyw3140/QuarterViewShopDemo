using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSceneLoader : MonoBehaviour
{
    public void LoadVillageMap()
    {
        SceneManager.LoadScene("VillageMap");  // 씬 이름 정확히!
    }
}
