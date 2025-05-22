using UnityEngine;
using System.Collections.Generic;
using Meta.XR;
using Meta.XR.MRUtilityKitSamples.EnvironmentPanelPlacement;
using Unity.Mathematics;

public class SkyboxManager : MonoBehaviour
{
    [Header("Skybox Materials")]
    public Material oceanSkybox;
    public Material jungleSkybox;
    public Material mountainSkybox;
    public Material spaceSkybox;
    public Material rainyDaySkybox;
    public Material defaultSkybox;

    [Header("Ambient Sounds")]
    public AudioClip oceanClip;
    public AudioClip jungleClip;
    public AudioClip mountainClip;
    public AudioClip spaceClip;
    public AudioClip rainyDayClip;

    [Header("Audio Source")]
    public AudioSource ambientAudioSource;

    [Header("Window Prefab & Spawn")]
    public GameObject windowPrefab;
    public Transform spawnTransform;

    private Dictionary<string, Material> skyboxDict;
    private Dictionary<string, AudioClip> audioDict;

    private bool hasSpawnedFirstWindow = false;

    public System.Action OnFirstWindowSpawned;
    
    public EnvironmentRaycastManager raycastManagerRef;
    public Transform centerEyeAnchorRef;
    public Transform raycastAnchorRef;

    private void Awake()
    {
        skyboxDict = new()
        {
            { "Ocean", oceanSkybox },
            { "Jungle", jungleSkybox },
            { "Mountain", mountainSkybox },
            { "Space", spaceSkybox },
            { "Rainy Day", rainyDaySkybox }
        };

        audioDict = new()
        {
            { "Ocean", oceanClip },
            { "Jungle", jungleClip },
            { "Mountain", mountainClip },
            { "Space", spaceClip },
            { "Rainy Day", rainyDayClip }
        };
    }

    public void ApplyEnvironment(string environment)
    {
        if (skyboxDict.TryGetValue(environment, out var skybox))
        {
            RenderSettings.skybox = skybox;
            DynamicGI.UpdateEnvironment();
        }

        if (audioDict.TryGetValue(environment, out var clip))
        {
            ambientAudioSource.clip = clip;
            ambientAudioSource.loop = true;
            ambientAudioSource.Play();
        }
    }

    public void SpawnWindow()
    {
        Camera cam = Camera.main;
        if (cam == null || windowPrefab == null) return;

        Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 1.5f;
        Quaternion spawnRotation = Quaternion.LookRotation(-cam.transform.forward);

        GameObject window = Instantiate(windowPrefab, spawnPosition, spawnRotation);

        // Configurar referencias necesarias
        var placement = window.GetComponent<EnvironmentPanelPlacement>();
        if (placement != null)
        {
            placement.GetType().GetField("_raycastManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(placement, raycastManagerRef);

            placement.GetType().GetField("_centerEyeAnchor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(placement, centerEyeAnchorRef);

            placement.GetType().GetField("_raycastAnchor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(placement, raycastAnchorRef);
        }

        if (!hasSpawnedFirstWindow)
        {
            hasSpawnedFirstWindow = true;
            OnFirstWindowSpawned?.Invoke();
        }
    }

    public void ResetEnvironment()
    {
        foreach (var w in GameObject.FindGameObjectsWithTag("VirtualWindow"))
        {
            Destroy(w);
        }

        ambientAudioSource.Stop();
        RenderSettings.skybox = defaultSkybox;
    }
}