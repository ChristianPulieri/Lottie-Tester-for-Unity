using UnityEngine;
using UnityEngine.UI;
using SkiaSharp.Unity;
using System.Runtime.InteropServices;
using TMPro;

/// <summary>
/// UI per caricare file Lottie JSON tramite file picker del browser
/// Standalone - non richiede comunicazione con React
/// Rendering con SkiaForUnity (Skottie)
/// </summary>
public class LottieFilePickerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button loadFileButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI fileNameText;

    [Header("Lottie Player")]
    [SerializeField] private SkottiePlayerV2 skottiePlayer;

    [Header("Settings")]
    [SerializeField] private bool autoPlay = true;

    // Import funzioni JavaScript
    [DllImport("__Internal")]
    private static extern void OpenFilePicker(string gameObjectName, string callbackMethodName);

    [DllImport("__Internal")]
    private static extern string GetUploadedJSON();

    [DllImport("__Internal")]
    private static extern void ClearUploadedJSON();

    private void Start()
    {
        if (skottiePlayer == null)
        {
            skottiePlayer = GetComponent<SkottiePlayerV2>();
        }

        if (loadFileButton != null)
        {
            loadFileButton.onClick.AddListener(OnLoadFileButtonClicked);
        }

        UpdateStatus("Pronto. Clicca 'Carica File' per scegliere un'animazione Lottie.");
    }

    private void OnLoadFileButtonClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        UpdateStatus("Apertura file picker...");
        OpenFilePicker(gameObject.name, nameof(OnFileSelected));
#else
        UpdateStatus("File picker funziona solo in WebGL build!");
        Debug.LogWarning("File picker disponibile solo in WebGL build");
#endif
    }

    public void OnFileSelected(string fileName)
    {
        Debug.Log($"File selezionato: {fileName}");
        UpdateStatus($"Caricamento {fileName}...");

        if (fileNameText != null)
        {
            fileNameText.text = fileName;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            string jsonData = GetUploadedJSON();

            if (string.IsNullOrEmpty(jsonData))
            {
                UpdateStatus("Errore: JSON vuoto o non valido");
                Debug.LogError("JSON data e' vuoto");
                return;
            }

            Debug.Log($"JSON ricevuto: {jsonData.Length} caratteri");

            ClearUploadedJSON();

            LoadAnimation(jsonData);
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"Errore: {ex.Message}");
            Debug.LogError($"Errore durante il caricamento del file: {ex.Message}\n{ex.StackTrace}");
        }
#endif
    }

    private void LoadAnimation(string jsonData)
    {
        if (skottiePlayer == null)
        {
            UpdateStatus("Errore: SkottiePlayerV2 non assegnato!");
            Debug.LogError("SkottiePlayerV2 e' null");
            return;
        }

        try
        {
            UpdateStatus("Parsing JSON...");

            skottiePlayer.LoadAnimation(jsonData);

            double duration = skottiePlayer.GetDurations();
            double fps = skottiePlayer.GetFps();

            UpdateStatus($"Caricato! Durata: {duration:F2}s, FPS: {fps:F1}");

            Debug.Log($"Animazione caricata - Durata: {duration}s, FPS: {fps}");

            if (autoPlay)
            {
                skottiePlayer.loop = true;
                skottiePlayer.PlayAnimation();
            }
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"Errore: {ex.Message}");
            Debug.LogError($"Errore caricamento animazione: {ex.Message}\n{ex.StackTrace}");
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log($"[LottieFilePicker] {message}");
    }
}
