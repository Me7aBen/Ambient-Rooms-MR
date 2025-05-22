using UnityEngine;
using UnityEngine.UI;

public class UIFlowController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject instructionsPanel;
    public GameObject formPanel;
    public GameObject readyPanel;
    public GameObject confirmResetPanel;

    [Header("Main Buttons")]
    public Button startButton;
    public Button spawnButton;
    public Button resetWindowsButton;
    public Button backToFormButton;
    public Button surpriseMeButton;
    
    public Button yesButton;
    public Button noButton;

    

    [Header("Environment Buttons")]
    public Button oceanButton;
    public Button jungleButton;
    public Button mountainButton;
    public Button spaceButton;
    public Button rainyDayButton;

    [Header("Manager References")]
    public SkyboxManager skyboxManager;
    public GameObject uiRoot;

    private string selectedEnvironment = "";
    private readonly string[] environments = { "Ocean", "Jungle", "Mountain", "Space", "Rainy Day" };

    private void Start()
    {
        ShowInstructions();

        startButton.onClick.AddListener(ShowForm);
        spawnButton.onClick.AddListener(OnSpawnWindow);
        resetWindowsButton.onClick.AddListener(ShowConfirmReset);
        backToFormButton.onClick.AddListener(ShowForm);
        surpriseMeButton.onClick.AddListener(SelectRandomEnvironment);
        
        yesButton.onClick.AddListener(OnConfirmReset);
        noButton.onClick.AddListener(HideConfirmReset);

        oceanButton.onClick.AddListener(() => SelectEnvironment("Ocean"));
        jungleButton.onClick.AddListener(() => SelectEnvironment("Jungle"));
        mountainButton.onClick.AddListener(() => SelectEnvironment("Mountain"));
        spaceButton.onClick.AddListener(() => SelectEnvironment("Space"));
        rainyDayButton.onClick.AddListener(() => SelectEnvironment("Rainy Day"));
        


        skyboxManager.OnFirstWindowSpawned += HandleFirstSpawn;
    }

    private void ShowInstructions() => ShowOnly(instructionsPanel);
    private void ShowForm() => ShowOnly(formPanel);
    private void ShowReady() => ShowOnly(readyPanel);
    private void ShowConfirmReset() => ShowOnly(confirmResetPanel);

    private void ShowOnly(GameObject panel)
    {
        instructionsPanel.SetActive(false);
        formPanel.SetActive(false);
        readyPanel.SetActive(false);
        confirmResetPanel.SetActive(false);

        panel?.SetActive(true);
    }

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

    private void OnSpawnWindow()
    {
        skyboxManager.SpawnWindow();
    }

    public void OnConfirmReset()
    {
        skyboxManager.ResetEnvironment();
        uiRoot.SetActive(true);
        ShowForm();
    }

    public void HideConfirmReset()
    {
        ShowReady();
    }

    private void HandleFirstSpawn()
    {
        // No se oculta la UI, se mantiene visible
    }
}