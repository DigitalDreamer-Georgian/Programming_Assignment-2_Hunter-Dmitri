using UnityEngine;
using UnityEngine.UI;
//health bar code for the health bar slider on enemys and player
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    void Update()
    {
        
    }
}
