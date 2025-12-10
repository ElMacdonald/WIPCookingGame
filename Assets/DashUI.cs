using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public RectTransform dashCooldownUI;
    private float maxWidth;
    public ThirdPersonMovement tpm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxWidth = dashCooldownUI.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceLastDash = Time.time - tpm.lastDashTime;
        if (timeSinceLastDash < tpm.dashCooldown)
        {
            float cooldownRatio = timeSinceLastDash / tpm.dashCooldown;
            dashCooldownUI.sizeDelta = new Vector2(maxWidth * cooldownRatio, dashCooldownUI.sizeDelta.y);
        }
        else
        {
            dashCooldownUI.sizeDelta = new Vector2(maxWidth, dashCooldownUI.sizeDelta.y);
        }
    }
}
