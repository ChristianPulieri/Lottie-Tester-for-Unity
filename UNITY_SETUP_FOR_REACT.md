# Setup Unity per Integrazione con React

## Script Creato

È stato creato lo script `Assets/Scripts/LottieWebGLBridge.cs` che permette a React di inviare JSON delle animazioni Lottie a Unity tramite `SendMessage`.

## Setup in Unity

### 1. Aprire il Progetto Unity
1. Apri Unity Hub
2. Apri il progetto "Lottie Tester for WebGL"
3. Aspetta che Unity compili lo script

### 2. Configurare la Scena

1. **Apri la scena** `Assets/Scenes/AnimationTesterScene.unity`

2. **Crea un Canvas (se non esiste già)**:
   - Hierarchy → Click destro → UI → Canvas
   - Il Canvas è necessario perché Image Lottie Player è un componente UI

3. **Crea un GameObject per il Bridge**:
   - Hierarchy → Click destro sul Canvas → UI → Image
   - Rinominalo in "LottieBridge"
   - IMPORTANTE: Deve essere figlio del Canvas!

4. **Rimuovi il componente Image (se presente) e aggiungi Image Lottie Player**:
   - Seleziona "LottieBridge"
   - Se c'è un componente "Image", rimuovilo (click destro → Remove Component)
   - Inspector → Add Component → Cerca "Image Lottie Player"
   - Aggiungi il componente
   - **Configura Image Lottie Player**:
     - Animation Asset: (lascia vuoto, verrà impostato da script)
     - Auto Play: On Start
     - Loop: ✅ (attivato)
     - Width: 960
     - Height: 600
     - Keep Aspect: ✅ (attivato)

5. **Aggiungi lo script LottieWebGLBridge**:
   - Con "LottieBridge" selezionato
   - Inspector → Add Component → Cerca "Lottie Web GL Bridge"
   - Aggiungi il componente

6. **Configura LottieWebGLBridge**:
   - Nel componente LottieWebGLBridge appena aggiunto
   - Trascina il componente **Image Lottie Player** (che hai aggiunto allo stesso GameObject) nel campo "Lottie Player"
   - ✅ **Play On Receive**: attivato (per far partire l'animazione automaticamente quando riceve JSON da React)

7. **IMPORTANTE - Verifica il nome del GameObject**:
   - Il GameObject DEVE chiamarsi esattamente **"LottieBridge"** (case-sensitive!)
   - React userà questo nome per inviare i messaggi

### 3. Build WebGL

1. **File → Build Settings**
2. **Platform: WebGL**
   - Se non è già selezionato, clicca "Switch Platform" (e aspetta)
3. **Player Settings** (pulsante in basso a sinistra):
   - **Resolution and Presentation**:
     - Default Canvas Width: 960
     - Default Canvas Height: 600
     - WebGL Template: Default (o Minimal)
   - **Publishing Settings**:
     - Compression Format: Brotli (già configurato)
     - Decompression Fallback: attivato
4. **Scenes in Build**:
   - Assicurati che "AnimationTesterScene" sia nella lista
   - Se non c'è, clicca "Add Open Scenes"
5. **Build**:
   - Clicca "Build"
   - Seleziona la cartella: `Export/ReactBuild`
   - Nomina il build: "LottieTester"
   - Aspetta il completamento del build

### 4. Verifica File Build

Dopo il build, la cartella `Export/ReactBuild` dovrebbe contenere:
```
ReactBuild/
├── index.html
├── TemplateData/
│   └── (vari file CSS e immagini)
└── Build/
    ├── LottieTester.loader.js
    ├── LottieTester.framework.js.br
    ├── LottieTester.data.br
    └── LottieTester.wasm.br
```

## Comunicazione React → Unity

Una volta integrato in React, potrai inviare animazioni JSON con:

```javascript
// Invia JSON a Unity
unityInstance.SendMessage('LottieBridge', 'ReceiveLottieJSON', jsonString);

// Altri controlli disponibili
unityInstance.SendMessage('LottieBridge', 'PlayAnimation');
unityInstance.SendMessage('LottieBridge', 'PauseAnimation');
unityInstance.SendMessage('LottieBridge', 'StopAnimation');
unityInstance.SendMessage('LottieBridge', 'SetSpeed', '2.0');
unityInstance.SendMessage('LottieBridge', 'SetLoop', '1');  // 1 = true, 0 = false
```

## Troubleshooting

**Script non si compila**:
- Assicurati che il plugin `unity-lottie-player` sia installato in `Packages/manifest.json`
- Verifica che la versione di Unity sia 2020.3+

**Image Lottie Player non appare nei componenti**:
- Verifica in Window → Package Manager che "Lottie Player" sia installato
- URL: `https://github.com/gilzoide/unity-lottie-player.git#1.1.1`
- Riavvia Unity se necessario

**Build fallisce**:
- Controlla la Console per errori di compilazione
- Verifica che WebGL sia installato in Unity Hub

**Animazione non appare in Unity**:
- Verifica che il componente Image Lottie Player sia assegnato nel campo dell'Inspector dello script LottieWebGLBridge
- Controlla che il GameObject "LottieBridge" abbia un Canvas come parent o crea un Canvas nella scena
- Controlla i log della Console WebGL del browser (F12 → Console)

## Note

- Il GameObject **DEVE** chiamarsi "LottieBridge" (React cercherà questo nome)
- Lo script è pronto per ricevere qualsiasi JSON Lottie valido
- L'animazione parte automaticamente quando riceve il JSON (se Play On Receive è attivo)
- Il build andrà copiato nella cartella `public` del progetto React

## ⚠️ Nota Importante su WebGL

Lo script carica il JSON dinamicamente usando `NativeLottieAnimation(jsonData, cacheKey, "")`, che crea l'animazione direttamente dalla stringa JSON.

**Se riscontri problemi in WebGL**:
1. Verifica la Console WebGL del browser (F12 → Console) per errori specifici
2. Controlla che il JSON sia valido e completo
3. Prova con animazioni JSON semplici prima di testare JSON complessi

**Nota**: `LottieAnimationAssetImporter` (nella cartella Editor) serve solo per importare asset statici in Unity Editor e **non** è disponibile a runtime in WebGL builds. Il nostro approccio dinamico non lo usa.
