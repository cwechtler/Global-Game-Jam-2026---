using UnityEngine;
using UnityEngine.UI;

public class CandleFlickerUI : MonoBehaviour
{
    [Header("Light Images")]
    [SerializeField] private Image lightA;
    [SerializeField] private Image lightB;

    [Header("Shadow Image")]
    [SerializeField] private RectTransform shadow;

    [Header("Color Flicker")]
    [SerializeField] private Color brightColor = Color.white;
    [SerializeField] private Color dimColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    [SerializeField] private float colorSpeed = 3f;

    [Header("Shadow Motion")]
    [SerializeField] private Vector2 shadowMoveRange = new Vector2(6f, 3f);
    [SerializeField] private float shadowSpeed = 2f;

    private Vector2 shadowStartPos;
    private float seedA;
    private float seedB;

    private void Awake()
    {
        shadowStartPos = shadow.anchoredPosition;
        seedA = Random.Range(0f, 100f);
        seedB = Random.Range(0f, 100f);
    }

    private void Update()
    {
        float tA = Mathf.PerlinNoise(seedA, Time.time * colorSpeed);
        float tB = Mathf.PerlinNoise(seedB, Time.time * (colorSpeed * 0.8f));

        lightA.color = Color.Lerp(dimColor, brightColor, tA);
        lightB.color = Color.Lerp(dimColor, brightColor, tB);

        float x = (Mathf.PerlinNoise(seedA + 50f, Time.time * shadowSpeed) - 0.5f)
                    * shadowMoveRange.x;
        float y = (Mathf.PerlinNoise(seedB + 25f, Time.time * shadowSpeed) - 0.5f)
                    * shadowMoveRange.y;

        shadow.anchoredPosition = shadowStartPos + new Vector2(x, y);
    }
}
