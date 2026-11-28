using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private TextMeshProUGUI moveDescriptionText;


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

    public void UpdateMoveDescriptionText(int button)
    {
        switch (button)
        {
            case 1:
                Debug.Log("hi");
                //this code will need to actually get the description of the moves according to the number (1 through 4)
                //also we need to check if it even HAS a move and try catch that probably, and on a catch just set the text to nothing
                moveDescriptionText.text = "Hi";
                break;
            case 2:
                moveDescriptionText.text = "Hello";
                break;
            case 3:
                moveDescriptionText.text = "Casts a nuclear bomb on the opposing pokemon";
                break;
            case 4:
                //yeah so this will need a way to actually get the moves description
                moveDescriptionText.text = "This is a long piece of text to test out how long we can make descriptions and if they can fit. This move does a lot of things. I wrote a lot here because this is for testing how long the description can be. Woah.";
                break;
            default:
                moveDescriptionText.text = "";
                break;
        }

    }

    // -------------------------------------------------------
}