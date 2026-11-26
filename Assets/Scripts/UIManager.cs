using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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


    public HealthBar PlayerBar;
    public HealthBar EnemyBar;
    
    // ---------- Text Printer --------------------------------
    private Coroutine activePrinter;
    [SerializeField] private TextMeshProUGUI textbox;
    [Range(0f, 0.1f)]
    [SerializeField] private float textSpeed;
    private string currentText;
    public bool FinishedPrinting { get; private set; }
    public void PrintText(string text)
    {
        text = text.Trim();
        currentText = text;
        if (activePrinter != null) StopCoroutine(activePrinter);
        activePrinter = StartCoroutine(Printer(text));
    }

    public void CancelPrint()
    {
        if (activePrinter != null) StopCoroutine(activePrinter);
        textbox.text = currentText;
        FinishedPrinting = true;
    }

    private IEnumerator Printer(string text)
    {
        FinishedPrinting = false;
        textbox.text = string.Empty;
        
        for (int i = 0; i < text.Length; i++)
        {
            textbox.text += text[i];
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        textbox.text = currentText;
        activePrinter = null; // end 
        FinishedPrinting = true;
        yield break;
    }

    // -------------------------------------------------------
}