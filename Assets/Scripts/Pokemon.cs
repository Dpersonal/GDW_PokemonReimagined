using System;
using UnityEngine;

public abstract class Pokemon
{
    public readonly struct PokeStats
    {
        public PokeType Type1 { get; }
        public PokeType Type2 { get; }
        public int MaxHP { get; }
        public string Name { get; }

        public PokeStats(PokeType type1, PokeType type2, int maxHP, string name = "Pokemon")
        {
            Type1 = type1;
            Type2 = type2;
            MaxHP = maxHP;
            Name = name;
        }
    }

    // ---------- Base Info ----------
    public abstract PokeStats Stats { get; }
    private int _hp;
    public int HP 
    {
        get { return _hp; }
        private set
        {
            _hp = value;
            if (_hp > Stats.MaxHP) _hp = Stats.MaxHP;
            if (_hp < 0) _hp = 0;
        }
    }

    /// <summary>
    /// Dereases the pokemon's HP value by a specified amount. <br></br>
    /// If <c>isHeal</c> is <c>true</c>, increase HP instead.
    /// </summary>
    public void ModifyHP(int amount, bool isHeal = false)
    {
        if (amount < 0) Debug.LogWarning("Using a negative value may cause unintended behaviour. If you would like to increase HP, set isHeal to true instead.");
        
        if (isHeal) HP += amount;
        else HP -= amount;
    }

    // ---------- Pokemon Move Handling ----------
    private Move[] Moves { get; set; }

    /// <summary>
    /// Returns an array of size 4 containing all of the pokemon's moves. Empty slots will be null.
    /// </summary>
    public Move[] GetMoves()
    {
        Move[] moves = new Move[] { null, null, null, null };
        for (int i = 0; i < Moves.Length; i++)
        {
            moves[i] = Moves[i];
        }
        return Moves;
    }

    /// <summary>
    /// Returns one of the pokemon's 4 moves. If the pokemon has less than 4 moves, this value may be null.
    /// </summary>
    public Move GetMove(int i)
    {
        if (i > 3 || i < 0) throw new ArgumentException("Trying to get move with index out of range");

        return GetMoves()[i];
    }

    /// <summary>
    /// Set the moves of the pokemon. Moves cannot exceed four.
    /// </summary>
    public void SetMoves(Move[] moves)
    {
        if (moves.Length > 4) throw new ArgumentException("Pokemon cannot have more than 4 moves");

        Moves = new Move[] { null, null, null, null };
        for (int i = 0;i < moves.Length; i++)
        {
            Moves[i] = moves[i];
            Moves[i].Owner = this;
        }
    }

    // ---------- CONSTRUCTOR ----------
    public Pokemon(bool isEnemy = false)
    {
        manager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        HP = Stats.MaxHP;
        IsEnemy = isEnemy;
    }

    public Pokemon(Move[] moves, bool isEnemy = false) : this(isEnemy)
    {
        SetMoves(moves);
    }

    public bool IsEnemy { get; }

    protected BattleManager manager;

    // ---------- Status (WIP) ----------
    public Status Status { get; set; }
    public bool IsConfused { get; private set; }
    public bool IsDead { get => HP <= 0; }

    // may not be used
    public void Confuse()
    {
        IsConfused = true;
        Debug.Log($"{Stats.Name} was confused!");
        // play animation/display message whatever
    }
}