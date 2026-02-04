using UnityEngine;
using UnityEngine.UI;
using Gilzoide.LottiePlayer;
using TMPro;
using System.IO;

/// <summary>
/// Lottie File Picker per build STANDALONE (Mac/Windows/Linux)
/// Usa SimpleFileBrowser per file picker nativo cross-platform
/// </summary>
public class LottieFilePickerStandalone : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button loadFileButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI fileNameText;
    [SerializeField] private TextMeshProUGUI instructionsText;

    [Header("Lottie Player")]
    [SerializeField] private ImageLottiePlayer lottiePlayer;

    [Header("Settings")]
    [SerializeField] private bool autoPlay = true;

    private LottieAnimation currentAnimation;
    private string lastFilePath = "";

    private void Start()
    {
        Debug.Log("=== LottieFilePickerStandalone START ===");

        if (lottiePlayer == null)
        {
            lottiePlayer = GetComponent<ImageLottiePlayer>();
            Debug.Log($"LottiePlayer trovato: {lottiePlayer != null}");
        }

        if (loadFileButton != null)
        {
            loadFileButton.onClick.AddListener(OnLoadFileButtonClicked);
            Debug.Log("Button collegato con successo");
        }
        else
        {
            Debug.LogWarning("⚠️ Button non assegnato! Premi SPAZIO per aprire il file picker.");
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
        // Shortcut: premi SPAZIO per aprire file picker (anche senza UI button)
        // Supporta sia Input System vecchio che nuovo
        bool spacePressed = false;

#if ENABLE_INPUT_SYSTEM
        // Nuovo Input System
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            spacePressed = UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        // Vecchio Input System
        spacePressed = Input.GetKeyDown(KeyCode.Space);
#else
        // Fallback se entrambi non sono definiti
        try
        {
            spacePressed = Input.GetKeyDown(KeyCode.Space);
        }
        catch
        {
            // Input non disponibile
        }
#endif

        if (spacePressed)
        {
            Debug.Log("SPAZIO premuto - apertura file picker");
            OnLoadFileButtonClicked();
        }
    }

    private void OnDisable()
    {
        Debug.Log(">>> OnDisable chiamato - fermo il player");
        if (lottiePlayer != null)
        {
            lottiePlayer.Pause();
        }
    }

    private void OnDestroy()
    {
        Debug.Log(">>> OnDestroy chiamato");

        // Ferma il player prima di dispose
        if (lottiePlayer != null)
        {
            try
            {
                lottiePlayer.Pause();
                Debug.Log(">>> Player messo in pausa");
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"⚠️ Errore durante pause: {ex.Message}");
            }
        }

        // Dispose dell'animazione
        if (currentAnimation != null)
        {
            try
            {
                Debug.Log(">>> Disposing currentAnimation...");
                if (currentAnimation.IsCreated)
                {
                    currentAnimation.Dispose();
                    Debug.Log(">>> Animation disposed con successo");
                }
                else
                {
                    Debug.Log(">>> Animation già disposed, skip");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"⚠️ Errore in Dispose: {ex.Message}");
            }
            finally
            {
                currentAnimation = null;
            }
        }

        Debug.Log(">>> OnDestroy completato");
    }

    /// <summary>
    /// Apre il file picker quando si clicca il pulsante
    /// </summary>
    private void OnLoadFileButtonClicked()
    {
        Debug.Log("Apertura file picker...");
        UpdateStatus("Apertura file picker...");

        // Usa SimpleFileBrowser se disponibile, altrimenti istruzioni
        StartCoroutine(ShowFileBrowser());
    }

    private System.Collections.IEnumerator ShowFileBrowser()
    {
#if UNITY_EDITOR
        // In Editor, usa il file picker nativo di Unity
        Debug.Log("Usando EditorUtility.OpenFilePanel");
        ShowEditorFilePicker();
#else
        // In build, usa SimpleFileBrowser se disponibile
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
        Debug.Log(">>> ShowEditorFilePicker: inizio");

        string startPath = string.IsNullOrEmpty(lastFilePath) ? Application.dataPath : System.IO.Path.GetDirectoryName(lastFilePath);
        Debug.Log($">>> Start path: {startPath}");

        string path = UnityEditor.EditorUtility.OpenFilePanel(
            "Seleziona file Lottie JSON",
            startPath,
            "json"
        );

        Debug.Log($">>> File picker restituito path: '{path}' (length: {path?.Length ?? 0})");

        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log($"✅ File selezionato: {path}");
            Debug.Log($"✅ File exists: {System.IO.File.Exists(path)}");
            Debug.Log($"✅ Chiamata LoadFileFromPath...");
            LoadFileFromPath(path);
        }
        else
        {
            Debug.Log("❌ Selezione annullata (path vuoto)");
            UpdateStatus("Selezione file annullata");
        }
    }
#endif

    private System.Collections.IEnumerator ShowSimpleFileBrowser()
    {
        // Questo sarà popolato quando installiamo SimpleFileBrowser
        // Per ora, fallback a input manuale
        ShowManualPathInput();
        yield return null;
    }

    /// <summary>
    /// Mostra input per path manuale (fallback)
    /// </summary>
    private void ShowManualPathInput()
    {
        UpdateStatus("💡 Inserisci il percorso del file JSON nella console Unity");

        if (instructionsText != null)
        {
            instructionsText.text = "ISTRUZIONI:\n1. Trova il tuo file .json\n2. Trascina il file nel Finder\n3. Copia il percorso (Cmd+C)\n4. Torna qui e premi 'P' sulla tastiera";
        }

        // Avvia coroutine per aspettare input da tastiera
        StartCoroutine(WaitForPathInput());
    }

    private System.Collections.IEnumerator WaitForPathInput()
    {
        Debug.Log("=== INCOLLA IL PERCORSO DEL FILE .JSON QUI SOTTO ===");
        Debug.Log("Premi 'P' quando sei pronto...");

        // Aspetta che l'utente prema 'P'
        while (!Input.GetKeyDown(KeyCode.P))
        {
            yield return null;
        }

        // Per ora, usa un file di test predefinito se esiste
        string testPath = System.IO.Path.Combine(Application.dataPath, "..", "test.json");

        if (File.Exists(testPath))
        {
            LoadFileFromPath(testPath);
        }
        else
        {
            UpdateStatus("❌ File test.json non trovato. Metti un file 'test.json' nella cartella del progetto.");
        }
    }

    /// <summary>
    /// Carica un file JSON dal percorso specificato
    /// </summary>
    public void LoadFileFromPath(string filePath)
    {
        Debug.Log($">>> LoadFileFromPath chiamato con: {filePath}");

        if (!File.Exists(filePath))
        {
            UpdateStatus($"❌ File non trovato: {filePath}");
            Debug.LogError($"❌ File non trovato: {filePath}");
            return;
        }

        Debug.Log(">>> File esiste, inizio lettura...");

        try
        {
            UpdateStatus($"Caricamento {Path.GetFileName(filePath)}...");

            if (fileNameText != null)
            {
                fileNameText.text = Path.GetFileName(filePath);
            }

            // Leggi il JSON
            Debug.Log(">>> File.ReadAllText...");
            string jsonData = File.ReadAllText(filePath);
            Debug.Log($">>> JSON letto: {jsonData.Length} caratteri");

            if (string.IsNullOrEmpty(jsonData))
            {
                UpdateStatus("❌ File JSON vuoto");
                Debug.LogError("❌ File JSON vuoto");
                return;
            }

            Debug.Log($"✅ JSON valido: {jsonData.Length} caratteri da {Path.GetFileName(filePath)}");

            // Salva percorso per la prossima volta
            lastFilePath = filePath;

            // Carica l'animazione
            Debug.Log(">>> Chiamata LoadAnimation...");
            LoadAnimation(filePath);
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"❌ Errore lettura file: {ex.Message}");
            Debug.LogError($"❌ EXCEPTION in LoadFileFromPath: {ex.Message}\n{ex.StackTrace}");
        }
    }

    /// <summary>
    /// Carica un'animazione Lottie
    /// </summary>
    private void LoadAnimation(string filePath)
    {
        Debug.Log($">>> LoadAnimation START con: {filePath}");

        if (lottiePlayer == null)
        {
            UpdateStatus("❌ ImageLottiePlayer non assegnato!");
            Debug.LogError("❌ ImageLottiePlayer è null - NON PUOI CARICARE ANIMAZIONI!");
            return;
        }

        Debug.Log($"✅ LottiePlayer trovato: {lottiePlayer.name}");

        try
        {
            // Disponi animazione precedente
            if (currentAnimation != null)
            {
                Debug.Log(">>> Disposing animazione precedente...");
                currentAnimation.Dispose();
                currentAnimation = null;
                Debug.Log(">>> Animation precedente disposed");
            }

            UpdateStatus("Creazione animazione...");

            // Crea animazione direttamente dal file (più efficiente su Standalone)
            Debug.Log($">>> Creando NativeLottieAnimation da: {filePath}");
            NativeLottieAnimation nativeAnimation = new NativeLottieAnimation(filePath);
            Debug.Log($">>> NativeLottieAnimation constructor completato");

            if (!nativeAnimation.IsCreated)
            {
                UpdateStatus("❌ Impossibile creare l'animazione dal JSON");
                Debug.LogError("❌ NativeLottieAnimation.IsCreated è FALSE - Il JSON potrebbe non essere valido o il plugin rlottie ha fallito");
                return;
            }

            Debug.Log("✅ NativeLottieAnimation creata con successo (IsCreated = true)");

            // Wrapper
            Debug.Log(">>> Creando wrapper LottieAnimation...");
            currentAnimation = new LottieAnimation(nativeAnimation);
            Debug.Log("✅ LottieAnimation wrapper creato");

            // Info animazione
            Debug.Log(">>> Leggendo info animazione...");
            double duration = currentAnimation.GetDuration();
            int totalFrames = (int)currentAnimation.GetTotalFrame();
            double fps = currentAnimation.GetFrameRate();
            Vector2Int size = currentAnimation.GetSize();
            Debug.Log($"✅ Info lette - Duration: {duration}, Frames: {totalFrames}, FPS: {fps}, Size: {size}");

            // Assegna al player
            Debug.Log(">>> Assegnando animazione al player...");
            lottiePlayer.SetAnimation(nativeAnimation);
            Debug.Log("✅ Animazione assegnata al player");

            // Forza rebuild della mesh UI
            Debug.Log(">>> Forzando rebuild UI...");
            if (lottiePlayer.canvas != null)
            {
                Debug.Log($"✅ Canvas trovato: {lottiePlayer.canvas.name}");
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(lottiePlayer.GetComponent<RectTransform>());
                lottiePlayer.SetVerticesDirty();
                Debug.Log("✅ UI rebuild richiesto");
            }
            else
            {
                Debug.LogWarning("⚠️ Canvas non trovato sul ImageLottiePlayer!");
            }

            UpdateStatus($"✅ Caricato! {duration:F2}s | {totalFrames} frame | {fps:F1} FPS | {size.x}x{size.y}px");

            Debug.Log($"✅✅✅ ANIMAZIONE CARICATA CON SUCCESSO! ✅✅✅\n" +
                     $"  Durata: {duration}s\n" +
                     $"  Frame: {totalFrames}\n" +
                     $"  FPS: {fps}\n" +
                     $"  Dimensioni: {size.x}x{size.y}");

            // Avvia animazione
            if (autoPlay)
            {
                Debug.Log(">>> Auto-play abilitato, avvio animazione...");
                lottiePlayer.Play();
                Debug.Log($"✅ lottiePlayer.Play() chiamato - IsPlaying: {lottiePlayer.IsPlaying}");
            }
            else
            {
                Debug.Log("⚠️ Auto-play disabilitato - chiama manualmente Play() per vedere l'animazione");
            }
        }
        catch (System.Exception ex)
        {
            UpdateStatus($"❌ Errore caricamento animazione: {ex.Message}");
            Debug.LogError($"❌❌❌ EXCEPTION in LoadAnimation: {ex.Message}\n{ex.StackTrace}");
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

    /// <summary>
    /// Metodi pubblici per controlli UI aggiuntivi
    /// </summary>
    public void PlayAnimation()
    {
        if (lottiePlayer != null && currentAnimation != null && currentAnimation.IsCreated)
        {
            lottiePlayer.Play();
            Debug.Log("Play animation");
        }
    }

    public void PauseAnimation()
    {
        if (lottiePlayer != null)
        {
            lottiePlayer.Pause();
            Debug.Log("Pause animation");
        }
    }

    public void ReloadCurrentFile()
    {
        if (!string.IsNullOrEmpty(lastFilePath) && File.Exists(lastFilePath))
        {
            LoadFileFromPath(lastFilePath);
        }
    }
}
