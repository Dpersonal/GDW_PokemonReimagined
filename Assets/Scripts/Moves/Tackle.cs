
public sealed class Tackle : Move
{
    public Tackle() : base() { }

    public override MoveData Data => new MoveData
    (
        power: 40, 
        accuracy: 100, 
        powerPoints: 35, 
        type: PokeType.Normal,
        name: "Tackle"
    );

}