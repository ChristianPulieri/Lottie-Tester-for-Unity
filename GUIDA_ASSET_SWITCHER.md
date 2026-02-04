# Guida: Lottie Asset Switcher

## Sistema Per Artisti - Nessun Crash, Funziona in Editor

Questo sistema permette agli artisti di testare animazioni Lottie **direttamente in Unity Editor** senza build e senza crash.

---

## Setup Iniziale (Una Volta Sola)

### 1. Crea Cartella Animazioni

```
Nella finestra Project:
Assets → Right-click → Create → Folder
Nome: "LottieAnimations"
```

Questa cartella conterrà tutti i file .json Lottie.

### 2. Crea UI Scene

**Canvas:**
```
Hierarchy → Right-click → UI → Canvas
```
- Canvas Scaler → UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920x1080

**Background (opzionale):**
```
Right-click su Canvas → UI → Panel
Rinomina: "Background"
Color: #1a1a1a (grigio scuro)
```

**Lottie Display:**
```
Right-click su Canvas → UI → Image
Rinomina: "LottieDisplay"
```
- RectTransform:
  - Anchor: Center
  - Width: 800
  - Height: 800
- Add Component: `ImageLottiePlayer`
  - **Animation Asset: LASCIA VUOTO**
  - Auto Play: None
  - Loop: ✓
  - Width: 512
  - Height: 512
  - Keep Aspect: ✓

**Dropdown per selezione:**
```
Right-click su Canvas → UI → Dropdown - TextMeshPro
Rinomina: "AnimationDropdown"
```
- RectTransform:
  - Anchor: Top Center
  - Width: 600
  - Height: 40
  - Pos Y: -50

**Status Text:**
```
Right-click su Canvas → UI → Text - TextMeshPro
Rinomina: "StatusText"
```
- RectTransform:
  - Anchor: Bottom Center
  - Width: 1800
  - Height: 80
  - Pos Y: 100
- Font Size: 18
- Alignment: Center
- Color: #FFFFFF
- Wrapping: Enabled

**Info Text:**
```
Right-click su Canvas → UI → Text - TextMeshPro
Rinomina: "InfoText"
```
- RectTransform:
  - Anchor: Top Left
  - Width: 400
  - Height: 200
  - Pos: 20, -100
- Font Size: 14
- Alignment: Top Left
- Color: #CCCCCC

**Buttons Playback (opzionale):**
```
Right-click su Canvas → UI → Button - TextMeshPro
Rinomina: "PlayButton"
Text: "▶️ Play"

Right-click su Canvas → UI → Button - TextMeshPro
Rinomina: "PauseButton"
Text: "⏸ Pause"

Right-click su Canvas → UI → Button - TextMeshPro
Rinomina: "ReloadButton"
Text: "🔄 Reload"
```

### 3. Aggiungi Script Manager

```
Hierarchy → Right-click → Create Empty
Rinomina: "LottieManager"
```

Add Component: `LottieAssetSwitcher`

**Assegna References:**
- **Lottie Player**: trascina "LottieDisplay" (quello con ImageLottiePlayer)
- **Dropdown**: trascina "AnimationDropdown"
- **Status Text**: trascina "StatusText"
- **Info Text**: trascina "InfoText"
- **Play Button**: trascina "PlayButton" (se lo hai creato)
- **Pause Button**: trascina "PauseButton" (se lo hai creato)
- **Reload Button**: trascina "ReloadButton" (se lo hai creato)
- **Animations**: LASCIA VUOTO per ora (lo popoleremo dopo)
- **Auto Play On Load**: ✓

### 4. Salva Scena

```
File → Save As → "LottieTester.unity"
```

---

## Workflow Artista (Quotidiano)

### Per Testare Una Nuova Animazione:

**1. Esporta JSON da After Effects**
   - Bodymovin/Lottie plugin
   - Esporta come .json

**2. Copia nella cartella Unity:**
   ```
   Trascina il file .json in Assets/LottieAnimations/ nella finestra Project
   ```
   Unity lo importa automaticamente come `LottieAnimationAsset`

**3. Aggiungi all'array:**
   - Seleziona "LottieManager" in Hierarchy
   - Nel componente `LottieAssetSwitcher`:
     - Espandi "Animations"
     - Aumenta "Size" di 1
     - Nell'ultimo slot vuoto, trascina il nuovo LottieAnimationAsset dalla cartella LottieAnimations

**4. Test:**
   - Premi Play ▶️
   - Seleziona l'animazione dal dropdown
   - L'animazione parte automaticamente!

**5. Modifiche:**
   - Se modifichi il JSON e lo risalvi, **reimportalo**:
     - Right-click sul file in Project → Reimport
     - Oppure: Delete + ri-trascina il file

---

## Comandi Rapidi

### In Play Mode:

- **Dropdown**: Seleziona animazione da testare
- **Play button**: Avvia/riavvia animazione
- **Pause button**: Metti in pausa
- **Reload button**: Ricarica l'animazione corrente

### Keyboard Shortcuts (opzionali - da aggiungere se servono):

Puoi estendere lo script per aggiungere:
- Freccia SU/GIÙ: animazione precedente/successiva
- SPAZIO: Play/Pause
- R: Reload

---

## Tips & Tricks

### Organizzazione File:

```
Assets/
  LottieAnimations/
    Character/
      walk.json
      run.json
      idle.json
    UI/
      button_hover.json
      loading.json
    Effects/
      explosion.json
      sparkle.json
```

Usa sottocartelle per organizzare per categoria.

### Info Visualizzate:

Il sistema mostra automaticamente:
- Nome animazione
- Durata (secondi)
- Numero di frame
- FPS
- Risoluzione (width x height)

### Se l'animazione non appare:

1. **Check ImageLottiePlayer:**
   - Width/Height devono essere > 0
   - Keep Aspect deve essere ✓

2. **Check RectTransform:**
   - Verifica che LottieDisplay sia visibile nella Scene view
   - Anchor e posizione corretti

3. **Check Console:**
   - Cerca errori rossi 🔴
   - Logs utili iniziano con ">>>" o emoji (✅ ❌ ⚠️)

4. **Check Animation Asset:**
   - In Project, seleziona il .json
   - Inspector dovrebbe mostrare "LottieAnimationAsset"
   - Se mostra solo "TextAsset", reimporta il file

---

## Vantaggi vs File Picker Dinamico

| Feature | Asset Switcher ✅ | File Picker ❌ |
|---------|------------------|----------------|
| Funziona in Editor | ✅ Sì | ❌ No (solo build) |
| Crash | ✅ Mai | ❌ Spesso |
| Velocità test | ✅ Istantaneo | ❌ Rebuild ogni volta |
| Limite memoria | ✅ Nessuno | ❌ WebGL crash |
| Workflow | ✅ Drag & drop | ❌ File picker |
| Multi-animazioni | ✅ Dropdown facile | ❌ Browse ripetuto |
| Version control | ✅ Tracciato in Git | ❌ File esterni |

---

## Distribuzione Agli Artisti

Se vuoi condividere questo tool:

### Opzione 1: Progetto Unity

Comprimi la cartella Unity e condividi:
```
LottieTester.zip
  ├── Assets/
  ├── Packages/
  ├── ProjectSettings/
  └── LottieTester.unity
```

Gli artisti:
1. Decomprimono
2. Aprono in Unity Hub
3. Aprono la scena LottieTester.unity
4. Trascinano .json in Assets/LottieAnimations/
5. Play!

### Opzione 2: Build Standalone (se proprio vuoi file picker)

Fai build Mac/Windows e distribuisci su Itch.io (vedi GUIDA_MAC_STANDALONE_ITCH.md)

---

## Estensioni Opzionali

### 1. Auto-Scan Cartella:

Aggiungi allo script per trovare automaticamente tutti i .json:

```csharp
[ContextMenu("Auto Find Animations")]
private void AutoFindAnimations()
{
#if UNITY_EDITOR
    string[] guids = UnityEditor.AssetDatabase.FindAssets("t:LottieAnimationAsset", new[] { "Assets/LottieAnimations" });
    List<LottieAnimationAsset> found = new List<LottieAnimationAsset>();

    foreach (string guid in guids)
    {
        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
        var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<LottieAnimationAsset>(path);
        if (asset != null) found.Add(asset);
    }

    animations = found.ToArray();
    Debug.Log($"✅ Trovate {animations.Length} animazioni");
#endif
}
```

Right-click sul componente → Auto Find Animations

### 2. Export Ancora Più Veloce:

Crea uno script Editor per export diretto da AE:

```csharp
// Assets/Editor/LottieImporter.cs
[InitializeOnLoad]
public class LottieAutoRefresh
{
    static LottieAutoRefresh()
    {
        UnityEditor.AssetDatabase.importPackageCompleted += OnPackageImport;
    }

    static void OnPackageImport(string packageName)
    {
        if (packageName.Contains(".json"))
        {
            Debug.Log("🔄 Lottie JSON importato automaticamente");
        }
    }
}
```

### 3. Hotkey per Next/Previous:

```csharp
private void Update()
{
    if (Input.GetKeyDown(KeyCode.RightArrow)) LoadNext();
    if (Input.GetKeyDown(KeyCode.LeftArrow)) LoadPrevious();
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (lottiePlayer.IsPlaying) Pause();
        else Play();
    }
}
```

---

## Troubleshooting

### "Nessuna animazione disponibile"
- Hai trascinato i LottieAnimationAsset nell'array Animations del componente?

### "Animation slot X è null"
- Un elemento dell'array è vuoto, rimuovilo o assegna un asset

### Dropdown vuoto
- Il dropdown è assegnato?
- L'array Animations ha elementi?

### Animazione non si aggiorna dopo modifica JSON
- Right-click sul file → Reimport
- Oppure: Reload button nell'app

### "ImageLottiePlayer mancante"
- Hai trascinato l'oggetto con ImageLottiePlayer nel campo "Lottie Player"?

---

## Costi

- Unity: **Gratis** (Unity Personal)
- Plugin Lottie: **Gratis** (open source)
- Workflow: **Gratis**

**Totale: $0** 🎉

Workflow perfetto per artisti, nessun programmatore necessario!
