using UnityEngine;
using TMPro;

public class TMPTextColorChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.gray;
    [SerializeField] private Color pressedColor = Color.black;

    private void Awake()
    {
        if (targetText != null)
            targetText.color = normalColor;
    }

    // Event Trigger → Pointer Enter
    public void OnHoverEnter()
    {
        if (targetText == null) return;
        targetText.color = hoverColor;
    }

    // Event Trigger → Pointer Exit
    public void OnHoverExit()
    {
        if (targetText == null) return;
        targetText.color = normalColor;
    }

    // Event Trigger → Pointer Down
    public void OnPress()
    {
        if (targetText == null) return;
        targetText.color = pressedColor;
    }

    // Event Trigger → Pointer Up
    public void OnRelease()
    {
        if (targetText == null) return;
        targetText.color = hoverColor;
    }
}
