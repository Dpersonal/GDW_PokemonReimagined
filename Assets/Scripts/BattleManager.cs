using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // The entire Type Matchup Chart
    // [Attacking, Defending]
    private static readonly float[,] Matchup =
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

    private static readonly float SkipTurnChance = 0.25f;
    private static readonly float BurnDamage = 0.0625f;
    private static readonly float PoisonDamage = 0.125f;



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

    

    public int RequiredRNG(int moveChoice)
    {
        int required = 0;
        if (ActivePlayerMon.Status == Status.Paralysis || ActivePlayerMon.Status == Status.Freeze)
            required++;
        foreach (int i in ActivePlayerMon.GetMove(moveChoice).GetAccuracy())
        {
            if (i < 100) required++;
        }
        return required;
    }

    // simplified version since moves will only have two rng rolls
    // can be improved if needed
    // GameController will call this method and perform appropriate animations as it recieves BattleActions
    public IEnumerable<BattleAction> PerformTurn(int playerChoice, int enemyChoice, float[] playerRNG)
    {
        // choice -1 = didn't do anything
        // also need to handle switching (choice = 4 - 5)

        bool playerMovesFirst = true; // check which move priority is higher, or pokemon speed if they are equal
        if(playerMovesFirst)
        {
            foreach (BattleAction action in PlayerMovesFirst(playerChoice, enemyChoice, playerRNG))
            {
                yield return action;
            }
        }
        else
        {
            foreach (BattleAction action in EnemyMovesFirst(playerChoice, enemyChoice, playerRNG))
            {
                yield return action;
            }
        }

        yield break;
    }

    public IEnumerable<BattleAction> EndTurn(Pokemon pokemon)
    {
        if (pokemon.Status == Status.Burn)
        {
            pokemon.ModifyHP((int)(pokemon.Stats.MaxHP / BurnDamage));
            yield return new BattleAction(BattleAction.ActionType.TakeDamage, performedByEnemy: pokemon.IsEnemy);
        }
        if (pokemon.Status == Status.Poison)
        {
            pokemon.ModifyHP((int)(pokemon.Stats.MaxHP / PoisonDamage));
            yield return new BattleAction(BattleAction.ActionType.TakeDamage, performedByEnemy: pokemon.IsEnemy);
        }
        if (pokemon.Status == Status.BadPoison)
        {
            pokemon.ModifyHP((int)(pokemon.Stats.MaxHP / (pokemon.BadPoisonCounter / 16f)));
            pokemon.BadPoisonCounter++;
            yield return new BattleAction(BattleAction.ActionType.TakeDamage, performedByEnemy: pokemon.IsEnemy);
        }
    }

    private IEnumerable<BattleAction> PlayerMovesFirst(int playerChoice, int enemyChoice, float[] playerRNG)
    {
        if (playerChoice < 4)
        {
            Move playerMove = ActivePlayerMon.GetMove(playerChoice);
            Queue<bool> playerRNGresult = new Queue<bool>();
            for (int i = 0; i < playerMove.GetAccuracy().Length; i++)
            {
                float probability = CalculateProbability(playerRNG[i], playerMove.GetAccuracy(i));
                Debug.Log($"Probability {i}: {probability}");
                if (UnityEngine.Random.value <= probability) playerRNGresult.Enqueue(true);
                else playerRNGresult.Enqueue(false);
            }
            foreach (BattleAction action in PerformMove(playerMove, ActiveEnemyMon, playerRNGresult))
            {
                yield return action;
            }
        }
        if (enemyChoice < 4)
        {
            Move enemyMove = ActiveEnemyMon.GetMove(enemyChoice);
            Queue<bool> enemyRNGresult = new Queue<bool>();
            for (int i = 0; i < enemyMove.GetAccuracy().Length; i++)
            {
                float probability = enemyMove.GetAccuracy(i) / 100f;
                if (UnityEngine.Random.value <= probability) enemyRNGresult.Enqueue(true);
                else enemyRNGresult.Enqueue(false);
            }
            foreach (BattleAction action in PerformMove(enemyMove, ActiveEnemyMon, enemyRNGresult))
            {
                yield return action;
            }
        }
    }

    private IEnumerable<BattleAction> EnemyMovesFirst(int playerChoice, int enemyChoice, float[] playerRNG)
    {
        if (playerChoice < 4)
        {
            Move playerMove = ActivePlayerMon.GetMove(playerChoice);
            Queue<bool> playerRNGresult = new Queue<bool>();
            for (int i = 0; i < playerMove.GetAccuracy().Length; i++)
            {
                float probability = CalculateProbability(playerRNG[i], playerMove.GetAccuracy(i));
                Debug.Log($"Probability {i}: {probability}");
                if (UnityEngine.Random.value <= probability) playerRNGresult.Enqueue(true);
                else playerRNGresult.Enqueue(false);
            }
            foreach (BattleAction action in PerformMove(playerMove, ActiveEnemyMon, playerRNGresult))
            {
                yield return action;
            }
        }
        if (enemyChoice < 4)
        {
            Move enemyMove = ActiveEnemyMon.GetMove(enemyChoice);
            Queue<bool> enemyRNGresult = new Queue<bool>();
            for (int i = 0; i < enemyMove.GetAccuracy().Length; i++)
            {
                float probability = enemyMove.GetAccuracy(i) / 100f;
                if (UnityEngine.Random.value <= probability) enemyRNGresult.Enqueue(true);
                else enemyRNGresult.Enqueue(false);
            }
            foreach (BattleAction action in PerformMove(enemyMove, ActiveEnemyMon, enemyRNGresult))
            {
                yield return action;
            }
        }
    }

    private IEnumerable<BattleAction> PerformMove(Move move, Pokemon target, Queue<bool> RNGresult)
    {
        // do player move if hit, else return miss action
        if (RNGresult.Dequeue())
        {
            Debug.Log("Move Hit!");
            foreach (BattleAction action in move.GetActions(target, RNGresult.Dequeue())) // perform move
            {
                yield return action;
            }
        }
        else
        {
            Debug.Log("Move Missed!");
            yield return new BattleAction(BattleAction.ActionType.Miss);
        }
    }

    /// <summary>
    /// Calculates the probability of rng succeeding based on the chance of the event and the performance of the qte. <br></br>
    /// x is a value from 0 to 1, accruacy is a value from 0 to 100. Accuracy is converted to a
    /// range of 5 to 0, x is raised to the power of that value. The result is returned.
    /// </summary>
    private float CalculateProbability(float x, float accuracy)
    {
        float a = 5f - (accuracy / 100f * 5f); // Accuracy converted from 0-100 to 5-0
        return Mathf.Pow(x, a);
    }

}

public readonly struct BattleAction
{
    public enum ActionType
    {
        Miss,
        Attack,
        Debuff, 
        DebuffCured,
        Switch,
        DoNothing,
        TakeDamage
    }
    public Effectiveness Effectiveness { get; }
    public ActionType Type { get; }
    public bool PerformedByEnemy { get; }

    public BattleAction(ActionType type, Effectiveness effectiveness = Effectiveness.Default, bool performedByEnemy = false)
    {
        Type = type;
        Effectiveness = effectiveness;
        PerformedByEnemy = performedByEnemy;
    }
}

public enum Effectiveness
{
    Default,
    SuperEffective,
    NotEffective,
    NoEffect
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


