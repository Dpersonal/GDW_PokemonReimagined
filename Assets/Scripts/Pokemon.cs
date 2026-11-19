using System;
using System.Collections.Generic;

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

    public void ModifyHP(int amount, bool isHeal = false)
    {
        if (isHeal) HP += amount;
        else HP -= amount;
    }

    // ---------- Pokemon Move Handling ----------
    private Move[] Moves { get; set; }

    public Move[] GetMoves()
    {
        Move[] moves = new Move[] { null, null, null, null };
        for (int i = 0; i < Moves.Length; i++)
        {
            moves[i] = Moves[i];
        }
        return Moves;
    }

    public Move GetMove(int i)
    {
        return GetMoves()[i];
    }

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
    public Pokemon()
    {
        manager = UnityEngine.GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        HP = Stats.MaxHP;
    }

    protected BattleManager manager;

    // ---------- Status ----------
    public Status Status { get; set; }
    public bool IsConfused { get; private set; }
    public bool IsDead { get => HP <= 0; }

    public void Confuse()
    {
        IsConfused = true;
        UnityEngine.Debug.Log($"{Stats.Name} was confused!");
        // play animation/display message whatever
    }
}