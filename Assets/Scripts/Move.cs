using System;

public abstract class Move
{
    // This is a struct that stores the basic data of a move
    public readonly struct MoveData
    {
        public string Name { get; }
        public int Power { get; }
        public int Accuracy { get; }
        public int PowerPoints { get; }
        public PokeType Type { get; }

        // Constructor
        public MoveData(int power, int accuracy, int powerPoints, PokeType type, string name = "Move")
        {
            if (accuracy > 100 || accuracy < 0) throw new ArgumentException("Accuracy must be between 0 and 100");
            if (power < 0 || powerPoints < 0) throw new ArgumentException("Move values must be positive");

            Name = name;
            Power = power;
            Accuracy = accuracy;
            PowerPoints = powerPoints;
            Type = type;
        }

        public override string ToString() => $"Move name: {Name}, Power: {Power}, Accuracy: {Accuracy}, PP: {PowerPoints}";

    }

    // The data of a move can be accessed through this variable
    public abstract MoveData Data { get; }
    public Pokemon Owner { get; set; }

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
    public virtual void GetActions(Pokemon target)
    {
        // By default, a move will just attack the target.
        // This method may be overridden by the inheriting class to change its behaviour
        Attack(target);
    }

    /// <summary>
    /// Attack the enemy pokemon with basic damage calculation
    /// </summary>
    protected void Attack(Pokemon target)
    {
        target.ModifyHP(manager.CalculateDamage(this, Owner, target, out float effectiveness));
        UnityEngine.Debug.Log($"{Owner.Stats.Name} used {Data.Name}!");
        if (effectiveness > 1) UnityEngine.Debug.Log("It was super effective!");
        else if (effectiveness < 1) UnityEngine.Debug.Log("It wasn't very effective...");
    }

    // WIP
    protected void Debuff(Pokemon target, Status status)
    {
        target.Status = status;
    }
}