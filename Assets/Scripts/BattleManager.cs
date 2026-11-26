using System;
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

    /// <summary>
    /// Gets the effectiveness of one type on another type from the type matchup chart.
    /// </summary>
    public float GetEffectiveness(PokeType attacking,  PokeType defending)
    {
        return Matchup[(int)attacking, (int)defending];
    }


    /// <summary>
    /// Calculates damage of a move using a formula. (move power * move effectiveness currently) <br></br>
    /// <c>effectiveness</c> returns the type matchup multiplier, which can be used to check if the move was super effective.
    /// </summary>
    public int CalculateDamage(Move move, Pokemon pokeAtk, Pokemon pokeDef, out float effectiveness)
    {
        Move.MoveData moveData = move.Data;
        Pokemon.PokeStats statsAtk = pokeAtk.Stats;
        Pokemon.PokeStats statsDef = pokeDef.Stats;

        // effectiveness on type1 * effectiveness on type2
        float typeMultiplier = GetEffectiveness(moveData.Type, statsDef.Type1) * GetEffectiveness(moveData.Type, statsDef.Type2);
        typeMultiplier = (float)Math.Round(typeMultiplier * 10) / 10;
        effectiveness = typeMultiplier; 

        return (int)(moveData.Power * typeMultiplier);
    }

    public void StartTurn()
    {
        // doSomething
    }

    public void EndTurn()
    {
        // maybe not necessary?
    }

    // ------------- Pokemon Teams --------------
    private Pokemon[] _playerTeam = new Pokemon[6];
    public Pokemon[] PlayerTeam
    {
        get { return _playerTeam; }
        set 
        {
            if (value.Length > 6)
            {
                throw new ArgumentException("Pokemon Team may not exceed 6 Pokemon");
            }
            _playerTeam = value; 
        }
    }

    public Pokemon ActivePlayerMon { get; set; }

    public void AddToPlayerTeam(Pokemon pokemon, int i)
    {
        if (i > 5 || i < 0) throw new ArgumentOutOfRangeException($"Invalid team slot: {i}");

        PlayerTeam[i] = pokemon;
    }


    private Pokemon[] _enemyTeam = new Pokemon[6];
    public Pokemon[] EnemyTeam
    {
        get{ return _enemyTeam; }
        set 
        {
            if (value.Length > 6)
            {
                throw new ArgumentException("Pokemon Team may not exceed 6 Pokemon");
            }
            _enemyTeam = value; 
        }
    }

    public Pokemon ActiveEnemyMon { get; set; }

    public void AddToEnemyTeam(Pokemon pokemon, int i)
    {
        if (i > 5 || i < 0) throw new ArgumentOutOfRangeException($"Invalid team slot: {i}");

        EnemyTeam[i] = pokemon;
    }
    // -----------------------------------------


    //---------------- Battle Demonstration (TEMPORARY) -------
    public Pokemon playerMon;
    public Pokemon cpuMon;

    [SerializeField] TextMeshProUGUI playerHP, cpuHP, deathText;
    [SerializeField] GameObject h1, h2;
    HealthBar health1, health2;

    private void Start()
    {
        playerMon = new Pikachu();
        playerMon.SetMoves(new Move[] { new Tackle(), new DebugMove(), new DebugMove2() });
        cpuMon = new Pikachu();
        cpuMon.SetMoves(new Move[] { new Tackle(), new DebugMove(), new DebugMove2() });
        health1 = h1.GetComponent<HealthBar>();
        health2 = h2.GetComponent<HealthBar>();
        deathText.gameObject.SetActive(false);
    }

    private void Update()
    {
        playerHP.text = $"{playerMon.Stats.Name} HP: {playerMon.HP}";
        cpuHP.text = $"{cpuMon.Stats.Name} HP: {cpuMon.HP}";
        //health1.HealthPercentage = playerMon.HP / (float)playerMon.Stats.MaxHP;
        //health1.SetHealthAnimated(playerMon.HP / (float)playerMon.Stats.MaxHP);
        //health2.HealthPercentage = cpuMon.HP / (float)cpuMon.Stats.MaxHP;
        //health2.SetHealthAnimated(cpuMon.HP / (float)cpuMon.Stats.MaxHP);
    }

    public void DebugUseMove1()
    {
        if (DeathCheck()) return;
        playerMon.GetMove(0).GetActions(cpuMon);
        if (DeathCheck()) return;
        cpuMon.GetMove(UnityEngine.Random.Range(0, 3)).GetActions(playerMon);
        DeathCheck();
    }

    public void DebugUseMove2()
    {
        if (DeathCheck()) return;
        playerMon.GetMove(1).GetActions(cpuMon);
        if (DeathCheck()) return;
        cpuMon.GetMove(UnityEngine.Random.Range(0, 3)).GetActions(playerMon);
        DeathCheck();
    }

    public void DebugUseMove3()
    {
        if (DeathCheck()) return;
        playerMon.GetMove(2).GetActions(cpuMon);
        if (DeathCheck()) return;
        cpuMon.GetMove(UnityEngine.Random.Range(0, 3)).GetActions(playerMon);
        DeathCheck();
    }

    public bool DeathCheck()
    {
        if(playerMon.IsDead)
        {
            deathText.gameObject.SetActive(true);
            deathText.text = "You Lose!";
            return true;
        }
        if (cpuMon.IsDead)
        {
            deathText.gameObject.SetActive(true);
            deathText.text = "You Win!";
            return true;
        }
        return false;
    }
    //--------------------------------------------------------

}

// Pokemon types
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

// Status effects
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


