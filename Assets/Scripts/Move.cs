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
    public virtual void GetActions(Pokemon target)
    {
        Attack(target);
    }

    protected void Attack(Pokemon target)
    {
        target.ModifyHP(manager.CalculateDamage(this, Owner, target, out float effectiveness));
        if (effectiveness > 1) UnityEngine.Debug.Log("It was super effective!");
        else if (effectiveness < 1) UnityEngine.Debug.Log("It wasn't very effective...");
    }

    protected void Debuff(Pokemon target, Status status)
    {
        target.Status = status;
    }
}