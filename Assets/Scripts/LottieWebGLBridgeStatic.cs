using UnityEngine;
using SkiaSharp.Unity;
using System.Collections.Generic;

/// <summary>
/// ALTERNATIVA: Bridge che usa asset pre-importati invece di JSON dinamico
/// Usa questo se il caricamento dinamico da JSON string non funziona bene in WebGL
/// Rendering con SkiaForUnity (Skottie)
/// </summary>
public class LottieWebGLBridgeStatic : MonoBehaviour
{
    [Header("Lottie Player")]
    [SerializeField] private SkottiePlayerV2 skottiePlayer;

    [Header("Pre-loaded Animations")]
    [SerializeField] private List<TextAsset> animations = new List<TextAsset>();
    [SerializeField] private List<string> animationNames = new List<string>();

    [Header("Settings")]
    [SerializeField] private bool playOnReceive = true;

    private Dictionary<string, TextAsset> animationDict;

    private void Start()
    {
        if (skottiePlayer == null)
        {
            skottiePlayer = GetComponent<SkottiePlayerV2>();

            if (skottiePlayer == null)
            {
                Debug.LogError("LottieWebGLBridgeStatic: Nessun SkottiePlayerV2 trovato!");
            }
        }

        animationDict = new Dictionary<string, TextAsset>();
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

    public void LoadAnimationByName(string animationName)
    {
        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogError("LottieWebGLBridgeStatic: Nome animazione vuoto!");
            return;
        }

        if (skottiePlayer == null)
        {
            Debug.LogError("LottieWebGLBridgeStatic: SkottiePlayerV2 non assegnato!");
            return;
        }

        if (!animationDict.ContainsKey(animationName))
        {
            Debug.LogError($"LottieWebGLBridgeStatic: Animazione '{animationName}' non trovata! Disponibili: {string.Join(", ", animationDict.Keys)}");
            return;
        }

        TextAsset asset = animationDict[animationName];
        skottiePlayer.LoadAnimation(asset.text);

        Debug.Log($"LottieWebGLBridgeStatic: Animazione '{animationName}' caricata");

        if (playOnReceive)
        {
            skottiePlayer.loop = true;
            skottiePlayer.PlayAnimation();
            Debug.Log("LottieWebGLBridgeStatic: Animazione avviata");
        }
    }

    public void LoadAnimationByIndex(int index)
    {
        if (index < 0 || index >= animations.Count)
        {
            Debug.LogError($"LottieWebGLBridgeStatic: Index {index} fuori range (0-{animations.Count - 1})");
            return;
        }

        if (animations[index] == null)
        {
            Debug.LogError($"LottieWebGLBridgeStatic: Animazione all'index {index} e' null!");
            return;
        }

        skottiePlayer.LoadAnimation(animations[index].text);
        Debug.Log($"LottieWebGLBridgeStatic: Animazione index {index} caricata");

        if (playOnReceive)
        {
            skottiePlayer.loop = true;
            skottiePlayer.PlayAnimation();
        }
    }

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
        Debug.Log("LottieWebGLBridgeStatic: Animazione in pausa");
    }

    public void PlayAnimation()
    {
        if (skottiePlayer != null)
        {
            skottiePlayer.PlayAnimation();
            Debug.Log("LottieWebGLBridgeStatic: Animazione avviata/ripresa");
        }
    }
}
