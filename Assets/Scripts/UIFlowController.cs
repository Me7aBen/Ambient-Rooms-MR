using UnityEngine;
using UnityEngine.UI;

public class UIFlowController : MonoBehaviour
{
    // =======================
    //   Panel References
    // =======================
    [Header("UI Panels")]
    public GameObject instructionsPanel;
    public GameObject formPanel;
    public GameObject loadingPanel;
    public GameObject readyPanel;
    public GameObject optionsPanel;

    // =======================
    //   Buttons & Input
    // =======================
    [Header("Main Buttons")]
    public Button startButton;
    public Button generateButton;
    public Button spawnButton;
    public Button openOptionsButton;
    public Button resetWindowsButton;
    public Button backToFormButton;
    public Button surpriseMeButton;

    [Header("Tag Buttons")]
    public Button[] tagButtons;

    [Header("Environment Input")]
    public InputField environmentInput;

    // =======================
    //   Internal State
    // =======================
    private string selectedEnvironment = "";

    // =======================
    //   Initialization
    // =======================
    private void Start()
    {
        ShowInstructions();

        // Bind main buttons
        startButton.onClick.AddListener(ShowForm);
        generateButton.onClick.AddListener(OnGenerate);
        spawnButton.onClick.AddListener(OnSpawnWindow);
        openOptionsButton.onClick.AddListener(ShowOptions);
        resetWindowsButton.onClick.AddListener(OnReset);
        backToFormButton.onClick.AddListener(ShowForm);
        surpriseMeButton.onClick.AddListener(SelectRandomEnvironment);

        // Bind tag buttons
        foreach (Button btn in tagButtons)
        {
            string tag = btn.GetComponentInChildren<Text>().text;
            btn.onClick.AddListener(() => SelectTag(tag));
        }
    }

    // =======================
    //   Panel Navigation
    // =======================
    private void ShowInstructions()
    {
        HideAllPanels();
        instructionsPanel.SetActive(true);
    }

    private void ShowForm()
    {
        HideAllPanels();
        formPanel.SetActive(true);
    }

    private void ShowLoading()
    {
        HideAllPanels();
        loadingPanel.SetActive(true);
    }

    private void ShowReady()
    {
        HideAllPanels();
        readyPanel.SetActive(true);
    }

    private void ShowOptions()
    {
        HideAllPanels();
        optionsPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        instructionsPanel.SetActive(false);
        formPanel.SetActive(false);
        loadingPanel.SetActive(false);
        readyPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    // =======================
    //   Actions
    // =======================
    private void OnGenerate()
    {
        selectedEnvironment = environmentInput.text.Trim();

        if (string.IsNullOrEmpty(selectedEnvironment))
        {
            Debug.LogWarning("Environment not specified.");
            return;
        }

        ShowLoading();

        // Simulate generation delay (replace with actual logic)
        Invoke(nameof(FinishEnvironmentGeneration), 2.5f);
    }

    private void FinishEnvironmentGeneration()
    {
        // After environment is generated, go to "Ready" state
        ShowReady();
    }

    private void OnSpawnWindow()
    {
        Debug.Log($"[AmbientRooms] Spawning window for environment: {selectedEnvironment}");

        // TODO: Connect this with your skybox/window instantiation logic
    }

    private void OnReset()
    {
        Debug.Log("[AmbientRooms] Resetting all windows...");

        // TODO: Implement actual logic to remove windows / reset scene
    }

    private void SelectTag(string tag)
    {
        environmentInput.text = tag;
        selectedEnvironment = tag;
    }

    private void SelectRandomEnvironment()
    {
        string[] predefined = { "Ocean", "Jungle", "Mountains", "Space", "Rainy Day" };
        string randomTag = predefined[Random.Range(0, predefined.Length)];
        SelectTag(randomTag);
    }
}