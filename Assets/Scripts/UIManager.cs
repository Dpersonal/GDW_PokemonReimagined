using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// WIP WIP WIP
public class UIManager : MonoBehaviour
{
    public static Dictionary<string, Sprite> SpriteLookup {  get; private set; }

    private void Awake()
    {
        Sprite[] pokeSprites = Resources.LoadAll<Sprite>("PokemonSprites");

        SpriteLookup = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in pokeSprites)
        {
            SpriteLookup.Add(sprite.name, sprite);
        }
    }
    // ---------- Text Printer --------------------------------
    private Coroutine activePrinter;
    [SerializeField] private TextMeshProUGUI textbox;
    [Range(0f, 0.1f)]
    [SerializeField] private float textSpeed;
    public void PrintText(string text)
    {
        if (activePrinter != null) StopCoroutine(activePrinter);
        activePrinter = StartCoroutine(Printer(text));
    }

    private IEnumerator Printer(string text)
    {
        textbox.text = string.Empty;
        text = text.Trim();
        float downTime = 0f;
        int i = 0;

        while (i < text.Length)
        {
            downTime += Time.deltaTime;
            if (downTime > textSpeed)
            {
                downTime = 0f;
                textbox.text += text[i];
                i++;
            }
            yield return null;
        }

        activePrinter = null; // end 
        yield break;
    }
    // -------------------------------------------------------
}