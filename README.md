# Lottie Tester for WebGL

Sistema completo per testare animazioni JSON in Unity senza utilizzare plugin esterni come Lottie. Gestisce internamente il parsing e la riproduzione di animazioni definite tramite file JSON.

## Prompt Originale

Ti trovi all'interno di un progetto Unity vuoto. Crea una scena, con tutti gli script annessi, che permetta di testare con efficacia delle animazioni JSON. Non voglio utilizzare plugin come Lottie, ma gestire tutto internamente. Inserisci questo prompt nel .md

## Struttura del Progetto

```
Assets/
├── Animations/          # File JSON con le animazioni
│   ├── BounceAnimation.json
│   ├── FadeRotateAnimation.json
│   ├── ColorTransitionAnimation.json
│   └── ComplexAnimation.json
├── Scripts/            # Script C# del sistema
│   ├── AnimationData.cs
│   ├── AnimationPlayer.cs
│   └── AnimationTesterUI.cs
├── Scenes/            # Scene Unity
│   └── AnimationTesterScene.unity
└── Prefabs/           # Prefab (opzionale)
```

## Componenti Principali

### 1. AnimationData.cs
Definisce le strutture dati per le animazioni JSON:
- `AnimationClipData`: Contenitore principale dell'animazione
- `AnimatedObject`: Oggetto animato con le sue proprietà
- `PropertyAnimation`: Animazione di una singola proprietà
- `Keyframe`: Punto chiave dell'animazione
- `Vector4Value`: Valore generico per position, rotation, scale, color

### 2. AnimationPlayer.cs
Motore di riproduzione delle animazioni:
- Carica file JSON e li converte in dati di animazione
- Gestisce play, pause, stop
- Interpola valori tra keyframe
- Supporta diversi tipi di easing (linear, easeIn, easeOut, easeInOut)
- Applica animazioni a oggetti Unity (position, rotation, scale, color, alpha)

### 3. AnimationTesterUI.cs
Controller UI per testare le animazioni:
- Pulsanti Play, Pause, Stop
- Slider timeline per navigare nell'animazione
- Slider velocità di riproduzione
- Toggle per il loop
- Display informazioni animazione

## Formato JSON

### Struttura Base

```json
{
  "name": "Nome Animazione",
  "duration": 3.0,
  "frameRate": 60,
  "objects": [
    {
      "objectName": "NomeOggetto",
      "properties": [
        {
          "propertyName": "position",
          "easingType": "easeInOut",
          "keyframes": [
            {
              "time": 0.0,
              "value": { "x": 0, "y": 0, "z": 0, "w": 0 }
            },
            {
              "time": 1.0,
              "value": { "x": 5, "y": 2, "z": 0, "w": 0 }
            }
          ]
        }
      ]
    }
  ]
}
```

### Proprietà Supportate

- **position**: Posizione locale (Vector3)
- **rotation**: Rotazione in Euler angles (Vector3)
- **scale**: Scala locale (Vector3)
- **color**: Colore RGBA (Color)
- **alpha**: Solo trasparenza (float)

### Tipi di Easing

- `linear`: Interpolazione lineare
- `easeIn`: Accelerazione graduale
- `easeOut`: Decelerazione graduale
- `easeInOut`: Accelerazione e decelerazione

## Setup della Scena

### Passo 1: Creare gli Oggetti Animati

1. Crea GameObject nella scena (es: Cube, Sphere, Sprite)
2. Assegna nomi univoci (corrispondenti a `objectName` nel JSON)
3. Aggiungi componenti grafici (SpriteRenderer o Image per colori/alpha)

### Passo 2: Configurare AnimationPlayer

1. Crea un GameObject vuoto chiamato "AnimationManager"
2. Aggiungi il componente `AnimationPlayer`
3. Configura i parametri:
   - **Json Animation File**: Trascina il file JSON
   - **Play On Start**: Auto-play all'avvio
   - **Loop**: Ripeti in loop
   - **Playback Speed**: Velocità di riproduzione
   - **Target Objects**: Trascina gli oggetti da animare

### Passo 3: Setup UI (Opzionale)

1. Crea un Canvas con UI Elements:
   - Pulsanti: Play, Pause, Stop
   - Slider: Timeline e Speed
   - Text: Informazioni animazione
   - Toggle: Loop
2. Aggiungi il componente `AnimationTesterUI`
3. Collega i riferimenti UI nell'Inspector

## Esempi di Animazioni Incluse

### BounceAnimation.json
Animazione di rimbalzo con scala dinamica per un oggetto "Ball"

### FadeRotateAnimation.json
Rotazione completa con fade in/out e scaling per un oggetto "Logo"

### ColorTransitionAnimation.json
Transizione colori RGB con movimento orizzontale per un oggetto "Square"

### ComplexAnimation.json
Animazione complessa multi-oggetto con 3 elementi che si muovono e cambiano colore

## Utilizzo da Script

```csharp
// Ottenere riferimento al player
AnimationPlayer player = GetComponent<AnimationPlayer>();

// Caricare animazione da JSON
string jsonText = jsonFile.text;
player.LoadAnimation(jsonText);

// Controllo riproduzione
player.Play();
player.Pause();
player.Stop();

// Navigare timeline
player.SetTime(1.5f);

// Modificare velocità
player.playbackSpeed = 2.0f;
```

## Estensioni Possibili

1. **Nuove proprietà animabili**:
   - Aggiungi nuovi case in `ApplyProperty()` in AnimationPlayer.cs
   - Esempi: intensità luce, volume audio, material properties

2. **Easing personalizzati**:
   - Modifica `ApplyEasing()` per aggiungere curve personalizzate
   - Supporto per AnimationCurve di Unity

3. **Eventi timeline**:
   - Aggiungi sistema di callback a tempi specifici
   - Trigger per suoni, particelle, etc.

4. **Path animation**:
   - Supporto per bezier curves
   - Animazione lungo percorsi complessi

5. **Export/Import**:
   - Tool editor per creare animazioni visualmente
   - Export da Unity Animation Clips a JSON

## Requisiti

- Unity 2020.3 o superiore
- TextMeshPro (per UI avanzata)
- Nessun plugin esterno richiesto

## Performance

Il sistema è ottimizzato per:
- Animazioni fino a 60 FPS
- Multiple oggetti animati simultaneamente
- WebGL build compatibili

Per migliorare le performance:
- Riduci il numero di keyframe
- Usa easing appropriato
- Limita gli oggetti animati contemporaneamente

## Troubleshooting

**L'animazione non parte**:
- Verifica che i nomi degli oggetti nel JSON corrispondano ai GameObject
- Controlla che il JSON sia valido
- Assicurati che gli oggetti siano nella lista Target Objects

**I colori non cambiano**:
- Aggiungi SpriteRenderer o Image component
- Verifica formato RGBA nel JSON (valori 0-1)

**Interpolazione irregolare**:
- Controlla che i keyframe siano ordinati per time
- Verifica che duration copra tutti i keyframe

## Licenza

Progetto di esempio per uso educativo e testing.
