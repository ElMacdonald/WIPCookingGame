using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput pi;
    private InputAction moveAction;

    public TopDownMove tdm;
    public CookingStation cs;
    public CuttingBoardMicrogame cbm;
    public IngredientInteraction ii;

    private int playerNum;

    void Awake()
    {
        // PlayerInput is guaranteed to be on same GameObject because PlayerInputManager spawns it
        pi = GetComponent<PlayerInput>();

        // Get the correct per-player input actions
        var actions = pi.actions;
        moveAction = actions["Move"];

        // Determine which player this is (0-based → convert to 1-based)
        playerNum = pi.playerIndex + 1;

        // Find associated objects in scene (based on consistent naming)
        tdm = GameObject.Find($"Player{playerNum}").GetComponent<TopDownMove>();
        cs = GameObject.Find($"Cooking Station P{playerNum}").GetComponent<CookingStation>();
        cbm = GameObject.Find($"Cutting Board P{playerNum}").GetComponent<CuttingBoardMicrogame>();
        ii = GameObject.Find($"Player{playerNum}").GetComponent<IngredientInteraction>();

        Debug.Log($"[InputHandler] Bound Player {playerNum} to controls.");
    }

    void FixedUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        tdm.MovePlayer(move.x, move.y);
    }

    // ONE BUTTON HANDLER FOR ALL A/B/X/Y/LT/RT
    public void OnButton(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        string buttonName = ctx.action.name;

        Debug.Log($"Player {playerNum} pressed {buttonName}");

        // Send button name to CuttingBoard input system
        cbm.takeInput(buttonName);

        // If pressing button near cooking station performs interaction:
        ii.takeInput(buttonName);
    }
}
