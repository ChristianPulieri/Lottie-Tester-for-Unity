# Approccio Alternativo: Asset Statici Pre-importati

## Quando usare questo approccio

Se il caricamento dinamico di JSON da React non funziona bene in WebGL, usa questo approccio alternativo che si basa su **asset Lottie pre-importati** in Unity.

## Differenze

| Approccio | Caricamento Dinamico | Asset Statici |
|-----------|---------------------|---------------|
| **Script** | `LottieWebGLBridge.cs` | `LottieWebGLBridgeStatic.cs` |
| **Input da React** | JSON completo (stringa) | Nome animazione (stringa) |
| **Flessibilità** | Qualsiasi JSON | Solo JSON pre-importati |
| **Affidabilità WebGL** | Potenzialmente problematico | Più stabile |

## Setup in Unity

### 1. Importa i JSON come Lottie Animation Assets

1. **Copia i tuoi file JSON** in `Assets/Animations/` (o altra cartella)

2. **Seleziona un file .json** nel Project panel

3. **Unity dovrebbe automaticamente riconoscerlo** come Lottie Animation Asset (grazie all'importer del plugin)
   - Verifica nell'Inspector che sia di tipo "Lottie Animation Asset"
   - Se non viene riconosciuto, assicurati che il plugin unity-lottie-player sia installato

4. **Ripeti per tutti i JSON** che vuoi usare

### 2. Configura lo Script

1. **Usa lo script alternativo** invece di quello dinamico:
   - Rimuovi `LottieWebGLBridge` dal GameObject "LottieBridge"
   - Aggiungi `LottieWebGLBridgeStatic` invece

2. **Configura l'Inspector**:
   - **Lottie Player**: Trascina il componente Image Lottie Player
   - **Animations**: Imposta Size = numero di animazioni (es: 3)
     - Trascina i Lottie Animation Asset dalla cartella Animations
   - **Animation Names**: Imposta Size = stesso numero
     - Inserisci i nomi (es: "test", "bounce", "fade")
     - Questi nomi verranno usati da React

3. **Play On Receive**: Attivato

**Esempio configurazione**:
```
Animations:
  [0] = Test.asset
  [1] = BounceAnimation.asset
  [2] = FadeAnimation.asset

Animation Names:
  [0] = "test"
  [1] = "bounce"
  [2] = "fade"
```

### 3. Build WebGL

Procedi normalmente con il build WebGL come descritto in `UNITY_SETUP_FOR_REACT.md`

## Setup in React

### Modifica App.js

Cambia il metodo di invio da JSON completo a nome animazione:

```javascript
// PRIMA (approccio dinamico):
const jsonString = JSON.stringify(currentAnimation);
unityInstanceRef.current.SendMessage('LottieBridge', 'ReceiveLottieJSON', jsonString);

// DOPO (approccio statico):
const animationName = 'test'; // o 'bounce', 'fade', ecc.
unityInstanceRef.current.SendMessage('LottieBridge', 'LoadAnimationByName', animationName);
```

### Esempio implementazione

Aggiungi un dropdown per selezionare le animazioni:

```javascript
const [selectedAnimation, setSelectedAnimation] = useState('test');
const availableAnimations = ['test', 'bounce', 'fade']; // Sync con Unity

const handleAnimationSelect = (animName) => {
  setSelectedAnimation(animName);

  // Invia a Unity
  if (unityInstanceRef.current) {
    unityInstanceRef.current.SendMessage('LottieBridge', 'LoadAnimationByName', animName);
  }
};

// Nel render:
<select onChange={(e) => handleAnimationSelect(e.target.value)} value={selectedAnimation}>
  {availableAnimations.map(name => (
    <option key={name} value={name}>{name}</option>
  ))}
</select>
```

## Pro e Contro

### ✅ Vantaggi
- Più stabile in WebGL
- Asset validati in Unity Editor
- Performance potenzialmente migliori
- Meno rischio di errori a runtime

### ❌ Svantaggi
- Devi importare ogni JSON in Unity prima del build
- Non puoi caricare JSON arbitrari dall'utente
- Devi rifare il build Unity per aggiungere nuove animazioni
- React e Unity devono essere sincronizzati sui nomi

## Quando usare quale approccio

### Usa Dinamico (`LottieWebGLBridge.cs`) se:
- ✅ Vuoi massima flessibilità
- ✅ L'utente può caricare JSON custom
- ✅ Non sai in anticipo quali animazioni servono
- ✅ Vuoi evitare rebuild Unity

### Usa Statico (`LottieWebGLBridgeStatic.cs`) se:
- ✅ Hai un set fisso di animazioni
- ✅ Il caricamento dinamico non funziona in WebGL
- ✅ Preferisci stabilità e performance
- ✅ Non serve upload utente di JSON custom

## Test

Per testare quale approccio funziona meglio:

1. Prova prima l'approccio dinamico
2. Se vedi errori Console WebGL tipo "Cannot load animation from JSON", passa allo statico
3. Confronta performance e stabilità

## Conversione tra i due approcci

È facile switchare tra gli approcci:

1. **In Unity**: Cambia componente script sul GameObject "LottieBridge"
2. **In React**: Cambia da `SendMessage(..., 'ReceiveLottieJSON', json)` a `SendMessage(..., 'LoadAnimationByName', name)`
3. **Rebuild** entrambi i progetti

## Supporto Ibrido

Puoi anche avere **entrambi gli script** sul GameObject:
- `LottieWebGLBridgeStatic` per animazioni comuni
- `LottieWebGLBridge` per upload utente custom

React può scegliere quale metodo chiamare:
```javascript
// Per animazioni pre-caricate
unityInstance.SendMessage('LottieBridge', 'LoadAnimationByName', 'test');

// Per JSON custom
unityInstance.SendMessage('LottieBridge', 'ReceiveLottieJSON', jsonString);
```

---

**Nota**: Prova prima l'approccio dinamico. Questo approccio statico è un fallback per problemi specifici di WebGL.
