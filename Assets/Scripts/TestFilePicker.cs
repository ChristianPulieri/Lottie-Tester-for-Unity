using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Test minimale per file picker
/// </summary>
public class TestFilePicker : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("=== TEST FILE PICKER ===");
        Debug.Log("Aspetto 2 secondi poi apro file picker...");
        Invoke(nameof(OpenFilePicker), 2f);
    }

    private void OpenFilePicker()
    {
#if UNITY_EDITOR
        Debug.Log("Chiamata EditorUtility.OpenFilePanel...");

        string path = EditorUtility.OpenFilePanel(
            "TEST - Seleziona un file JSON",
            Application.dataPath,
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log($"✅ FILE SELEZIONATO: {path}");
        }
        else
        {
            Debug.Log("❌ Selezione annullata o nessun file selezionato");
        }
#else
        Debug.Log("EditorUtility disponibile solo in Editor");
#endif
    }
}
