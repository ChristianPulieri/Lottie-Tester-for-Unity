# Setup Unity Standalone con File Picker

## Nuova Architettura
Invece di embedddare Unity in React, Unity gira **standalone** con un file picker integrato per caricare JSON Lottie direttamente dal browser.

## Vantaggi
- ✅ Nessuna comunicazione iframe/localStorage
- ✅ Nessun problema di memory overflow
- ✅ UI più semplice e diretta
- ✅ Più performante

## Setup Scena Unity

### 1. Apri Unity e carica la scena principale

### 2. Crea la UI

**Canvas:**
1. GameObject → UI → Canvas
2. Canvas Scaler:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920x1080

**Background Panel:**
1. Sotto Canvas: GameObject → UI → Panel
2. Rinomina: "Background"
3. Color: nero o grigio scuro

**Lottie Display:**
1. GameObject → UI → Image
2. Rinomina: "LottieDisplay"
3. Aggiungi componente: `ImageLottiePlayer`
4. Configurazione:
   - Width: 512 (o maggiore)
   - Height: 512
   - Keep Aspect: ✓
   - Loop: ✓
   - Auto Play: None

**Load Button:**
1. GameObject → UI → Button - TextMeshPro
2. Rinomina: "LoadFileButton"
3. Testo: "Carica File Lottie JSON"
4. Position: in basso o dove preferisci

**Status Text:**
1. GameObject → UI → Text - TextMeshPro
2. Rinomina: "StatusText"
3. Font Size: 18-24
4. Alignment: Center
5. Color: bianco

**File Name Text:**
1. GameObject → UI → Text - TextMeshPro
2. Rinomina: "FileNameText"
3. Font Size: 14-16
4. Color: grigio chiaro

### 3. Aggiungi Script

1. Crea un GameObject vuoto: "LottieFilePicker"
2. Aggiungi il component `LottieFilePickerUI`
3. Assegna references:
   - Load File Button → LoadFileButton
   - Status Text → StatusText
   - File Name Text → FileNameText
   - Lottie Player → LottieDisplay (ImageLottiePlayer)
4. Settings:
   - Auto Play: ✓

### 4. Build Settings

**File → Build Settings:**
- Platform: WebGL
- Compression Format: **Gzip**
- Build to: `Export/StandaloneBuild`

**Player Settings (importante!):**
- Publishing Settings:
  - Initial Memory Size: **256 MB** (o più)
  - Maximum Memory Size: **1024 MB**

### 5. Build & Test

1. Click "Build"
2. Apri `Export/StandaloneBuild/index.html` in un browser
3. Click "Carica File Lottie JSON"
4. Seleziona un file .json
5. L'animazione dovrebbe apparire e partire automaticamente

## Troubleshooting

**"File picker funziona solo in WebGL build"**
- Stai provando nell'Editor. Fai il build WebGL e prova nel browser.

**"JSON vuoto o non valido"**
- Assicurati che il file sia un JSON Lottie valido
- Controlla la console del browser per errori

**Memory access out of bounds**
- Aumenta Initial Memory Size nelle Player Settings
- Prova con JSON più piccoli

**Animazione non appare**
- Controlla che ImageLottiePlayer sia configurato correttamente
- Verifica Width/Height siano > 0
- Controlla i log Unity nella console del browser

## Note

- Il file picker usa l'API nativa del browser (più affidabile)
- Il JSON viene salvato come file temporaneo per evitare problemi di memoria
- Funziona solo in WebGL build (non nell'Editor)
