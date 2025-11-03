using UnityEngine;

/// <summary>
/// This script continuously rotates the scene's active skybox material.
/// To use it, attach this script to any active GameObject in your scene,
/// such as the Main Camera or an empty "Managers" object.
/// </summary>
public class SkyboxRotator : MonoBehaviour
{
    [Tooltip("The speed at which the skybox will rotate, in degrees per second. " +
             "Positive values rotate one way, negative values rotate the other.")]
    // By making this variable public, it will appear in the Inspector window
    // so you can change the speed without editing the code.
    public float rotationSpeed = 1.0f;

    // We use a private variable to track the current rotation.
    // This is slightly more efficient than calling GetFloat() from the material every frame.
    private float currentRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        // First, check if a skybox material is actually assigned in the render settings
        // to prevent errors if no skybox is set.
        if (RenderSettings.skybox == null)
        {
            // If there's no skybox, do nothing.
            return;
        }

        // Calculate how much to rotate in this single frame.
        // We multiply by Time.deltaTime to make the rotation speed
        // consistent regardless of your game's frame rate.
        // This means rotationSpeed is "degrees per second".
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        // Add this frame's rotation to our current rotation tracker.
        currentRotation += rotationThisFrame;

        // Use the modulo operator (%) to wrap the rotation value back to 0
        // once it reaches 360 degrees. This keeps the number clean and manageable.
        currentRotation = currentRotation % 360f;

        // Apply the calculated rotation to the skybox material's "_Rotation" property.
        // "_Rotation" is the standard name for the rotation property in most of
        // Unity's built-in skybox shaders (like Panoramic and 6-Sided).
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);
    }
}
