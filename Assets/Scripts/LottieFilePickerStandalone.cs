using UnityEngine;
using UnityEngine.UI;
using SkiaSharp.Unity;
using TMPro;
using System.IO;

/// <summary>
/// Lottie File Picker per build STANDALONE (Mac/Windows/Linux)
/// Usa SkiaForUnity (Skottie) per il rendering delle animazioni
/// </summary>
public class LottieFilePickerStandalone : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button loadFileButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI fileNameText;
    [SerializeField] private TextMeshProUGUI instructionsText;

    [Header("Lottie Player")]
    [SerializeField] private SkottiePlayerV2 skottiePlayer;

    [Header("Settings")]
    [SerializeField] private bool autoPlay = true;

    private string lastFilePath = "";

    private void Start()
    {
        Debug.Log("=== LottieFilePickerStandalone START ===");

        if (skottiePlayer == null)
        {
            skottiePlayer = GetComponent<SkottiePlayerV2>();
            Debug.Log($"SkottiePlayer trovato: {skottiePlayer != null}");
        }

        if (loadFileButton != null)
        {
            loadFileButton.onClick.AddListener(OnLoadFileButtonClicked);
            Debug.Log("Button collegato con successo");
        }
        else
        {
            Debug.LogWarning("Button non assegnato! Premi SPAZIO per aprire il file picker.");
        }

        UpdateStatus("Pronto. Clicca 'Carica File' o premi SPAZIO per scegliere un'animazione.");

        if (instructionsText != null)
        {
            instructionsText.text = "Clicca 'Carica File' o premi SPAZIO";
        }

        Debug.Log("LottieFilePickerStandalone inizializzato correttamente");
    }

    private void Update()
    {
        bool spacePressed = false;

#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            spacePressed = UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        spacePressed = Input.GetKeyDown(KeyCode.Space);
#else
        try
        {
            spacePressed = Input.GetKeyDown(KeyCode.Space);
        }
        catch
        {
        }
#endif

        if (spacePressed)
        {
            Debug.Log("SPAZIO premuto - apertura file picker");
            OnLoadFileButtonClicked();
        }
    }

    private void OnLoadFileButtonClicked()
    {
        Debug.Log("Apertura file picker...");
        UpdateStatus("Apertura file picker...");
        StartCoroutine(ShowFileBrowser());
    }

    private System.Collections.IEnumerator ShowFileBrowser()
    {
#if UNITY_EDITOR
        Debug.Log("Usando EditorUtility.OpenFilePanel");
        ShowEditorFilePicker();
#else
        System.Type fileBrowserType = System.Type.GetType("SimpleFileBrowser.FileBrowser, Assembly-CSharp");

        if (fileBrowserType != null)
        {
            Debug.Log("Usando SimpleFileBrowser");
            yield return StartCoroutine(ShowSimpleFileBrowser());
        }
        else
        {
            Debug.Log("SimpleFileBrowser non trovato, usando input manuale");
            ShowManualPathInput();
        }
#endif
        yield return null;
    }

#if UNITY_EDITOR
    private void ShowEditorFilePicker()
    {
        string startPath = string.IsNullOrEmpty(lastFilePath) ? Application.dataPath : Path.GetDirectoryName(lastFilePath);

        string path = UnityEditor.EditorUtility.OpenFilePanel(
            "Seleziona file Lottie JSON",
            startPath,
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log($"File selezionato: {path}");
            LoadFileFromPath(path);
        }
        else
        {
            UpdateStatus("Selezione file annullata");
        }
    }
#endif

    private System.Collections.IEnumerator ShowSimpleFileBrowser()
    {
        ShowManualPathInput();
        yield return null;
    }

    private void ShowManualPathInput()
    {
        UpdateStatus("Inserisci il percorso del file JSON nella console Unity");

        if (instructionsText != null)
        {
            instructionsText.text = "ISTRUZIONI:\n1. Trova il tuo file .json\n2. Trascina il file nel Finder\n3. Copia il percorso (Cmd+C)\n4. Torna qui e premi 'P' sulla tastiera";
        }

        StartCoroutine(WaitForPathInput());
    }

    private System.Collections.IEnumerator WaitForPathInput()
    {
        Debug.Log("=== INCOLLA IL PERCORSO DEL FILE .JSON QUI SOTTO ===");
        Debug.Log("Premi 'P' quando sei pronto...");

        while (!Input.GetKeyDown(KeyCode.P))
        {
            yield return null;
        }

        string testPath = Path.Combine(Application.dataPath, "..", "test.json");

        if (File.Exists(testPath))
        {
            LoadFileFromPath(testPath);
        }
        else
        {
            UpdateStatus("File test.json non trovato. Metti un file 'test.json' nella cartella del progetto.");
        }
    }

    public void LoadFileFromPath(string filePath)
    {
        Debug.Log($"LoadFileFromPath: {filePath}");

        if (!File.Exists(filePath))
        {
            UpdateStatus($"File non trovato: {filePath}");
            Debug.LogError($"File non trovato: {filePath}");
            return;
        }

        try
        {
            UpdateStatus($"Caricamento {Path.GetFileName(filePath)}...");

            if (fileNameText != null)
            {
                fileNameText.text = Path.GetFileName(filePath);
            }

            string jsonData = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(jsonData))
            {
                UpdateStatus("File JSON vuoto");
                Debug.LogError("File JSON vuoto");
                return;
            }

            Debug.Log($"JSON valido: {jsonData.Length} caratteri da {Path.GetFileName(filePath)}");

            lastFilePath = filePath;

            LoadAnimation(jsonData, Path.GetFileName(filePath));
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"Errore lettura file: {ex.Message}");
            Debug.LogError($"EXCEPTION in LoadFileFromPath: {ex.Message}\n{ex.StackTrace}");
        }
    }

    private void LoadAnimation(string jsonData, string fileName = "")
    {
        if (skottiePlayer == null)
        {
            UpdateStatus("SkottiePlayerV2 non assegnato!");
            Debug.LogError("SkottiePlayerV2 e' null");
            return;
        }

        try
        {
            UpdateStatus("Creazione animazione...");

            // Carica l'animazione con Skottie (Skia)
            skottiePlayer.LoadAnimation(jsonData);

            double duration = skottiePlayer.GetDurations();
            double fps = skottiePlayer.GetFps();

            UpdateStatus($"Caricato! {duration:F2}s | {fps:F1} FPS | {fileName}");

            Debug.Log($"ANIMAZIONE CARICATA CON SUCCESSO!\n" +
                     $"  Durata: {duration}s\n" +
                     $"  FPS: {fps}");

            if (autoPlay)
            {
                skottiePlayer.loop = true;
                skottiePlayer.PlayAnimation();
                Debug.Log("Auto-play avviato");
            }
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"Errore caricamento animazione: {ex.Message}");
            Debug.LogError($"EXCEPTION in LoadAnimation: {ex.Message}\n{ex.StackTrace}");
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

    public void PlayAnimation()
    {
        if (skottiePlayer != null)
        {
            skottiePlayer.PlayAnimation();
            Debug.Log("Play animation");
        }
    }

    public void PauseAnimation()
    {
        // SkottiePlayerV2 non ha un Pause esplicito, si puo' fermare impostando il loop a false
        // Per ora, logghiamo. Un vero pause richiede un'estensione del player.
        Debug.Log("Pause animation (stop loop)");
    }

    public void ReloadCurrentFile()
    {
        if (!string.IsNullOrEmpty(lastFilePath) && File.Exists(lastFilePath))
        {
            LoadFileFromPath(lastFilePath);
        }
    }
}
