# Setup Instructions - Lottie Tester

## Problema Attuale
La scena contiene un AnimationManager ma gli script non sono ancora compilati da Unity.

## Soluzione: Setup Manuale della Scena

### Step 1: Compilare gli Script
1. Apri Unity e aspetta che compili gli script in `Assets/Scripts/`
2. Verifica nella Console che non ci siano errori di compilazione
3. Gli script dovrebbero apparire nel Project panel

### Step 2: Configurare AnimationManager

1. **Seleziona** il GameObject `AnimationManager` nella Hierarchy
2. **Nell'Inspector**, clicca su "Add Component"
3. **Cerca e aggiungi** il componente `AnimationPlayer`
4. **Configura** i parametri:
   - ✅ Play On Start: attivato
   - ✅ Loop: attivato
   - ⚙️ Playback Speed: 1

### Step 3: Creare Oggetti da Animare

#### Per BounceAnimation.json (esempio):
1. **Crea** GameObject → 3D Object → Sphere
2. **Rinominalo** in "Ball" (importante!)
3. **Posizionalo** a (0, 0, 0)
4. **Seleziona** AnimationManager
5. **Trascina** il file `Assets/Animations/BounceAnimation.json` in "Json Animation File"
6. **Trascina** la sfera "Ball" nella lista "Target Objects" (clicca + per aggiungere slot)

#### Per FadeRotateAnimation.json:
1. **Crea** GameObject → 2D Object → Sprite → Square
2. **Rinominalo** in "Logo"
3. **Trascina** `FadeRotateAnimation.json` in AnimationManager
4. **Aggiungi** "Logo" alla lista Target Objects

#### Per ColorTransitionAnimation.json:
1. **Crea** GameObject → 2D Object → Sprite → Square
2. **Rinominalo** in "Square"
3. **Configura** l'animazione come sopra

#### Per ComplexAnimation.json:
1. **Crea** 3 GameObject → 2D Object → Sprite → Circle
2. **Rinominali**: "Circle1", "Circle2", "Center"
3. **Posizionali** in punti diversi per vedere l'effetto
4. **Aggiungi** tutti e 3 alla lista Target Objects

### Step 4: Test

1. **Premi Play** in Unity
2. L'animazione dovrebbe partire automaticamente
3. Controlla la Console per eventuali errori

## Setup UI (Opzionale)

Se vuoi aggiungere controlli UI:

1. **Crea** Canvas: GameObject → UI → Canvas
2. **Aggiungi** elementi UI:
   - 3 Button: "Play", "Pause", "Stop"
   - 2 Slider: per timeline e velocità
   - TextMeshPro per info
   - Toggle per loop

3. **Aggiungi** componente `AnimationTesterUI` al Canvas
4. **Collega** tutti i riferimenti UI nell'Inspector

## Verifica Setup

✅ Gli script sono compilati senza errori?
✅ AnimationPlayer è aggiunto a AnimationManager?
✅ Il file JSON è assegnato?
✅ I GameObject hanno i nomi ESATTI del JSON?
✅ I GameObject sono nella lista Target Objects?

## Troubleshooting

**Script non appaiono come componenti**:
- Aspetta che Unity finisca la compilazione
- Controlla errori nella Console
- Riavvia Unity se necessario

**Animazione non parte**:
- Verifica i nomi degli oggetti (case-sensitive!)
- Controlla che il JSON sia valido
- Verifica che gli oggetti siano nella lista Target Objects

**Script reference missing**:
- Dopo la compilazione, riassegna il componente AnimationPlayer

## Quick Start Alternativo

Se vuoi testare rapidamente:

1. Apri Unity
2. Aspetta compilazione
3. Crea una Sphere chiamata "Ball"
4. Seleziona AnimationManager
5. Add Component → AnimationPlayer
6. Trascina BounceAnimation.json
7. Trascina Ball in Target Objects
8. Play!
