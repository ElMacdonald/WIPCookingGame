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

    [Header("Cut Result Sprites")]
    public Sprite perfectCutSprite;
    public Sprite imperfectCutSprite;

    private string[] buttonNames = { "A", "B", "X", "Y", "LT", "RT" };
    public string[] buttonOne = new string[4];
    public string[] buttonTwo = new string[4];

    private int currentCut = 0;
    private bool waitingForSecondInput = false;
    private bool gameActive = false;
    private bool inputLocked = false;

    [Header("Settings")]
    public float feedbackDuration = 0.4f;  // seconds before next cut
    public float inputDelay = 0.25f;       // delay after opening menu

    public int playerNum;
    public GameObject knifePrefab;

    private GameObject currentKnifeInstance;

    public void OpenMenu()
    {
        cuttingBoardPanel.SetActive(true);
        currentCut = 0;
        waitingForSecondInput = false;

        // Re-enable all buttons in case they were hidden last round
        foreach (GameObject btn in cuttingBoardButtonImages)
        {
            btn.SetActive(true);
        }

        StartCoroutine(EnableAfterDelay(inputDelay)); // delay input so open button isn’t detected
    }

    IEnumerator EnableAfterDelay(float delay)
    {
        inputLocked = true;
        SetButtonImages(); // preload button images
        HighlightNextCut(); // highlight first pair
        yield return new WaitForSeconds(delay);
        inputLocked = false;
        gameActive = true;
    }

    void SetButtonImages()
    {
        for (int i = 0; i < 4; i++)
        {
            int randIndex1 = Random.Range(0, buttonNames.Length);
            cuttingBoardButtonImages[i].GetComponent<Image>().sprite = cuttingBoardButtonSprites[randIndex1];
            buttonOne[i] = buttonNames[randIndex1];

            int randIndex2 = Random.Range(0, buttonNames.Length);
            cuttingBoardButtonImages[i + 4].GetComponent<Image>().sprite = cuttingBoardButtonSprites[randIndex2];
            buttonTwo[i] = buttonNames[randIndex2];
        }
    }

    void HighlightNextCut()
    {
        if (currentCut >= 4) return;

        // Reset all to normal first
        for (int i = 0; i < cuttingBoardButtonImages.Length; i++)
        {
            Image img = cuttingBoardButtonImages[i].GetComponent<Image>();
            if (img != null && i < 4)
                img.sprite = cuttingBoardButtonSprites[System.Array.IndexOf(buttonNames, buttonOne[i])];
            else if (img != null && i >= 4)
                img.sprite = cuttingBoardButtonSprites[System.Array.IndexOf(buttonNames, buttonTwo[i - 4])];
        }

        // Highlight current top + bottom
        int topIndex = currentCut;
        int bottomIndex = currentCut + 4;

        Image topImg = cuttingBoardButtonImages[topIndex].GetComponent<Image>();
        Image bottomImg = cuttingBoardButtonImages[bottomIndex].GetComponent<Image>();

        if (topImg != null && bottomImg != null)
        {
            topImg.sprite = cuttingBoardButtonHighlightSprites[System.Array.IndexOf(buttonNames, buttonOne[currentCut])];
            bottomImg.sprite = cuttingBoardButtonHighlightSprites[System.Array.IndexOf(buttonNames, buttonTwo[currentCut])];
        }
    }

    void Update()
    {
        if (!gameActive || inputLocked) return;

        foreach (string button in buttonNames)
        {
            if (GetButtonDown(button))
            {
                HandleInput(button);
                break;
            }
        }
    }

    void HandleInput(string pressedButton)
    {
        if (!waitingForSecondInput)
        {
            if (pressedButton == buttonOne[currentCut])
            {
                waitingForSecondInput = true;
            }
            else
            {
                StartCoroutine(ShowFeedback(false)); // wrong first input
            }
        }
        else
        {
            if (pressedButton == buttonTwo[currentCut])
            {
                StartCoroutine(ShowFeedback(true)); // correct full combo
            }
            else
            {
                StartCoroutine(ShowFeedback(false)); // wrong second input
            }
        }
    }

    IEnumerator ShowFeedback(bool success)
    {
        gameActive = false; // pause input during feedback
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
            // Spawn knife prefab between the two buttons
            RectTransform topRect = cuttingBoardButtonImages[topIndex].GetComponent<RectTransform>();
            RectTransform bottomRect = cuttingBoardButtonImages[bottomIndex].GetComponent<RectTransform>();

            Vector3 midpoint = (topRect.position + bottomRect.position) / 2f;
            currentKnifeInstance = Instantiate(knifePrefab, midpoint, Quaternion.identity, cuttingBoardPanel.transform);
        }

        yield return new WaitForSeconds(feedbackDuration);

        // Destroy knife prefab after feedback
        if (currentKnifeInstance != null)
        {
            Destroy(currentKnifeInstance);
        }

        // Restore sprites or hide buttons depending on success
        if (success)
        {
            cuttingBoardButtonImages[topIndex].SetActive(false);
            cuttingBoardButtonImages[bottomIndex].SetActive(false);
            waitingForSecondInput = false;
            currentCut++;

            if (currentCut >= buttonOne.Length)
            {
                WinMinigame();
            }
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
            FailMinigame();
        }

        inputLocked = false;
    }

    bool GetButtonDown(string buttonName)
    {
        if (playerNum == 1)
        {
            switch (buttonName)
            {
                case "A": return Input.GetButtonDown("Interact_P1");
                case "B": return Input.GetButtonDown("B_P1");
                case "X": return Input.GetButtonDown("Reload_P1");
                case "Y": return Input.GetButtonDown("Y_P1");
                case "LT": return Input.GetButtonDown("L2_P1");
                case "RT": return Input.GetButtonDown("Fire_P1");
                default: return false;
            }
        }
        else
        {
            switch (buttonName)
            {
                case "A": return Input.GetButtonDown("Interact_P2");
                case "B": return Input.GetButtonDown("B_P2");
                case "X": return Input.GetButtonDown("Reload_P2");
                case "Y": return Input.GetButtonDown("Y_P2");
                case "LT": return Input.GetButtonDown("L2_P2");
                case "RT": return Input.GetButtonDown("Fire_P2");
                default: return false;
            }
        }
    }

    void FailMinigame()
    {
        Debug.Log("You failed the cut!");
        cuttingBoardPanel.SetActive(false);
        gameActive = false;

        if (playerNum == 1)
        {
            GameObject player = GameObject.Find("Player1");
            if (player != null)
            {
                IngredientInteraction ii = player.GetComponent<IngredientInteraction>();
                IngredientHolding ih = player.GetComponent<IngredientHolding>();
                if (ih != null && ih.ingredientCurrentlyHeld != null)
                {
                    ih.ingredientCurrentlyHeld.quality = "Imperfectly Cut " + ih.ingredientCurrentlyHeld.quality;
                    Debug.Log("Ingredient is now: " + ih.ingredientCurrentlyHeld.quality);
                    ih.trashIngredient();
                }
                if (ii != null)
                {
                    ii.cutting = false;
                }
            }
        }
        else
        {
            GameObject player = GameObject.Find("Player2");
            if (player != null)
            {
                IngredientInteraction ii = player.GetComponent<IngredientInteraction>();
                IngredientHolding ih = player.GetComponent<IngredientHolding>();
                if (ih != null && ih.ingredientCurrentlyHeld != null)
                {
                    ih.ingredientCurrentlyHeld.quality = "Imperfectly Cut " + ih.ingredientCurrentlyHeld.quality;
                    Debug.Log("Ingredient is now: " + ih.ingredientCurrentlyHeld.name);
                    ih.trashIngredient();
                }
                if (ii != null)
                {
                    ii.cutting = false;
                }
            }
        }
    }

    void WinMinigame()
    {
        Debug.Log("All cuts perfect!");
        cuttingBoardPanel.SetActive(false);
        gameActive = false;

        if (playerNum == 1)
        {
            GameObject player = GameObject.Find("Player1");
            if (player != null)
            {
                IngredientInteraction ii = player.GetComponent<IngredientInteraction>();
                IngredientHolding ih = player.GetComponent<IngredientHolding>();
                if (ih != null && ih.ingredientCurrentlyHeld != null)
                {
                    ih.ingredientCurrentlyHeld.quality = "Perfectly Cut " + ih.ingredientCurrentlyHeld.quality;
                    ih.ingredientCurrentlyHeld.name = "Cut " + ih.ingredientCurrentlyHeld.name;
                    Debug.Log("Ingredient is now: " + ih.ingredientCurrentlyHeld.quality);
                }
                if (ii != null)
                {
                    ii.cutting = false;
                }
            }
        }
        else
        {
            GameObject player = GameObject.Find("Player2");
            if (player != null)
            {
                IngredientInteraction ii = player.GetComponent<IngredientInteraction>();
                IngredientHolding ih = player.GetComponent<IngredientHolding>();
                if (ih != null && ih.ingredientCurrentlyHeld != null)
                {
                    ih.ingredientCurrentlyHeld.quality = "Perfectly Cut " + ih.ingredientCurrentlyHeld.quality;
                    ih.ingredientCurrentlyHeld.name = "Cut " + ih.ingredientCurrentlyHeld.name;
                    Debug.Log("Ingredient is now: " + ih.ingredientCurrentlyHeld.name);
                }
                if (ii != null)
                {
                    ii.cutting = false;
                }
            }
        }
    }
}
