using UnityEngine;
using Gilzoide.LottiePlayer;
using System.Runtime.InteropServices;

/// <summary>
/// Bridge per comunicare tra React e Unity WebGL
/// Usa localStorage come ponte per evitare problemi con SendMessage
/// </summary>
public class LottieWebGLBridge : MonoBehaviour
{
    [Header("Lottie Player")]
    [SerializeField] private ImageLottiePlayer lottiePlayer;

    [Header("Settings")]
    [SerializeField] private bool playOnReceive = true;
    [SerializeField] private float checkInterval = 0.5f;

    private LottieAnimation currentAnimation;
    private string lastLoadedHash = "";

    // Import JavaScript functions per leggere da localStorage
    [DllImport("__Internal")]
    private static extern string GetLottieJSON();

    [DllImport("__Internal")]
    private static extern string GetLottieHash();

    private void Start()
    {
        if (lottiePlayer == null)
        {
            lottiePlayer = GetComponent<ImageLottiePlayer>();

            if (lottiePlayer == null)
            {
                Debug.LogError("LottieWebGLBridge: Nessun ImageLottiePlayer trovato!");
            }
        }

        Debug.Log("LottieWebGLBridge: Pronto a ricevere animazioni da React (via localStorage)");

        // Controlla periodicamente se c'è un nuovo JSON in localStorage
        InvokeRepeating(nameof(CheckForNewAnimation), 1f, checkInterval);
    }

    private void OnDestroy()
    {
        // Cleanup dell'animazione
        if (currentAnimation != null)
        {
            currentAnimation.Dispose();
            currentAnimation = null;
        }
    }

    /// <summary>
    /// Controlla periodicamente se React ha messo un nuovo JSON in localStorage
    /// </summary>
    private void CheckForNewAnimation()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            string hash = GetLottieHash();

            if (!string.IsNullOrEmpty(hash) && hash != lastLoadedHash)
            {
                Debug.Log($"LottieWebGLBridge: 📥 Rilevato nuovo JSON (hash: {hash})");

                string jsonData = GetLottieJSON();

                if (!string.IsNullOrEmpty(jsonData))
                {
                    LoadAnimationFromJSON(jsonData);
                    lastLoadedHash = hash;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"LottieWebGLBridge: ❌ Errore check localStorage: {ex.Message}");
        }
#endif
    }

    /// <summary>
    /// Carica un'animazione da JSON string
    /// </summary>
    private void LoadAnimationFromJSON(string jsonData)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("LottieWebGLBridge: JSON ricevuto è vuoto!");
            return;
        }

        Debug.Log($"LottieWebGLBridge: JSON ricevuto ({jsonData.Length} caratteri)");

        if (lottiePlayer == null)
        {
            Debug.LogError("LottieWebGLBridge: ImageLottiePlayer non assegnato!");
            return;
        }

        try
        {
            // Disponi la vecchia animazione se esiste
            if (currentAnimation != null)
            {
                currentAnimation.Dispose();
                currentAnimation = null;
            }

            // WORKAROUND per WebGL: salva JSON come file temporaneo invece di passarlo come stringa
            // Passare grandi stringhe JSON da C# a C causa "memory access out of bounds" in WebGL
            string tempPath = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, "temp_lottie.json");
            System.IO.File.WriteAllText(tempPath, jsonData);

            Debug.Log($"LottieWebGLBridge: JSON salvato in {tempPath}");

            // Crea NativeLottieAnimation caricando dal file
            NativeLottieAnimation nativeAnimation = new NativeLottieAnimation(tempPath);

            if (!nativeAnimation.IsCreated)
            {
                Debug.LogError("LottieWebGLBridge: Impossibile creare NativeLottieAnimation dal file");
                return;
            }

            // Crea wrapper LottieAnimation
            currentAnimation = new LottieAnimation(nativeAnimation);

            // Assegna l'animazione al player
            lottiePlayer.SetAnimation(nativeAnimation);

            double duration = currentAnimation.GetDuration();
            Debug.Log($"LottieWebGLBridge: ✅ Animazione caricata - Durata: {duration}s");

            // Riproduci automaticamente se richiesto
            if (playOnReceive)
            {
                lottiePlayer.Play();
                Debug.Log("LottieWebGLBridge: ▶️  Animazione avviata");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"LottieWebGLBridge: ❌ Errore caricamento: {ex.Message}\n{ex.StackTrace}");
        }
    }

    /// <summary>
    /// Mette in pausa l'animazione
    /// </summary>
    public void PauseAnimation()
    {
        if (lottiePlayer != null)
        {
            lottiePlayer.Pause();
            Debug.Log("LottieWebGLBridge: Animazione in pausa");
        }
    }

    /// <summary>
    /// Riprende/avvia l'animazione
    /// </summary>
    public void PlayAnimation()
    {
        if (lottiePlayer != null)
        {
            lottiePlayer.Play();
            Debug.Log("LottieWebGLBridge: Animazione avviata/ripresa");
        }
    }
}
