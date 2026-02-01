using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Slider playerHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        //playerHealthBar.value = playerController.Health;
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
