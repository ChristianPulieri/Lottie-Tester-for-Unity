# Installazione Plugin Lottie Ufficiale

## LottieFiles dotLottie for Unity (RACCOMANDATO)

Questo è il plugin **ufficiale** e più stabile da LottieFiles.

### Metodo 1: Unity Package Manager (Git URL)

1. **Apri Unity**
2. **Window → Package Manager**
3. Click sul **+** in alto a sinistra
4. Seleziona **Add package from git URL...**
5. Incolla questo URL:
   ```
   https://github.com/LottieFiles/dotlottie-unity.git
   ```
6. Click **Add**
7. Aspetta il download e compilazione

### Metodo 2: Download Manuale (Se Git fallisce)

1. **Vai su GitHub**:
   ```
   https://github.com/LottieFiles/dotlottie-unity
   ```

2. Click sul pulsante verde **Code** → **Download ZIP**

3. **Estrai il file ZIP**

4. **In Unity**:
   - Apri la cartella del progetto nel Finder/Explorer
   - Vai in `Packages/`
   - Copia la cartella estratta dentro `Packages/`
   - Rinominala in `com.lottiefiles.dotlottie`

5. **Riavvia Unity**

## Setup Scena

### 1. Crea UI Canvas

```
GameObject → UI → Canvas
```

### 2. Aggiungi dotLottie Animation

**Opzione A - Component su UI:**
1. Tasto destro sul Canvas
2. UI → Raw Image
3. Seleziona il Raw Image
4. Add Component → **dotLottie Animation**

**Opzione B - GameObject dedicato:**
1. GameObject → UI → dotLottie Animation (se disponibile)

### 3. Configura

Nell'Inspector del componente dotLottie:

- **Animation Data**: Trascina il tuo file `.json` o `.lottie`
- **Auto Play**: ✓ Attivato
- **Loop**: ✓ Attivato
- **Speed**: 1.0

### 4. Premi Play ▶️

## Funzionalità

✅ **WebGL Compatible** - Funziona perfettamente su WebGL
✅ **Tutti i layer types** - Shape, Image, Text, ecc.
✅ **Animazioni complesse** - Gradienti, maschere, effects
✅ **Performance ottimizzate**
✅ **Documentazione completa**
✅ **Supporto ufficiale**

## Formato File

Il plugin supporta:
- `.json` - File Lottie standard
- `.lottie` - Formato dotLottie (compresso)

## API Basic

```csharp
using LottieFiles.DotLottie;

public class LottieController : MonoBehaviour
{
    public DotLottieAnimation lottieAnimation;

    void Start()
    {
        lottieAnimation.Play();
    }

    public void PauseAnimation()
    {
        lottieAnimation.Pause();
    }

    public void StopAnimation()
    {
        lottieAnimation.Stop();
    }

    public void SetSpeed(float speed)
    {
        lottieAnimation.Speed = speed;
    }
}
```

## Testing

### Con SimpleTest.json:
1. Trascina `SimpleTest.json` nel campo Animation Data
2. Play - Dovresti vedere un quadrato blu con bordo rosso animato

### Con Test.json:
1. Trascina `Test.json`
2. Play - Dovresti vedere l'animazione complessa funzionare perfettamente

## Troubleshooting

### "Package not found" o 404

Se il git URL non funziona:

1. Verifica la connessione internet
2. Prova il download manuale (Metodo 2)
3. Contatta il supporto: https://lottiefiles.com/support

### "Component not found"

Dopo l'installazione:
1. Aspetta la compilazione completa (barra in basso)
2. Riavvia Unity
3. Verifica in Project → Packages che appaia "dotLottie"

### Animazione non appare

1. Verifica che il Canvas sia in **Screen Space - Overlay**
2. Controlla la Camera nel Canvas
3. Verifica il file JSON su: https://lottiefiles.com/preview

## Build WebGL

Il plugin è ottimizzato per WebGL:

1. File → Build Settings
2. Seleziona **WebGL**
3. Switch Platform
4. Build and Run

Le animazioni funzioneranno perfettamente nel browser!

## Link Utili

- Repository: https://github.com/LottieFiles/dotlottie-unity
- Documentazione: https://developers.lottiefiles.com/docs/dotlottie-unity
- Esempi: https://lottiefiles.com/
- Supporto: https://lottiefiles.com/support

## Alternative Stabili

Se dotLottie non funziona, prova:

### Thorvg (Samsung, molto stabile):
```
https://github.com/Samsung/thorvg.git
```

Ma richiede configurazione nativa più complessa.

---

**Il plugin dotLottie è la scelta migliore per stabilità, features complete e supporto WebGL!** 🎨
