using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Player player;

    private void Start()
    {
        healthText = GetComponent<Text>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if(player.GetHealth() > 0)
        {
            healthText.text = player.GetHealth().ToString();
        }
        else
        {
            healthText.text = "0";
        }
    }
}
