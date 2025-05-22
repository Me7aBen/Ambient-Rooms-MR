using UnityEngine;
using System;
using Meta.XR;
using Meta.XR.MRUtilityKitSamples.EnvironmentPanelPlacement;

public class SkyboxManager : MonoBehaviour
{
    [Header("Renderer for Environment Visual")]
    public MeshRenderer environmentRenderer;

    [Header("Materials")]
    public Material oceanMaterial;
    public Material jungleMaterial;
    public Material mountainMaterial;
    public Material spaceMaterial;
    public Material rainyDayMaterial;

    [Header("Ambient Sounds")]
    public AudioSource ambientAudioSource;
    public AudioClip oceanClip;
    public AudioClip jungleClip;
    public AudioClip mountainClip;
    public AudioClip spaceClip;
    public AudioClip rainyDayClip;

    [Header("Window Prefab")]
    public GameObject windowPrefab;
    
    public EnvironmentRaycastManager raycastManagerRef;
    public Transform centerEyeAnchorRef;
    public Transform raycastAnchorRef;

    public event Action OnFirstWindowSpawned;

    private bool hasSpawnedFirstWindow = false;

    public void ApplyEnvironment(string environmentName)
    {
        Material selectedMaterial = null;
        AudioClip selectedClip = null;

        switch (environmentName)
        {
            case "Ocean":
                selectedMaterial = oceanMaterial;
                selectedClip = oceanClip;
                break;
            case "Jungle":
                selectedMaterial = jungleMaterial;
                selectedClip = jungleClip;
                break;
            case "Mountain":
                selectedMaterial = mountainMaterial;
                selectedClip = mountainClip;
                break;
            case "Space":
                selectedMaterial = spaceMaterial;
                selectedClip = spaceClip;
                break;
            case "Rainy Day":
                selectedMaterial = rainyDayMaterial;
                selectedClip = rainyDayClip;
                break;
        }

        if (selectedMaterial && environmentRenderer)
        {
            environmentRenderer.material = selectedMaterial;
        }

        if (ambientAudioSource && selectedClip)
        {
            ambientAudioSource.clip = selectedClip;
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
        foreach (var go in GameObject.FindGameObjectsWithTag("VirtualWindow"))
        {
            Destroy(go);
        }

        if (ambientAudioSource) ambientAudioSource.Stop();
        if (environmentRenderer) environmentRenderer.material = null;
    }
    
    
}