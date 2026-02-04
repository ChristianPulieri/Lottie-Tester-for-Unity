using UnityEngine;
using UnityEngine.UI;
using Gilzoide.LottiePlayer;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Switcher per animazioni Lottie preimportate come asset
/// Workflow: artisti copiano .json in Assets/LottieAnimations/, Unity li importa automaticamente
/// </summary>
public class LottieAssetSwitcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ImageLottiePlayer lottiePlayer;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Animations")]
    [Tooltip("Trascina qui tutte le LottieAnimationAsset da Assets/LottieAnimations/")]
    [SerializeField] private LottieAnimationAsset[] animations;

    [Header("Playback Controls")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button reloadButton;

    [Header("Settings")]
    [SerializeField] private bool autoPlayOnLoad = true;

    private int currentAnimationIndex = -1;

    private void Start()
    {
        Debug.Log("=== LottieAssetSwitcher START ===");

        // Verifica lottiePlayer
        if (lottiePlayer == null)
        {
            Debug.LogError("❌ ImageLottiePlayer non assegnato!");
            UpdateStatus("❌ ImageLottiePlayer mancante!");
            return;
        }

        // Setup dropdown
        if (dropdown != null)
        {
            SetupDropdown();
        }
        else
        {
            Debug.LogWarning("⚠️ Dropdown non assegnato - caricare solo prima animazione");
        }

        // Setup buttons
        if (playButton != null) playButton.onClick.AddListener(Play);
        if (pauseButton != null) pauseButton.onClick.AddListener(Pause);
        if (reloadButton != null) reloadButton.onClick.AddListener(ReloadCurrent);

        // Carica prima animazione
        if (animations.Length > 0)
        {
            Debug.Log($"✅ Trovate {animations.Length} animazioni");
            LoadAnimation(0);
        }
        else
        {
            Debug.LogWarning("⚠️ Nessuna animazione assegnata! Trascina LottieAnimationAsset nell'array 'Animations'");
            UpdateStatus("⚠️ Nessuna animazione disponibile. Aggiungi LottieAnimationAsset all'array.");
        }
    }

    private void SetupDropdown()
    {
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i] != null)
            {
                options.Add(animations[i].name);
            }
            else
            {
                options.Add($"[Null #{i}]");
                Debug.LogWarning($"⚠️ Animation slot {i} è null");
            }
        }

        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(OnDropdownChanged);

        Debug.Log($"✅ Dropdown configurato con {options.Count} opzioni");
    }

    private void OnDropdownChanged(int index)
    {
        Debug.Log($">>> Dropdown cambiato: index {index}");
        LoadAnimation(index);
    }

    private void LoadAnimation(int index)
    {
        if (index < 0 || index >= animations.Length)
        {
            Debug.LogError($"❌ Index {index} fuori range (0-{animations.Length - 1})");
            return;
        }

        if (animations[index] == null)
        {
            Debug.LogError($"❌ Animation all'index {index} è null");
            UpdateStatus($"❌ Animazione #{index} mancante");
            return;
        }

        Debug.Log($">>> Caricamento animazione: {animations[index].name}");
        UpdateStatus($"Caricamento {animations[index].name}...");

        try
        {
            // Setta l'animation asset
            lottiePlayer.SetAnimationAsset(animations[index]);
            currentAnimationIndex = index;

            // Info animazione
            var nativeAnim = animations[index].CreateNativeAnimation();
            if (nativeAnim.IsCreated)
            {
                double duration = nativeAnim.GetDuration();
                uint totalFrames = nativeAnim.GetTotalFrame();
                double fps = nativeAnim.GetFrameRate();
                Vector2Int size = nativeAnim.GetSize();

                string info = $"✅ {animations[index].name}\n" +
                             $"Durata: {duration:F2}s | Frame: {totalFrames} | FPS: {fps:F1} | Size: {size.x}x{size.y}";

                UpdateStatus(info);
                UpdateInfo(info);

                Debug.Log($"✅ Animazione caricata:\n" +
                         $"  Nome: {animations[index].name}\n" +
                         $"  Durata: {duration}s\n" +
                         $"  Frame: {totalFrames}\n" +
                         $"  FPS: {fps}\n" +
                         $"  Size: {size.x}x{size.y}");

                nativeAnim.Dispose(); // Dispose della copia temporanea
            }

            // Auto play
            if (autoPlayOnLoad)
            {
                lottiePlayer.Play();
                Debug.Log("✅ Auto-play avviato");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Errore caricamento animazione: {ex.Message}\n{ex.StackTrace}");
            UpdateStatus($"❌ Errore: {ex.Message}");
        }
    }

    public void Play()
    {
        if (lottiePlayer != null)
        {
            lottiePlayer.Play();
            Debug.Log("▶️ Play");
        }
    }

    public void Pause()
    {
        if (lottiePlayer != null)
        {
            lottiePlayer.Pause();
            Debug.Log("⏸ Pause");
        }
    }

    public void ReloadCurrent()
    {
        if (currentAnimationIndex >= 0)
        {
            Debug.Log("🔄 Reload");
            LoadAnimation(currentAnimationIndex);
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log($"[Status] {message}");
    }

    private void UpdateInfo(string message)
    {
        if (infoText != null)
        {
            infoText.text = message;
        }
    }

    // Metodi pubblici per testing/debug
    public void LoadNext()
    {
        if (animations.Length == 0) return;
        int next = (currentAnimationIndex + 1) % animations.Length;
        LoadAnimation(next);
        if (dropdown != null) dropdown.value = next;
    }

    public void LoadPrevious()
    {
        if (animations.Length == 0) return;
        int prev = currentAnimationIndex - 1;
        if (prev < 0) prev = animations.Length - 1;
        LoadAnimation(prev);
        if (dropdown != null) dropdown.value = prev;
    }
}
