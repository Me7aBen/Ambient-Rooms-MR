using UnityEngine;
using System;

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

        Vector3 spawnPos = cam.transform.position + cam.transform.forward * 1.5f;
        Quaternion spawnRot = Quaternion.LookRotation(-cam.transform.forward);
        Instantiate(windowPrefab, spawnPos, spawnRot);

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