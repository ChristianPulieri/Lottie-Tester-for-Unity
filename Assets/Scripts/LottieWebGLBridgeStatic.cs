using UnityEngine;
using Gilzoide.LottiePlayer;
using System.Collections.Generic;

/// <summary>
/// ALTERNATIVA: Bridge che usa asset pre-importati invece di JSON dinamico
/// Usa questo se il caricamento dinamico da JSON string non funziona bene in WebGL
/// </summary>
public class LottieWebGLBridgeStatic : MonoBehaviour
{
    [Header("Lottie Player")]
    [SerializeField] private ImageLottiePlayer lottiePlayer;

    [Header("Pre-loaded Animations")]
    [SerializeField] private List<LottieAnimationAsset> animations = new List<LottieAnimationAsset>();
    [SerializeField] private List<string> animationNames = new List<string>();

    [Header("Settings")]
    [SerializeField] private bool playOnReceive = true;

    private Dictionary<string, LottieAnimationAsset> animationDict;

    private void Start()
    {
        if (lottiePlayer == null)
        {
            lottiePlayer = GetComponent<ImageLottiePlayer>();

            if (lottiePlayer == null)
            {
                Debug.LogError("LottieWebGLBridgeStatic: Nessun ImageLottiePlayer trovato!");
            }
        }

        // Crea dizionario nome -> asset
        animationDict = new Dictionary<string, LottieAnimationAsset>();
        for (int i = 0; i < Mathf.Min(animations.Count, animationNames.Count); i++)
        {
            if (animations[i] != null && !string.IsNullOrEmpty(animationNames[i]))
            {
                animationDict[animationNames[i]] = animations[i];
                Debug.Log($"LottieWebGLBridgeStatic: Registrata animazione '{animationNames[i]}'");
            }
        }

        Debug.Log($"LottieWebGLBridgeStatic: Pronto con {animationDict.Count} animazioni pre-caricate");
    }

    /// <summary>
    /// Riceve il NOME dell'animazione da React (invece del JSON completo)
    /// Chiamato da React con: unityInstance.SendMessage('LottieBridge', 'LoadAnimationByName', 'nomeAnimazione')
    /// </summary>
    public void LoadAnimationByName(string animationName)
    {
        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogError("LottieWebGLBridgeStatic: Nome animazione vuoto!");
            return;
        }

        if (lottiePlayer == null)
        {
            Debug.LogError("LottieWebGLBridgeStatic: ImageLottiePlayer non assegnato!");
            return;
        }

        if (!animationDict.ContainsKey(animationName))
        {
            Debug.LogError($"LottieWebGLBridgeStatic: Animazione '{animationName}' non trovata! Animazioni disponibili: {string.Join(", ", animationDict.Keys)}");
            return;
        }

        LottieAnimationAsset asset = animationDict[animationName];
        lottiePlayer.SetAnimationAsset(asset);

        Debug.Log($"LottieWebGLBridgeStatic: Animazione '{animationName}' caricata");

        if (playOnReceive)
        {
            lottiePlayer.Play();
            Debug.Log("LottieWebGLBridgeStatic: Animazione avviata");
        }
    }

    /// <summary>
    /// Riceve un index numerico (alternativa al nome)
    /// </summary>
    public void LoadAnimationByIndex(int index)
    {
        if (index < 0 || index >= animations.Count)
        {
            Debug.LogError($"LottieWebGLBridgeStatic: Index {index} fuori range (0-{animations.Count - 1})");
            return;
        }

        if (animations[index] == null)
        {
            Debug.LogError($"LottieWebGLBridgeStatic: Animazione all'index {index} è null!");
            return;
        }

        lottiePlayer.SetAnimationAsset(animations[index]);
        Debug.Log($"LottieWebGLBridgeStatic: Animazione index {index} caricata");

        if (playOnReceive)
        {
            lottiePlayer.Play();
        }
    }

    /// <summary>
    /// Lista delle animazioni disponibili (per debug)
    /// </summary>
    public void ListAnimations()
    {
        Debug.Log($"LottieWebGLBridgeStatic: Animazioni disponibili ({animationDict.Count}):");
        foreach (var name in animationDict.Keys)
        {
            Debug.Log($"  - {name}");
        }
    }

    public void PauseAnimation()
    {
        if (lottiePlayer != null)
        {
            lottiePlayer.Pause();
            Debug.Log("LottieWebGLBridgeStatic: Animazione in pausa");
        }
    }

    public void PlayAnimation()
    {
        if (lottiePlayer != null)
        {
            lottiePlayer.Play();
            Debug.Log("LottieWebGLBridgeStatic: Animazione avviata/ripresa");
        }
    }
}
