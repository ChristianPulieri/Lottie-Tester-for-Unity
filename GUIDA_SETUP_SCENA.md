# Guida Setup Scena Unity - Lottie File Picker

## Preparazione Completata ✅

Ho già creato:
- ✅ `Assets/Plugins/WebGLFilePicker.jslib` - Plugin JavaScript per file picker
- ✅ `Assets/Scripts/LottieFilePickerUI.cs` - Script UI per caricare file
- ✅ `Assets/WebGLTemplates/LottieTemplate/` - Template HTML ottimizzato

## Setup Scena (fai in Unity Editor)

### 1. Apri Unity Editor
Apri il progetto "Lottie Tester for WebGL"

### 2. Crea/Apri Scena
- File → New Scene → 2D
- Oppure usa la scena esistente

### 3. Crea UI Canvas

**a) Canvas principale:**
```
GameObject → UI → Canvas
```
Configurazione Canvas:
- Render Mode: Screen Space - Overlay
- Canvas Scaler:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920x1080
  - Match: 0.5 (width/height)

**b) Background (opzionale):**
```
Right-click su Canvas → UI → Panel
Rinomina: "Background"
```
Configurazione:
- Anchor: Stretch (tutti e 4 gli angoli)
- Color: #1a1a1a (grigio scuro) o nero

### 4. Crea Lottie Display

**a) Crea Image per Lottie:**
```
Right-click su Canvas → UI → Image
Rinomina: "LottieDisplay"
```

Configurazione RectTransform:
- Anchor: Center
- Width: 800
- Height: 800
- Pos X: 0
- Pos Y: 0

**b) Aggiungi ImageLottiePlayer:**
```
Click su LottieDisplay → Add Component → cerca "ImageLottiePlayer"
```

Configurazione ImageLottiePlayer:
- Animation Asset: (lascia vuoto, verrà caricato da file)
- Auto Play: None
- Loop: ✓
- Width: 512
- Height: 512
- Keep Aspect: ✓

### 5. Crea Button "Carica File"

**a) Crea Button:**
```
Right-click su Canvas → UI → Button - TextMeshPro
```
(Se chiede di importare TMP Essentials, clicca "Import TMP Essentials")

Rinomina: "LoadFileButton"

Configurazione RectTransform:
- Anchor: Bottom Center
- Width: 300
- Height: 60
- Pos X: 0
- Pos Y: 100

**b) Modifica testo del button:**
```
Espandi LoadFileButton → seleziona "Text (TMP)"
```
- Text: "📁 Carica File Lottie"
- Font Size: 20
- Alignment: Center
- Color: bianco

### 6. Crea Status Text

```
Right-click su Canvas → UI → Text - TextMeshPro
Rinomina: "StatusText"
```

Configurazione RectTransform:
- Anchor: Top Center
- Width: 1800
- Height: 60
- Pos X: 0
- Pos Y: -50

Configurazione Text:
- Text: "Pronto. Clicca 'Carica File' per iniziare."
- Font Size: 20
- Alignment: Center
- Color: #CCCCCC (grigio chiaro)
- Wrapping: Enabled

### 7. Crea File Name Text

```
Right-click su Canvas → UI → Text - TextMeshPro
Rinomina: "FileNameText"
```

Configurazione RectTransform:
- Anchor: Top Center
- Width: 1800
- Height: 40
- Pos X: 0
- Pos Y: -110

Configurazione Text:
- Text: ""
- Font Size: 16
- Alignment: Center
- Color: #888888 (grigio medio)
- Font Style: Italic

### 8. Aggiungi Script Manager

**a) Crea GameObject vuoto:**
```
Right-click nella Hierarchy → Create Empty
Rinomina: "LottieFilePickerManager"
```

**b) Aggiungi script:**
```
Click su LottieFilePickerManager → Add Component → cerca "LottieFilePickerUI"
```

**c) Assegna References (IMPORTANTE!):**

Nel componente `LottieFilePickerUI`:
- **Load File Button**: trascina `LoadFileButton` dall'Hierarchy
- **Status Text**: trascina `StatusText` dall'Hierarchy
- **File Name Text**: trascina `FileNameText` dall'Hierarchy
- **Lottie Player**: trascina `LottieDisplay` (quello con ImageLottiePlayer) dall'Hierarchy

Settings:
- **Auto Play**: ✓ (check)

### 9. Salva Scena
```
File → Save As → "LottieFilePicker.unity"
```

## Build Settings

### 1. Apri Build Settings
```
File → Build Settings
```

### 2. Aggiungi Scena
Se "LottieFilePicker" non è nella lista:
```
Click "Add Open Scenes"
```

### 3. Configura WebGL
- Platform: **WebGL** (seleziona e clicca "Switch Platform" se necessario)
- Compression Format: **Gzip**

### 4. Player Settings

Click "Player Settings..." → WebGL tab (icona Unity):

**a) Resolution and Presentation:**
- Default Canvas Width: 1920
- Default Canvas Height: 1080
- Run In Background: ✓

**b) Publishing Settings:**
- Compression Format: **Gzip**
- **Initial Memory Size: 256 MB** (IMPORTANTE!)
- **Maximum Memory Size: 1024 MB** (IMPORTANTE!)

**c) Other Settings (importante per template!):**
- WebGL Template: **LottieTemplate** (seleziona il nostro template custom)

### 5. Build

```
Click "Build"
Scegli folder: Export/StandaloneBuild
Click "Select Folder"
```

Aspetta il build (può richiedere alcuni minuti la prima volta).

## Test

### 1. Apri il Build
```
Apri: Export/StandaloneBuild/index.html
```
Nel browser (Chrome/Firefox/Safari).

### 2. Test File Picker
1. Aspetta che Unity carichi (barra di progresso)
2. Click sul button "📁 Carica File Lottie"
3. Seleziona un file JSON Lottie
4. L'animazione dovrebbe apparire e partire automaticamente

### 3. Controlla Console
Apri DevTools del browser (F12):
- Console: dovresti vedere "✅ Unity caricato e pronto"
- Quando carichi un file: "File selezionato: ..."
- "JSON ricevuto: XXX caratteri"
- "Animazione caricata - Durata: Xs"

## Troubleshooting

### "File picker funziona solo in WebGL build"
- Stai provando nell'Editor. Devi fare il build e aprire nel browser.

### Button non risponde
- Verifica che LottieFilePickerUI abbia il reference al button
- Controlla la console del browser per errori JavaScript

### "ImageLottiePlayer non assegnato"
- Nel GameObject LottieFilePickerManager, verifica che il campo "Lottie Player" punti a LottieDisplay

### Animazione non appare
- Controlla che ImageLottiePlayer abbia Width/Height > 0
- Verifica che Keep Aspect sia checked
- Controlla i log nella console del browser

### Memory access out of bounds
- Aumenta Initial Memory Size a 512 MB nelle Player Settings
- Prova con un JSON più piccolo/semplice prima

### Template non appare nella lista
- Chiudi e riapri Unity
- Verifica che la cartella Assets/WebGLTemplates/LottieTemplate esista
- Refresh: Right-click su Assets → Refresh

## Note
- Il file picker usa l'API nativa HTML `<input type="file">`
- Il JSON viene salvato come file temporaneo in persistentDataPath
- Funziona solo in WebGL build (non nell'Editor Unity)
- La prima animazione potrebbe richiedere un po' per caricare
