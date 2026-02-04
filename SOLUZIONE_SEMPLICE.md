# Soluzione Semplice - Usa Asset Preimportati

## Il Problema

Il plugin `unity-lottie-player` con caricamento dinamico:
- ❌ Crasha in Unity Editor Play Mode
- ❌ Crasha in WebGL (memory overflow)
- ✅ Funziona SOLO in Build Standalone (Mac/Windows/Linux)

## Soluzione Per Gli Artisti

**Invece di file picker dinamico, usa drag & drop in Unity Editor:**

### Setup:

1. **Crea cartella:**
   - `Assets/LottieAnimations/` (per i file .json)

2. **Gli artisti:**
   - Copiano il file .json in `Assets/LottieAnimations/`
   - Unity lo importa automaticamente come `LottieAnimationAsset`
   - Trascinano l'asset sul player
   - Premi Play → l'animazione funziona!

3. **Script opzionale per dropdown:**

```csharp
using UnityEngine;
using UnityEngine.UI;
using Gilzoide.LottiePlayer;
using TMPro;

public class LottieAssetSwitcher : MonoBehaviour
{
    [SerializeField] private ImageLottiePlayer lottiePlayer;
    [SerializeField] private LottieAnimationAsset[] animations;
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        // Popola dropdown
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var anim in animations)
        {
            options.Add(anim.name);
        }
        dropdown.AddOptions(options);

        // Listener
        dropdown.onValueChanged.AddListener(OnDropdownChanged);

        // Carica prima animazione
        if (animations.Length > 0)
        {
            LoadAnimation(0);
        }
    }

    private void OnDropdownChanged(int index)
    {
        LoadAnimation(index);
    }

    private void LoadAnimation(int index)
    {
        if (index >= 0 && index < animations.Length)
        {
            lottiePlayer.SetAnimationAsset(animations[index]);
            lottiePlayer.Play();
        }
    }
}
```

### Vantaggi:

- ✅ Funziona in Editor (no build necessari)
- ✅ Nessun crash
- ✅ Nessun limite di memoria
- ✅ Gli artisti vedono subito il risultato
- ✅ Possono switchare tra animazioni con dropdown

### Workflow Artista:

1. Esporta JSON da After Effects
2. Copia in `Assets/LottieAnimations/`
3. Unity reimporta automaticamente
4. Seleziona dal dropdown in Play Mode
5. Vede l'animazione immediatamente

## Alternative

Se vuoi DAVVERO file picker dinamico:
- **DEVI fare Build Standalone** (non funziona in Editor)
- Distribuisci la .app su Itch.io
- Gli artisti scaricano e usano l'app standalone

Ma per workflow veloce, **drag & drop in Unity Editor è molto più pratico**.
