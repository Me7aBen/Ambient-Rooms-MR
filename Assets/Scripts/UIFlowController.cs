using UnityEngine;
using UnityEngine.UI;

public class UIFlowController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject instructionsPanel;
    public GameObject formPanel;
    public GameObject readyPanel;
    public GameObject optionsPanel;
    public GameObject confirmResetPanel;

    [Header("Main Buttons")]
    public Button startButton;
    public Button spawnButton;
    public Button resetWindowsButton;
    public Button backToFormButton;
    public Button surpriseMeButton;

    [Header("Tag Buttons")]
    public Button[] tagButtons;

    [Header("Manager References")]
    public SkyboxManager skyboxManager;
    public GameObject uiRoot;

    [Header("Floating Prompt UI")]
    public GameObject pressXShow;  // Imagen: "Press X to show options"
    public GameObject pressXHide;  // Imagen: "Press X to hide options"
    public GameObject optionsPanelAnchor; // Panel que se activa/desactiva con el botón X

    [Header("OVR Input Settings")]
    [SerializeField] private OVRInput.RawButton toggleButton = OVRInput.RawButton.X; // Botón X del mando izquierdo

    private string selectedEnvironment = "";
    private readonly string[] environments = { "Ocean", "Jungle", "Mountain", "Space", "Rainy Day" };

    private bool hasSpawnedFirstWindow = false;
    private bool isOptionsVisible = false;

    // =======================
    //   Initialization
    // =======================
    private void Start()
    {
        ShowInstructions();

        startButton.onClick.AddListener(ShowForm);
        spawnButton.onClick.AddListener(OnSpawnWindow);
        resetWindowsButton.onClick.AddListener(ShowConfirmReset);
        backToFormButton.onClick.AddListener(ShowForm);
        surpriseMeButton.onClick.AddListener(SelectRandomEnvironment);

        foreach (Button btn in tagButtons)
        {
            string tag = btn.GetComponentInChildren<Text>().text;
            btn.onClick.AddListener(() => SelectEnvironment(tag));
        }

        skyboxManager.OnFirstWindowSpawned += HandleFirstSpawn;
    }

    // =======================
    //   Update (X button logic)
    // =======================
    private void Update()
    {
        if (!hasSpawnedFirstWindow) return;

        if (OVRInput.GetDown(toggleButton))
        {
            isOptionsVisible = !isOptionsVisible;

            optionsPanelAnchor.SetActive(isOptionsVisible);
            pressXShow.SetActive(!isOptionsVisible);
            pressXHide.SetActive(isOptionsVisible);
        }
    }

    // =======================
    //   UI Navigation
    // =======================
    private void ShowInstructions() => ShowOnly(instructionsPanel);
    private void ShowForm() => ShowOnly(formPanel);
    private void ShowReady() => ShowOnly(readyPanel);
    private void ShowOptions() => ShowOnly(optionsPanel);
    private void ShowConfirmReset() => ShowOnly(confirmResetPanel);

    private void ShowOnly(GameObject panel)
    {
        instructionsPanel.SetActive(false);
        formPanel.SetActive(false);
        readyPanel.SetActive(false);
        optionsPanel.SetActive(false);
        confirmResetPanel.SetActive(false);

        panel?.SetActive(true);
    }

    // =======================
    //   Environment Selection
    // =======================
    private void SelectEnvironment(string env)
    {
        selectedEnvironment = env;
        skyboxManager.ApplyEnvironment(env);
        ShowReady();
    }

    private void SelectRandomEnvironment()
    {
        int index = Random.Range(0, environments.Length);
        SelectEnvironment(environments[index]);
    }

    // =======================
    //   Spawn & Reset
    // =======================
    private void OnSpawnWindow()
    {
        skyboxManager.SpawnWindow();
    }

    public void OnConfirmReset()
    {
        skyboxManager.ResetEnvironment();
        uiRoot.SetActive(true);
        pressXShow.SetActive(false);
        pressXHide.SetActive(false);
        optionsPanelAnchor.SetActive(false);
        ShowForm();
    }

    public void HideConfirmReset()
    {
        ShowOptions();
    }

    // =======================
    //   After first spawn
    // =======================
    private void HandleFirstSpawn()
    {
        hasSpawnedFirstWindow = true;
        uiRoot.SetActive(false);
        pressXShow.SetActive(true);
        pressXHide.SetActive(false);
        optionsPanelAnchor.SetActive(false);
    }
}