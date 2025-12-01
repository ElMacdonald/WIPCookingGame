using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CuttingBoardMicrogame : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject cuttingBoardPanel;
    public GameObject[] cuttingBoardButtonImages;  // Top 4 + bottom 4 = 8
    public Sprite[] cuttingBoardButtonSprites;
    public Sprite[] cuttingBoardButtonHighlightSprites;
    public Sprite[] cuttingBoardIngredientSprites;
    public Image cuttingBoardIngredientImage;

    [Header("Cut Result Sprites")]
    public Sprite perfectCutSprite;
    public Sprite imperfectCutSprite;

    private string[] buttonNames = { "A", "B", "X", "Y", "LT", "RT" };
    public string[] buttonOne = new string[4];
    public string[] buttonTwo = new string[4];

    private int currentCut = 0;
    private bool gameActive = false;
    private bool inputLocked = false;

    [Header("Settings")]
    public float feedbackDuration = 0.4f;  // seconds before next cut
    public float inputDelay = 0.25f;       // delay after opening menu
    public float simultaneousPressWindow = 10f; // max time between top & bottom press

    public int playerNum;
    public GameObject knifePrefab;

    private GameObject currentKnifeInstance;
    private PlayerControls input;

    // Tracking simultaneous presses
    private bool topPressed = false;
    private bool bottomPressed = false;
    private float firstPressTime = 0f;

    void Start()
    {
        input = new PlayerControls();
        input.Enable();
    }


    // Opens the cutting board minigame UI
    public void OpenMenu()
    {
        if (playerNum == 1)
        {
            GameObject player = GameObject.Find("Player1");
            if (player != null)
            {
                IngredientHolding ih = player.GetComponent<IngredientHolding>();
                if (ih != null && ih.ingredientCurrentlyHeld != null)
                {
                    switch (ih.ingredientCurrentlyHeld.name)
                    {
                        case "Salmon":
                            cuttingBoardIngredientImage.sprite = cuttingBoardIngredientSprites[0];
                            break;
                        case "Bamboo":
                            cuttingBoardIngredientImage.sprite = cuttingBoardIngredientSprites[1];
                            break;
                        case "Shrimp":
                            cuttingBoardIngredientImage.sprite = cuttingBoardIngredientSprites[2];
                            break;
                        case null:
                            Debug.Log("Ingredient not cuttable!");
                            break;
                    }
                    //disable player movement while in minigame
                    TopDownMove tdm = player.GetComponent<TopDownMove>();
                    if (tdm != null)
                    {
                        tdm.canMove = false;
                    }
                }
            }
        }
        else
        {
            GameObject player = GameObject.Find("Player2");
            if (player != null)
            {
                IngredientHolding ih = player.GetComponent<IngredientHolding>();
                if (ih != null && ih.ingredientCurrentlyHeld != null)
                {
                    switch (ih.ingredientCurrentlyHeld.name)
                    {
                        case "Salmon":
                            cuttingBoardIngredientImage.sprite = cuttingBoardIngredientSprites[0];
                            break;
                        case "Bamboo":
                            cuttingBoardIngredientImage.sprite = cuttingBoardIngredientSprites[1];
                            break;
                        case "Shrimp":
                            cuttingBoardIngredientImage.sprite = cuttingBoardIngredientSprites[2];
                            break;
                        case null:
                            Debug.Log("Ingredient not cuttable!");
                            break;
                    }
                    //disable player movement while in minigame
                    TopDownMove tdm = player.GetComponent<TopDownMove>();
                    if (tdm != null)
                    {
                        tdm.canMove = false;
                    }
                }
                
            }
        }

        cuttingBoardPanel.SetActive(true);
        currentCut = 0;
        topPressed = false;
        bottomPressed = false;
        firstPressTime = 0f;

        foreach (GameObject btn in cuttingBoardButtonImages)
        {
            btn.SetActive(true);
        }

        StartCoroutine(EnableAfterDelay(inputDelay));
    }

    // Enables input after a delay when opening the menu
    IEnumerator EnableAfterDelay(float delay)
    {
        inputLocked = true;
        SetButtonImages();
        HighlightNextCut();
        yield return new WaitForSeconds(delay);
        inputLocked = false;
        gameActive = true;
    }

    // Sets random button images for the cutting board buttons
    void SetButtonImages()
    {
        for (int i = 0; i < 4; i++)
        {
            int randIndex1 = Random.Range(0, buttonNames.Length);
            cuttingBoardButtonImages[i].GetComponent<Image>().sprite = cuttingBoardButtonSprites[randIndex1];
            buttonOne[i] = buttonNames[randIndex1];

            int randIndex2;
            // Keep picking until it's different from the top button
            do
            {
                randIndex2 = Random.Range(0, buttonNames.Length);
            } while (randIndex2 == randIndex1);

            cuttingBoardButtonImages[i + 4].GetComponent<Image>().sprite = cuttingBoardButtonSprites[randIndex2];
            buttonTwo[i] = buttonNames[randIndex2];
        }
    }

    // Highlights the next cut's buttons
    void HighlightNextCut()
    {
        if (currentCut >= 4) return;

        for (int i = 0; i < cuttingBoardButtonImages.Length; i++)
        {
            Image img = cuttingBoardButtonImages[i].GetComponent<Image>();
            if (img != null && i < 4)
                img.sprite = cuttingBoardButtonSprites[System.Array.IndexOf(buttonNames, buttonOne[i])];
            else if (img != null && i >= 4)
                img.sprite = cuttingBoardButtonSprites[System.Array.IndexOf(buttonNames, buttonTwo[i - 4])];
        }

        int topIndex = currentCut;
        int bottomIndex = currentCut + 4;

        Image topImg = cuttingBoardButtonImages[topIndex].GetComponent<Image>();
        Image bottomImg = cuttingBoardButtonImages[bottomIndex].GetComponent<Image>();

        if (!topPressed && topImg != null)
            topImg.sprite = cuttingBoardButtonHighlightSprites[System.Array.IndexOf(buttonNames, buttonOne[currentCut])];
        else if (!bottomPressed && bottomImg != null)
            bottomImg.sprite = cuttingBoardButtonHighlightSprites[System.Array.IndexOf(buttonNames, buttonTwo[currentCut])];
    }


    // Handles input from InputHandler
    public void takeInput(string button)
    {
        if (!gameActive || inputLocked) return;

        HandleInput(button);
    }

    void Update()
    {
        if (!gameActive || inputLocked) return;

        // Fail if second button not pressed within time window
        if ((topPressed || bottomPressed) && Time.time - firstPressTime > simultaneousPressWindow)
        {
            StartCoroutine(ShowFeedback(false));
        }

        // This is your placeholder "always false" check for manual controller input
        foreach (string button in buttonNames)
        {
            if (false) // keep this as is so nothing happens yet
            {
                HandleInput(button);
                break;
            }
        }
    }


    // Handles input logic for button presses
    void HandleInput(string pressedButton)
    {
        int topIndex = currentCut;
        int bottomIndex = currentCut;

        bool anyNewPress = false;

        if (pressedButton == buttonOne[currentCut] && !topPressed)
        {
            topPressed = true;
            anyNewPress = true;
        }
        if (pressedButton == buttonTwo[currentCut] && !bottomPressed)
        {
            bottomPressed = true;
            anyNewPress = true;
        }

        if (anyNewPress)
        {
            if (topPressed && bottomPressed)
            {
                StartCoroutine(ShowFeedback(true));
            }
            else
            {
                if (firstPressTime == 0f)
                    firstPressTime = Time.time;
                HighlightNextCut();
            }
        }
        else
        {
            StartCoroutine(ShowFeedback(false));
        }
    }

    // Shows feedback for success or failure of the cut
    IEnumerator ShowFeedback(bool success)
    {
        gameActive = false;
        inputLocked = true;

        int topIndex = currentCut;
        int bottomIndex = currentCut + 4;

        Image topSprite = cuttingBoardButtonImages[topIndex].GetComponent<Image>();
        Image bottomSprite = cuttingBoardButtonImages[bottomIndex].GetComponent<Image>();

        Sprite originalTop = topSprite.sprite;
        Sprite originalBottom = bottomSprite.sprite;

        topSprite.sprite = success ? perfectCutSprite : imperfectCutSprite;
        bottomSprite.sprite = success ? perfectCutSprite : imperfectCutSprite;

        if (success)
        {
            RectTransform topRect = cuttingBoardButtonImages[topIndex].GetComponent<RectTransform>();
            RectTransform bottomRect = cuttingBoardButtonImages[bottomIndex].GetComponent<RectTransform>();

            Vector3 midpoint = (topRect.position + bottomRect.position) / 2f;
            currentKnifeInstance = Instantiate(knifePrefab, midpoint, Quaternion.identity, cuttingBoardPanel.transform);
        }

        yield return new WaitForSeconds(feedbackDuration);

        if (currentKnifeInstance != null)
            Destroy(currentKnifeInstance);

        if (success)
        {
            cuttingBoardButtonImages[topIndex].SetActive(false);
            cuttingBoardButtonImages[bottomIndex].SetActive(false);

            currentCut++;
            topPressed = false;
            bottomPressed = false;
            firstPressTime = 0f;

            if (currentCut >= buttonOne.Length)
                WinMinigame();
            else
            {
                HighlightNextCut();
                gameActive = true;
            }
        }
        else
        {
            topSprite.sprite = originalTop;
            bottomSprite.sprite = originalBottom;
            topPressed = false;
            bottomPressed = false;
            firstPressTime = 0f;

            FailMinigame();
        }

        inputLocked = false;
        //reenable player input
        

    }

    // Function that takes player number and button name, returns if that button was pressed this frame
    bool GetButtonDown(string buttonName)
    {
        if (playerNum == 1)
        {
            switch (buttonName)
            {
                case "A": return input.Player.A.ReadValue<float>() > 0;
                case "B": return input.Player.B.ReadValue<float>() > 0;
                case "X": return input.Player.X.ReadValue<float>() > 0;
                case "Y": return input.Player.Y.ReadValue<float>() > 0;
                case "LT": return input.Player.LT.ReadValue<float>() > 0;
                case "RT": return input.Player.RT.ReadValue<float>() > 0;
                default: return false;
            }
        }
        else
        {
            switch (buttonName)
            {
                case "A": return input.Player.A.ReadValue<float>() > 0;
                case "B": return input.Player.B.ReadValue<float>() > 0;
                case "X": return input.Player.X.ReadValue<float>() > 0;
                case "Y": return input.Player.Y.ReadValue<float>() > 0;
                case "LT": return input.Player.LT.ReadValue<float>() > 0;
                case "RT": return input.Player.RT.ReadValue<float>() > 0;
                default: return false;
            }
        }
    }

    // For when the minigame is failed, gives the player the imperfectly cut ingredient
    void FailMinigame()
    {
        Debug.Log("You failed the cut!");
        cuttingBoardPanel.SetActive(false);
        gameActive = false;

        GameObject player = playerNum == 1 ? GameObject.Find("Player1") : GameObject.Find("Player2");
        if (player != null)
        {
            IngredientInteraction ii = player.GetComponent<IngredientInteraction>();
            IngredientHolding ih = player.GetComponent<IngredientHolding>();
            if (ih != null && ih.ingredientCurrentlyHeld != null)
            {
                ih.ingredientCurrentlyHeld.quality = "Imperfectly Cut " + ih.ingredientCurrentlyHeld.quality;
                ih.trashIngredient();
            }
            if (ii != null)
                ii.cutting = false;

            TopDownMove tdm = player.GetComponent<TopDownMove>();
            if (tdm != null)
            {
                tdm.canMove = true;
            }
        }

    }


    // For when the minigame is won, gives the player the cut ingredient
    void WinMinigame()
    {
        Debug.Log("All cuts perfect!");
        cuttingBoardPanel.SetActive(false);
        gameActive = false;

        GameObject player = playerNum == 1 ? GameObject.Find("Player1") : GameObject.Find("Player2");
        if (player != null)
        {
            IngredientInteraction ii = player.GetComponent<IngredientInteraction>();
            IngredientHolding ih = player.GetComponent<IngredientHolding>();
            if (ih != null && ih.ingredientCurrentlyHeld != null)
            {
                ih.ingredientCurrentlyHeld.quality = "Perfectly Cut " + ih.ingredientCurrentlyHeld.quality;
                ih.ingredientCurrentlyHeld.name = "Cut " + ih.ingredientCurrentlyHeld.name;
            }
            if (ii != null)
                ii.cutting = false;
            TopDownMove tdm = player.GetComponent<TopDownMove>();
            if (tdm != null)
            {
                tdm.canMove = true;
            }
        }
    }
}
