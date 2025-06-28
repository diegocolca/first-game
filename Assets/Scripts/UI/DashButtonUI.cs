using UnityEngine;

public class DashButton : MonoBehaviour
{
    public Move move;

    public void onDashButtonPressed()
    {
        if (move != null)
        {
            StartCoroutine(
                move.Dash()
            );
        }
        Debug.Log("Dash button pressed");
    }
}
