# Guida: Unity Mac Standalone + Distribuzione Itch.io

## Tool per Artisti - Lottie Tester

Questo tool permette agli artisti di testare animazioni Lottie in Unity **senza limiti di memoria WebGL**.

---

## Parte 1: Setup Scena Unity

### 1. Apri Unity Editor

### 2. Crea Scena UI

**Canvas:**
```
GameObject → UI → Canvas
```
- Canvas Scaler → UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920x1080

**Background Panel (opzionale):**
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
  - Auto Play: None
  - Loop: ✓
  - Width: 512
  - Height: 512
  - Keep Aspect: ✓

**Instructions Text (grande, centrale):**
```
Right-click su Canvas → UI → Text - TextMeshPro
Rinomina: "InstructionsText"
```
- RectTransform:
  - Anchor: Center
  - Width: 1200
  - Height: 300
  - Pos: 0, 0
- Text: "Trascina un file .json nella finestra"
- Font Size: 48
- Alignment: Center
- Color: #FFFFFF (bianco)
- Enable Auto Size

**Status Text:**
```
Right-click su Canvas → UI → Text - TextMeshPro
Rinomina: "StatusText"
```
- RectTransform:
  - Anchor: Bottom Center
  - Width: 1800
  - Height: 60
  - Pos Y: 100
- Font Size: 20
- Alignment: Center
- Color: #CCCCCC

**File Name Text:**
```
Right-click su Canvas → UI → Text - TextMeshPro
Rinomina: "FileNameText"
```
- RectTransform:
  - Anchor: Top Center
  - Width: 1800
  - Height: 40
  - Pos Y: -50
- Font Size: 16
- Alignment: Center
- Color: #888888
- Font Style: Italic

**Help Button (opzionale ma utile):**
```
Right-click su Canvas → UI → Button - TextMeshPro
Rinomina: "HelpButton"
```
- RectTransform:
  - Anchor: Bottom Center
  - Width: 200
  - Height: 50
  - Pos Y: 30
- Text: "❓ Aiuto"

### 3. Aggiungi Script Manager

```
Hierarchy → Right-click → Create Empty
Rinomina: "LottieFilePickerManager"
```

Add Component: `LottieFilePickerStandalone`

**Assegna References:**
- Load File Button: HelpButton (opzionale)
- Status Text: StatusText
- File Name Text: FileNameText
- Instructions Text: InstructionsText
- Lottie Player: LottieDisplay (ImageLottiePlayer)

Settings:
- Auto Play: ✓
- Allow Drag And Drop: ✓

### 4. Salva Scena

```
File → Save As → "LottieTesterStandalone.unity"
```

---

## Parte 2: Build Mac Standalone

### 1. Build Settings

```
File → Build Settings
```

- Platform: **Mac, Linux, Windows** (seleziona Mac)
  - Se non vedi Mac: Click "Switch Platform" (può richiedere qualche minuto)
- Architecture: **Apple Silicon** (se hai M1/M2/M3) o **Intel 64-bit** (se hai Intel Mac)
  - **IMPORTANTE**: Se vuoi supportare entrambi, seleziona **Universal**

### 2. Aggiungi Scena

```
Click "Add Open Scenes"
```
Verifica che "LottieTesterStandalone" sia nella lista.

### 3. Player Settings

Click "Player Settings..." → Mac icon:

**Resolution and Presentation:**
- Fullscreen Mode: **Windowed**
- Default Screen Width: **1920**
- Default Screen Height: **1080**
- Resizable Window: ✓
- Run In Background: ✓

**Other Settings:**
- Color Space: **Linear** (per migliore qualità)
- Scripting Backend: **IL2CPP** (più performante)

**Identification:**
- Company Name: "Il tuo nome/studio"
- Product Name: "Lottie Tester for Unity"
- Version: 1.0.0

**Icon (opzionale ma professionale):**
- Default Icon: (assegna un'icona se ne hai una)

### 4. Build!

```
Click "Build"
```

Scegli destinazione:
- Folder: `Export/MacStandalone`
- Nome app: `LottieTester.app`

Click "Save" e aspetta (può richiedere 5-10 minuti).

### 5. Test Locale

1. Vai in `Export/MacStandalone/`
2. Double-click su `LottieTester.app`
3. Se macOS dice "non può essere aperto perché lo sviluppatore non è verificato":
   - Right-click sulla .app → Apri
   - Click "Apri" nel dialogo
4. Trascina un file .json nella finestra
5. L'animazione dovrebbe apparire!

---

## Parte 3: Distribuzione su Itch.io

### 1. Crea Account Itch.io

1. Vai su https://itch.io
2. Click "Register"
3. Scegli username (sarà nell'URL: `username.itch.io`)
4. Conferma email

### 2. Prepara il Build per Upload

**a) Comprimi la .app:**
```bash
cd "Export/MacStandalone"
zip -r LottieTester-Mac.zip LottieTester.app
```

**Dimensione finale:** circa 50-100 MB (dipende dal progetto)

**b) Crea README.txt:**
```
Lottie Tester for Unity
Versione 1.0

COME USARE:
1. Apri l'app LottieTester
2. Trascina un file .json Lottie nella finestra
3. L'animazione apparirà automaticamente

REQUISITI:
- macOS 10.15 o superiore
- Apple Silicon (M1/M2/M3) o Intel

SUPPORTO:
Per problemi o domande: tuaemail@example.com

---
Creato con Unity e unity-lottie-player
```

### 3. Crea Progetto su Itch.io

1. Vai su https://itch.io/game/new
2. Compila:

**Title:** Lottie Tester for Unity

**Project URL:** `lottie-tester` (diventerà `username.itch.io/lottie-tester`)

**Short description:**
```
Test your Lottie JSON animations in Unity. Simple drag-and-drop tool for artists and animators.
```

**Classification:**
- Kind: **Tool**
- Release status: **Released**

**Pricing:**
- **Free** (o "Pay what you want" se vuoi donazioni opzionali)

**Uploads:**
- Click "Upload files"
- Seleziona `LottieTester-Mac.zip`
- This file will be played in the: **Desktop** (Not browser)

**Platform:** ✓ macOS

**System requirements:**
- macOS 10.15+
- Supporta Apple Silicon e Intel

**Details:**
```markdown
# Lottie Tester for Unity

Un tool semplice per testare animazioni Lottie (.json) in Unity.

## Come usare:
1. Scarica e apri l'app
2. Trascina un file .json nella finestra
3. L'animazione apparirà automaticamente

## Caratteristiche:
- ✅ Drag and drop semplice
- ✅ Nessun limite di dimensione JSON
- ✅ Info dettagliate (durata, FPS, risoluzione)
- ✅ Playback in loop automatico

## Requisiti:
- macOS 10.15 o superiore
- File .json Lottie validi

---

Creato con Unity e [unity-lottie-player](https://github.com/gilzoide/unity-lottie-player)
```

**Screenshots (importante!):**
1. Fai uno screenshot dell'app in azione
2. Upload come Cover Image
3. Aggiungi 2-3 screenshot aggiuntivi

**Tags:**
- tool
- unity
- lottie
- animation
- json

**Visibility & access:**
- **Public** (o "Restricted" se vuoi solo condividere il link con i tuoi artisti)

### 4. Pubblica!

Click **Save & view page**

Il tuo tool sarà disponibile a:
```
https://username.itch.io/lottie-tester
```

---

## Parte 4: Condivisione con Artisti

### Opzione A: Link Diretto (Pubblico)

Condividi semplicemente:
```
https://username.itch.io/lottie-tester
```

Gli artisti:
1. Aprono il link
2. Click "Download"
3. Scaricano il .zip
4. Decomprimono
5. Aprono l'app

### Opzione B: Link Privato (Solo per il tuo team)

Se hai impostato "Restricted":
1. Vai su Dashboard → Your Game → Distribute
2. Abilita "Download keys"
3. Genera keys per ogni artista
4. Condividi key + link

### Opzione C: Download Diretto (Più semplice)

Carica il .zip anche su:
- Google Drive
- Dropbox
- OneDrive

E condividi il link diretto.

---

## Parte 5: Aggiornamenti Futuri

### Come fare una nuova versione:

1. **In Unity:**
   - Modifica quello che serve
   - Player Settings → Version: incrementa (es. 1.0.0 → 1.1.0)
   - Build di nuovo

2. **Su Itch.io:**
   - Dashboard → Your Game → Edit game
   - Upload files → Upload la nuova versione
   - (Itch.io mantiene automaticamente la vecchia versione come backup)
   - Save

3. **Notifica artisti:**
   - Itch.io ha un sistema di "Devlog" per annunci
   - Oppure manda email/messaggio al team

---

## Troubleshooting

### macOS dice "l'app è danneggiata"

Questo succede perché l'app non è firmata con certificato Apple Developer ($99/anno).

**Soluzione per gli artisti:**
```bash
# Rimuovi quarantena
xattr -cr /path/to/LottieTester.app

# Poi apri normalmente
```

Oppure:
```
Right-click → Apri (invece di double-click)
```

### App crasha al lancio

- Verifica che abbiano macOS 10.15+
- Controlla che l'architettura sia corretta (Intel vs Apple Silicon)
- Chiedi di aprire Console.app e cercare errori

### Animazione non appare

- Verifica che il JSON sia valido Lottie
- Controlla che il file sia effettivamente .json
- Prova con un JSON semplice/piccolo prima

### "Build Windows anche?"

Se gli artisti usano Windows:
1. File → Build Settings → Windows
2. Architecture: x86_64
3. Build
4. Comprimi e upload su Itch.io come secondo file
5. Itch.io rileverà automaticamente e mostrerà pulsanti separati Mac/Win

---

## Costi

- **Unity:** Gratis (Unity Personal è free fino a $100k revenue)
- **Itch.io:** Gratis (0% fee)
- **Hosting:** Gratis (Itch.io ospita i file)
- **Distribuzione:** Gratis (download illimitati)

**Totale: $0** 🎉

---

## Note Finali

- Gli artisti **non hanno bisogno di Unity** installato
- Non serve account Itch.io per scaricare (se pubblico)
- Il tool funziona completamente offline
- Nessun limite di dimensione JSON (a differenza di WebGL)

Buon testing! 🎨
