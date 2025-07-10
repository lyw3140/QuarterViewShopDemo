using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static GameObject instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 제거
        }
    }
}
