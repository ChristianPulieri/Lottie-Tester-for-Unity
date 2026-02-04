# Lottie Animation Player for Unity

Sistema completo per riprodurre animazioni Lottie (Bodymovin/After Effects) in Unity senza plugin esterni.

## Caratteristiche

- **Parser Lottie completo**: Supporta il formato JSON standard di Bodymovin
- **Interpolazione avanzata**: Bezier easing curves per animazioni fluide
- **Rendering 2D**: LineRenderer e SpriteRenderer per forme e riempimenti
- **Controlli timeline**: Play, Pause, Stop, Scrubbing, Speed control
- **Supporto layer**: Multiple layer con trasformazioni indipendenti
- **Animazioni proprietà**: Position, Scale, Rotation, Opacity, Color
- **Shape support**: Paths, Rectangle, Ellipse, Fill, Stroke, Groups

## Struttura del Progetto

```
Assets/
├── Scripts/
│   ├── LottieData.cs              # Strutture dati Lottie
│   ├── LottieParser.cs            # Parser JSON con Newtonsoft
│   ├── LottieRenderer.cs          # Rendering shapes in Unity
│   ├── LottieInterpolator.cs     # Interpolazione keyframes
│   ├── LottieAnimationPlayer.cs  # Player principale
│   └── LottieTesterUI.cs         # UI controller
├── Animations/
│   └── Test.json                  # File Lottie di esempio
└── Scenes/
    └── AnimationTesterScene.unity # Scena di test
```

## Dipendenze

- **Newtonsoft.Json** (3.2.1+): Parser JSON per strutture complesse
- **TextMeshPro** (3.0.9+): UI text avanzato
- **Unity 2021.3+**: Versione Unity supportata

Le dipendenze sono già configurate in `Packages/manifest.json`.

## Setup Rapido

### 1. Aprire la Scena

Apri `Assets/Scenes/AnimationTesterScene.unity`

### 2. Configurare il Player

Nella scena troverai un GameObject "AnimationManager" con il componente `LottieAnimationPlayer`:

1. **Lottie Json File**: Trascina il tuo file .json Lottie
2. **Play On Start**: ✓ Attivato per auto-play
3. **Loop**: ✓ Attivato per ripetizione
4. **Playback Speed**: 1.0 (velocità normale)
5. **Pixels Per Unit**: 1.0 (scala rendering)

### 3. Premere Play

L'animazione partirà automaticamente!

## Formato Lottie Supportato

Il sistema supporta i file JSON esportati da:
- **Adobe After Effects** con plugin Bodymovin
- **LottieFiles Creator**
- **Altri editor Lottie**

### Esempio di struttura Lottie

```json
{
  "nm": "Nome Animazione",
  "w": 512,
  "h": 512,
  "ip": 0,
  "op": 150,
  "fr": 60,
  "layers": [
    {
      "ty": 4,
      "nm": "Shape Layer",
      "ks": {
        "p": { "a": 1, "k": [...keyframes...] },
        "s": { "a": 0, "k": [100, 100] },
        "r": { "a": 1, "k": [...keyframes...] },
        "o": { "a": 0, "k": 100 }
      },
      "shapes": [...]
    }
  ]
}
```

## Layer Types Supportati

| Type | Codice | Supporto | Note |
|------|--------|----------|------|
| Shape Layer | 4 | ✅ Completo | Forme vettoriali |
| Precomp | 0 | ⚠️ Parziale | Composizioni nidificate |
| Solid | 1 | ⚠️ Parziale | Layer solidi |
| Image | 2 | ❌ Non supportato | Richiede asset esterni |
| Null | 3 | ✅ Completo | Layer vuoti |
| Text | 5 | ❌ Non supportato | Text layer |

## Shape Types Supportati

| Shape | Codice | Supporto | Implementazione |
|-------|--------|----------|-----------------|
| Group | gr | ✅ Completo | Gerarchia shapes |
| Path | sh | ✅ Completo | LineRenderer |
| Rectangle | rc | ✅ Completo | SpriteRenderer |
| Ellipse | el | ✅ Completo | SpriteRenderer |
| Fill | fl | ✅ Completo | Color properties |
| Stroke | st | ✅ Completo | LineRenderer width/color |
| Transform | tr | ✅ Completo | Position/Scale/Rotation |
| Trim Paths | tm | ⚠️ Parziale | Da implementare |
| Gradient | gf/gs | ❌ Non supportato | Richiede shader custom |

## Proprietà Animate Supportate

### Transform Properties
- **Position (p)**: ✅ Vector3 con keyframes
- **Scale (s)**: ✅ Vector3 percentage (0-100)
- **Rotation (r)**: ✅ Float in gradi
- **Anchor Point (a)**: ✅ Vector3
- **Opacity (o)**: ✅ Float percentage (0-100)
- **Skew (sk)**: ⚠️ Non implementato
- **Skew Axis (sa)**: ⚠️ Non implementato

### Shape Properties
- **Color (c)**: ✅ RGBA con keyframes
- **Stroke Width (w)**: ✅ Float con keyframes
- **Size (s)**: ✅ Vector2 per rect/ellipse

## Easing Curves

Il sistema supporta le curve di easing Bezier di Lottie:

```javascript
// Easing types in keyframes
{
  "o": { "x": 0.167, "y": 0.167 },  // Out easing
  "i": { "x": 0.833, "y": 0.833 }   // In easing
}
```

Implementate:
- ✅ Linear
- ✅ Ease In/Out
- ✅ Cubic Bezier custom
- ✅ Hold frames

## Utilizzo da Script

### Caricamento Animazione

```csharp
using AnimationTester.Lottie;

public class MyScript : MonoBehaviour
{
    public TextAsset lottieJson;
    private LottieAnimationPlayer player;

    void Start()
    {
        player = GetComponent<LottieAnimationPlayer>();
        player.LoadAnimation(lottieJson.text);
    }
}
```

### Controllo Playback

```csharp
// Play/Pause/Stop
player.Play();
player.Pause();
player.Stop();

// Navigazione timeline
player.SetFrame(30f);           // Va al frame 30
player.SetTime(1.5f);            // Va a 1.5 secondi

// Velocità
player.playbackSpeed = 2.0f;     // 2x speed

// Loop
player.loop = true;
```

### Informazioni Animazione

```csharp
// Durata e frame
float duration = player.Duration;        // Secondi
float totalFrames = player.TotalFrames;  // Frame totali
float currentFrame = player.CurrentFrame;
float currentTime = player.CurrentTime;

// Dati animazione
LottieAnimation anim = player.Animation;
Debug.Log($"Nome: {anim.nm}");
Debug.Log($"Size: {anim.w}x{anim.h}");
Debug.Log($"FPS: {anim.fr}");
Debug.Log($"Layers: {anim.layers.Count}");
```

## API Principale

### LottieAnimationPlayer

```csharp
// Proprietà pubbliche
public TextAsset lottieJsonFile;
public bool playOnStart = true;
public bool loop = true;
public float playbackSpeed = 1f;
public float pixelsPerUnit = 1f;

// Metodi
void LoadAnimation(string jsonText);
void Play();
void Pause();
void Stop();
void SetFrame(float frame);
void SetTime(float time);

// Proprietà readonly
float CurrentFrame { get; }
float CurrentTime { get; }
float Duration { get; }
float TotalFrames { get; }
bool IsPlaying { get; }
LottieAnimation Animation { get; }
```

### LottieParser

```csharp
// Parse JSON Lottie
LottieAnimation animation = LottieParser.Parse(jsonText);
```

### LottieInterpolator

```csharp
// Interpola valori animati
float value = LottieInterpolator.InterpolateFloat(property, frame);
Vector3 pos = LottieInterpolator.InterpolateVector3(property, frame);
Color color = LottieInterpolator.InterpolateColor(property, frame);
```

## Performance

### Ottimizzazioni

- **Caching keyframes**: I keyframe sono pre-processati
- **Update solo quando playing**: Calcoli solo durante playback
- **Object pooling**: Riuso GameObject per layer
- **Interpolazione efficiente**: Ricerca binaria keyframes

### Best Practices

1. **Pixels Per Unit**: Usa valori più alti per animazioni piccole
2. **Framerate**: Limita FPS a 30-60 per performance migliori
3. **Complessità**: Riduci numero di layer e shapes
4. **Disable hidden layers**: Layer nascosti non vengono processati

### Limiti Consigliati

| Elemento | Limite Soft | Limite Hard |
|----------|-------------|-------------|
| Layers | 10-20 | 50 |
| Shapes per layer | 5-10 | 30 |
| Keyframes per property | 50-100 | 500 |
| Dimensione canvas | 512x512 | 2048x2048 |

## Troubleshooting

### Animazione non si vede

1. ✅ Verifica che il JSON sia valido
2. ✅ Controlla la Console per errori di parsing
3. ✅ Assicurati che `pixelsPerUnit` sia appropriato
4. ✅ Verifica che la Camera sia posizionata correttamente

### Performance basse

1. ⚙️ Riduci `playbackSpeed` per debug
2. ⚙️ Disabilita layer non necessari nel JSON
3. ⚙️ Semplifica le shape complesse
4. ⚙️ Riduci il numero di keyframes

### Colori sbagliati

- Lottie usa valori RGB 0-1, non 0-255
- Verifica che l'animazione non usi gradienti (non supportati)

### Forme non renderizzate

- Verifica che i material siano assegnati
- Controlla che le shape abbiano Fill o Stroke
- Assicurati che i layer siano nell'in/out point range

## Limitazioni Note

1. **Gradienti**: Non supportati (richiede shader custom)
2. **Mask complesse**: Supporto parziale
3. **Effects**: Non supportati (blur, glow, etc.)
4. **Text layers**: Non supportati
5. **Image layers**: Non supportati
6. **Expressions**: Non supportati
7. **3D layers**: Non supportati

## Roadmap Future

- [ ] Supporto trim paths completo
- [ ] Gradienti con shader custom
- [ ] Mask avanzate
- [ ] Path morphing
- [ ] Export to sprite sheet
- [ ] Baking animations
- [ ] WebGL optimization

## Risorse

- **Lottie Docs**: https://lottiefiles.github.io/lottie-docs/
- **Bodymovin Plugin**: https://github.com/airbnb/lottie-web
- **LottieFiles**: https://lottiefiles.com/

## Licenza

Progetto di esempio per uso educativo e testing.
