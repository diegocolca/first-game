using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarPlayerUI : MonoBehaviour
{
    public HealthPlayer playerHealth;
    public Image fillImage;
    public TextMeshProUGUI vidaTexto;

    void Update()
    {
        if (playerHealth != null && fillImage != null)
        {
            float fillAmount = (float)playerHealth.GetCurrentHealth() / playerHealth.maxHealth;
            fillImage.fillAmount = fillAmount;

            if (vidaTexto != null)
            {
                vidaTexto.text = $"{playerHealth.GetCurrentHealth()} / {playerHealth.maxHealth}";
            }
        }
    }
}
