using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput pi;
    private InputAction moveAction;
    private InputAction lookAction;

    public TopDownMove tdm;
    public CookingStation cs;
    public CuttingBoardMicrogame cbm;
    public IngredientInteraction ii;
    public ThirdPersonMovement tpm;
    public ThirdPersonCamera tpc;

    public BamboomStick bs;
    public Weapon weap;

    private int playerNum;

    void Awake()
    {
        // PlayerInput is guaranteed to be on same GameObject because PlayerInputManager spawns it
        pi = GetComponent<PlayerInput>();

        // Get the correct per-player input actions
        var actions = pi.actions;
        moveAction = actions["Move"];
        lookAction = actions["Look"];

        // Determine which player this is 
        playerNum = pi.playerIndex + 1;

        // Find associated objects in scene (based on consistent naming)
        tdm = GameObject.Find($"Player{playerNum}").GetComponent<TopDownMove>();
        cs = GameObject.Find($"Cooking Station P{playerNum}").GetComponent<CookingStation>();
        cbm = GameObject.Find($"Cutting Board P{playerNum}").GetComponent<CuttingBoardMicrogame>();
        ii = GameObject.Find($"Player{playerNum}").GetComponent<IngredientInteraction>();

        var all = FindObjectsOfType<ThirdPersonMovement>(true);

        foreach (var m in all)
        {
            if(playerNum == 1)
                if (m.name == "Soosh new") tpm = m;
            if (playerNum == 2)
                if (m.name == "Stew new")  tpm = m;
        }

        var allCams = FindObjectsOfType<ThirdPersonCamera>(true);

        foreach (var c in allCams)
        {
            if (playerNum == 1)
                if (c.name == "p1 Cam") tpc = c;
            if (playerNum == 2)
                if (c.name == "p2 Cam") tpc = c;
        }

        var allWeapons = FindObjectsOfType<Weapon>(true);

        foreach (var w in allWeapons)
        {
            if (playerNum == 1)
                if (w.name.Contains("1") && w.name.Contains("Shrimp")) weap = w;
            if (playerNum == 2)
                if (w.name.Contains("2") && w.name.Contains("Shrimp")) weap = w;
        }

        var allBamboomSticks = FindObjectsOfType<BamboomStick>(true);
        foreach (var b in allBamboomSticks)
        {
            if (playerNum == 1)
                if (b.name.Contains("1") && b.name.Contains("bamboomstick")) bs = b;
            if (playerNum == 2)
                if (b.name.Contains("2") && b.name.Contains("bamboomstick")) bs = b;
        }

        Debug.Log($"[InputHandler] Bound Player {playerNum} to controls.");
    }

    void FixedUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector2 look = lookAction.ReadValue<Vector2>();
        tdm.MovePlayer(move.x, move.y);
        tpm.MovePlayer(move.x, move.y);
        tpc.MoveCamera(look.x * Time.deltaTime, look.y * Time.deltaTime);
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
        // If pressing button during combat phase, send to ThirdPersonMovement
        tpm.TakeInput(buttonName == "A");
        weap.TakeInput(buttonName);
        bs.TakeInput(buttonName);
    }
}
