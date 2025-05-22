using UnityEngine;
using System.Collections.Generic;
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
        Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 0.5f;
        Quaternion spawnRotation = quaternion.LookRotation(-cam.transform.forward, cam.transform.up);
        GameObject window = Instantiate(windowPrefab, spawnPosition, spawnRotation);
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