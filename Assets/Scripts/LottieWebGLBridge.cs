using UnityEngine;
using SkiaSharp.Unity;
using System.Runtime.InteropServices;

/// <summary>
/// Bridge per comunicare tra React e Unity WebGL
/// Usa localStorage come ponte per evitare problemi con SendMessage
/// Rendering con SkiaForUnity (Skottie)
/// </summary>
public class LottieWebGLBridge : MonoBehaviour
{
    [Header("Lottie Player")]
    [SerializeField] private SkottiePlayerV2 skottiePlayer;

    [Header("Settings")]
    [SerializeField] private bool playOnReceive = true;
    [SerializeField] private float checkInterval = 0.5f;

    private string lastLoadedHash = "";

    // Import JavaScript functions per leggere da localStorage
    [DllImport("__Internal")]
    private static extern string GetLottieJSON();

    [DllImport("__Internal")]
    private static extern string GetLottieHash();

    private void Start()
    {
        if (skottiePlayer == null)
        {
            skottiePlayer = GetComponent<SkottiePlayerV2>();

            if (skottiePlayer == null)
            {
                Debug.LogError("LottieWebGLBridge: Nessun SkottiePlayerV2 trovato!");
            }
        }

        Debug.Log("LottieWebGLBridge: Pronto a ricevere animazioni da React (via localStorage)");

        InvokeRepeating(nameof(CheckForNewAnimation), 1f, checkInterval);
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
                Debug.Log($"LottieWebGLBridge: Rilevato nuovo JSON (hash: {hash})");

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
            Debug.LogError($"LottieWebGLBridge: Errore check localStorage: {ex.Message}");
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
            Debug.LogError("LottieWebGLBridge: JSON ricevuto e' vuoto!");
            return;
        }

        Debug.Log($"LottieWebGLBridge: JSON ricevuto ({jsonData.Length} caratteri)");

        if (skottiePlayer == null)
        {
            Debug.LogError("LottieWebGLBridge: SkottiePlayerV2 non assegnato!");
            return;
        }

        try
        {
            skottiePlayer.LoadAnimation(jsonData);

            double duration = skottiePlayer.GetDurations();
            Debug.Log($"LottieWebGLBridge: Animazione caricata - Durata: {duration}s");

            if (playOnReceive)
            {
                skottiePlayer.loop = true;
                skottiePlayer.PlayAnimation();
                Debug.Log("LottieWebGLBridge: Animazione avviata");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"LottieWebGLBridge: Errore caricamento: {ex.Message}\n{ex.StackTrace}");
        }
    }

    public void PauseAnimation()
    {
        Debug.Log("LottieWebGLBridge: Animazione in pausa");
    }

    public void PlayAnimation()
    {
        if (skottiePlayer != null)
        {
            skottiePlayer.PlayAnimation();
            Debug.Log("LottieWebGLBridge: Animazione avviata/ripresa");
        }
    }
}
