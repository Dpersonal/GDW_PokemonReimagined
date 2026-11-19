using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // The entire Type Matchup Chart
    // [Attacking, Defending]
    public static readonly float[,] Matchup =
    {
        // N/A   NO    GR    FI    WA    EL    BU    FL    RO    PO    GD    IC    FT    PS    GH    DR    DA    ST    FA
        {  1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1    },// N/A
        {  1,    1,    1,    1,    1,    1,    1,    1,    0.5f, 1,    1,    1,    1,    1,    0f,   1,    1,    0.5f, 1    },// NO
        {  1,    1,    0.5f, 0.5f, 2f,   1,    0.5f, 0.5f, 2f,   0.5f, 2f,   1,    1,    1,    1,    0.5f, 1,    0.5f, 1    },// GR
        {  1,    1,    2f,   0.5f, 0.5f, 1,    2f,   1,    0.5f, 1,    1,    2f,   1,    1,    1,    0.5f, 1,    2f,   1    },// FI
        {  1,    1,    0.5f, 2f,   0.5f, 1,    1,    1,    2f,   1,    2f,   1,    1,    1,    1,    0.5f, 1,    1,    1    },// WA
        {  1,    1,    0.5f, 1,    2f,   0.5f, 1,    2f,   1,    1,    0f,   1,    1,    1,    1,    0.5f, 1,    1,    1    },// EL
        {  1,    1,    2f,   0.5f, 1,    1,    1,    0.5f, 1,    0.5f, 1,    1,    0.5f, 2f,   0.5f, 1,    2f,   0.5f, 0.5f },// BU
        {  1,    1,    2f,   1,    1,    0.5f, 2f,   1,    0.5f, 1,    1,    1,    2f,   1,    1,    1,    1,    0.5f, 1    },// FL
        {  1,    1,    1,    2f,   1,    1,    2f,   2f,   1,    1,    0.5f, 2f,   0.5f, 1,    1,    1,    1,    0.5f, 1    },// RO
        {  1,    1,    2f,   1,    1,    1,    1,    1,    0.5f, 0.5f, 0.5f, 1,    1,    1,    0.5f, 1,    1,    0f,   2f   },// PO
        {  1,    1,    0.5f, 2f,   1,    2f,   0.5f, 0f,   2f,   2f,   1,    1,    1,    1,    1,    1,    1,    2f,   1    },// GD
        {  1,    1,    2f,   0.5f, 0.5f, 1,    1,    2f,   1,    1,    2f,   0.5f, 1,    1,    1,    2f,   1,    0.5f, 1    },// IC
        {  1,    2f,   1,    1,    1,    1,    0.5f, 0.5f, 2f,   0.5f, 1,    2f,   1,    0.5f, 0f,   1,    2f,   2f,   0.5f },// FT
        {  1,    1,    1,    1,    1,    1,    1,    1,    1,    2f,   1,    1,    2f,   0.5f, 1,    1,    0f,   0.5f, 1    },// PS
        {  1,    0f,   1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    2f,   2f,   1,    0.5f, 1,    1    },// GH
        {  1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    2f,   1,    0.5f, 0f   },// DR
        {  1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,    0.5f, 2f,   2f,   1,    0.5f, 1,    0.5f },// DA
        {  1,    1,    1,    0.5f, 0.5f, 0.5f, 1,    1,    2f,   1,    1,    2f,   1,    1,    1,    1,    1,    0.5f, 2f   },// ST
        {  1,    1,    1,    0.5f, 1,    1,    1,    1,    1,    1,    1,    1,    2f,   1,    1,    2f,   2f,   0.5f, 1    },// FA
    };

    public float GetEffectiveness(PokeType attacking,  PokeType defending)
    {
        return Matchup[(int)attacking, (int)defending];
    }

    // Currently only considers type effectiveness, but can easily be expanded
    public int CalculateDamage(Move move, Pokemon pokeAtk, Pokemon pokeDef, out float effectiveness)
    {
        Move.MoveData moveData = move.Data;
        Pokemon.PokeStats statsAtk = pokeAtk.Stats;
        Pokemon.PokeStats statsDef = pokeDef.Stats;

        // effectiveness on type1 * effectiveness on type2
        float typeMultiplier = GetEffectiveness(moveData.Type, statsDef.Type1) * GetEffectiveness(moveData.Type, statsDef.Type2);
        effectiveness = typeMultiplier; 

        return (int)(moveData.Power * typeMultiplier);
    }

//---------------- Battle Demonstration ------------------
    public Pokemon playerMon;
    public Pokemon cpuMon;

    [SerializeField] TextMeshProUGUI playerHP, cpuHP;
    [SerializeField] GameObject h1, h2;
    HealthBar health1, health2;

    private void Start()
    {
        playerMon = new Pikachu();
        playerMon.SetMoves(new Move[] { new Tackle(), new DebugMove() });
        cpuMon = new Pikachu();
        health1 = h1.GetComponent<HealthBar>();
        health2 = h2.GetComponent<HealthBar>();
    }

    private void Update()
    {
        playerHP.text = $"{playerMon.Stats.Name} HP: {playerMon.HP}";
        cpuHP.text = $"{cpuMon.Stats.Name} HP: {cpuMon.HP}";
        //health1.HealthPercentage = playerMon.HP / (float)playerMon.Stats.MaxHP;
        health2.HealthPercentage = cpuMon.HP / (float)cpuMon.Stats.MaxHP;
    }

    public void DebugUseMove1()
    {
        playerMon.GetMove(0).GetActions(cpuMon);
    }

    public void DebugUseMove2()
    {
        playerMon.GetMove(1).GetActions(cpuMon);
    }
//--------------------------------------------------------
    
}

public enum PokeType
{
    None,
    Normal,
    Grass,
    Fire,
    Water,
    Electric,
    Bug,
    Flying,
    Rock,
    Poison,
    Ground,
    Ice,
    Fighting,
    Psychic,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}

public enum Status
{
    None,
    Burn,
    Freeze,
    Paralysis,
    Poison,
    BadPoison,
    Sleep
}


