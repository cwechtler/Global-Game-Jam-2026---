using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthBar.value = playerController.Health;
    }

    public void ReduceHealthBar(int amount)
    {
        
    }
}
