using TMPro;
using UnityEngine;

public class LoopDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI loopText;

    [Header("Loop Info")]
    public int currentLoop = 1;
    public int maxLoop = 10;

    void Start()
    {
        UpdateLoopText();
    }

    public void UpdateLoop(int current, int max)
    {
        currentLoop = current;
        maxLoop = max;
        UpdateLoopText();
    }

    void UpdateLoopText()
    {
        if (loopText != null)
            loopText.text = $"∑Á«¡ {currentLoop} / {maxLoop}";
    }
}
