using UnityEngine;
using UnityEngine.UI;
using Gilzoide.LottiePlayer;
using System.Runtime.InteropServices;
using TMPro;

/// <summary>
/// UI per caricare file Lottie JSON tramite file picker del browser
/// Standalone - non richiede comunicazione con React
/// </summary>
public class LottieFilePickerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button loadFileButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI fileNameText;

    [Header("Lottie Player")]
    [SerializeField] private ImageLottiePlayer lottiePlayer;

    [Header("Settings")]
    [SerializeField] private bool autoPlay = true;

    private LottieAnimation currentAnimation;

    // Import funzioni JavaScript
    [DllImport("__Internal")]
    private static extern void OpenFilePicker(string gameObjectName, string callbackMethodName);

    [DllImport("__Internal")]
    private static extern string GetUploadedJSON();

    [DllImport("__Internal")]
    private static extern void ClearUploadedJSON();

    private void Start()
    {
        if (lottiePlayer == null)
        {
            lottiePlayer = GetComponent<ImageLottiePlayer>();
        }

        if (loadFileButton != null)
        {
            loadFileButton.onClick.AddListener(OnLoadFileButtonClicked);
        }

        UpdateStatus("Pronto. Clicca 'Carica File' per scegliere un'animazione Lottie.");
    }

    private void OnDestroy()
    {
        if (currentAnimation != null)
        {
            currentAnimation.Dispose();
            currentAnimation = null;
        }
    }

    /// <summary>
    /// Chiamato quando si clicca il pulsante "Carica File"
    /// </summary>
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

    /// <summary>
    /// Callback chiamato da JavaScript quando un file è stato selezionato
    /// </summary>
    /// <param name="fileName">Nome del file selezionato</param>
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
            // Leggi il JSON dalla variabile globale JavaScript
            string jsonData = GetUploadedJSON();

            if (string.IsNullOrEmpty(jsonData))
            {
                UpdateStatus("❌ Errore: JSON vuoto o non valido");
                Debug.LogError("JSON data è vuoto");
                return;
            }

            Debug.Log($"JSON ricevuto: {jsonData.Length} caratteri");

            // Pulisci la variabile globale
            ClearUploadedJSON();

            // Carica l'animazione
            LoadAnimation(jsonData);
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"❌ Errore: {ex.Message}");
            Debug.LogError($"Errore durante il caricamento del file: {ex.Message}\n{ex.StackTrace}");
        }
#endif
    }

    /// <summary>
    /// Carica un'animazione da JSON
    /// </summary>
    private void LoadAnimation(string jsonData)
    {
        if (lottiePlayer == null)
        {
            UpdateStatus("❌ Errore: ImageLottiePlayer non assegnato!");
            Debug.LogError("ImageLottiePlayer è null");
            return;
        }

        try
        {
            // Disponi animazione precedente
            if (currentAnimation != null)
            {
                currentAnimation.Dispose();
                currentAnimation = null;
            }

            UpdateStatus("Parsing JSON...");

            // Salva il JSON come file temporaneo (workaround per limiti WebGL)
            string tempPath = System.IO.Path.Combine(Application.persistentDataPath, "temp_lottie.json");
            System.IO.File.WriteAllText(tempPath, jsonData);

            Debug.Log($"JSON salvato in: {tempPath}");

            // Crea animazione dal file
            NativeLottieAnimation nativeAnimation = new NativeLottieAnimation(tempPath);

            if (!nativeAnimation.IsCreated)
            {
                UpdateStatus("❌ Impossibile creare l'animazione");
                Debug.LogError("NativeLottieAnimation non è stata creata");
                return;
            }

            // Wrapper
            currentAnimation = new LottieAnimation(nativeAnimation);

            // Assegna al player
            lottiePlayer.SetAnimation(nativeAnimation);

            double duration = currentAnimation.GetDuration();
            int totalFrames = (int)currentAnimation.GetTotalFrame();
            double fps = currentAnimation.GetFrameRate();

            UpdateStatus($"✅ Caricato! Durata: {duration:F2}s, Frame: {totalFrames}, FPS: {fps:F1}");

            Debug.Log($"Animazione caricata - Durata: {duration}s, Frame: {totalFrames}, FPS: {fps}");

            // Avvia animazione
            if (autoPlay)
            {
                lottiePlayer.Play();
            }
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"❌ Errore: {ex.Message}");
            Debug.LogError($"Errore caricamento animazione: {ex.Message}\n{ex.StackTrace}");
        }
    }

    /// <summary>
    /// Aggiorna il testo di stato
    /// </summary>
    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log($"[LottieFilePicker] {message}");
    }
}
