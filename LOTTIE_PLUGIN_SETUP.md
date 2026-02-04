# Setup Plugin Lottie per Unity

Invece di costruire un renderer Lottie completo da zero (che è estremamente complesso), useremo un plugin esistente.

## Opzione Consigliata: rlottie-unity

### Installazione

1. **In Unity**, vai a **Window → Package Manager**

2. Clicca sul **+ (plus)** in alto a sinistra

3. Seleziona **Add package from git URL...**

4. Inserisci:
   ```
   https://github.com/gilzoide/rlottie-unity.git
   ```

5. Clicca **Add**

6. Aspetta che Unity scarichi e compili il package

### Setup nella Scena

1. **Crea un Canvas** se non ce l'hai già:
   - GameObject → UI → Canvas

2. **Aggiungi RawImage** al Canvas:
   - Tasto destro sul Canvas → UI → Raw Image

3. **Aggiungi il componente Lottie**:
   - Seleziona il RawImage
   - Add Component → "Lottie Animation"

4. **Configura**:
   - Trascina il tuo file `.json` nel campo "Animation"
   - ✓ **Play On Enable**
   - ✓ **Loop**
   - Speed: 1

5. **Premi Play** ▶️

## Alternativa: LottieFiles Unity SDK

Se rlottie-unity non funziona, prova:

### Installazione

1. Scarica da: https://lottiefiles.com/plugins/unity

2. Importa il `.unitypackage` nel progetto:
   - Assets → Import Package → Custom Package

3. Segui le istruzioni del wizard

### Setup

1. GameObject → UI → Lottie Animation

2. Trascina il file JSON

3. Play!

## Compatibilità WebGL

**rlottie-unity**:
- ✅ Supporto WebGL nativo
- ✅ Performance ottimizzate
- ✅ Lightweight

**LottieFiles SDK**:
- ⚠️ WebGL limitato
- ✅ Più features complete
- ❌ File size più grande

## Troubleshooting

### "Package not found"

Se il git URL non funziona:

1. Scarica manualmente:
   ```bash
   git clone https://github.com/gilzoide/rlottie-unity.git
   ```

2. Copia la cartella in `Packages/` del progetto

3. Riavvia Unity

### "Animation not playing"

- Verifica che il file JSON sia valido su https://lottiefiles.com/
- Controlla la Console per errori
- Assicurati che il Canvas sia in modalità Screen Space

### Performance basse

- Riduci FPS dell'animazione
- Usa dimensioni canvas più piccole
- Disabilita antialiasing

## File Esistenti del Progetto

Il progetto contiene già i nostri script custom in `Assets/Scripts/`:
- LottieData.cs
- LottieParserSimple.cs
- LottieRenderer.cs
- ecc.

Puoi:
1. **Tenerli** come backup/reference
2. **Rimuoverli** se usi solo il plugin
3. **Usare entrambi** per confrontare

I nostri script custom funzionano ma sono limitati. Il plugin è molto più completo!

## Prossimi Passi

Una volta installato il plugin:

1. Crea una nuova scena di test
2. Prova con `SimpleTest.json` (semplice)
3. Poi prova con `Test.json` (complesso)
4. Configura UI controls se necessario

Buon divertimento! 🎨
