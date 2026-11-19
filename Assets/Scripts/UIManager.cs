using System.Collections.Generic;
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
}