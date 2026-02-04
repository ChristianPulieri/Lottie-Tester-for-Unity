# Fix Compressione Unity WebGL per React

## Problema
I file Brotli (`.br`) non vengono serviti correttamente dal server React di sviluppo.

## Soluzione: Usa Gzip invece di Brotli

### Passo 1: Apri Unity
1. Apri Unity Hub
2. Apri il progetto "Lottie Tester for WebGL"

### Passo 2: Cambia Impostazioni Build

1. **File → Build Settings**
2. **Seleziona Platform: WebGL** (dovrebbe essere già selezionato)
3. **Clicca "Player Settings"** (pulsante in basso a sinistra)
4. **Nel pannello Inspector a destra:**
   - Cerca "Publishing Settings" (o "Impostazioni pubblicazione")
   - Espandi la sezione
5. **Trova "Compression Format"**
   - Cambia da "Brotli" a **"Gzip"**
   - Oppure scegli **"Disabled"** (nessuna compressione - file più grandi ma più compatibili)

### Passo 3: Rifai il Build

1. **Torna a Build Settings** (File → Build Settings)
2. **Clicca "Build"** (NON "Build and Run")
3. **Seleziona la cartella**: `Export/ReactBuild`
   - Se chiede di sovrascrivere, clicca "Sì"
4. **Aspetta il completamento** (5-10 minuti)

### Passo 4: Copia il Nuovo Build in React

```bash
cd "/Volumes/SSD 1/Unity/Projects"
./copy-unity-to-react.sh
```

### Passo 5: Ricarica React

Il server React è già in esecuzione, quindi:
1. Vai al browser (http://localhost:3000)
2. **Ricarica la pagina** (Cmd+R o Ctrl+R)
3. Clicca "Solo Unity" o "Entrambe"

Unity dovrebbe caricarsi senza errori!

## Differenze tra Compressioni

| Tipo | Dimensione File | Compatibilità | Velocità Caricamento |
|------|----------------|---------------|---------------------|
| **Brotli** | Più piccoli | Richiede config server | Veloce (se configurato) |
| **Gzip** | Medi | Ottima | Veloce |
| **Disabled** | Più grandi | Perfetta | Più lento |

**Raccomandazione**: Usa **Gzip** per sviluppo e **Brotli** per produzione (con server configurato correttamente).

## Nota Importante

Se stai già facendo il setup Unity seguendo `UNITY_SETUP_FOR_REACT.md`, cambia la compressione PRIMA di fare il build finale.

Se hai già fatto il build con Brotli, cambia e rifai il build.
