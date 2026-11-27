using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move
{
    // This is a struct that stores the basic data of a move
    public readonly struct MoveData
    {
        public string Name { get; }
        public int Power { get; }
        public int Accuracy { get; }
        public Status Status { get; }
        public int StatusAccuracy { get; }
        public int PowerPoints { get; }
        public PokeType Type { get; }

        // Constructor
        public MoveData(int power, int accuracy, int powerPoints, PokeType type, string name = "Move", Status status = Status.None, int statusAccuracy = 100)
        {
            if (accuracy > 100 || accuracy < 0 || statusAccuracy > 100 || statusAccuracy < 0) throw new ArgumentException("Accuracy must be between 0 and 100");
            if (power < 0 || powerPoints < 0) throw new ArgumentException("Move values must be positive");

            Name = name;
            Power = power;
            Accuracy = accuracy;
            PowerPoints = powerPoints;
            Type = type;
            Status = status;
            StatusAccuracy = statusAccuracy;
        }

        public override string ToString() => $"Move name: {Name}, Power: {Power}, Accuracy: {Accuracy}, PP: {PowerPoints}";

    }

    // The data of a move can be accessed through this variable
    public abstract MoveData Data { get; }
    public Pokemon Owner { get; set; }

    public float[] GetAccuracy()
    {
        return new float[] { Data.Accuracy, Data.StatusAccuracy };
    }

    public float GetAccuracy(int i)
    {
        float[] acc = GetAccuracy();
        if (i >= acc.Length) throw new ArgumentOutOfRangeException($"No accuracy found at index {i}");
        return acc[i];
    }

    protected BattleManager manager;

    // ---------- CONSTRUCTOR ----------
    public Move()
    {
        manager = UnityEngine.GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
    }

    // ---------- Move actions ----------
    /// <summary>
    /// Performs this move's actions, directed at the specified target pokemon.
    /// </summary>
    public virtual IEnumerable<BattleAction> GetActions(Pokemon target, bool statusHit)
    {
        // By default, a move will just attack the target.
        // This method may be overridden by the inheriting class to change its behaviour
        yield return Attack(target);
        if(Data.Status != Status.None && statusHit)
        {
            Debug.Log("Status Hit!");
            if (CanDebuff(target, Data.Status))
                yield return Debuff(target, Data.Status);
        }
        yield break;
    }

    /// <summary>
    /// Attack the enemy pokemon with basic damage calculation
    /// </summary>
    protected BattleAction Attack(Pokemon target)
    {
        target.ModifyHP(manager.CalculateDamage(this, Owner, target, out float effectiveness));
        Effectiveness e;
        if (effectiveness == 0) e = Effectiveness.NoEffect;
        else if (effectiveness > 1) e = Effectiveness.SuperEffective;
        else if (effectiveness < 1) e = Effectiveness.NotEffective;
        else e = Effectiveness.Default;
        return new BattleAction(BattleAction.ActionType.Attack, e, Owner.IsEnemy);
    }

    // WIP
    protected BattleAction Debuff(Pokemon target, Status status)
    {
        if (status == Status.Poison && target.Status == Status.Poison)
        {
            status = Status.BadPoison;
            target.BadPoisonCounter = 1;
        }
        target.Status = status;
        if (status == Status.Freeze) target.FreezeAndSleepCounter = 3;
        if (status == Status.Sleep) target.FreezeAndSleepCounter = UnityEngine.Random.Range(1, 4);
        return new BattleAction(BattleAction.ActionType.Debuff, performedByEnemy: Owner.IsEnemy);
    }

    protected bool CanDebuff(Pokemon target, Status status)
    {
        if (target.Status != Status.None || (target.Status == Status.Poison && status != Status.Poison)) return false;
        switch (status)
        {
            case Status.Poison:
            case Status.BadPoison:
                return (target.Stats.Type1 != PokeType.Poison || target.Stats.Type2 != PokeType.Poison);
            case Status.Burn:
                return (target.Stats.Type1 != PokeType.Fire || target.Stats.Type2 != PokeType.Fire);
            case Status.Paralysis:
                return (target.Stats.Type1 != PokeType.Electric || target.Stats.Type2 != PokeType.Electric);
            case Status.Freeze:
                return (target.Stats.Type1 != PokeType.Ice || target.Stats.Type2 != PokeType.Ice);
            default:
                return true;
        }
    }
}