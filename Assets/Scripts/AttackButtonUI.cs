using UnityEngine;

public class AttackButtonUI : MonoBehaviour
{
    public PlayerAttack playerAttack;

    public void OnAttackButtonPressed()
    {
        if (playerAttack != null)
        {
            playerAttack.TriggerAttack();
        }
    }
}
